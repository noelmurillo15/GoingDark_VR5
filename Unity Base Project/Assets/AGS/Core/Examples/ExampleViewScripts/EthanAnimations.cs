using System;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Classes.ViewScripts;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterSystem;
using UnityEngine;

namespace AGS.Core.Examples.ExampleViewScripts
{
    /// <summary>
    /// EthanAnimations ViewScript for Ethan controller
    /// </summary>
    public class EthanAnimations : ViewScriptBase
    {
        public float JumpLegInterval = 0.5f;

        // cached references
        private Animator _animator;
        private CharacterBaseView _characterBaseView;
        private CharacterBase _character;
        private UpdateTemporaryGameObject _verticalVelocityUpdater;
        private float _jumpLegVal = 1;
        private float _axisSpeedX;
        public override void Awake()
        {
            base.Awake();
            _animator = gameObject.GetComponentInChildren<Animator>();
        }

        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                _characterBaseView = ViewReference as CharacterBaseView;
                if (_characterBaseView == null) return;

                _character = _characterBaseView.Character;

            }
            if (_character == null) return;

            if (_character.CharacterController.Value != null)
            {
                _character.CharacterController.Value.MoveVector.OnValueChanged += (sender, moveVector) => OnAxisSpeedChanged(moveVector.Value);
            }
            
            if (_character.MovementSkills.Value == null) return;
            if (_character.MovementSkills.Value.HorizontalMovement.Value != null)
            {
                _character.MovementSkills.Value.HorizontalMovement.Value.HorizontalMovementCurrentState.OnValueChanged += (sender, state) => OnMovementStateChanged(state.Value);
            }
            if (_character.MovementSkills.Value.VerticalMovement.Value != null)
            {
                _character.MovementSkills.Value.VerticalMovement.Value.VerticalMovementCurrentState.OnValueChanged += (sender, state) => OnJumpingStateChanged(state.Value);
            }

            var jumpLegVal = ComponentExtensions.AddComponentOnEmptyChild<TimerPersistantGameObject>(gameObject, "JumpLeg updater");
            jumpLegVal.TimerMethod = () =>
            {
                _jumpLegVal *= -1;
                if (_character.IsGrounded.Value)
                {
                    _animator.SetFloat("JumpLeg", _jumpLegVal * _animator.GetFloat("Forward"));
                }
            };
            jumpLegVal.SetupIntervalInfinite(JumpLegInterval);
        }

       /// <summary>
        /// Called when [axis speed changed].
        /// </summary>
        /// <param name="axisSpeed">The axis speed.</param>
        private void OnAxisSpeedChanged(Vector2 axisSpeed)
        {
            if (_animator == null)
                return;

            _axisSpeedX = Mathf.Lerp(_axisSpeedX, axisSpeed.x, 0.25f); // For smooth turning
           if (Math.Abs(_axisSpeedX) < 0.01f)
           {
               _axisSpeedX = 0f;
           }
           _animator.SetFloat("Turn", _axisSpeedX);
            _animator.SetFloat("Forward", axisSpeed.y);

        }

        /// <summary>
        /// Called when [movement state changed].
        /// </summary>
        /// <param name="state">The state.</param>
        private void OnMovementStateChanged(HorizontalMovementState state)
        {
            if (_animator == null)
                return;
            _animator.SetBool("Crouch", state == HorizontalMovementState.Crouching);            
        }

        /// <summary>
        /// Called when [jumping state changed].
        /// </summary>
        /// <param name="state">The state.</param>
        private void OnJumpingStateChanged(VerticalMovementState state)
        {
            if (_verticalVelocityUpdater != null)
            {
                _verticalVelocityUpdater.Stop();
            }
            if (_animator == null)
                return;
            if (state == VerticalMovementState.Idle)
            {
                _animator.SetBool("OnGround", true);
            }
            if (state == VerticalMovementState.Landing)
            {
                _animator.SetFloat("Jump", 0);
                _animator.SetBool("OnGround", true);
            }
            if (state == VerticalMovementState.Jumping || state == VerticalMovementState.Falling)
            {
                _animator.SetBool("OnGround", false);
                _verticalVelocityUpdater = ComponentExtensions.AddComponentOnEmptyChild<UpdateTemporaryGameObject>(gameObject, "Vertical velocity updater");
                _verticalVelocityUpdater.UpdateMethod = () =>
                {
                    if (_characterBaseView.GetComponent<CharacterController>() != null)
                    {
                        _animator.SetFloat("Jump", _characterBaseView.GetComponent<CharacterController>().velocity.y);
                    }
                };
            }
        }
    }
}
