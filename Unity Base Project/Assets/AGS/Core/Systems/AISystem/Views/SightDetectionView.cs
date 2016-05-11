using System;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Classes.MonoExtensions;
using UnityEngine;

namespace AGS.Core.Systems.AISystem
{
    /// <summary>
    /// SightDetectionView "looks for" for collisions with the PlayerBaseView
    /// When a collision is detected a raycast is done to check if line of sight is not blocked (i.e can I really see the player?)
    /// </summary>
    [Serializable]
    public class SightDetectionView : DetectionVolumeBaseView
    {
        #region Public properties
        public GameObject RaycastStart; // From where should the raycast for finding the player start
        public LayerMask HitMask; // LayerMask for raycast to detect player
        #endregion

        public SightDetection SightDetection;

        #region AGS Setup
        public override void InitializeView()
        {
            SightDetection = new SightDetection();
            SolveModelDependencies(SightDetection);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            if (RaycastStart == null)
            {
                RaycastStart = gameObject;
            }
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            // Set up trigger stay action that listens for trigger with PlayerBaseView
            Action<PlayerBaseView> checkPlayerDetection = CheckPlayerDetection;
            gameObject.OnTriggerActionStayWith(checkPlayerDetection);

            // Set up trigger exit action that listens for trigger with PlayerBaseView. Sets IsDetectingPlayer to false when event is triggered
            Action<PlayerBaseView> playerExitAction = _ => { SightDetection.IsDetectingPlayer.Value = false; };
            gameObject.OnTriggerActionExitWith(playerExitAction);
        }
        #endregion

        #region private functions
        /// <summary>
        /// Sets the IsDetectingPlayer boolean
        /// </summary>
        /// <param name="playerView">The player view.</param>
        private void CheckPlayerDetection(PlayerBaseView playerView)
        {
            if (SightDetection.OwnerAI.Value == null
                ||
                SightDetection.OwnerAI.Value.OwnerCombatEntity.Value == null)
            {
                return;
            }

            // While player is in the trigger volume, raycast to see if player is also in line of sight
            var canSeePlayer = false;
            var halfPlayerHeight = playerView.Player.Height.Value / 2f;
            for (int i = 0; i < 3; i++)
            {
                Debug.DrawLine(RaycastStart.transform.position, new Vector3(playerView.transform.position.x, playerView.transform.position.y + i * halfPlayerHeight, playerView.transform.position.z), Color.blue);
                RaycastHit hitInfo;
                var rayCastHit = Physics.Linecast(RaycastStart.transform.position, new Vector3(playerView.transform.position.x, playerView.transform.position.y + i * halfPlayerHeight, playerView.transform.position.z), out hitInfo, HitMask);

                if (!rayCastHit)
                {
                    continue;
                }
                if (hitInfo.transform.tag == "Player")
                {
                    canSeePlayer = true;
                    break;
                }
                // line is blocked, check next
            }
            if (canSeePlayer
                &&
                SightDetection.OwnerAI.Value.OwnerCombatEntity.Value.Target.Value == null
                &&
                SightDetection.OwnerAI.Value.OwnerCombatEntity.Value.DamageableCurrentState.Value != DamageableState.Destroyed)
            {
                SightDetection.IsDetectingPlayer.Value = true;
                SightDetection.OwnerAI.Value.OwnerCombatEntity.Value.SetTarget(playerView.Player);
            }

            else if (SightDetection.OwnerAI.Value.OwnerCombatEntity.Value.Target.Value != null && SightDetection.OwnerAI.Value.OwnerCombatEntity.Value.Target.Value.DamageableCurrentState.Value == DamageableState.Destroyed)
            {
                SightDetection.OwnerAI.Value.KilledPlayer();
                SightDetection.IsDetectingPlayer.Value = false;
            }
            else if (canSeePlayer && !SightDetection.IsDetectingPlayer.Value)
            {
                SightDetection.IsDetectingPlayer.Value = true;
                SightDetection.OwnerAI.Value.OwnerCombatEntity.Value.SetTarget(playerView.Player);
            }
            else if (!canSeePlayer)
            {
                SightDetection.IsDetectingPlayer.Value = false;
            }
        }
        #endregion
    }
}