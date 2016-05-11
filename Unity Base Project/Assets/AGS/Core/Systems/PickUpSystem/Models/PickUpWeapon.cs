using AGS.Core.Systems.CharacterSystem;
using Object = UnityEngine.Object;

namespace AGS.Core.Systems.PickUpSystem
{
    /// <summary>
    /// A PickUp that can add both a weapon and a combat move set to go with it to a character
    /// </summary>
    public class PickUpWeapon : PickUpItemBase
    {
        #region public functions

        /// <summary>
        /// Makes the player collect the weapon and combat move set.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="equipableWeaponPrefab">The equipable weapon prefab.</param>
        /// <param name="combatMoveSetPrefab">The combat move set prefab.</param>
        public void CollectWeapon(Player player, Object equipableWeaponPrefab, Object combatMoveSetPrefab)
        {
            player.CollectWeapon(equipableWeaponPrefab, combatMoveSetPrefab);
        }
        #endregion

    }
}
