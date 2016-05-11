using System;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.MissionSystem
{
    /// <summary>
    /// Inherit from this to create a specific MissionObjectiveView
    /// </summary>
    [Serializable]
	public abstract class MissionObjectiveBaseView : ActionView
    {
		#region Public properties
        // Fields to be set in the editor
        public string Description;
        public bool IsRequired;
		#endregion

		public MissionObjective MissionObjective;
		
		
	}
}