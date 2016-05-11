using System;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterSystem;

namespace AGS.Core.Systems.PickUpSystem
{
    /// <summary>
    /// This view only need the ThrowableWeaponType info and quanity to be set up
    /// </summary>
    [Serializable]
    public class PickUpThrowableView : PickUpItemBaseView
    {
		
		#region Public properties
        // Values to be set in editor
        public ThrowableWeaponType ThrowableWeaponType;
        public int Quantity;
		#endregion

        public PickUpThrowable PickUpThrowable;

        #region AGK setup
        public override void InitializeView()
        {
            PickUpThrowable = new PickUpThrowable(ThrowableWeaponType, Quantity); 
            SolveModelDependencies(PickUpThrowable);
        }
        #endregion

        /// <summary>
        /// Called when [trigger enter notification] with PlayerBaseView.
        /// </summary>
        /// <param name="playerView">The player view.</param>
        protected override void OnTriggerEnterNotification(PlayerBaseView playerView)
        {
            PickUpThrowable.CollectThrowable(playerView.Player);
            base.OnTriggerEnterNotification(playerView);
        }
	}
}