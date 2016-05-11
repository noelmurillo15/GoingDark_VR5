using System;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.HazardSystem
{
    /// <summary>
    /// This view does not inherit from HazardBaseView since it does not handle collision detection (which is handled by AreaOfEffectView)
    /// Like HazardBaseView it requires a mecanim animator and is responsible for setting the animator in the correct state based on the models current state.
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public class AreaHazardView : ActionView
    {
        #region Public properties
        // Fields to be set in the editor
        public float SecondsBetweenTicks;
        public float SecondsActive;
        public float SecondsRecharging;
        public bool DeactivateOnTrigger;
        public bool DestroyOnTrigger;

        // References to be set in the editor
        public StatusEffectComboView StatusEffectComboView;
        public AreaOfEffectView AreaOfEffectView;
        #endregion

        public AreaHazard AreaHazard;
        private Animator _animator;
        protected TimerTemporaryGameObject _effectsInterval;
        private TimerTemporaryGameObject _activeStateCountDownTimer;
        private TimerTemporaryGameObject _rechargingStateCountDownTimer;
        #region AGS Setup
        public override void InitializeView()
        {
            AreaHazard = new AreaHazard(SecondsActive, SecondsRecharging, SecondsBetweenTicks, DeactivateOnTrigger, DestroyOnTrigger, transform);
            SolveModelDependencies(AreaHazard);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            if (AreaHazard == null)
            {
                AreaHazard = model as AreaHazard;

            }
            if (AreaHazard == null) return;            
            if (AreaHazard != null && StatusEffectComboView != null)
            {
                AreaHazard.EffectsCombo.Value = StatusEffectComboView.StatusEffectCombo;
            }
            if (AreaHazard != null && AreaOfEffectView != null)
            {
                AreaHazard.AreaOfEffect.Value = AreaOfEffectView.AreaOfEffect;
            }

        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            if (AreaHazard != null)
            {
                AreaHazard.HazardCurrentState.OnValueChanged += (sender, state) => OnCurrentStateChanged(state.Value);
                AreaHazard.TransitionToStateActivate();
            }
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
        private void OnCurrentStateChanged(HazardState currentState)
        {
            _animator.SetInteger("HazardState", (int)currentState);
        }

        /// <summary>
        /// Called when [state enter active].
        /// </summary>
        public virtual void OnStateEnterActive()
        {
            // Setup a time interval for when to hit targets             
            if (AreaHazard.SecondsBetweenTicks > 0)
            {
                _effectsInterval = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Active State Interval");
                _effectsInterval.TimerMethod = () =>
                {
                    AreaHazard.ApplyEffectsToTargets();
                    AreaHazard.ApplyEffectsToRagdolls();
                };
                _effectsInterval.SetupIntervalInfinite(AreaHazard.SecondsBetweenTicks);
            }
            else if (AreaHazard.EffectsCombo.Value != null)
            {
                Debug.LogError("Must set SecondsBetweenTicks for AreaHazards with status effects");
                return;
            }

            // do not toggle to deactivated if SecondsActive == 0
            if (AreaHazard.SecondsActive > 0)
            {
                _activeStateCountDownTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Active State CountDown Timer");
                _activeStateCountDownTimer.TimerMethod = () => AreaHazard.TransitionToStateRecharge();
                _activeStateCountDownTimer.Invoke(AreaHazard.SecondsActive);
            }
        }

        /// <summary>
        /// Called when [state exit active].
        /// </summary>
        public virtual void OnStateExitActive()
        {
            // Stop interval and countdown
            if (_effectsInterval != null)
            {
                _effectsInterval.FinishTimer();
            }
            if (_activeStateCountDownTimer != null)
            {
                _activeStateCountDownTimer.FinishTimer();
            }
        }

        /// <summary>
        /// Called when [state enter recharging].
        /// </summary>
        public virtual void OnStateEnterRecharging()
        {
            // Start count down for transitioning back to active
            _rechargingStateCountDownTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Recharging State CountDown Timer");
            _rechargingStateCountDownTimer.TimerMethod = () => AreaHazard.TransitionToStateActivate();
            _rechargingStateCountDownTimer.Invoke(AreaHazard.SecondsRecharging);
        }

        /// <summary>
        /// Called when [state exit recharging].
        /// </summary>
        public virtual void OnStateExitRecharging()
        {
            if (_rechargingStateCountDownTimer != null)
            {
                _rechargingStateCountDownTimer.FinishTimer();
            }
        }

        /// <summary>
        /// Called when [state enter inactive].
        /// </summary>
        public virtual void OnStateEnterInactive()
        {
            AreaHazard.AreaOfEffect.Value.KillableTargets.Clear();
            AreaHazard.AreaOfEffect.Value.RagdollTargets.Clear();
        }
        #endregion
    }
}