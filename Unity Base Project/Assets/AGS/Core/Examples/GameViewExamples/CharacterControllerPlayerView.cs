using AGS.Core.Classes.DataClasses;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Examples.GameViewExamples
{
    /// <summary>
    /// CharacterControllerPlayerView.
    /// </summary>
    public class CharacterControllerPlayerView : PlayerBaseView
    {
        // cached references
        private CharacterController _characterController;
        private float _verticalSpeed;
        private float _currentSpeed;
        private bool _didJump;
        // timers
        private FixedUpdateTemporaryGameObject _standUpUpdater;

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            if (Player.MovementSkills.Value != null)
            {
                if (Player.MovementSkills.Value.HorizontalMovement.Value != null)
                {
                    Player.MovementSkills.Value.HorizontalMovement.Value.CurrentSpeed.OnValueChanged +=
                        (sender, currentSpeed) => _currentSpeed = currentSpeed.Value;

                }
                if (Player.MovementSkills.Value.VerticalMovement.Value != null)
                {
                    Player.MovementSkills.Value.VerticalMovement.Value.VerticalMovementCurrentState.OnValueChanged +=
                        (sender, currentState) =>
                        {
                            _didJump = currentState.Value == VerticalMovementState.Jumping;
                        };

                }
            }
        }

        #region MonoBehaviour
        public override void Awake()
        {
            base.Awake();
            _characterController = GetComponent<CharacterController>();
        }

        public override void Update()
        {
            base.Update();

            TurnInInputDirection();

            var moveVector = _currentSpeed * transform.TransformDirection(Vector3.forward);
            moveVector.y = CalculateVerticalSpeed();
            _characterController.Move(moveVector * Time.deltaTime);
        }
        #endregion
        
        #region private functions
        /// <summary>
        /// Turns in the direction.
        /// </summary>
        private void TurnInInputDirection()
        {
            if (Character.CharacterController == null || Character.CharacterController.Value == null) return;
            var effectiveTurnSpeed = Character.TurnSpeed * Character.RelativeSpeedEffect.Value;
            effectiveTurnSpeed = effectiveTurnSpeed + Character.FixedSpeedEffect.Value;
            transform.Rotate(0f, effectiveTurnSpeed * Character.CharacterController.Value.MoveVector.Value.x, 0f);
        }

        /// <summary>
        /// Calculates the vertical speed.
        /// </summary>
        private float CalculateVerticalSpeed()
        {
            if (_didJump)
            {
                _verticalSpeed = Player.MovementSkills.Value.VerticalMovement.Value.CurrentJumpSpeed.Value;
            }
            else
            {
                _verticalSpeed -= -Physics.gravity.y * Time.deltaTime;
            }
            return _verticalSpeed;
        }
        #endregion

        #region member overrides
        /// <summary>
        /// Gets the current velocity.
        /// </summary>
        /// <returns></returns>
        protected override Vector3 GetCurrentVelocity()
        {
            return _characterController.velocity;
        }

        /// <summary>
        /// Turns the character around 180 degrees.
        /// </summary>
        protected override void TurnAround()
        {
            transform.eulerAngles = Character.FacingGameLevelForward.Value ? new Vector3(transform.eulerAngles.x, 270f, transform.eulerAngles.z) : new Vector3(transform.eulerAngles.x, 90f, transform.eulerAngles.z);
        }


        /// <summary>
        /// Rotates character in the specified direction
        /// </summary>
        /// <param name="direction">The direction.</param>
        protected override void RotateTowardsDirectionNotification(Direction direction)
        {

        }

        /// <summary>
        /// Turns the character in the given direction.
        /// </summary>
        /// <param name="degrees"></param>
        protected override void TurnInDirection(Vector3 degrees)
        {
            
        }

        /// <summary>
        /// Checks if the CombatEntity is facing forward.
        /// </summary>
        /// <returns></returns>
        protected override bool CheckFacingGameLevelForward()
        {
            return Mathf.Abs(transform.eulerAngles.y) < 5f;
        }

        /// <summary>
        /// Check if character is moving backwards.
        /// </summary>
        /// <returns></returns>
        protected override bool CheckMovingBackwards()
        {
            if (Character.CharacterController.Value == null) return false;
            return Character.FacingGameLevelForward.Value && Character.CharacterController.Value.MoveVector.Value.y < -0.1f
                    ||
                    !Character.FacingGameLevelForward.Value && Character.CharacterController.Value.MoveVector.Value.y > 0.1f;
        }

        /// <summary>
        /// Checks if the character is on moving ground.
        /// </summary>
        /// <param name="hitObstacle">The hit obstacle.</param>
        protected override void CheckOnMovingGround(HitObstacle hitObstacle)
        {
            // Not implemented
        }


        /// <summary>
        /// Gets the height of the character.
        /// </summary>
        /// <returns></returns>
        protected override float GetCharacterHeight()
        {
            return _characterController.height;
        }

        /// <summary>
        /// Sets the height of the character.
        /// </summary>
        /// <param name="height">The height.</param>
        protected override void SetCharacterHeight(float height)
        {
            if (_characterController != null)
            {
                _characterController.height = height;
            }
            else
            {
                Debug.LogError("CharacterController is not set");
            }
        }

        /// <summary>
        /// Gets the character height center.
        /// </summary>
        /// <returns></returns>
        protected override Vector3 GetCharacterHeightCenter()
        {
            return _characterController.center;
        }

        /// <summary>
        /// Sets the character height center.
        /// </summary>
        /// <param name="center">The center.</param>
        protected override void SetCharacterHeightCenter(Vector3 center)
        {
            if (_characterController != null)
            {
                _characterController.center = center;
            }
            else
            {
                Debug.LogError("CharacterController is not set");
            }
        }

        /// <summary>
        /// Gets the character radius.
        /// </summary>
        /// <returns></returns>
        protected override float GetCharacterRadius()
        {
            return _characterController.radius;
        }

        /// <summary>
        /// Sets the character radius.
        /// </summary>
        /// <param name="radius">The radius.</param>
        protected override void SetCharacterRadius(float radius)
        {
            if (_characterController != null)
            {
                _characterController.radius = radius;
            }
            else
            {
                Debug.LogError("CharacterController is not set");
            }
        }

        protected override void FaceSlopeDirection()
        {
            // Not implemented
        }

        /// <summary>
        /// Called when the characters lands after being airborne
        /// </summary>
        protected override void LandedNotification()
        {
            // Not implemented
        }

        /// <summary>
        /// Stops the characters movement.
        /// </summary>
        protected override void StopMovement()
        {
            // Not implemented
        }

        /// <summary>
        /// Triggers the push effect.
        /// </summary>
        /// <param name="pushForce">The push force.</param>
        /// <param name="forceMode">The force mode.</param>
        protected override void TriggerPushEffect(Vector3 pushForce, ForceMode forceMode)
        {
            _characterController.Move(pushForce);
        }

        /// <summary>
        /// Sets UseGravity.
        /// </summary>
        /// <param name="useGravity">if set to <c>true</c> then character [use gravity].</param>
        protected override void SetGravity(bool useGravity)
        {
            // Not implemented
        }

        /// <summary>
        /// Sets UsePhysics.
        /// </summary>
        /// <param name="usePhysics">if set to <c>true</c> then character [use physics].</param>
        protected override void SetPhysics(bool usePhysics)
        {
            // Not implemented
        }

        /// <summary>
        /// Stands the character up.
        /// </summary>
        protected override void StandUpright()
        {
            var standUprightRotation = Quaternion.Euler(0f, Character.FacingGameLevelForward.Value ? 0 : 180f, 0f);

            _standUpUpdater = ComponentExtensions.AddComponentOnEmptyChild<FixedUpdateTemporaryGameObject>(gameObject, "Stand up updater");
            _standUpUpdater.UpdateMethod = () =>
            {
                if (transform == null) return;
                if (_standUpUpdater != null && Mathf.Abs(transform.rotation.eulerAngles.x) < 0.01f)
                {
                    _standUpUpdater.Stop();
                }
                transform.rotation = Quaternion.RotateTowards(transform.rotation, standUprightRotation, 20f);
            };
        }

        /// <summary>
        /// Gets the current character motion.
        /// </summary>
        /// <returns></returns>
        protected override MotionData GetCurrentCharacterMotion()
        {
            return new MotionData
            {
                Velocity = _characterController.velocity,
            };
        }

        /// <summary>
        /// Gets the effective proximity modifier calculated from this Killable to a Transform.
        /// Get promiximity to an AreaOfEffects SphereCollider center
        /// </summary>
        /// <param name="effectOrigin">The effect origin.</param>
        /// <returns></returns>
        protected override float GetProximityModifier(Transform effectOrigin)
        {
            var effectArea = effectOrigin.parent.parent.GetComponentInChildren<AreaOfEffectView>().GetComponentInChildren<SphereCollider>(); // path to collider of the AreaOfEffectView
            if (effectArea == null) return 1; // Could not find effect area, return full efffect
            if (_characterController == null) return 0; // Cant find collider, return zero effect

            var closestPointOnColliderToAreaEffect = _characterController.ClosestPointOnBounds(effectOrigin.position);

            var distanceToOrigin = (effectOrigin.position - closestPointOnColliderToAreaEffect).magnitude;
            var relativeEffect = 1 - (distanceToOrigin / effectArea.radius);
            return relativeEffect < 0 ? 0f : relativeEffect; // In case relative effect is negative (i.e outside of sphere), return zero efffect
        }

        #endregion
    }
}
