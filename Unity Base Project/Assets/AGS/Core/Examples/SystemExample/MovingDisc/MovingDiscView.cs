using System;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Classes.TimerComponents;
using UnityEngine;

namespace AGS.Core.Examples.SystemExample
{
    [Serializable]
    public class MovingDiscView : ActionView
    {
        public MovingDisc MovingDisc;
        private Animator _animator;

        private TimerPersistantGameObject _forwardCountDownTimer;
        private TimerPersistantGameObject _backwardsCountDownTimer;

        private bool _goingForward;
        private float _forwardCountDown;
        private float _backwardsCountDown;

        #region AGS Setup
        public override void InitializeView()
        {
            MovingDisc = new MovingDisc();
            SolveModelDependencies(MovingDisc);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            MovingDisc.CurrentDirection.OnValueChanged += (sender, currentState) =>
            {

                OnStateChanged(currentState.Value);
            };
            GoForward();
        }

public override void Awake(){
    base.Awake();
    _animator = GetComponent<Animator>();
}
        private void OnStateChanged(MovingDiscDirection value)
        {
            _animator.SetInteger("MovingDiscState", (int)value);
        }

        public void GoForward()
        {
            _goingForward = true;
            MovingDisc.TransitionToStateForward();
        }

        public void GoBackward()
        {
            _goingForward = false;
            MovingDisc.TransitionToStateBackward();
        }

        public void Stop()
        {
            MovingDisc.TransitionToStateStop();
        }

        public void Continue()
        {
            if (_goingForward)
            {
                GoForward();
            }
            else
            {
                GoBackward();
            }
        }

        public void OnStateEnterForward()
        {
            if (_forwardCountDownTimer == null)
            {
                _forwardCountDownTimer = ComponentExtensions.AddComponent<TimerPersistantGameObject>(gameObject);
                _forwardCountDownTimer.TimerMethod = () =>
                {
                    if (_forwardCountDown <= 0)
                    {
                        _forwardCountDown = 20f;
                        GoBackward();
                    }
                    _forwardCountDown--;
                };
                _forwardCountDownTimer.SetupIntervalInfinite(0.1f);
            }
            else
            {
                _forwardCountDownTimer.StartInterval();
            }
        }

        public void OnStateUpdateForward()
        {
            transform.Translate(transform.forward * Time.deltaTime * 5f);
        }

        public void OnStateExitForward()
        {
            if (_forwardCountDownTimer != null)
            {
                _forwardCountDownTimer.StopInterval();
            }
        }

        public void OnStateEnterBackward()
        {
            if (_backwardsCountDownTimer == null)
            {
                _backwardsCountDownTimer = ComponentExtensions.AddComponent<TimerPersistantGameObject>(gameObject);
                _backwardsCountDownTimer.TimerMethod = () =>
                {
                    if (_backwardsCountDown <= 0)
                    {
                        _backwardsCountDown = 20f;
                        GoForward();
                    }
                    _backwardsCountDown--;
                };
                _backwardsCountDownTimer.SetupIntervalInfinite(0.1f);
            }
            else
            {
                _backwardsCountDownTimer.StartInterval();
            }
        }
        public void OnStateUpdateBackward()
        {
            transform.Translate(-transform.forward * Time.deltaTime * 5f);
        }
        public void OnStateExitBackward()
        {
            if (_backwardsCountDownTimer != null)
            {
                _backwardsCountDownTimer.StopInterval();
            }
        }

        #endregion
    }
}