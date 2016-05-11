using AGS.Core.Systems.GameLevelSystem;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScriptBase that holds references to GameLevelBaseView and GameLevel
    /// </summary>
    public abstract class GameLevelViewScriptBase : ViewScriptBase
    {
        #region properties
        protected GameLevelBaseView GameLevelView;
        protected GameLevel GameLevel;
        #endregion

        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                GameLevelView = ViewReference as GameLevelBaseView;
                if (GameLevelView == null) return;

                GameLevel = GameLevelView.GameLevel;

            }
            if (GameLevel == null) return;
        }


    }
}
