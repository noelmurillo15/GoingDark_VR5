using AGS.Core.Enums;
using AGS.Core.Systems.MovementSystem.MovementSkills.HorizontalMovement;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that subscribes to MovementSkills state
    /// </summary>
    public class MovementSkillsFX : ViewScriptBase
    {
        protected HorizontalMovementBaseView HorizontalMovementBaseView;
        protected HorizontalMovement HorizontalMovement;



        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                HorizontalMovementBaseView = ViewReference as HorizontalMovementBaseView;
                if (HorizontalMovementBaseView == null) return;

                HorizontalMovement = HorizontalMovementBaseView.HorizontalMovement;

            }
            if (HorizontalMovement == null) return;
            HorizontalMovement.HorizontalMovementCurrentState.OnValueChanged += (sender, state) => OnHorizontalMovementStateChanged(state.Value);

        }

        /// <summary>
        /// Called when [horizontal movement state changed].
        /// </summary>
        /// <param name="state">The state.</param>
        protected virtual void OnHorizontalMovementStateChanged(HorizontalMovementState state){}

    }
}
