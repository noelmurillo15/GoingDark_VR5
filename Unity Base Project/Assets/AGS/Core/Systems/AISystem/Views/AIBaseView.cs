using System;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterControlSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.MovingEnvironmentSystem;
using UnityEngine;

namespace AGS.Core.Systems.AISystem
{
    /// <summary>
    /// AIBaseView requires a mecanim animator and is responsible for setting the animator in the correct state based on AI's current state.
    /// Instances of this class need to implement SetAIStateIntention() which is called each Update,
    /// as well as FaceTarget() and MoveAICharacterTowardsTarget(float speed, Vector3 target) which are called from the AI's state machine behaviours
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class AIBaseView : CharacterControllerBaseView
    {
        #region Public properties
        public float PatrolSpeed;
        public float PatrolPointRadius;
        public EnvironmentPathDirection PathDirection;
        public float SecondsPuzzled;

        // References to be set in the editor
        public EnvironmentPathView PatrolPathView;
        public EnvironmentPathPointView StartingPatrolPointView;
        public Transform DetectionVolumesContainer;
        #endregion

        public AI AI;
        public KillableBase StaticAICharacter;
        public CharacterBase MovableAICharacter;
        private Animator _animator;

        /// <summary>
        /// Gets a value indicating whether [ai and target is valid].
        /// </summary>
        /// <value>
        /// <c>true</c> if [ai and target is valid]; otherwise, <c>false</c>.
        /// </value>
        public bool AIAndTargetIsValid
        {
            get
            {
                return MovableAICharacter != null
                       &&
                       MovableAICharacter.Transform != null
                       &&
                       MovableAICharacter.Target.Value != null
                       &&
                       MovableAICharacter.Target.Value.Transform != null;
            }
        }

        #region AGS Setup
        public override void InitializeView()
        {
            AI = new AI(PatrolSpeed, PatrolPointRadius, PathDirection, SecondsPuzzled);
            SolveModelDependencies(AI);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            if (PatrolPathView != null)
            {
                AI.PatrolPath.Value = PatrolPathView.EnvironmentPath;
            }
            if (StartingPatrolPointView != null)
            {
                AI.CurrentPatrolPoint.Value = StartingPatrolPointView.EnvironmentPathPoint;
            }
            if (DetectionVolumesContainer != null)
            {
                foreach (var detectionVolumeView in DetectionVolumesContainer.GetComponentsInChildren<DetectionVolumeBaseView>())
                {
                    AI.DetectionVolumes.Add(detectionVolumeView.DetectionVolumeBase);
                }
            }
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            // Set cached references to static and movable AI's
            StaticAICharacter = AI.OwnerCombatEntity.Value;
            MovableAICharacter = StaticAICharacter as CharacterBase;
            // Subscribe to AI's current state
            AI.AICurrentState.OnValueChanged += (sender, aiState) => OnCurrentStateChanged(aiState.Value);
        }
        #endregion

        #region MonoBehaviours

        public override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        public override void Update()
        {
            base.Update();
            AI.Intention.Value = SetAIStateIntention();
        }
        #endregion

        #region State machine functions
        /// <summary>
        /// Sets the ai state intention.
        /// </summary>
        /// <returns></returns>
        protected abstract AIStateIntention SetAIStateIntention();

        /// <summary>
        /// Called when [current state changed].
        /// Sets mecanim animator in the correct state
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void OnCurrentStateChanged(AIStateMachineState currentState)
        {
            /* State ID's
             * Idle == 0
             * Patrolling == 1
             * Chasing == 2
             * Attacking == 3
             * Puzzled == 4 */
            _animator.SetInteger("AIStateMachineState", (int)currentState);
        }

        /// <summary>
        /// Called when [state enter idle].
        /// </summary>
        public virtual void OnStateEnterIdle() {}

        /// <summary>
        /// Called when [state enter patrolling].
        /// </summary>
        public virtual void OnStateEnterPatrolling() { }

        /// <summary>
        /// Called when [state update patrolling].
        /// </summary>
        public virtual void OnStateUpdatePatrolling() { }

        /// <summary>
        /// Called when [state enter chasing].
        /// </summary>
        public virtual void OnStateEnterChasing() { }

        /// <summary>
        /// Called when [state update chasing].
        /// </summary>
        public virtual void OnStateUpdateChasing() { }

        /// <summary>
        /// Called when [state enter attacking].
        /// </summary>
        public virtual void OnStateEnterAttacking() { }

        /// <summary>
        /// Called when [state update attacking].
        /// </summary>
        public virtual void OnStateUpdateAttacking() { }

        /// <summary>
        /// Called when [state enter puzzled].
        /// </summary>
        public virtual void OnStateEnterPuzzled() { }
        #endregion

        #region abstract functions

        #endregion
    }
}