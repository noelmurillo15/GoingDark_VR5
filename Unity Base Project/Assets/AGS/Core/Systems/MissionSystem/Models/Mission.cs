using System.Linq;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.MissionSystem
{
    /// <summary>
    /// Missions are based on its objectives, and game level states can be set to fail or success depending on MissionCurrentState
    /// </summary>
    public class Mission : ActionModel
    {
        #region Properties
        // Constructor properties
        public string Description { get; private set; } 

        // Subscribable properties
        public ActionProperty<MissionState> MissionCurrentState { get; private set; } // Current mission state
        public ActionList<MissionObjective> Objectives { get; private set; } // All objectives linked to this mission.
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="Mission"/> class.
        /// </summary>
        /// <param name="description">Description of this mission.</param>
        public Mission(string description)
        {
            Description = description;
            MissionCurrentState = new ActionProperty<MissionState>();
            Objectives = new ActionList<MissionObjective>();
            Objectives.ListItemAdded += ObjectiveAdded;
        }

        #region private functions        
        /// <summary>
        /// ListItem notification. MissionObjective was added.
        /// </summary>
        /// <param name="objectiveAdd">The objective add.</param>
        private void ObjectiveAdded(MissionObjective objectiveAdd)
        {
            // Set up subsriptions for objectives Completed and Failed booleans
            objectiveAdd.Completed.OnValueChanged += (sender, x) => CheckMissionCompleted();
            objectiveAdd.Failed.OnValueChanged += (sender, x) =>
            {
                if (objectiveAdd.IsRequired && x.Value) TransitionToStateFail(); // Instant mission fail for failed required objectives
            };
        }

        /// <summary>
        /// Checks if the mission is completed.
        /// </summary>
        private void CheckMissionCompleted()
        {
            if (Objectives.Where(x => x.IsRequired).All(x => x.Completed.Value))
            {
                TransitionToStateComplete(); // Mission success when all objectives are completed
            }
        }

        #endregion

        #region TransitionToStates
        /// <summary>
        /// Transitions to state complete.
        /// </summary>
        public virtual void TransitionToStateComplete()
        {
            if (MissionCurrentState.Value == MissionState.Active)
            {
                MissionCurrentState.Value = MissionState.Completed;
            }

        }

        /// <summary>
        /// Transitions to state fail.
        /// </summary>
        public virtual void TransitionToStateFail()
        {
            if (MissionCurrentState.Value == MissionState.Active)
            {
                MissionCurrentState.Value = MissionState.Failed;
            }
        }

        /// <summary>
        /// Transitions to state reset.
        /// </summary>
        public virtual void TransitionToStateReset()
        {
            if (MissionCurrentState.Value == MissionState.Failed)
            {
                MissionCurrentState.Value = MissionState.Active;
            }

        }
        #endregion
    }
}
