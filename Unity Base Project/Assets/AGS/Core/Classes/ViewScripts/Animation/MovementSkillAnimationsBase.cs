using System.Linq;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterSystem;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that subscribes to MovementSkills and sets animator parameters. Override for specific characters
    /// </summary>
    public abstract class MovementSkillAnimationsBase : ViewScriptBase
    {
        // cached references
        protected Animator Animator;
        protected CharacterBaseView CharacterBaseView;
        protected CharacterBase Character;
        protected UpdateTemporaryGameObject VerticalVelocityUpdater;


        public override void Awake()
        {
            base.Awake();
            Animator = gameObject.GetComponentInChildren<Animator>();
        }

        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                CharacterBaseView = ViewReference as CharacterBaseView;
                if (CharacterBaseView == null) return;

                Character = CharacterBaseView.Character;

            }
            if (Character == null) return;

            Character.IsGrounded.OnValueChanged += (sender, isGrounded) => OnIsGroundedChanged(isGrounded.Value);
            Character.FacingGameLevelForward.OnValueChanged += (sender, isFacingForward) => OnFacingForwardChanged(isFacingForward.Value);

            Character.FixedSpeedEffect.OnValueChanged += (sender, _) => SetCharacterAnimationSpeed();
            Character.RelativeSpeedEffect.OnValueChanged += (sender, _) => SetCharacterAnimationSpeed();

            if (Character.CharacterController.Value != null)
            {
                Character.CharacterController.Value.MoveVector.OnValueChanged += (sender, moveVector) => OnAxisSpeedChanged(moveVector.Value);
            }
            if (Character.MovementSkills.Value == null) return;
            if (Character.MovementSkills.Value.HorizontalMovement.Value != null)
            {
                Character.MovementSkills.Value.HorizontalMovement.Value.HorizontalMovementCurrentState.OnValueChanged += (sender, state) => OnMovementStateChanged(state.Value);
            }
            if (Character.MovementSkills.Value.VerticalMovement.Value != null)
            {
                Character.MovementSkills.Value.VerticalMovement.Value.VerticalMovementCurrentState.OnValueChanged += (sender, state) => OnJumpingStateChanged(state.Value);
            }
            if (Character.MovementSkills.Value.Sliding.Value != null)
            {
                Character.MovementSkills.Value.Sliding.Value.SlidingCurrentState.OnValueChanged += (sender, state) => OnSlidingStateChanged(state.Value);
            }
            if (Character.MovementSkills.Value.Swimming.Value != null)
            {
                Character.MovementSkills.Value.Swimming.Value.SwimmingCurrentState.OnValueChanged += (sender, state) => OnSwimmingStateChanged(state.Value);
            }


        }

        /// <summary>
        /// Sets the character animation speed.
        /// </summary>
        protected virtual void SetCharacterAnimationSpeed()
        {
            if (Mathf.Abs(Character.Speed - (Character.Speed + Character.FixedSpeedEffect.Value) * Character.RelativeSpeedEffect.Value) < 0.01f)
            {
                Animator.speed = 1f;
            }
            else
            {
                Animator.speed = (Character.Speed + Character.FixedSpeedEffect.Value) * Character.RelativeSpeedEffect.Value / Character.Speed;
            }
        }

        /// <summary>
        /// Called when [facing forward changed].
        /// </summary>
        /// <param name="isFacingForward">if set to <c>true</c> [is facing forward].</param>
        protected virtual void OnFacingForwardChanged(bool isFacingForward)
        {
            if (Animator == null)
                return;
            Animator.SetBool("FacingForward", isFacingForward);
        }

        /// <summary>
        /// Called when [is grounded changed].
        /// </summary>
        /// <param name="grounded">if set to <c>true</c> [grounded].</param>
        protected virtual void OnIsGroundedChanged(bool grounded)
        {
            if (Animator == null)
                return;
            Animator.SetBool("Grounded", grounded);
        }

        /// <summary>
        /// Called when [axis speed changed].
        /// </summary>
        /// <param name="axisSpeed">The axis speed.</param>
        protected virtual void OnAxisSpeedChanged(Vector2 axisSpeed)
        {
            if (Animator == null)
                return;
            Animator.SetFloat("InputHorizontal", axisSpeed.x);
            Animator.SetFloat("InputVertical", axisSpeed.y);

        }

        /// <summary>
        /// Called when [sliding state changed].
        /// </summary>
        /// <param name="state">The state.</param>
        protected abstract void OnSlidingStateChanged(SlidingState state);

        /// <summary>
        /// Called when [swimming state changed].
        /// </summary>
        /// <param name="state">The state.</param>
        protected abstract void OnSwimmingStateChanged(SwimmingState state);

        /// <summary>
        /// Called when [jumping state changed].
        /// </summary>
        /// <param name="state">The state.</param>
        protected abstract void OnJumpingStateChanged(VerticalMovementState state);

        /// <summary>
        /// Called when [movement state changed].
        /// </summary>
        /// <param name="state">The state.</param>
        protected abstract void OnMovementStateChanged(HorizontalMovementState state);
    }
}
