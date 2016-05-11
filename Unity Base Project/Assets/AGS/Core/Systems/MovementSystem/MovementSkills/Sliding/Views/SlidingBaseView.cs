using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.MovementSystem.Base;
using UnityEngine;

namespace AGS.Core.Systems.MovementSystem.MovementSkills.Sliding
{
    /// <summary>
    /// SlidingBaseView requires a mecanim animator and is responsible for setting the animator in the correct state based on the models current state.
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class SlidingBaseView : MovementSkillBaseView
    {
        #region Public properties
        // Fields to be set in the editor
        public float SlidingSpeed;
        public float SlopeLimitNaturalSliding;
        public float SlopeLimitHelplessSliding;
        public float MaxManualSlidingSpeed;
        public float MaxHelplessSlidingSpeed;
        #endregion

        public Sliding Sliding;

        private UpdatePersistantGameObject _slidingIntentionUpdater; // For use when updating the characters intention
        private Animator _animator;

        #region AGS Setup
        public override void InitializeView()
        {
            Sliding = new Sliding(SlidingSpeed, SlopeLimitNaturalSliding, SlopeLimitHelplessSliding, MaxManualSlidingSpeed, MaxHelplessSlidingSpeed);
            SolveModelDependencies(Sliding);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            Sliding.ResourceCost.Value = ResourceEffectCostCombo != null ? ResourceEffectCostCombo.ResourceEffectCombo : null;
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            Sliding.IsEnabled.OnValueChanged += (sender, isEnabled) => OnSkillStateChanged(isEnabled.Value);
        }
        #endregion

        #region MonoBehaviours
        public override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }
        #endregion

        #region private functions
        /// <summary>
        /// Called when [skill state changed].
        /// Only subscribe to SlidingCurrentState and only run the intention update method if skill is enabled
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void OnSkillStateChanged(bool isEnabled)
        {
            if (isEnabled)
            {
                _slidingIntentionUpdater = ComponentExtensions.SetupComponent<UpdatePersistantGameObject>(gameObject);
                _slidingIntentionUpdater.UpdateMethod = () =>
                {
                    Sliding.Intention.Value = SetSlidingStateIntention();
                };
                Sliding.SlidingCurrentState.OnValueChanged += (sender, state) => OnCurrentStateChanged(state.Value);
            }
            else
            {
                if (_slidingIntentionUpdater != null)
                {
                    _slidingIntentionUpdater.Stop();
                }
                Sliding.SlidingCurrentState.OnValueChanged -= OnSlidingDisabled;
            }
        }

        /// <summary>
        /// Called when [sliding disabled].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="state">The <see cref="ActionPropertyEventArgs{T}"/> instance containing the event data.</param>
        private void OnSlidingDisabled(object sender, ActionPropertyEventArgs<SlidingState> state)
        {
            OnCurrentStateChanged(state.Value);
        }
        #endregion

        #region State machine functions
        /// <summary>
        /// Sets the sliding state intention.
        /// Intention input should always be implemented in the child view since controls differs from game to game.
        /// </summary>
        /// <returns></returns>
        protected abstract SlidingIntention SetSlidingStateIntention();

        /// <summary>
        /// Called when [current state changed].
        /// Sets mecanim animator in the correct state
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void OnCurrentStateChanged(SlidingState currentState)
        {
            /* SlidingStates 
             * Idle == 0
             * ManualSliding == 1
             * NaturalSliding == 2
             * HelplessSliding == 3
             * PreventSliding == 4 */
            _animator.SetInteger("SlidingState", (int)currentState);
        }

        /// <summary>
        /// Called when [state enter idle].
        /// </summary>
        public virtual void OnStateEnterIdle()
        {

        }

        /// <summary>
        /// Called when [state enter prevent sliding].
        /// </summary>
        public virtual void OnStateEnterPreventSliding()
        {

        }

        /// <summary>
        /// Called when [state update prevent sliding].
        /// </summary>
        public virtual void OnStateUpdatePreventSliding()
        {

        }

        /// <summary>
        /// Called when [state enter manual sliding].
        /// </summary>
        public virtual void OnStateEnterManualSliding()
        {

        }

        /// <summary>
        /// Called when [state update manual sliding].
        /// </summary>
        public virtual void OnStateUpdateManualSliding()
        {

        }

        /// <summary>
        /// Called when [state enter natural sliding].
        /// </summary>
        public virtual void OnStateEnterNaturalSliding()
        {

        }

        /// <summary>
        /// Called when [state update natural sliding].
        /// </summary>
        public virtual void OnStateUpdateNaturalSliding()
        {

        }
        
        /// <summary>
        /// Called when [state enter helpless sliding].
        /// </summary>
        public virtual void OnStateEnterHelplessSliding()
        {

        }

        /// <summary>
        /// Called when [state update helpless sliding].
        /// </summary>
        public virtual void OnStateUpdateHelplessSliding()
        {

        }
        #endregion

        #region abstract functions
        /// <summary>
        /// Reduces the sliding speed.
        /// Use when max sliding speed is reached. Dampens speed a little to keep sliding speed around max without jitter
        /// </summary>
        public abstract void ReduceSlidingSpeed();

        /// <summary>
        /// Determines if character has reached the maximum sliding speed.
        /// </summary>
        /// <param name="maxSpeed">The maximum speed.</param>
        /// <returns></returns>
        public abstract bool ReachedMaxSlidingSpeed(float maxSpeed);
        #endregion
    }
}