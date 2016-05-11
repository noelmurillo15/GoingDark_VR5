using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.InteractionSystem.Base;
using AGS.Core.Systems.InteractionSystem.Interactables;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.InteractionSkills.Swinging
{
    /// <summary>
    /// SwingingBaseView requires a mecanim animator and is responsible for setting the animator in the correct state based on the models current state.
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class SwingingBaseView : InteractionSkillBaseView
    {
        #region Public properties
        // Fields to be set in the editor
        public float ClimbSpeed;
        public float ExitMargin;
        public Vector3 JumpSpeed;
        #endregion

        public bool JustGrabbedSwing { get; set; }
        private UpdatePersistantGameObject _swingingIntentionUpdater;
        public Swinging Swinging;
        private Animator _animator;
        private float _smoothness;

        /// <summary>
        /// Convenience property. Gets the current swing target.
        /// </summary>
        /// <value>
        /// The current swing target.
        /// </value>
        public SwingUnit CurSwingTarget
        {
            get { return CurrentInteractableTarget as SwingUnit; }
        }

        #region AGS Setup
        public override void InitializeView()
        {
            Swinging = new Swinging(ApproachMargin, OffsetVertical, OffsetHorizontal, ClimbSpeed, ExitMargin, JumpSpeed);
            SolveModelDependencies(Swinging);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            Swinging.IsEnabled.OnValueChanged += (sender, isEnabled) => OnSkillStateChanged(isEnabled.Value);
        }
        #endregion
        #region MonoBehaviours
        public override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }
        #endregion
        #region State machine functions
        /// <summary>
        /// Sets the swinging state intention.
        /// Intention input should always be implemented in the child view since controls differs from game to game.
        /// </summary>
        /// <returns></returns>
        protected abstract SwingingStateIntention SetSwingingStateIntention();

        /// <summary>
        /// Called when [current state changed].
        /// Sets mecanim animator in the correct state
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void OnCurrentStateChanged(SwingingState currentState)
        {
            /* SwingingStates 
             * Idle == 0
             * Approaching == 1
             * Swinging == 2 
             * Climbing == 3
             * StopSwinging == 4
             * Releasing == 5
             * Jumping == 6 */
            _animator.SetInteger("SwingingState", (int)currentState);
        }

        /// <summary>
        /// Called when [state enter idle].
        /// </summary>
        public override void OnStateEnterIdle()
        {
            // Do nothing
        }

        /// <summary>
        /// Called when [state enter approaching].
        /// </summary>
        public override void OnStateEnterApproaching()
        {
            if (CurSwingTarget == null)
            {
                return;
            }
            OwnerCharacter.UsePhysics(false);
            _smoothness = 10f;
        }

        /// <summary>
        /// Called when [state update approaching].
        /// </summary>
        public override void OnStateUpdateApproaching()
        {
            if (CurSwingTarget == null || CurSwingTarget.Transform == null)
            {
                // Something went wrong. SwingUnit is not available. Release.
                Swinging.TransitionToStateRelease();
                return;
            }
            // Move towards target until approach margin is reached
            var targetPosition = GetTargetPosition();
            MoveTowardsInteractableTarget(targetPosition, _smoothness);
            if (ReachedTargetPosition(targetPosition, Swinging.ApproachMargin))
            {
                JustGrabbedSwing = true;
                Swinging.TransitionToStateInteract();
            }
        }

        /// <summary>
        /// Called when [state enter releasing].
        /// </summary>
        public override void OnStateEnterReleasing()
        {
            // Release that connection that holds the character to the swing unit and transition to idle
            ReleaseGrip();
            ResetCharacter();
            var resetToIdleTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Reset to idle timer");
            resetToIdleTimer.TimerMethod = () => Swinging.TransitionToStateIdle();
            resetToIdleTimer.Invoke(0.1f);
        }

        /// <summary>
        /// Called when [state enter climbing].
        /// </summary>
        public virtual void OnStateEnterClimbing()
        {
            OwnerCharacter.UsePhysics(false);
            // Release that connection that holds the character to the swing unit
            ReleaseGrip();
            _smoothness = Swinging.ClimbSpeed * Time.deltaTime;
        }

        /// <summary>
        /// Called when [state update climbing].
        /// </summary>
        public virtual void OnStateUpdateClimbing()
        {
            if (Swinging.OnEndOfSwing.Value && (OwnerCharacterController.Direction.Value == Direction.Down
                                               ||
                                               OwnerCharacterController.Direction.Value == Direction.DownLeft
                                               ||
                                               OwnerCharacterController.Direction.Value == Direction.DownRight))
            {
                // If character is on end of swing and and direction is downward, release
                Swinging.TransitionToStateRelease();
                return;
            }
            if ((OwnerCharacterController.Direction.Value == Direction.Up && Swinging.OnTopOfSwing.Value)
                ||
                OwnerCharacterController.Direction.Value == Direction.Up && OwnerCharacter.HitAbove.Value != HitObstacle.None)
            {
                // If direction is up and the character is on top of swing or there is some obstacle above the character, do nothing
                return;
            }
            // Past all checks, climb the swing
            ClimbSwing(_smoothness);
        }


        /// <summary>
        /// Called when [state enter swinging].
        /// </summary>
        public virtual void OnStateEnterSwinging()
        {
            // Sets up a connection that holds the character to a swing unit
            AttachToSwing();
            OwnerCharacter.UsePhysics(true);
            // Tell the swing to start swinging
            CurSwingTarget.OwnerSwing.Value.LetSwing();
            // Timer used for make sure that swingintentions are delayed a short while after grabbing a swing
            var justGrabbedTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Just grabbed timer");
            justGrabbedTimer.TimerMethod = () => JustGrabbedSwing = false;
            justGrabbedTimer.Invoke(0.25f);
        }


        /// <summary>
        /// Called when [state update swinging].
        /// </summary>
        public virtual void OnStateUpdateSwinging()
        {
            if (JustGrabbedSwing) return;
            if (!IsConnectedToSwing())
            {
                // May have lost to the connection if for example using break force setting on a joint
                Swinging.TransitionToStateRelease();
            }

        }

        /// <summary>
        /// Called when [state enter stop swinging].
        /// </summary>
        public virtual void OnStateEnterStopSwinging()
        {
            // Tell the swing to break to a halt
            CurSwingTarget.OwnerSwing.Value.StopSwing();
        }

        /// <summary>
        /// Called when [state update stop swinging].
        /// </summary>
        public virtual void OnStateUpdateStopSwinging()
        {
            if (!IsConnectedToSwing())
            {
                // May have lost to the connection if for example using break force setting on a joint
                Swinging.TransitionToStateRelease();
            }
        }

        /// <summary>
        /// Called when [state enter jumping].
        /// </summary>
        public virtual void OnStateEnterJumping()
        {
            OwnerCharacter.AirborneSpeedEffect.Value = 1f; // reset MovementMultiplier
            // Release that connection that holds the character to the swing unit and then jump
            ReleaseGrip();
            OwnerCharacter.UseGravity(true);
            OwnerCharacter.UsePhysics(true);
            
            
            var jumpTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Jump timer");
            jumpTimer.TimerMethod = () =>
            {
                //_rigidbody.velocity = CurSwingTarget.Transform.GetComponent<Rigidbody>().velocity;
                DoJump();
                ResetCharacter();
                //Swinging.TransitionToStateIdle();
            };
            jumpTimer.Invoke(0.0f);
            var resetToIdleTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Reset to idle timer");
            resetToIdleTimer.TimerMethod = () => Swinging.TransitionToStateIdle();
            resetToIdleTimer.Invoke(0.3f);
        }
        #endregion
        #region private functions
        /// <summary>
        /// Called when [skill state changed].
        /// Only subscribe to SwingingCurrentState and only run the intention update method if skill is enabled.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void OnSkillStateChanged(bool isEnabled)
        {
            if (isEnabled)
            {
                _swingingIntentionUpdater = ComponentExtensions.SetupComponent<UpdatePersistantGameObject>(gameObject);
                _swingingIntentionUpdater.UpdateMethod = () =>
                {
                    Swinging.Intention.Value = SetSwingingStateIntention();
                };
                Swinging.SwingingCurrentState.OnValueChanged += (sender, state) => OnCurrentStateChanged(state.Value);
            }
            else
            {
                if (_swingingIntentionUpdater != null)
                {
                    _swingingIntentionUpdater.Stop();
                }
                Swinging.SwingingCurrentState.OnValueChanged -= OnSwingingDisabled;
            }
        }

        /// <summary>
        /// Called when [swinging disabled].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="state">The <see cref="ActionPropertyEventArgs{T}"/> instance containing the event data.</param>
        private void OnSwingingDisabled(object sender, ActionPropertyEventArgs<SwingingState> state)
        {
            OnCurrentStateChanged(state.Value);
        }

        #endregion

        #region abstract functions
        /// <summary>
        /// Attaches character to swing.
        /// </summary>
        public abstract void AttachToSwing();


        /// <summary>
        /// Resets the character.
        /// </summary>
        public abstract void ResetCharacter();

        /// <summary>
        /// Releases the grip on the swing.
        /// </summary>
        public abstract void ReleaseGrip();

        /// <summary>
        /// Makes character climb the swing.
        /// </summary>
        /// <param name="smoothness">The smoothness.</param>
        public abstract void ClimbSwing(float smoothness);


        /// <summary>
        /// Does the jump.
        /// </summary>
        public abstract void DoJump();

        /// <summary>
        /// Determines whether character [is connected to swing].
        /// </summary>
        /// <returns></returns>
        public abstract bool IsConnectedToSwing();
        #endregion
    }
}