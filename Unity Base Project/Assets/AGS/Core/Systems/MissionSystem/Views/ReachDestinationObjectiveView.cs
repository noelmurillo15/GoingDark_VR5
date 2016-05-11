using System;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;

namespace AGS.Core.Systems.MissionSystem
{
    /// <summary>
    /// A MissionObjectiveView that is completed when Player trigger its collider
    /// </summary>
    public class ReachDestinationObjectiveView : MissionObjectiveBaseView
    {
        #region AGS Setup
        public override void InitializeView()
        {
            MissionObjective = new MissionObjective(Description, IsRequired);
            SolveModelDependencies(MissionObjective);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            // Set up a trigger listener with PlayerBaseView
            Action<PlayerBaseView> addPlayerConnectAction = playerView => MissionObjective.Complete();
            gameObject.OnTriggerActionEnterWith(addPlayerConnectAction);
        }
        #endregion

    }
}
