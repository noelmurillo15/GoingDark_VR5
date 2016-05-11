using System;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.MissionSystem
{
    /// <summary>
    /// Mission view. Dependent on its MissionObjectiveViews
    /// </summary>
    [Serializable]
    public class MissionView : ActionView
    {
        #region Public properties
        // Field to be set in the editor
        public string Description;

        public Transform ObjectivesContainer; // Attach MissionObjectiveViews on separate child GameObjects to this Transform
        #endregion

        public Mission Mission;

        #region AGS Setup
        public override void InitializeView()
        {
            Mission = new Mission(Description);
            SolveModelDependencies(Mission);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            if (ObjectivesContainer != null)
            {
                foreach (var objectiveBaseView in ObjectivesContainer.GetComponentsInChildren<MissionObjectiveBaseView>())
                {
                    Mission.Objectives.Add(objectiveBaseView.MissionObjective);
                }
            }

        }
        #endregion
    }
}