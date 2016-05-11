using UnityEngine;

namespace AGS.Core.Systems.CharacterSystem
{
    /// <summary>
    /// Static enemies can be turrets, defense towers or similar.
    /// </summary>
    public class StaticEnemy : CombatEntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticEnemy" /> class.
        /// </summary>
        /// <param name="transform">The CombatEntityBase transform.</param>
        /// <param name="name">CombatEntityBase name. Is instantiating ragdolls upon death, this need to match Ragdoll prefix. A Killable with the name John will try to instantiate JohnRagdoll</param>
        /// <param name="turnSpeed">The turn speed.</param>
        public StaticEnemy(Transform transform, string name, float turnSpeed)
            : base(transform, name, turnSpeed)
	    {
        }
    }
}
