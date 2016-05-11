using System;
using AGS.Core.Systems.CharacterSystem;

namespace AGS.Core.Systems.PickUpSystem
{
    /// <summary>
    /// PickUpWeaponView needs references to preset weapons and combat move set (optional) prefabs
    /// </summary>
    [Serializable]
    public class PickUpWeaponView : PickUpItemBaseView
    {
		
		#region Public properties
        public UnityEngine.Object EquipableWeaponPrefab;
        public UnityEngine.Object CombatMoveSetPrefab;
		#endregion

        public PickUpWeapon PickUpWeapon;

        #region AGK setup
        public override void InitializeView()
        {
            PickUpWeapon = new PickUpWeapon(); 
            SolveModelDependencies(PickUpWeapon);
        }
        #endregion
        /// <summary>
        /// Called when [trigger enter notification] with PlayerBaseView.
        /// </summary>
        /// <param name="playerView">The player view.</param>
        protected override void OnTriggerEnterNotification(PlayerBaseView playerView)
        {
            PickUpWeapon.CollectWeapon(playerView.Player, EquipableWeaponPrefab, CombatMoveSetPrefab);
            base.OnTriggerEnterNotification(playerView);
        }

	}
}