using System;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;

namespace AGS.Core.Examples.SystemExample
{
    /// <summary>
    /// PressurePlateView example view
    /// </summary>
    [Serializable]
    public class PressurePlateView : ActionView
    {
        public PressurePlate PressurePlate;

        #region AGS Setup
        public override void InitializeView()
        {
            PressurePlate = new PressurePlate();
            SolveModelDependencies(PressurePlate);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            // Set up an action delegate for on trigger enter action with a player
            Action<PlayerBaseView> pressureOnAction = playerView => PressurePlate.TriggerMechanism(true);
            gameObject.OnTriggerActionEnterWith(pressureOnAction);

            // Set up another action delegate for on trigger exit action with a player
            Action<PlayerBaseView> pressureOffAction = playerView => PressurePlate.TriggerMechanism(false);
            gameObject.OnTriggerActionExitWith(pressureOffAction);
        }	
        #endregion
    }
}