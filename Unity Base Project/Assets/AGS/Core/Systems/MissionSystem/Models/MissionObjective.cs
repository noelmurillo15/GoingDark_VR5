using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.MissionSystem
{
    /// <summary>
    /// Objectives should not be used on their own, but rather be part of a Mission.
    /// </summary>
    public class MissionObjective : ActionModel
    {
        #region Properties
        // Constructor properties
        public string Description { get; private set; }
        public bool IsRequired { get; private set; }

        // Subscribable properties
        public ActionProperty<bool> Completed { get; private set; }
        public ActionProperty<bool> Failed { get; private set; }


        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="MissionObjective"/> class.
        /// </summary>
        /// <param name="description">Description of this objective.</param>
        /// <param name="isRequired">Determines if this objective is optional</param>
        public MissionObjective(string description, bool isRequired)
        {
            Description = description;
            IsRequired = isRequired;
            Completed = new ActionProperty<bool>();
            Failed = new ActionProperty<bool>();
            
        }

        #region public functions
        /// <summary>
        /// Completes this objective.
        /// </summary>
        public void Complete()
        {
            Completed.Value = true;
        }

        /// <summary>
        /// Fails this objective.
        /// </summary>
        public void Fail()
        {
            Failed.Value = true;
        }
        #endregion
    }
}
