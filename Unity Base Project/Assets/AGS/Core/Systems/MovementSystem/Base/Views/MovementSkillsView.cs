using System;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.MovementSystem.MovementSkills.HorizontalMovement;
using AGS.Core.Systems.MovementSystem.MovementSkills.Sliding;
using AGS.Core.Systems.MovementSystem.MovementSkills.Swimming;
using AGS.Core.Systems.MovementSystem.MovementSkills.VerticalMovement;

namespace AGS.Core.Systems.MovementSystem.Base
{
    /// <summary>
    /// This view owns all movement skill views for a character
    /// </summary>
    [Serializable]
    public class MovementSkillsView : ActionView
    {
        #region Public properties
        // Set references to movement skills in the editor
        public HorizontalMovementBaseView HorizontalMovementBaseView;
        public VerticalMovementBaseView VerticalMovementBaseView;
        public SwimmingBaseView SwimmingBaseView;
        public SlidingBaseView SlidingBaseView;
        #endregion

        public MovementSkills MovementSkills;

        #region AGS Setup
        public override void InitializeView()
        {
            MovementSkills = new MovementSkills();
            SolveModelDependencies(MovementSkills);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            MovementSkills.HorizontalMovement.Value = HorizontalMovementBaseView != null ? HorizontalMovementBaseView.HorizontalMovement : null;
            MovementSkills.VerticalMovement.Value = VerticalMovementBaseView != null ? VerticalMovementBaseView.VerticalMovement : null;
            MovementSkills.Swimming.Value = SwimmingBaseView != null ? SwimmingBaseView.Swimming : null;
            MovementSkills.Sliding.Value = SlidingBaseView != null ? SlidingBaseView.Sliding : null;
        }
        #endregion
    }
}