using System;
using System.Linq;
using AGS.Core.Base;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.DataClasses;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Interfaces;
using AGS.Core.Systems.CombatSkillSystem;
using AGS.Core.Systems.MovementSystem.Base;
using AGS.Core.Systems.StatusEffectSystem;
using AGS.Core.Systems.WeaponSystem;
using UnityEngine;

namespace AGS.Core.Systems.CharacterSystem
{
    /// <summary>
    /// Base class for characters. Characters inherit from CombatEntityBase and thus inherit combat skills and killable functionality. Character also implement IMovable.
    /// Suitable for bipeds, but could easily fit any living GameObject that is capable of movement.
    /// </summary>
    public abstract class CharacterBase : CombatEntityBase, IMovable
    {
        #region Properties
        // Constructor properties
        public float Speed { get; set; }
        public int SkinCorrectionRays { get; set; }
        public float SlopeLimitMoving { get; set; }
        public float SlopeLimitSliding { get; set; }

        // Subscribable properties
        public ActionProperty<bool> FacingGameLevelForward { get; set; } // Subscribe to this for boolean of the character is facing in the game scenes general forward direction. For a sidescroller it would be natural to equal Vector3.right or Vector2.right. For a 3rd person game it would be Vector3.forward
        public ActionProperty<Vector3> CurrentVelocity { get; set; } // Current velocity
        public ActionProperty<float> Height; // Character Collider height
        public ActionProperty<Vector3> HeightCenter; // Character Collider height center
        public ActionProperty<float> Radius; // Character Collider radius 
        public ActionProperty<float> SkinWidth { get; set; } // SkinWidth determines how far out from Character Collider raycasts that determines environment detection should go
        public ActionProperty<float> AirborneSpeedEffect { get; set; } // Used for reducing movement while airborne
        public ActionProperty<float> FixedSpeedEffect { get; set; } // A fixed speed effect that is added to general character speed
        public ActionProperty<float> RelativeSpeedEffect { get; set; } // A relative speed effect that is multiplied to general character speed
        public ActionProperty<float> HitForwardAngle { get; set; } // Angle of slope in front of character
        public ActionProperty<Vector3> ForwardReflectionVector { get; set; } // Reflection angle of obstacle in front of character
        public ActionProperty<Vector3> ForwardNormalVector { get; set; } // Normal of the obstacle in front of character
        public ActionProperty<float> SlopeAngle { get; set; } // Angle of slope below character
        public ActionProperty<float> SlopeAngleForward { get; set; } // Angle of slope below character relative to character forward direction
        public ActionProperty<Vector3> GroundNormal { get; set; } // normal of ground below character
        public ActionProperty<bool> MovingBackwards { get; set; } // Is character moving backwards?
        public ActionProperty<bool> OnUnevenGround { get; set; } // Is ground flat or irregular?
        public ActionProperty<Vector3> GroundVelocity { get; set; } // Velocity of the ground below character. Used for moving with moving environments
        public ActionProperty<MotionData> CurrentMotion { get; set; } // Current motion of character. Used for transferring motion to ragdoll
        public ActionProperty<bool> DirectionLocked { get; set; } // Can character turn around?        
        public ActionProperty<bool> VerticalVelocityLocked { get; set; } // Ok to change vertical velocity?
        public ActionProperty<bool> IsGrounded { get; set; } // Is the character currently grounded?
        public ActionProperty<bool> BeingPushed { get; set; } // This value is temporary set to true upon recieveing a pusheffect to make sure pusheffect is not overridden
        public ActionProperty<HitObstacle> HitForward { get; set; }  // Obstacle in front of character within SkinWidth
        public ActionProperty<HitObstacle> HitBelow { get; set; } // Obstacle below character within SkinWidth
        public ActionProperty<HitObstacle> HitAbove { get; set; } // Obstacle above character within SkinWidth
        public ActionProperty<bool> CharacterOutOfBoundsHorizontal { get; private set; } // Is the character out of GameLevelBounds in the horizontal plane?
        public ActionProperty<bool> CharacterOutOfBoundsVertical { get; private set; } // Is the character out of GameLevelBounds in the vertical plane?
        public ActionProperty<bool> AffectedByGravity { get; set; } // Is the character currently affected by gravity?
        public ActionProperty<bool> AffectedByPhysics { get; set; } // Is the character currently affected by physics?

        public ActionProperty<MovementSkills> MovementSkills { get; set; } // Owned movement skills
        public ActionProperty<ThrowingSkill> ThrowingSkill { get; set; } // Owned throwing skill
        public ActionProperty<ThrowableWeaponType> ActiveThrowableType { get; set; } // Active throwable weapon type
        public ActionList<MovementEffect> ActiveMovementEffects { get; set; } // Holds any movement effects that is currently affecting the character
        public ActionProperty<MovableState> MovableCurrentState { get; set; } // MovableCurrentState is directly dependent on ActiveMovementEffects
        public ActionList<ThrowableWeapon> ThrownTrowables { get; set; } // When throwing (instaniating a ThrowableWeapon), also add the ThrowableWeapon to this list.
        
        public ActionList<ThrowableWeaponStash> ThrowableWeaponStashes { get; set; } // Contains all differenct throwables and count, owned by character
        

        // Subscribable actions
        public Action<HitObstacle> HitBelowAction { get; set; }

        public Action FaceSlopeDirectionAction { get; set; }
        public Action FinishedSlideAction { get; set; }
        public Action<Direction> RotateTowardsDirectionAction { get; set; }
        public Action StandUprightAction { get; set; }
        public Action<Vector3> MoveToPositionAction { get; set; }
        public Action<PushEffect, bool> PushEffectAction { get; set; }
        public Action<Transform> OnMovingGroundAction { get; set; }
        public Action LandedAction { get; set; }
        public Action StopMovementAction { get; set; }

        #endregion Properties

        // private
        private TimerComponent _airborneStateInterval;
        private UpdateComponent _updateComponent;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterBase"/> class.
        /// </summary>
        /// <param name="transform">The CharacterBase transform.</param>
        /// <param name="name">CharacterBase name. Is instantiating ragdolls upon death, this need to match Ragdoll prefix. A Killable with the name John will try to instantiate JohnRagdoll</param>
        /// <param name="speed">General speed of the character</param>
        /// <param name="turnSpeed">Turn speed of the character</param>
        /// <param name="skinWidth">SkinWidth determines how far out from Character Collider raycasts that determines environment detection should go</param>
        /// <param name="skinCorrectionRays">How many rays should be used for detecting environment</param>
        /// <param name="slopeLimitMoving">Slope angle treshold in degrees before character begins to slide on slopes</param>
        /// <param name="slopeLimitSliding">Slope angle treshold in degress that determines if character should crouch or start manual sliding</param>
        protected CharacterBase(Transform transform, string name, float speed, float turnSpeed, float skinWidth, int skinCorrectionRays, float slopeLimitMoving, float slopeLimitSliding)
            : base(transform, name, turnSpeed)
        {
            Height = new ActionProperty<float>();
            HeightCenter = new ActionProperty<Vector3>();
            Radius = new ActionProperty<float>();
            Speed = speed;
            CurrentVelocity = new ActionProperty<Vector3>();
            FacingGameLevelForward = new ActionProperty<bool>() { Value = true };
            SkinWidth = new ActionProperty<float>() { Value = skinWidth };
            AirborneSpeedEffect = new ActionProperty<float>() { Value = 1f };
            FixedSpeedEffect = new ActionProperty<float>() { Value = 0f };
            RelativeSpeedEffect = new ActionProperty<float>() { Value = 1f };
            HitForwardAngle = new ActionProperty<float>() { Value = 0f };
            ForwardReflectionVector = new ActionProperty<Vector3>();
            ForwardNormalVector = new ActionProperty<Vector3>();
            SlopeAngleForward = new ActionProperty<float>() { Value = 0f };
            SlopeAngle = new ActionProperty<float>() { Value = 0f };
            GroundNormal = new ActionProperty<Vector3>() { Value = Vector3.zero };
            MovingBackwards = new ActionProperty<bool>() { Value = false };
            OnUnevenGround = new ActionProperty<bool>() { Value = false };
            GroundVelocity = new ActionProperty<Vector3>() { Value = Vector3.zero };
            CurrentMotion = new ActionProperty<MotionData>() { Value = new MotionData() };
            SkinCorrectionRays = skinCorrectionRays;
            SlopeLimitMoving = slopeLimitMoving;
            SlopeLimitSliding = slopeLimitSliding;
            DirectionLocked = new ActionProperty<bool>() { Value = false };
            VerticalVelocityLocked = new ActionProperty<bool>() { Value = false };
            IsGrounded = new ActionProperty<bool>();
            HitForward = new ActionProperty<HitObstacle>() { Value = HitObstacle.None };
            HitAbove = new ActionProperty<HitObstacle>() { Value = HitObstacle.None };
            HitBelow = new ActionProperty<HitObstacle>() { Value = HitObstacle.None };
            HitBelow.OnValueChanged += (sender, hitBelow) =>
            {
                if (HitBelowAction != null)
                {
                    HitBelowAction(hitBelow.Value);
                }
                if (MovementSkills != null
                    &&
                    MovementSkills.Value != null
                    &&
                    MovementSkills.Value.Swimming.Value != null
                    &&
                    MovementSkills.Value.Swimming.Value.SwimmingCurrentState.Value == SwimmingState.InWater)
                {
                    IsGrounded.Value = false;
                }
                else
                {
                    IsGrounded.Value = hitBelow.Value != HitObstacle.None;    
                }
                
            };
            IsGrounded.OnValueChanged += (sender, isGrounded) => OnIsGroundedChanged(isGrounded.Value);
            BeingPushed = new ActionProperty<bool>();
            CharacterOutOfBoundsHorizontal = new ActionProperty<bool>();
            CharacterOutOfBoundsVertical = new ActionProperty<bool>();
            MovableCurrentState = new ActionProperty<MovableState>();
            MovableCurrentState.OnValueChanged += (sender, state) => CharacterStateChanged(state.Value);
            AffectedByGravity = new ActionProperty<bool>() { Value = true };
            AffectedByPhysics = new ActionProperty<bool>() { Value = true };
            MovementSkills = new ActionProperty<MovementSkills>();
            ThrowingSkill = new ActionProperty<ThrowingSkill>();
            ThrowingSkill.OnValueChanged += (sender, throwingSkill) => OnThrowingSkillChanged(throwingSkill.Value);
            ActiveThrowableType = new ActionProperty<ThrowableWeaponType>();
            MovementSkills.OnValueChanged += (sender, movementSkills) => OnMovementSkillsChanged(movementSkills.Value);
            ActiveMovementEffects = new ActionList<MovementEffect>();
            ActiveMovementEffects.ListItemAdded += MovementEffectAdded;
            ActiveMovementEffects.ListItemRemoved += MovementEffectRemoved;     
            ThrowableWeaponStashes = new ActionList<ThrowableWeaponStash>();
            ThrowableWeaponStashes.ListItemAdded += ThrowableStashAdded;
            ThrownTrowables = new ActionList<ThrowableWeapon>();
            ThrownTrowables.ListItemAdded += ThrowableAdded;            
        }

        #region private functions
        /// <summary>
        /// List add notification. ThrowableWeaponStash was added.
        /// </summary>
        /// <param name="throwableWeaponStash">The throwable weapon stash.</param>
        private void ThrowableStashAdded(ThrowableWeaponStash throwableWeaponStash)
        {
            ActiveThrowableType.Value = throwableWeaponStash.ThrowableWeaponType;
        }

        /// <summary>
        ///List add notification. ThrowableWeapon was added.
        /// </summary>
        /// <param name="throwableWeapon">The throwable weapon.</param>
        private void ThrowableAdded(ThrowableWeapon throwableWeapon)
        {
            throwableWeapon.OwnerCombatEntity.Value = this;
        }
        
        /// <summary>
        /// Called when [throwing skill changed].
        /// Disable combat skills when throwing. Override if other logic is needed
        /// </summary>
        /// <param name="throwingSkill">The throwing skill.</param>
        protected virtual void OnThrowingSkillChanged(ThrowingSkill throwingSkill)
        {
            throwingSkill.OwnerCharacter.Value = this;
            throwingSkill.BeginThrowAction += DisableCombatSkills;
            throwingSkill.EndThrowAction += EnableCombatSkills;
            throwingSkill.ReleaseThrowableAction += (throwabletype, throwingType, throwingSpeed) => CalculateThrowablesLeft();
        }

        /// <summary>
        /// Calculates how many throwables are left.
        /// </summary>
        protected void CalculateThrowablesLeft()
        {
            var activeThrowables = ThrowableWeaponStashes.FirstOrDefault(x => x.ThrowableWeaponType == ActiveThrowableType.Value);
            if (activeThrowables == null) return;
            activeThrowables.Count.Value--;
        }

        /// <summary>
        /// Called when [is grounded changed].
        /// Notification of IsGrounded value. If in air, and not already sprinting, disallow start sprinting in air. Resets on ground.
        /// </summary>
        /// <param name="isGrounded">if set to <c>true</c> then character [is grounded].</param>
        private void OnIsGroundedChanged(bool isGrounded)
        {
            if (MovementSkills == null || MovementSkills.Value == null || MovementSkills.Value.HorizontalMovement.Value == null) return;
            if (isGrounded)
            {
                MovementSkills.Value.HorizontalMovement.Value.CanSprint.Value = true;
                if (_updateComponent != null)
                {
                    _updateComponent.OnFinishedAction();
                }
            }

            else
            {
                _updateComponent = ComponentExtensions.AddComponentOnEmptyChild<UpdateTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Grounded observer");
                _updateComponent.UpdateMethod = () =>
                {
                    if (CharacterController.Value == null) return;
                    if (!CharacterController.Value.Sprint.Value &&
                        MovementSkills.Value.HorizontalMovement.Value.CanSprint.Value)
                    {
                        MovementSkills.Value.HorizontalMovement.Value.CanSprint.Value = false;
                    }
                };
                UpdateComponents.Add(_updateComponent);
            }
        }
      
        /// <summary>
        /// Called when [movement skills changed].
        /// </summary>
        /// <param name="movementSkills">The movement skills.</param>
        protected virtual void OnMovementSkillsChanged(MovementSkills movementSkills)
        {
            if (MovementSkills.Value == null) return;
            movementSkills.OwnerCharacter.Value = this;
            // Subscribe to MovementSkills separately
            movementSkills.VerticalMovement.OnValueChanged +=
                (sender, verticalMovement) =>
                {
                    verticalMovement.Value.VerticalMovementCurrentState.OnValueChanged += (_, state) => OnVerticalMovementStateChanged(state.Value);
                };
            movementSkills.Sliding.OnValueChanged += (sender, sliding) =>
            {
                sliding.Value.SlidingCurrentState.OnValueChanged += (_, state) => OnSlidingStateChanged(state.Value);
            };
            movementSkills.Swimming.OnValueChanged += (sender, swimming) =>
            {
                swimming.Value.SwimmingCurrentState.OnValueChanged += (_, state) => OnSwimmingStateChanged(state.Value);
            };

        }

        /// <summary>
        /// Called when [vertical movement state changed].
        /// Enables and disables other skills based on VerticalMovementState. Also handles movability while airborne
        /// </summary>
        /// <param name="verticalMovementState">State of the vertical movement.</param>
        private void OnVerticalMovementStateChanged(VerticalMovementState verticalMovementState)
        {
            if (_airborneStateInterval != null)
            {
                _airborneStateInterval.FinishTimer();
                TimerComponents.Remove(_airborneStateInterval);
            }
            if (verticalMovementState == VerticalMovementState.WallJumping)
            {
                MovementSkills.Value.HorizontalMovement.Value.CanSprint.Value = false;
                MovementSkills.Value.HorizontalMovement.Value.SkillTransitionToStateDisable();

                var wallJumpingTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Wall jumping timer");
                wallJumpingTimer.TimerMethod = () =>
                {
                    DirectionLocked.Value = false;
                    MovementSkills.Value.HorizontalMovement.Value.SkillTransitionToStateEnable();
                };
                wallJumpingTimer.Invoke(0.5f);
                TimerComponents.Add(wallJumpingTimer);
            }
            if (verticalMovementState == VerticalMovementState.Landing)
            {
                if (LandedAction != null)
                {
                    LandedAction();
                }
            }
            if (verticalMovementState == VerticalMovementState.Jumping
                ||
                verticalMovementState == VerticalMovementState.Falling)
            {
                AirborneMovementReduction();
            }
            else
            {
                AirborneSpeedEffect.Value = 1f; // use normal movement

            }
        }

        /// <summary>
        /// Reduces effective movement while airborne.
        /// </summary>
        private void AirborneMovementReduction()
        {
            _airborneStateInterval = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Airborne State Interval");
            _airborneStateInterval.TimerMethod =
                () =>
                {
                    AirborneSpeedEffect.Value *= 0.975f; // gradually reduce movability while airborne
                };
            _airborneStateInterval.SetupIntervalInfinite(0.1f);
            TimerComponents.Add(_airborneStateInterval);
        }

        /// <summary>
        /// Called when [sliding state changed].
        /// Enables and disables other skills based on SlidingState. Also turns character to face slope direction
        /// </summary>
        /// <param name="slidingState">State of the sliding.</param>
        private void OnSlidingStateChanged(SlidingState slidingState)
        {
            if (slidingState == SlidingState.HelplessSliding)
            {
                if (MovementSkills.Value.HorizontalMovement.Value != null)
                {
                    MovementSkills.Value.HorizontalMovement.Value.SkillTransitionToStateDisable();
                }
                if (MovementSkills.Value.VerticalMovement.Value != null)
                {
                    MovementSkills.Value.VerticalMovement.Value.SkillTransitionToStateDisable();
                }

                DisableCombatSkills();
            }
            else
            {
                if (MovementSkills.Value.HorizontalMovement.Value != null)
                {
                    MovementSkills.Value.HorizontalMovement.Value.SkillTransitionToStateEnable();
                }
                if (MovementSkills.Value.VerticalMovement.Value != null)
                {
                    MovementSkills.Value.VerticalMovement.Value.SkillTransitionToStateEnable();
                }
                EnableCombatSkills();
            }
            if (slidingState == SlidingState.Idle
                ||
                slidingState == SlidingState.PreventSliding)
            {
                if (FinishedSlideAction != null)
                {
                    FinishedSlideAction();
                }
                DirectionLocked.Value = false;
            }
            else
            {
                if (FaceSlopeDirectionAction != null)
                {
                    FaceSlopeDirectionAction();
                }
                DirectionLocked.Value = true;
            }
            DirectionLocked.Value = false;
        }

        /// <summary>
        /// Called when [swimming state changed].
        /// Enables and disables other skills based on SwimmingState. Also resets movability while airborne when surface jumping
        /// </summary>
        /// <param name="swimmingState">State of the swimming.</param>
        protected virtual void OnSwimmingStateChanged(SwimmingState swimmingState)
        {
            if (_airborneStateInterval != null)
            {
                _airborneStateInterval.FinishTimer();
                TimerComponents.Remove(_airborneStateInterval);
            }
            if (swimmingState == SwimmingState.OutOfWater)
            {
                StandUpright();
                MovementAndJumpingDirectionUnLock();
                EnableHorizontalMovementSkill();
                EnableVerticalMovementSkill();
                EnableSlidingSkill();
                EnableThrowingSkill();
                AirborneSpeedEffect.Value = 1f; // reset to  normal movement

            }
            else if (swimmingState == SwimmingState.DoingStroke)
            {
                // dont mind this state
            }
            else
            {
                DisableHorizontalMovementSkill();
                DisableVerticalMovementSkill();
                DisableSlidingSkill();
                DisableThrowingSkill();
                if (swimmingState == SwimmingState.SurfaceJumping)
                {
                    AirborneSpeedEffect.Value = 1f; // reset to normal movement
                    AirborneMovementReduction();
                }
            }
        }

        /// <summary>
        /// Disables the throwing skill.
        /// </summary>
        protected void DisableThrowingSkill()
        {
            if (ThrowingSkill.Value != null)
            {
                ThrowingSkill.Value.SkillTransitionToStateDisable();    
            }            
        }

        /// <summary>
        /// Enables the throwing skill.
        /// </summary>
        protected void EnableThrowingSkill()
        {
            if (ThrowingSkill.Value != null)
            {
                ThrowingSkill.Value.SkillTransitionToStateEnable();
            }
        }
        #endregion

        #region public functions
        /// <summary>
        /// Set character to be affected by gravity.
        /// </summary>
        /// <param name="useGravity">if set to <c>true</c> then character [use gravity].</param>
        public void UseGravity(bool useGravity)
        {
            AffectedByGravity.Value = useGravity;
        }

        /// <summary>
        /// Set character to be affected by physics.
        /// </summary>
        /// <param name="usePhysics">if set to <c>true</c> then character [use physics].</param>
        public void UsePhysics(bool usePhysics)
        {
            AffectedByPhysics.Value = usePhysics;
        }

        /// <summary>
        /// Stands the character up.
        /// </summary>
        public void StandUpright()
        {
            if (StandUprightAction != null)
            {
                StandUprightAction();
            }
        }

        /// <summary>
        /// Rotates the character towards a direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        public void RotateTowardsDirection(Direction direction)
        {
            if (RotateTowardsDirectionAction != null)
            {
                RotateTowardsDirectionAction(direction);
            }
        }

        /// <summary>
        /// Moves character to the position.
        /// </summary>
        /// <param name="position">The position.</param>
        public void MoveToPosition(Vector3 position)
        {
            MoveToPositionAction(position);
        }

        /// <summary>
        /// Applies a movement effect if there is not a similar, stronger effect already present. Refreshes duration if same effect is present
        /// </summary>
        /// <param name="movementEffect">The movement effect.</param>
        public void ApplyMovementEffect(MovementEffect movementEffect)
        {
            var alreadyPresentSimilarEffect = ActiveMovementEffects.FirstOrDefault(x => x.StrengthType == movementEffect.StrengthType
                                                                                &&
                                                                                ((x.Strength > 0 && movementEffect.Strength > 0)
                                                                                    ||
                                                                                    (x.Strength < 0 && movementEffect.Strength < 0)));
            if (alreadyPresentSimilarEffect != null)
            {
                if (alreadyPresentSimilarEffect.Strength > movementEffect.Strength || alreadyPresentSimilarEffect.IsInfinite)
                    return;
                // if there an effect of same type already present, and its not stronger, remove it before apply.
                ActiveMovementEffects.Remove(alreadyPresentSimilarEffect);
            }
            ActiveMovementEffects.Add(movementEffect);
            if (!movementEffect.IsInfinite)
            {
                var wallJumpingTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Movement effect timer");
                wallJumpingTimer.TimerMethod = () => ActiveMovementEffects.Remove(movementEffect);
                wallJumpingTimer.Invoke(movementEffect.Duration);
                TimerComponents.Add(wallJumpingTimer);
            }

        }

        /// <summary>
        /// Removes the movement effect.
        /// </summary>
        /// <param name="movementEffect">The movement effect.</param>
        public void RemoveMovementEffect(MovementEffect movementEffect)
        {
            if (ActiveMovementEffects.Contains(movementEffect))
            {
                ActiveMovementEffects.Remove(movementEffect);    
            }
            
        }

		/// <summary>
		/// Applies the push effect.
		/// </summary>
		/// <param name="pushEffect">The push effect.</param>
		public void ApplyPushEffect(PushEffect pushEffect)
		{
			ApplyPushEffect (pushEffect, false);
		}

        /// <summary>
        /// Applies the push effect.
        /// </summary>
        /// <param name="pushEffect">The push effect.</param>
        /// <param name="hitFromBehind">if set to <c>true</c> then character is [hit from behind].</param>
        public void ApplyPushEffect(PushEffect pushEffect, bool hitFromBehind)
        {
            if (PushEffectAction != null)
            {
                PushEffectAction(pushEffect, hitFromBehind);
            }
        }

        /// <summary>
        /// Stops the movement.
        /// </summary>
        public void StopMovement()
        {
            if (StopMovementAction != null)
            {
                StopMovementAction();
            }
        }
        #endregion
        
        #region SkillEnablers
        /// <summary>
        /// Disables all movement skills.
        /// </summary>
        protected void DisableMovementSkills()
        {
            if (MovementSkills.Value == null || MovementSkills.Value == null) return;
            if (MovementSkills.Value.HorizontalMovement.Value != null)
            {
                MovementSkills.Value.HorizontalMovement.Value.SkillTransitionToStateDisable();
            }
            if (MovementSkills.Value.VerticalMovement.Value != null)
            {
                MovementSkills.Value.VerticalMovement.Value.SkillTransitionToStateDisable();
            }
            if (MovementSkills.Value.Swimming.Value != null)
            {
                MovementSkills.Value.Swimming.Value.SkillTransitionToStateDisable();
            }
            if (MovementSkills.Value.Sliding.Value != null)
            {
                MovementSkills.Value.Sliding.Value.SkillTransitionToStateDisable();
            }
        }

        /// <summary>
        /// Enables all movement skills.
        /// </summary>
        protected void EnableMovementSkills()
        {
            if (MovementSkills == null || MovementSkills.Value == null) return;
            if (MovementSkills.Value.HorizontalMovement.Value != null)
            {
                MovementSkills.Value.HorizontalMovement.Value.SkillTransitionToStateEnable();
            }
            if (MovementSkills.Value.VerticalMovement.Value != null)
            {
                MovementSkills.Value.VerticalMovement.Value.SkillTransitionToStateEnable();
            }
            if (MovementSkills.Value.Swimming.Value != null)
            {
                MovementSkills.Value.Swimming.Value.SkillTransitionToStateEnable();
            }
            if (MovementSkills.Value.Sliding.Value != null)
            {
                MovementSkills.Value.Sliding.Value.SkillTransitionToStateEnable();
            }
        }

        /// <summary>
        /// Enables horizontal & vertical movement and unlocks direction change
        /// </summary>
        protected void MovementAndJumpingDirectionUnLock()
        {
            EnableHorizontalMovementSkill();
            EnableVerticalMovementSkill();
            DirectionLocked.Value = false;
        }

        /// <summary>
        /// Disables horizontal & vertical movement and locks direction
        /// </summary>
        protected void MovementAndJumpingDirectionLock()
        {
            DisableHorizontalMovementSkill();
            DisableVerticalMovementSkill();            
            DirectionLocked.Value = true;
        }

        /// <summary>
        /// Enables the horizontal movement skill.
        /// </summary>
        protected void EnableHorizontalMovementSkill()
        {
            if (MovementSkills != null
                &&
                MovementSkills.Value != null
                &&
                MovementSkills.Value.HorizontalMovement.Value != null)
            {
                MovementSkills.Value.HorizontalMovement.Value.SkillTransitionToStateEnable();
            }
        }

        /// <summary>
        /// Disables the horizontal movement skill.
        /// </summary>
        protected void DisableHorizontalMovementSkill()
        {
            if (MovementSkills != null
                &&
                MovementSkills.Value != null
                &&
                MovementSkills.Value.HorizontalMovement.Value != null)
            {
                MovementSkills.Value.HorizontalMovement.Value.SkillTransitionToStateDisable();
            }
        }

        /// <summary>
        /// Enables the vertical movement skill.
        /// </summary>
        protected void EnableVerticalMovementSkill()
        {
            if (MovementSkills != null
                &&
                MovementSkills.Value != null
                &&
                MovementSkills.Value.VerticalMovement.Value != null)
            {
                MovementSkills.Value.VerticalMovement.Value.SkillTransitionToStateEnable();
            }
        }

        /// <summary>
        /// Disables the vertical movement skill.
        /// </summary>
        protected void DisableVerticalMovementSkill()
        {
            if (MovementSkills != null
                &&
                MovementSkills.Value != null
                &&
                MovementSkills.Value.VerticalMovement.Value != null)
            {
                MovementSkills.Value.VerticalMovement.Value.SkillTransitionToStateDisable();
            }
        }

        /// <summary>
        /// Enables the sliding skill.
        /// </summary>
        protected void EnableSlidingSkill()
        {
            if (MovementSkills != null
                &&
                MovementSkills.Value != null
                &&
                MovementSkills.Value.Sliding.Value != null)
            {
                MovementSkills.Value.Sliding.Value.SkillTransitionToStateEnable();
            }
        }

        /// <summary>
        /// Disables the sliding skill.
        /// </summary>
        protected void DisableSlidingSkill()
        {
            if (MovementSkills != null
                &&
                MovementSkills.Value != null
                &&
                MovementSkills.Value.Sliding.Value != null)
            {
                MovementSkills.Value.Sliding.Value.SkillTransitionToStateDisable();
            }
        }

        /// <summary>
        /// Enables the swimming skill.
        /// </summary>
        protected void EnableSwimmingSkill()
        {
            if (MovementSkills != null
                &&
                MovementSkills.Value != null
                &&
                MovementSkills.Value.Swimming.Value != null)
            {
                MovementSkills.Value.Swimming.Value.SkillTransitionToStateEnable();
            }
        }

        /// <summary>
        /// Disables the swimming skill.
        /// </summary>
        protected void DisableSwimmingSkill()
        {
            if (MovementSkills != null
                &&
                MovementSkills.Value != null
                &&
                MovementSkills.Value.Swimming.Value != null)
            {
                MovementSkills.Value.Swimming.Value.SkillTransitionToStateDisable();
            }
        }
        #endregion

        #region StateNotifications
        /// <summary>
        /// Called when the characters movable state changed.
        /// </summary>
        /// <param name="movableState">New movable state</param>
        public void CharacterStateChanged(MovableState movableState)
        {
            if (movableState == MovableState.Stunned)
            {
                DirectionLocked.Value = true;
                DisableMovementSkills();
            }
            else
            {
                DirectionLocked.Value = false;
                EnableMovementSkills();
            }
        }
        #endregion

        #region MovementEffects notifications
        /// <summary>
        /// List add notification. MovementEffect was added.
        /// </summary>
        /// <param name="movementEffect">The movement effect.</param>
        private void MovementEffectAdded(MovementEffect movementEffect)
        {
            if (movementEffect.EffectType == MovementEffectType.SpeedChange)
            {
                switch (movementEffect.StrengthType)
                {
                    case StatusEffectStrengthType.FixedValue:
                        FixedSpeedEffect.Value += movementEffect.Strength;
                        break;
                    case StatusEffectStrengthType.Percentage:
                        RelativeSpeedEffect.Value += movementEffect.Strength / 100f;
                        break;
                }

            }
            CalculateMovementState();
        }

        /// <summary>
        /// List add notification. MovementEffect was removed.
        /// </summary>
        /// <param name="movementEffect">The movement effect.</param>
        private void MovementEffectRemoved(MovementEffect movementEffect)
        {
            if (movementEffect.EffectType == MovementEffectType.SpeedChange)
            {
                switch (movementEffect.StrengthType)
                {
                    case StatusEffectStrengthType.FixedValue:
                        FixedSpeedEffect.Value -= movementEffect.Strength;
                        break;
                    case StatusEffectStrengthType.Percentage:
                        RelativeSpeedEffect.Value -= movementEffect.Strength / 100f;
                        break;
                }
            }
            CalculateMovementState();
        }

        /// <summary>
        /// Calculates the MovableCurrentState
        /// </summary>
        private void CalculateMovementState()
        {
            if (ActiveMovementEffects.Any(x => x.EffectType == MovementEffectType.Stun))
            {
                if (MovableCurrentState.Value != MovableState.Stunned)
                {
                    TransitionToStateStun();
                }
            }
            else
            {
                if (Mathf.Abs(Speed - (Speed + FixedSpeedEffect.Value) * RelativeSpeedEffect.Value) < 0.01f)
                {
                    if (MovableCurrentState.Value != MovableState.Normal)
                    {
                        TransitionToStateMoveNormal();
                    }
                }
                else
                {
                    if ((Speed + FixedSpeedEffect.Value) * RelativeSpeedEffect.Value > Speed)
                    {
                        if (MovableCurrentState.Value != MovableState.Hasted)
                        {
                            TransitionToStateHaste();
                        }

                    }
                    else
                    {
                        if (MovableCurrentState.Value != MovableState.Slowed)
                        {
                            TransitionToStateSlow();
                        }
                    }
                }
            }
        }
        #endregion

        #region TransitionToStates
        public virtual void TransitionToStateStun()
        {
            MovableCurrentState.Value = MovableState.Stunned;
        }

        public virtual void TransitionToStateMoveNormal()
        {
            MovableCurrentState.Value = MovableState.Normal; ;
        }
        public virtual void TransitionToStateHaste()
        {
            MovableCurrentState.Value = MovableState.Hasted;
        }

        public virtual void TransitionToStateSlow()
        {
            MovableCurrentState.Value = MovableState.Slowed;
        }
        #endregion
    }
}
