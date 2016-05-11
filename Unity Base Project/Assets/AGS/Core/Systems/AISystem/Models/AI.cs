using System.Linq;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterControlSystem;
using AGS.Core.Systems.MovingEnvironmentSystem;
using UnityEngine;

namespace AGS.Core.Systems.AISystem
{
    /// <summary>
    /// AI is a character controller driven that should be driven by AI logic.
    /// In addition to normal movement and action, AI can also patrol EnvironmentPaths.
    /// The AiCurrentState holds the current state set by the AI logic
    /// </summary>
    public class AI : CharacterControllerBase
    {

        #region Properties
        // Constructor properties
        public float PatrolSpeed { get; set; }
        public float SecondsPuzzled { get; set; }
        public float PatrolPointRadius { get; set; }

        // Subscribable properties
        public ActionProperty<EnvironmentPathDirection> PatrolDirection { get; private set; } 
        public ActionProperty<EnvironmentPath> PatrolPath { get; private set; } // Needed for AI state patrolling
        public ActionProperty<EnvironmentPathPoint> CurrentPatrolPoint { get; private set; } // Current target patrol point on the path
        public ActionProperty<AIStateMachineState> AICurrentState { get; private set; } // The state of the AI
        public ActionProperty<AIStateIntention> Intention { get; private set; } // The current intention of the AI
        public ActionList<DetectionVolumeBase> DetectionVolumes { get; private set; } // All owned detection volumes
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="AI"/> class.
        /// </summary>
        /// <param name="patrolSpeed">The patrol speed.</param>
        /// <param name="patrolPointRadius">Determines how close to CurrentPatrolPoint the AI need to come before seeking next</param>
        /// <param name="pathDirection">AI's direction on the set patrol path</param>
        /// <param name="secondsPuzzled">Determines how long the AI is puzzled after losing player detection</param>
        public AI(float patrolSpeed, float patrolPointRadius, EnvironmentPathDirection pathDirection, float secondsPuzzled)
        {
            PatrolSpeed = patrolSpeed;
            PatrolPointRadius = patrolPointRadius;
            SecondsPuzzled = secondsPuzzled;
            PatrolDirection = new ActionProperty<EnvironmentPathDirection>() { Value = pathDirection };
            CurrentPatrolPoint = new ActionProperty<EnvironmentPathPoint>();
            PatrolPath = new ActionProperty<EnvironmentPath>();
            PatrolPath.OnValueChanged += (sender, patrolPath) =>
            {
                if (patrolPath.Value != null)
                {
                    OnPatrolPathChanged(patrolPath.Value);
                }
            };

            AICurrentState = new ActionProperty<AIStateMachineState>();
            Intention = new ActionProperty<AIStateIntention>();
            Intention.OnValueChanged += (sender, aiState) => SetAIState(aiState.Value);
            DetectionVolumes = new ActionList<DetectionVolumeBase>();
            DetectionVolumes.ListItemAdded += DetectionVolumeAdded;
        }

        #region private functions
        private void OnPatrolPathChanged(EnvironmentPath patrolPath)
        {
            if (CurrentPatrolPoint.Value != null) return;

            // check for first environment path point
            if (patrolPath.Points.Any())
            {
                SetStartingPatrolPoint(patrolPath.Points.First());
            }
            else
            {
                patrolPath.Points.ListItemAdded += SetStartingPatrolPoint;
            }

        }

        private void SetStartingPatrolPoint(EnvironmentPathPoint environmentPathPoint)
        {
            CurrentPatrolPoint.Value = environmentPathPoint;
        }

        private void DetectionVolumeAdded(DetectionVolumeBase detectionVolumeAdd)
        {
            detectionVolumeAdd.OwnerAI.Value = this;
        }

        private void SetAIState(AIStateIntention intention)
        {
            switch (intention)
            {
                case AIStateIntention.None:
                    TransitionToStateIdle();
                    break;
                case AIStateIntention.Patrol:
                    TransitionToStatePatrol();
                    break;
                case AIStateIntention.Chase:
                    TransitionToStateChase();
                    break;
                case AIStateIntention.Attack:
                    TransitionToStateAttack();
                    break;
            }
        }

        #endregion

        #region state transitions
        /// <summary>
        /// Transitions to state idle.
        /// </summary>
        public void TransitionToStateIdle()
        {
            AICurrentState.Value = AIStateMachineState.Idle;
        }

        /// <summary>
        /// Transitions to state patrol.
        /// </summary>
        public void TransitionToStatePatrol()
        {
            if (AICurrentState.Value == AIStateMachineState.Idle
                ||
                 AICurrentState.Value == AIStateMachineState.Puzzled)
            {
                AICurrentState.Value = AIStateMachineState.Patrolling;
            }
        }

        /// <summary>
        /// Transitions to state chase.
        /// </summary>
        public void TransitionToStateChase()
        {
            AICurrentState.Value = AIStateMachineState.Chasing;
        }

        /// <summary>
        /// Transitions to state attack.
        /// </summary>
        public void TransitionToStateAttack()
        {
            AICurrentState.Value = AIStateMachineState.Attacking;
        }

        /// <summary>
        /// Transitions to state puzzled.
        /// </summary>
        public void TransitionToStatePuzzled()
        {
            if (AICurrentState.Value == AIStateMachineState.Chasing
                ||
                 AICurrentState.Value == AIStateMachineState.Attacking)
            {
                AICurrentState.Value = AIStateMachineState.Puzzled;
            }
        }
        #endregion
        #region public functions
        
        /// <summary>
        /// Call this when a patrol point is reached.
        /// Sets the next patrol point
        /// </summary>
        public void PatrolPointReached()
        {
            CurrentPatrolPoint.Value = PatrolPath.Value.GetNextPoint(PatrolDirection, CurrentPatrolPoint.Value);
        }

        /// <summary>
        /// Call this if the target player was killed. Sets all detection volumes IsDetectingPlayer to false
        /// </summary>
        public void KilledPlayer()
        {
            foreach (var detectionVolumeBase in DetectionVolumes)
            {
                detectionVolumeBase.IsDetectingPlayer.Value = false;
            }
        }

        /// <summary>
        /// Stops the AI from moving.
        /// </summary>
        public void Stop()
        {
            MoveVector.Value = new Vector2(0, 0);
        }


        /// <summary>
        /// Stops the AI from attacking.
        /// </summary>
        public void StopAttack()
        {
            Fire1.Value = false;
            Fire2.Value = false;
            Attack1.Value = false;
            Attack2.Value = false;
            Attack3.Value = false;
        }
        #endregion


    }
}
