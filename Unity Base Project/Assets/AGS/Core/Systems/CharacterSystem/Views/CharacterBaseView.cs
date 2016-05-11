using System;
using System.Collections.Generic;
using System.Linq;
using AGS.Core.Classes.DataClasses;
using AGS.Core.Classes.Helpers;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CombatSkillSystem;
using AGS.Core.Systems.HazardSystem;
using AGS.Core.Systems.MovementSystem.Base;
using AGS.Core.Systems.StatusEffectSystem;
using AGS.Core.Utilities;
using UnityEngine;

namespace AGS.Core.Systems.CharacterSystem
{
    /// <summary>
    /// Base view for characters. Inheriting views must implement several abstracts that are view specific
    /// </summary>
    public abstract class CharacterBaseView : CombatEntityBaseView
    {
        #region Public properties
        // Fields to be set in the editor
        public float Speed;
        public float SkinWidth;
        public int SkinCorrectionRays;
        public float SlopeLimitMoving;
        public float SlopeLimitSliding;

        // References to be set in the editor
        public MovementSkillsView MovementSkillsView;
        public ThrowingSkillView ThrowingSkillView;

        #endregion

        public CharacterBase Character;

        protected LayerMask GroundMask; // Mask for skinrays detection
        protected Vector3 PushForceToAdd;
        protected Transform MovingGround;
        protected Vector3 PreviousMovingGroundPos = Vector3.zero;
        private bool _justSwitchedThrowable;

        #region AGS setup
        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            Character = model as CharacterBase;
            if (Character == null) return;
            Character.MovementSkills.Value = MovementSkillsView != null ? MovementSkillsView.MovementSkills : null;
            Character.ThrowingSkill.Value = ThrowingSkillView != null ? ThrowingSkillView.ThrowingSkill : null;
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            // Varius subscribtions to character ActionProperties
            Character.Radius.Value = GetCharacterRadius();
            Character.Radius.OnValueChanged += (sender, radius) => SetCharacterRadius(radius.Value);
            Character.Height.Value = GetCharacterHeight();
            Character.Height.OnValueChanged += (sender, height) => SetCharacterHeight(height.Value);
            Character.HeightCenter.Value = GetCharacterHeightCenter();
            Character.HeightCenter.OnValueChanged += (sender, heightCenter) => SetCharacterHeightCenter(heightCenter.Value);

            Character.FaceSlopeDirectionAction += FaceSlopeDirection;
            Character.RotateTowardsDirectionAction += RotateTowardsDirectionNotification;
            Character.HitBelowAction += CheckOnMovingGround;
            Character.PushEffectAction += HandlePushEffects;
            Character.StandUprightAction += StandUpright;
            Character.LandedAction += LandedNotification;
            Character.StopMovementAction += StopMovement;
            Character.AffectedByGravity.OnValueChanged += (sender, useGravity) => SetGravity(useGravity.Value);
            Character.AffectedByPhysics.OnValueChanged += (sender, usePhysics) => SetPhysics(usePhysics.Value);
            if (Character.MovementSkills.Value == null || Character.MovementSkills.Value.Swimming.Value == null) return;

            // Fluid trigger notifications
            Action<FluidBaseView> spashIntoWaterAction = SpashIntoWater;
            gameObject.OnTriggerActionEnterWith(spashIntoWaterAction);
            Action<FluidBaseView> outOfWaterAction = _ => Character.MovementSkills.Value.Swimming.Value.OutOfFluid();
            gameObject.OnTriggerActionExitWith(outOfWaterAction);
        }
        #endregion

        #region MonoBehaviours
        public override void Awake()
        {
            base.Awake();

            if (GetComponent<GroundMask>() != null)
            {
                GroundMask = GetComponent<GroundMask>().Mask;
            }
        }

        public override void Update()
        {
            base.Update();
            CheckThrowablesSwitching();
            SetCharacterMovementValues();
            Character.CurrentVelocity.Value = GetCurrentVelocity();
        }
        #endregion

        #region private functions
        /// <summary>
        /// Sets the character movement values.
        /// This function is called every update
        /// </summary>
        protected virtual void SetCharacterMovementValues()
        {
            if (Character == null) return;
            Character.FacingGameLevelForward.Value = CheckFacingGameLevelForward();
            Character.MovingBackwards.Value = CheckMovingBackwards();
            Character.HitAbove.Value = CheckHitAbove();
            Character.HitBelow.Value = CheckHitBelow();
            Character.HitForward.Value = CheckHitForward();
            Character.CurrentMotion.Value = GetCurrentCharacterMotion();
        }

        /// <summary>
        /// Checks if Character should switch throwable weapon type.
        /// </summary>
        private void CheckThrowablesSwitching()
        {
            if (_justSwitchedThrowable) return;
            if (Character == null || Character.CharacterController.Value == null) return;
            if (CombatEntity.ActiveCombatMoveSet.Value == null) return;
            if (Character.ActiveCombatMoveSet.Value.ActiveCombatMove.Value != null) return; // Dont allow throwable weapon switching in middle of attack
            if (Character.CharacterController.Value.NextThrowable.Value)
            {
                NextThrowable();
            }
        }

        /// <summary>
        /// Selects next throwable type in the ThrowableWeaponStash
        /// </summary>
        private void NextThrowable()
        {
            var curThrowableStash = Character.ThrowableWeaponStashes.First(x => x.ThrowableWeaponType == Character.ActiveThrowableType.Value);
            var curThrowableStashIndex = Character.ThrowableWeaponStashes.IndexOf(curThrowableStash);

            var throwableWeaponStash = Character.ThrowableWeaponStashes.FirstOrDefault();
            if (throwableWeaponStash != null)
            {
                Character.ActiveThrowableType.Value = curThrowableStashIndex == Character.ThrowableWeaponStashes.Count - 1 ? throwableWeaponStash.ThrowableWeaponType : Character.ThrowableWeaponStashes[curThrowableStashIndex + 1].ThrowableWeaponType;
            }
            ThrowableSwitchThrottle();
        }

        /// <summary>
        /// Sets up a short timer to prevent spamming throwable weapon switching
        /// </summary>
        private void ThrowableSwitchThrottle()
        {
            _justSwitchedThrowable = true;
            var weaponSwitchThrottle = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Throwable switch throttle");
            weaponSwitchThrottle.TimerMethod = () => _justSwitchedThrowable = false;
            weaponSwitchThrottle.Invoke(0.5f);
        }

        /// <summary>
        /// Set character to start swimming in Fluid.
        /// </summary>
        /// <param name="fluidView">The fluid view.</param>
        private void SpashIntoWater(FluidBaseView fluidView)
        {
            if (fluidView == null) return;
            Character.MovementSkills.Value.Swimming.Value.StartSwimming(fluidView.Fluid);
        }

        /// <summary>
        /// Handles the push effects. Checks if push effect has additional ticks.
        /// </summary>
        /// <param name="pushEffect">The push effect.</param>
        /// <param name="hitFromBehind">if set to <c>true</c> character is [hit from behind].</param>
        private void HandlePushEffects(PushEffect pushEffect, bool hitFromBehind)
        {
            if (pushEffect.Ticks == 0)
            {
                ApplyPushEffect(pushEffect, hitFromBehind);
            }
            else
            {
                // set up a timer for ticking push effects
                var pushEffectsInterval = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Push effect interval");
                pushEffectsInterval.TimerMethod = () => ApplyPushEffect(pushEffect, hitFromBehind);
                pushEffectsInterval.SetupIntervalFinite(TimeSpan.FromSeconds(pushEffect.SecondsBetweenTicks), pushEffect.Ticks);
            }

        }

        /// <summary>
        /// Applies the pusheffect with correct values depening on situation, and notifies any subscriber to do the actual push
        /// </summary>
        /// <param name="pushEffect">The push effect.</param>
        /// <param name="hitFromBehind">if set to <c>true</c> then character is [hit from behind].</param>
        protected void ApplyPushEffect(PushEffect pushEffect, bool hitFromBehind)
        {
            switch (pushEffect.Direction)
            {
                case VectorDirection.Forward:
                    PushForceToAdd = hitFromBehind ? -transform.forward * pushEffect.Strength
                                                       : transform.forward * pushEffect.Strength;
                    break;
                case VectorDirection.Back:
                    PushForceToAdd = hitFromBehind ? transform.forward * pushEffect.Strength
                                                       : -transform.forward * pushEffect.Strength;
                    break;
                case VectorDirection.Up:
                    PushForceToAdd = Vector3.up * pushEffect.Strength;
                    break;
                case VectorDirection.Down:
                    PushForceToAdd = -Vector3.up * pushEffect.Strength;
                    break;
                default:
                    PushForceToAdd = Vector3.zero;
                    break;
            }
            Character.BeingPushed.Value = true;

            var pushEffectsTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Push effect timer");
            pushEffectsTimer.TimerMethod = () =>
            {
                Character.BeingPushed.Value = false;
            };
            pushEffectsTimer.Invoke(0.25f);

            TriggerPushEffect(PushForceToAdd, ForceConverter.ForceTypeToUnityForceMode(pushEffect.ForceType));
        }

        /// <summary>
        /// Returns an angle based on a raycast hit and the transform.forward vector
        /// </summary>
        /// <param name="hitAngle">The hit angle.</param>
        /// <returns></returns>
        private float GetHitAngleForward(RaycastHit hitAngle)
        {
            var angle = 0f;
            if (!(Mathf.Abs(Vector3.Dot(transform.forward, hitAngle.normal)) > 0.001f))
                return angle;


            angle = Vector3.Angle(transform.forward, hitAngle.normal);
            // make angle a number between -90 & 90, so that we know if facing downhill or uphill
            angle -= 90f;
            return angle;
        }

        /// <summary>
        /// Returns the slope angle based on the ground normal parameter and the Vector3.up vector
        /// </summary>
        /// <param name="groundNormal">The ground normal.</param>
        /// <returns></returns>
        private float GetSlopeAngle(Vector3 groundNormal)
        {
            return Vector3.Angle(Vector3.up, groundNormal);
        }

        /// <summary>
        /// Gets the current velocity.
        /// </summary>
        /// <returns></returns>
        protected abstract Vector3 GetCurrentVelocity();

        #endregion
        #region HitObstacle functions
        /// <summary>
        /// Sets the ground values.
        /// </summary>
        /// <param name="hitInfo">The hit information.</param>
        private void SetGroundValues(RaycastHit hitInfo)
        {
            Character.SlopeAngleForward.Value = GetHitAngleForward(hitInfo);
            Character.SlopeAngle.Value = GetSlopeAngle(hitInfo.normal);
            Character.GroundNormal.Value = hitInfo.normal;
        }

        /// <summary>
        /// Checks HitObstacle below the character, within SkinWidth.
        /// </summary>
        /// <returns></returns>
        protected virtual HitObstacle CheckHitBelow()
        {
            var rayDirection = -transform.up;
            Vector3 startPosition;
            if (Character.SkinCorrectionRays == 1)
            {
                startPosition = new Vector3(transform.position.x, transform.position.y + Character.Radius.Value, transform.position.z);

                Debug.DrawRay(startPosition, rayDirection * Character.SkinWidth.Value, Color.magenta);
                RaycastHit hitInfo;

                var rayCastHit = Physics.Raycast(startPosition, rayDirection, out hitInfo, Character.SkinWidth.Value, GroundMask);

                if (!rayCastHit)
                {

                    Character.OnUnevenGround.Value = false;
                    Character.SlopeAngleForward.Value = 0f;
                    Character.GroundNormal.Value = Vector3.zero;
                    return HitObstacle.None;
                }
                SetGroundValues(hitInfo);
                var hitObstacle = HitObstacleHelper.GetHitObstacle(hitInfo.transform);
                MovingGround = hitObstacle == HitObstacle.MovingGround ? hitInfo.transform : null;
                return hitObstacle;
            }

            startPosition = new Vector3(transform.position.x - transform.forward.x * Character.Radius.Value, transform.position.y + Character.Radius.Value, transform.position.z - transform.forward.z * Character.Radius.Value);

            Character.SlopeAngleForward.Value = 0f;
            Character.SlopeAngle.Value = 0f;
            Character.GroundNormal.Value = Vector3.zero;

            var hitList = new List<HitAngleAndNormal>();

            for (var i = 0; i <= Character.SkinCorrectionRays; i++)
            {

                var rayVector = new Vector3(startPosition.x + transform.forward.x * Character.Radius.Value * 2 / Character.SkinCorrectionRays * i, startPosition.y, startPosition.z + transform.forward.z * Character.Radius.Value * 2 / Character.SkinCorrectionRays * i);

                Debug.DrawRay(rayVector, rayDirection * Character.SkinWidth.Value, Color.magenta);
                RaycastHit hitInfo;

                var rayCastHit = Physics.Raycast(rayVector, rayDirection, out hitInfo, Character.SkinWidth.Value, GroundMask);

                if (!rayCastHit)
                {
                    hitList.Add(new HitAngleAndNormal { GroundNormal = Vector3.zero, HitAngleForward = 0f, HitObstacle = HitObstacle.None });
                    continue;
                }

                var hitAngleAndNormal = new HitAngleAndNormal { HitAngleForward = GetHitAngleForward(hitInfo), SlopeAngle = GetSlopeAngle(hitInfo.normal), GroundNormal = hitInfo.normal };
                hitList.Add(hitAngleAndNormal);

                hitAngleAndNormal.HitObstacle = HitObstacleHelper.GetHitObstacle(hitInfo.transform);
                MovingGround = hitAngleAndNormal.HitObstacle == HitObstacle.MovingGround ? hitInfo.transform : null;
            }

            // Find the most common keyvalue of angle and slope in our list of raycast hits (excluding non-hits)
            var hitAnglesAndNormals = hitList.GroupBy(keyValue => keyValue.HitAngleForward)
                                .OrderByDescending(group => group.Count())
                                .SelectMany(group => group)
                                .Where(x => x != null && x.HitObstacle != HitObstacle.None).ToList();

            if (hitAnglesAndNormals.Any())
            {
                var onUneveGround = hitAnglesAndNormals.Distinct(new HitAngleAndNormalComparer());

                Character.OnUnevenGround.Value = onUneveGround.Count() > 1;

                var groundValues = hitAnglesAndNormals.First();
                Character.SlopeAngleForward.Value = groundValues.HitAngleForward;
                Character.SlopeAngle.Value = groundValues.SlopeAngle;
                Character.GroundNormal.Value = groundValues.GroundNormal;
                return groundValues.HitObstacle;
            }


            Character.OnUnevenGround.Value = false;
            Character.SlopeAngleForward.Value = 0f;
            Character.SlopeAngle.Value = 0f;
            Character.GroundNormal.Value = Vector3.zero;
            return HitObstacle.None;

        }

        /// <summary>
        /// Checks HitObstacle above the character, within SkinWidth.
        /// </summary>
        /// <returns></returns>
        protected virtual HitObstacle CheckHitAbove()
        {
            var rayDirection = transform.up;
            var startPosition = new Vector3(transform.position.x - transform.forward.x * Character.Radius.Value, transform.position.y + Character.Height.Value, transform.position.z - transform.forward.z * Character.Radius.Value);
            var hitObstacle = HitObstacle.None;
            // init closestHitDistance to something a litter longer than raycast length
            var closestHitDistance = Character.SkinWidth.Value / 4f + 0.1f;

            for (var i = 0; i <= Character.SkinCorrectionRays; i++)
            {
                var rayVector = new Vector3(startPosition.x + transform.forward.x * Character.Radius.Value * 2 / Character.SkinCorrectionRays * i, startPosition.y, startPosition.z + transform.forward.z * Character.Radius.Value * 2 / Character.SkinCorrectionRays * i);
                Debug.DrawRay(rayVector, rayDirection * Character.SkinWidth.Value, Color.magenta);
                RaycastHit hitInfo;
                var rayCastHit = Physics.Raycast(rayVector, rayDirection, out hitInfo, Character.SkinWidth.Value, GroundMask);

                if (!rayCastHit)
                {
                    continue;
                }
                var hitDistance = Vector3.Distance(rayVector, hitInfo.point);
                if (hitDistance >= closestHitDistance)
                {
                    // we have hit something closer
                    continue;
                }
                closestHitDistance = hitDistance;
                hitObstacle = HitObstacleHelper.GetHitObstacle(hitInfo.transform);
            }
            return hitObstacle;
        }

        /// <summary>
        /// Checks HitObstacle in front of the character, within SkinWidth.
        /// </summary>
        /// <returns></returns>
        protected virtual HitObstacle CheckHitForward()
        {
            var rayDirection = transform.forward;
            if (Character.MovingBackwards.Value && Character.MovementSkills.Value.VerticalMovement.Value != null && Character.MovementSkills.Value.VerticalMovement.Value.VerticalMovementCurrentState.Value != VerticalMovementState.WallJumping)
            {
                // shoot rays from back instead
                rayDirection = -transform.forward;
            }
            var startPosition = new Vector3(transform.position.x, transform.position.y + Character.Radius.Value, transform.position.z);
            var hitObstacle = HitObstacle.None;
            Character.HitForwardAngle.Value = 0f;
            Character.ForwardReflectionVector.Value = Vector3.zero;
            Character.ForwardNormalVector.Value = Vector3.zero;
            // init closestHitDistance to something a litter longer than raycast length
            var closestHitDistance = Character.SkinWidth.Value + 0.1f;
            for (var i = 0; i <= Character.SkinCorrectionRays; i++)
            {

                var rayVector = new Vector3(startPosition.x, startPosition.y + (Character.Height.Value - Character.Radius.Value * 2) / Character.SkinCorrectionRays * i, startPosition.z);

                Debug.DrawRay(rayVector, rayDirection * Character.SkinWidth.Value, Color.red);
                RaycastHit hitInfo;
                var rayCastHit = Physics.Raycast(rayVector, rayDirection, out hitInfo, Character.SkinWidth.Value, GroundMask);

                if (!rayCastHit)
                {
                    continue;
                }
                var hitDistance = Vector3.Distance(rayVector, hitInfo.point);
                if (hitDistance >= closestHitDistance)
                {
                    // we have hit something closer
                    continue;
                }
                closestHitDistance = hitDistance;
                Character.HitForwardAngle.Value = GetHitAngleForward(hitInfo);

                Character.ForwardReflectionVector.Value = Vector3.Reflect(rayDirection, hitInfo.normal);
                Character.ForwardNormalVector.Value = hitInfo.normal;
                hitObstacle = HitObstacleHelper.GetHitObstacle(hitInfo.transform);
            }

            return hitObstacle;
        }

        /// <summary>
        /// Called when the character turns around.
        /// </summary>
        protected override void TurnAroundNotification()
        {
            base.TurnAroundNotification();
            if (Character.OwnerGameLevel != null && Character.CharacterOutOfBoundsHorizontal.Value)
            {
                Character.CharacterOutOfBoundsHorizontal.Value = false;
            }
        }

        #endregion

        #region abstract functions
        /// <summary>
        /// Checks if the CombatEntity is facing forward.
        /// </summary>
        /// <returns></returns>
        protected abstract bool CheckFacingGameLevelForward();

        /// <summary>
        /// Gets the height of the character.
        /// </summary>
        /// <returns></returns>
        protected abstract float GetCharacterHeight();

        /// <summary>
        /// Sets the height of the character.
        /// </summary>
        /// <param name="height">The height.</param>
        protected abstract void SetCharacterHeight(float height);

        /// <summary>
        /// Gets the character height center.
        /// </summary>
        /// <returns></returns>
        protected abstract Vector3 GetCharacterHeightCenter();

        /// <summary>
        /// Sets the character height center.
        /// </summary>
        /// <param name="center">The center.</param>
        protected abstract void SetCharacterHeightCenter(Vector3 center);

        /// <summary>
        /// Gets the character radius.
        /// </summary>
        /// <returns></returns>
        protected abstract float GetCharacterRadius();

        /// <summary>
        /// Sets the character radius.
        /// </summary>
        /// <param name="radius">The radius.</param>
        protected abstract void SetCharacterRadius(float radius);


        /// <summary>
        /// Checks if the character is on moving ground.
        /// </summary>
        /// <param name="hitObstacle">The hit obstacle.</param>
        protected abstract void CheckOnMovingGround(HitObstacle hitObstacle);

        /// <summary>
        /// Stands the character up.
        /// </summary>
        protected abstract void StandUpright();

        /// <summary>
        /// Faces the slope direction.
        /// </summary>
        protected abstract void FaceSlopeDirection();

        /// <summary>
        /// Called when the characters lands after being airborne
        /// </summary>
        protected virtual void LandedNotification() { }

        /// <summary>
        /// Stops the characters movement.
        /// </summary>
        protected abstract void StopMovement();

        /// <summary>
        /// Triggers the push effect.
        /// </summary>
        /// <param name="pushForceToAdd">The push force to add.</param>
        /// <param name="value">The value.</param>
        protected abstract void TriggerPushEffect(Vector3 pushForceToAdd, ForceMode value);

        /// <summary>
        /// Sets UseGravity.
        /// </summary>
        /// <param name="useGravity">if set to <c>true</c> then character [use gravity].</param>
        protected abstract void SetGravity(bool useGravity);

        /// <summary>
        /// Sets UsePhysics.
        /// </summary>
        /// <param name="usePhysics">if set to <c>true</c> then character [use physics].</param>
        protected abstract void SetPhysics(bool usePhysics);

        /// <summary>
        /// Check if character is moving backwards.
        /// </summary>
        /// <returns></returns>
        protected abstract bool CheckMovingBackwards();

        /// <summary>
        /// Gets the current character motion.
        /// </summary>
        /// <returns></returns>
        protected abstract MotionData GetCurrentCharacterMotion();
        #endregion


    }



}
