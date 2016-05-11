using System;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.InteractionSystem.InteractionSkills.LadderClimbing;
using AGS.Core.Systems.InteractionSystem.InteractionSkills.LedgeClimbing;
using AGS.Core.Systems.InteractionSystem.InteractionSkills.ObjectMovement;
using AGS.Core.Systems.InteractionSystem.InteractionSkills.Swinging;
using AGS.Core.Systems.InteractionSystem.InteractionSkills.SwitchInteraction;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.Base
{
    /// <summary>
    /// This view owns all InteractionSkillViews and InteractionVolumeViews for a character
    /// </summary>
    [Serializable]
    public class InteractionSkillsView : ActionView
    {
        #region Public properties
        // Set references to interaction skills in the editor
        public LadderClimbingBaseView LadderClimbingBaseView;
        public LedgeClimbingBaseView LedgeClimbingBaseView;
        public ObjectMovementBaseView ObjectMovementBaseView;
        public SwingingBaseView SwingingBaseView;
        public SwitchInteractionBaseView SwitchInteractionBaseView;

        public Transform InteractionVolumesContainer; // Put any InteractionVolumeViews on separate child GameObjects to this transform
        #endregion

        public InteractionSkills InteractionSkills;

        #region AGS Setup
        public override void InitializeView()
        {
            InteractionSkills = new InteractionSkills();
            SolveModelDependencies(InteractionSkills);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            InteractionSkills.LadderClimbing.Value = LadderClimbingBaseView != null ? LadderClimbingBaseView.LadderClimbing : null;
            InteractionSkills.LedgeClimbing.Value = LedgeClimbingBaseView != null ? LedgeClimbingBaseView.LedgeClimbing : null;
            InteractionSkills.ObjectMovement.Value = ObjectMovementBaseView != null ? ObjectMovementBaseView.ObjectMovement : null;
            InteractionSkills.Swinging.Value = SwingingBaseView != null ? SwingingBaseView.Swinging : null;
            InteractionSkills.SwitchInteraction.Value = SwitchInteractionBaseView != null ? SwitchInteractionBaseView.SwitchInteraction : null;
            if (InteractionVolumesContainer != null)
            {
                foreach (var interactionVolumeView in InteractionVolumesContainer.GetComponentsInChildren<InteractionVolumeBaseView>())
                {
                    InteractionSkills.InteractionVolumes.Add(interactionVolumeView.InteractionVolume);
                }
            }
        }
        #endregion
    }
}