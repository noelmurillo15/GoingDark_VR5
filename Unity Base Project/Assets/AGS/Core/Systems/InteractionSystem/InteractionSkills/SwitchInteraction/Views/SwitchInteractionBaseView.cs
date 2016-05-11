using System;
using AGS.Core.Base;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.InteractionSystem.Base;
using AGS.Core.Systems.InteractionSystem.Interactables;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.InteractionSkills.SwitchInteraction
{
    /// <summary>
    /// SwitchInteractionBaseView requires a mecanim animator and is responsible for setting the animator in the correct state based on the models current state.
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class SwitchInteractionBaseView : InteractionSkillBaseView
    {
        public SwitchInteraction SwitchInteraction;
        private Animator _animator;
        private float _smoothness;
        private Vector3 _targetPosition;
        #region AGS Setup
        public override void InitializeView()
        {
            SwitchInteraction = new SwitchInteraction(ApproachMargin, OffsetVertical, OffsetHorizontal);
            SolveModelDependencies(SwitchInteraction);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            SwitchInteraction.IsEnabled.OnValueChanged += (sender, isEnabled) => OnSkillStateChanged(isEnabled.Value);
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
        /// Called when [current state changed].
        /// Sets mecanim animator in the correct state
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void OnCurrentStateChanged(SwitchInteractionState currentState)
        {
            /* SwitchInteractionStates 
             * Idle == 0
             * Approaching == 1
             * Switching == 2 */
            _animator.SetInteger("SwitchInteractionState", (int)currentState);
        }

        /// <summary>
        /// Called when [state enter idle].
        /// </summary>
        public override void OnStateEnterIdle()
        {
            OwnerCharacter.UsePhysics(true);
        }

        /// <summary>
        /// Called when [state enter approaching].
        /// </summary>
        public override void OnStateEnterApproaching()
        {
            OwnerCharacter.UsePhysics(false);
            _smoothness = 3f * Time.deltaTime;
            // calculate offsets
            _targetPosition = GetTargetPosition();
        }

        /// <summary>
        /// Called when [state update approaching].
        /// </summary>
        public override void OnStateUpdateApproaching()
        {

            // Move towards target until approach margin is reached
            if (CurrentInteractableTarget != null)
            {
                MoveTowardsInteractableTarget(_targetPosition, _smoothness);

            }
            if (ReachedTargetPosition(_targetPosition, SwitchInteraction.ApproachMargin))
            {
                SwitchInteraction.TransitionToStateInteract();
            }
        }

        /// <summary>
        /// Called when [state enter releasing].
        /// </summary>
        public override void OnStateEnterReleasing()
        {
            // Do nothing
        }

        /// <summary>
        /// Called when [state enter switching].
        /// </summary>
        public virtual void OnStateEnterSwitching()
        {
            var switchElement = CurrentInteractableTarget as Switch;
            if (switchElement == null)
            {
                // If CurrentInteractableTarget is not a Switch, something weird has happened. Transition to release.
                SwitchInteraction.TransitionToStateRelease();
                return;
            }

            // Activate the swith, then set up a timer for release based of its seconds switching prop.
            switchElement.Activate(!switchElement.On.Value);

            var releaseTimer = Classes.MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Release timer");
            releaseTimer.TimerMethod = () => SwitchInteraction.TransitionToStateRelease();
            releaseTimer.Invoke(switchElement.SecondsSwitching);            
        }
        #endregion
        #region private functions
        /// <summary>
        /// Called when [skill state changed].
        /// Only subscribe to SwitchInteractionCurrentState if skill is enabled
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void OnSkillStateChanged(bool isEnabled)
        {
            if (isEnabled)
            {
                SwitchInteraction.SwitchInteractionCurrentState.OnValueChanged += (sender, state) => OnCurrentStateChanged(state.Value);
            }
            else
            {
                SwitchInteraction.SwitchInteractionCurrentState.OnValueChanged -= OnSwitchInteractionDisabled;
            }
        }

        /// <summary>
        /// Called when [switch interaction disabled].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="state">The <see cref="ActionPropertyEventArgs{T}"/> instance containing the event data.</param>
        private void OnSwitchInteractionDisabled(object sender, ActionPropertyEventArgs<SwitchInteractionState> state)
        {
            OnCurrentStateChanged(state.Value);
        }

        #endregion
    }
}