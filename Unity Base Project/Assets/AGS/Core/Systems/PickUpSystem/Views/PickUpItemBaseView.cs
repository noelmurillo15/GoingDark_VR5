using System;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Classes.MonoExtensions;

namespace AGS.Core.Systems.PickUpSystem
{
    /// <summary>
    /// PickUpItemBaseView sets up the trigger with a player. Inherit from this to create specific pick up views.
    /// </summary>
    [Serializable]
    public abstract class PickUpItemBaseView : ActionView
    {
        public PickUpItemBase PickUpItemBase;

        #region AGS Setup

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            PickUpItemBase = model as PickUpItemBase;
            if (PickUpItemBase == null) return;
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            Action<PlayerBaseView> triggerEnterNotification = OnTriggerEnterNotification;
            gameObject.OnTriggerActionEnterWith(triggerEnterNotification);
            PickUpItemBase.SetActiveAction += SetActive;
        }
        #endregion

        #region protected functions
        /// <summary>
        /// Called when [trigger enter notification] with PlayerBaseView.
        /// </summary>
        /// <param name="playerView">The player view.</param>
        protected virtual void OnTriggerEnterNotification(PlayerBaseView playerView)
        {
            PickUpItemBase.PickUp();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Sets the pick up to active/inactive.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        protected virtual void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
        #endregion
	}
}