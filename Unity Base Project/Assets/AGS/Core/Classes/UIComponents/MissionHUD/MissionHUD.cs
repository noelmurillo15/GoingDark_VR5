using AGS.Core.Systems.MissionSystem;
using UnityEngine;
using UnityEngine.UI;

namespace AGS.Core.Classes.UIComponents
{
    /// <summary>
    /// This UI component shows current Mission status
    /// </summary>
    public class MissionHUD : UIScriptBase
    {
        public Text MissionDescription;
        public RectTransform[] ObjectivesRectTransforms;
        private int _objectiveIndex;
        public override void Awake()
        {
            base.Awake();
            if (MissionDescription == null)
            {
                MissionDescription = GetComponent<Text>();    
            }
            
        }

        protected override void SetupModelBindings()
        {
            base.SetupModelBindings();
            if (GameLevel.Mission != null)
            {
                MissionDescription.text = GameLevel.Mission.Value.Description;
                foreach (var missionObjective in GameLevel.Mission.Value.Objectives)
                {
                    HandleMissionObjective(GameLevel.Mission.Value, missionObjective);
                }
            }

            if (GameLevel.Mission != null)
            {
                GameLevel.Mission.OnValueChanged += (sender, mission) => OnMissionChanged(mission.Value);
            }
        }

        /// <summary>
        /// Called when [mission changed].
        /// </summary>
        /// <param name="mission">The mission.</param>
        private void OnMissionChanged(Mission mission)
        {
            if (mission == null) return;
            MissionDescription.text = mission.Description;

            mission.Objectives.ListItemAdded += objective => HandleMissionObjective(mission, objective);
        }

        /// <summary>
        /// Handles the mission objective.
        /// </summary>
        /// <param name="mission">The mission.</param>
        /// <param name="missionObjective">The mission objective.</param>
        private void HandleMissionObjective(Mission mission, MissionObjective missionObjective)
        {
            ObjectivesRectTransforms[_objectiveIndex].GetComponent<CanvasGroup>().alpha = 1;
            ObjectivesRectTransforms[_objectiveIndex].FindChild("Description").GetComponent<Text>().text = missionObjective.Description;
            missionObjective.Completed.OnValueChanged += (sender, completed) =>
                {
                    var rect = ObjectivesRectTransforms[mission.Objectives.IndexOf(missionObjective)];
                    rect.GetComponentInChildren<Text>().text = completed.Value ? "√" : "";
                };
            _objectiveIndex++;
        }
    }
}
