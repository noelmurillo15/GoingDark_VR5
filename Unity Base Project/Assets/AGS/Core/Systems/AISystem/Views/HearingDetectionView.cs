using System;
using AGS.Core.Classes.ViewScripts;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Classes.MonoExtensions;

namespace AGS.Core.Systems.AISystem
{
    /// <summary>
    /// HearingDetectionView "listens" for collisions with a PlayerNoiseVolume
    /// </summary>
    [Serializable]
    public class HearingDetectionView : DetectionVolumeBaseView
    {
        public HearingDetection HearingDetection;

        #region AGS Setup
        public override void InitializeView()
        {
            HearingDetection = new HearingDetection();
            SolveModelDependencies(HearingDetection);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            Action<PlayerNoiseVolume> playerDetectionStayAction = CheckPlayerDetection;
            gameObject.OnTriggerActionStayWith(playerDetectionStayAction);

            Action<PlayerNoiseVolume> playerDetectionExitAction = _ => { HearingDetection.IsDetectingPlayer.Value = false; };
            gameObject.OnTriggerActionExitWith(playerDetectionExitAction);
        }
        #endregion

        #region private functions

        /// <summary>
        /// Determines the IsDetectingPlayer boolean
        /// </summary>
        /// <param name="noiseVolumeView">The noise volume view.</param>
        private void CheckPlayerDetection(PlayerNoiseVolume noiseVolumeView)
        {
            if (HearingDetection.OwnerAI.Value == null
                ||
                HearingDetection.OwnerAI.Value.OwnerCombatEntity.Value == null)
            {
                return;
            }
            if (noiseVolumeView.Player == null) return; // player is not initialized yet
            if (!HearingDetection.IsDetectingPlayer.Value)
            {
                HearingDetection.IsDetectingPlayer.Value = true;
            }


            if (HearingDetection.OwnerAI.Value.OwnerCombatEntity.Value.Target.Value == null
                &&
                HearingDetection.OwnerAI.Value.OwnerCombatEntity.Value.DamageableCurrentState.Value != DamageableState.Destroyed)
            {
                HearingDetection.OwnerAI.Value.OwnerCombatEntity.Value.SetTarget(noiseVolumeView.Player);
            }
            else if (HearingDetection.OwnerAI.Value.OwnerCombatEntity.Value.Target.Value != null && HearingDetection.OwnerAI.Value.OwnerCombatEntity.Value.Target.Value.DamageableCurrentState.Value == DamageableState.Destroyed)
            {
                HearingDetection.OwnerAI.Value.KilledPlayer();
                HearingDetection.IsDetectingPlayer.Value = false;
            }
        }
        #endregion
    }
}