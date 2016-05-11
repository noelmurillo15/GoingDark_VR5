using AGS.Core.Systems.GameLevelSystem;
using UnityEngine;

namespace AGS.Core.Classes.UIComponents
{
    /// <summary>
    /// UIScripts are meant to be used as separate UI Components, to provide modularity, testability, readability and reduce file length.
    /// UIScriptBase inherits a reference to the GameLevelHUDBaseView and the GameLevel element. SetupModelBindings will be called when HUD is initialzed. Override this function to set up subscriptions etc. 
    /// Make sure to call base.SetupModelBindings() to get properly set references 
    /// </summary>
    public abstract class UIScriptBase : MonoBehaviour {

        public GameLevelHUDBaseView GameLevelHUDView;
        protected GameLevel GameLevel;

        void OnEnable()
        {
            GameLevelHUDView.HUDInitialized += SetupModelBindings;
        }

        public virtual void Awake()
        {

        }

        protected virtual void SetupModelBindings()
        {
            if (GameLevelHUDView == null)
            {
                GameLevelHUDView = GetComponentInParent<GameLevelHUDBaseView>();
            }
            if (GameLevelHUDView == null)
            {
                Debug.LogError("Cant find HUDView for TextCounter");
                return;
            }
            GameLevel = GameLevelHUDView.GameLevel;
        }
    }
}
