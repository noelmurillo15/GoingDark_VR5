using System;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Classes.MonoExtensions;
using UnityEngine;
using UnityEngine.Audio;

namespace AGS.Core.Utilities
{
    /// <summary>
    /// Use this script on a triggerzone to blend between AudioMixerSnapShots based on if the player is present in the zone or not
    /// </summary>
    public class AreaSnapshotBlender : MonoBehaviour
    {
        public AudioMixer AudioMixer;
        public AudioMixerSnapshot[] AudioMixerSnapshots;
        public float[] WeightsInArea;
        public float[] WeightsOutOfArea;
        public bool BlendOnPlayerEnter;
        public bool BlendOnPlayerExit;
        public float BlendTime;
        
        void Start()
        {
            Action<PlayerBaseView> playerEnterAction = PlayerEnteredArea;
            gameObject.OnTriggerActionEnterWith(playerEnterAction);

            Action<PlayerBaseView> playerExitAction = PlayerLeftArea;
            gameObject.OnTriggerActionExitWith(playerExitAction);
        }

        private void PlayerEnteredArea(PlayerBaseView player)
        {
            if (BlendOnPlayerEnter)
            {
                AudioMixer.TransitionToSnapshots(AudioMixerSnapshots, WeightsInArea, BlendTime);    
            }
            
        }

        private void PlayerLeftArea(PlayerBaseView player)
        {
            if (BlendOnPlayerExit)
            {
                AudioMixer.TransitionToSnapshots(AudioMixerSnapshots, WeightsOutOfArea, BlendTime);    
            }
            
        }
    }
}
