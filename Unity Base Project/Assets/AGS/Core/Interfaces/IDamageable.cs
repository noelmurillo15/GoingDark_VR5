using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Interfaces
{
    /// <summary>
    /// Entities that can be destroyed by resource damage should implement this interface
    /// </summary>
    public interface IDamageable {
        Transform Transform { get; set; }
        ActionList<DamageableResource> Resources { get; set; }
        ActionProperty<DamageableState> DamageableCurrentState { get; set; }
        void ApplyResourceEffect(ResourceEffect resourceEffect, bool hitFromBehind);
    }
}
