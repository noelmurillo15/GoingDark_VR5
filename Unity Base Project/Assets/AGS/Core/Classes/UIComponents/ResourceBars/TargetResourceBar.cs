using System.Linq;
using AGS.Core.Systems.CharacterSystem;
using UnityEngine.UI;

namespace AGS.Core.Classes.UIComponents
{
    /// <summary>
    /// DamageableResourceBar override. For use with a Players active target
    /// </summary>
    public class TargetResourceBar : DamageableResourceBar
    {
        public Text TargetName;

        /// <summary>
        /// Sets the target.
        /// </summary>
        /// <param name="killable">The killable.</param>
        protected override void SetTarget(KillableBase killable)
        {
            TargetKillable = killable;
            if (TargetKillable != null)
            {
                TargetName.text = TargetKillable.Name;
                CanvasGroup.alpha = 1;
                var resourceToObserve = TargetKillable.Resources.First(resource => resource.ResourceType.Value == ResourceType);
                if (resourceToObserve == null)
                    return;

                OnResourceChanged(resourceToObserve);
            }
            else
            {
                CanvasGroup.alpha = 0;
            }
        }

        /// <summary>
        /// Called when [player changed].
        /// </summary>
        /// <param name="player">The player.</param>
        protected override void OnPlayerChanged(Player player)
        {
            SetTarget(player.Target.Value);
            player.Target.OnValueChanged += (sender, target) => SetTarget(target.Value);
        }

    }
}
