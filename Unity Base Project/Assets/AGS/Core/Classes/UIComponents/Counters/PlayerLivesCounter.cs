using AGS.Core.Base;
using AGS.Core.Classes.ActionProperties;

namespace AGS.Core.Classes.UIComponents
{
    /// <summary>
    /// This UI component shows current count of throwables
    /// </summary>
    public class PlayerLivesCounter : TextCounterBase
    {
        protected override void SetupCounter()
        {
            UpdatePlayerLives(GameManager.GameData.PlayerLives.Value);
            GameManager.GameData.PlayerLives.OnValueChanged += UpdatePlayerLives;
        }

        /// <summary>
        /// Updates the player lives.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="lives">The <see cref="int"/> instance containing the event data.</param>
        private void UpdatePlayerLives(object sender, ActionPropertyEventArgs<int> lives)
        {
            Counter = lives.Value;
            UpdateGUIText();
        }

        /// <summary>
        /// Updates the player lives.
        /// </summary>
        /// <param name="lives">The lives.</param>
        private void UpdatePlayerLives(int lives)
        {
            Counter = lives;
            UpdateGUIText();
        }
    }
}
