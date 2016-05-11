using System.Linq;
using AGS.Core.Systems.CharacterSystem;

namespace AGS.Core.Classes.UIComponents
{
    /// <summary>
    /// DamageableResourceBar override. For use with a PlayerResourceBar
    /// </summary>
    public class PlayerResourceBar : DamageableResourceBar
    {
        /// <summary>
        /// Sets the target.
        /// </summary>
        /// <param name="killable">The killable.</param>
        protected override void SetTarget(KillableBase killable)
        {

            TargetKillable = killable;
        }

        /// <summary>
        /// Called when [player changed].
        /// </summary>
        /// <param name="player">The player.</param>
        protected override void OnPlayerChanged(Player player)
        {

            SetTarget(player);
            if (TargetKillable == null || TargetKillable.Resources == null) return;
            DamageableResource resourceToObserve = null;
            if (TargetKillable.Resources.Any(resource => resource.ResourceType.Value == ResourceType))
            {
                resourceToObserve = TargetKillable.Resources.First(resource => resource.ResourceType.Value == ResourceType);

            }
            else
            {
                TargetKillable.Resources.ListItemAdded += resource =>
                {
                    if (resource.ResourceType.Value == ResourceType)
                    {
                        resourceToObserve = resource;
                    }
                };
            }
            if (resourceToObserve == null)
                return;
            OnResourceChanged(resourceToObserve);
        }
    }
}
