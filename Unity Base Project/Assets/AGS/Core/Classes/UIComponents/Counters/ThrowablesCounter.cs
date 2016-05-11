using System.Linq;
using AGS.Core.Classes.DataClasses;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterSystem;

namespace AGS.Core.Classes.UIComponents
{
    /// <summary>
    /// This UI component shows current count of throwables
    /// </summary>
    public class ThrowablesCounter : TextCounterBase
    {
        private ThrowableWeaponStash _throwableWeaponStash;
        protected override void SetupCounter()
        {
            _throwableWeaponStash = new ThrowableWeaponStash();
            GameLevel.Player.OnValueChanged += (sender, player) => OnPlayerChanged(player.Value);
        }

        private void OnPlayerChanged(Player player)
        {

            player.ActiveThrowableType.OnValueChanged += (sender, activeTrowableType) => OnActiveThrowableChanged(player, activeTrowableType.Value);
        }

        /// <summary>
        /// Called when [active throwable changed].
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="activeTrowable">The active trowable.</param>
        private void OnActiveThrowableChanged(Player player, ThrowableWeaponType activeTrowable)
        {
            _throwableWeaponStash = player.ThrowableWeaponStashes.FirstOrDefault(x => x.ThrowableWeaponType == activeTrowable);
            if (_throwableWeaponStash != null)
            {
                _throwableWeaponStash.Count.OnValueChanged += (sender, throwables) =>
                {
                    Counter = throwables.Value;
                    UpdateGUIText();
                };
            }
        }
    }
}
