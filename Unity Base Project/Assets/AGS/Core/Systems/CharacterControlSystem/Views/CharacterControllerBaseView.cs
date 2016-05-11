using System;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.CharacterControlSystem
{
    /// <summary>
    /// Base view for CharacterControllerViews. Only holds a reference to the CharacterController.
    /// </summary>
    [Serializable]
	public abstract class CharacterControllerBaseView : ActionView {

        public CharacterControllerBase CharacterController;

        #region AGS Setup
        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            CharacterController = model as CharacterControllerBase;
        }
        #endregion
    }
}