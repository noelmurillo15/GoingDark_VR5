using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.MovingEnvironmentSystem
{
    /// <summary>
    /// MovingEnvironmentBaseView requires a mecanim animator and is responsible for setting the animator in the correct state based on MovingEnvironment's current state.
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class MovingEnvironmentBaseView : ActionView
    {
        #region Public properties
        public int Speed;
        public float TargetPointRadius;
        public MovingEnvironmentMovementType MovementType;
        public EnvironmentPathDirection Direction;

        // References to be set in the editor
        public EnvironmentPathView EnvironmentPathView;
        public EnvironmentPathPointView CurrentTargetView;
        #endregion

        public MovingEnvironment MovingEnvironment;
        private Animator _animator;

        #region AGS Setup
        public override void InitializeView()
        {
            MovingEnvironment = new MovingEnvironment(Speed, TargetPointRadius, MovementType, Direction);
            SolveModelDependencies(MovingEnvironment);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            if (EnvironmentPathView != null)
            {
                MovingEnvironment.EnvironmentPath.Value = EnvironmentPathView.EnvironmentPath;
            }
            if (CurrentTargetView != null)
            {
                MovingEnvironment.CurrentTarget = CurrentTargetView.EnvironmentPathPoint;
            }
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            MovingEnvironment.MovingEnvironmentCurrentState.OnValueChanged += OnCurrentStateChanged;
            StartMoving();
        }

        #endregion

        #region MonoBehaviours
        public override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }
        #endregion

        #region public functions
        /// <summary>
        /// Starts moving the environment.
        /// </summary>
        public void StartMoving()
        {
            MovingEnvironment.TransitionToStateMove();
        }

        /// <summary>
        /// Stops moving the environment.
        /// </summary>
        public void StopMoving()
        {
            MovingEnvironment.TransitionToStateStop();
        }
        #endregion

        #region State machine functions
        /// <summary>
        /// Called when [current state changed].
        /// Sets mecanim animator in the correct state
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="currentState">The <see cref="ActionPropertyEventArgs{T}"/> instance containing the event data.</param>
        private void OnCurrentStateChanged(object sender, ActionPropertyEventArgs<MovingEnvironmentState> currentState)
        {
            switch (currentState.Value)
            {
                case MovingEnvironmentState.Idle:
                    _animator.SetBool("Move", false);
                    break;
                case MovingEnvironmentState.Moving:
                    _animator.SetBool("Move", true);
                    break;
            }
        }

        /// <summary>
        /// Called when [state update move].
        /// </summary>
        public virtual void OnStateUpdateMove()
        {
            if (MovingEnvironment.CurrentTarget == null)
                return;

            switch (MovingEnvironment.MovementType)
            {
                case MovingEnvironmentMovementType.Normal:
                    MovePositionNormal();
                    break;
                case MovingEnvironmentMovementType.Lerp:
                    MovePositionLerp();
                    break;
                case MovingEnvironmentMovementType.Slerp:
                    MovePositionSlerp();
                    break;
            }

            var sqrLen = SqrLenToTarget();
            if (sqrLen < MovingEnvironment.TargetPointRadius * MovingEnvironment.TargetPointRadius)
            {
                MovingEnvironment.SetNextTarget();
            }
        }
        #endregion

        #region abstract functions
        /// <summary>
        /// Move linearly towards next target point.
        /// </summary>
        public abstract void MovePositionNormal();

        /// <summary>
        /// Move towards next target point by lerping.
        /// </summary>
        public abstract void MovePositionLerp();

        /// <summary>
        /// Move towards next target point by slerping.
        /// </summary>
        public abstract void MovePositionSlerp();

        /// <summary>
        /// Calculates the square length to the current target point
        /// </summary>
        /// <returns></returns>
        public abstract float SqrLenToTarget();
        #endregion

    }
}
