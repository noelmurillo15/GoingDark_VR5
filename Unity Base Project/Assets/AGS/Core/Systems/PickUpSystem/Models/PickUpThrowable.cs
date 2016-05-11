using AGS.Core.Enums;
using AGS.Core.Systems.CharacterSystem;

namespace AGS.Core.Systems.PickUpSystem
{
    /// <summary>
    /// A PickUp that contains info of how many throwabled and of what type of throwables should be added when picked up
    /// </summary>
    public class PickUpThrowable : PickUpItemBase
    {
        #region Properties
        public ThrowableWeaponType ThrowableWeaponType { get; private set; }
        public int Quantity { get; private set; }

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="PickUpThrowable"/> class.
        /// </summary>
        /// <param name="throwableWeaponType">Type of throwable weapons to add</param>
        /// <param name="quantity">The quantity.</param>
        public PickUpThrowable(ThrowableWeaponType throwableWeaponType, int quantity)
        {
            ThrowableWeaponType = throwableWeaponType;
            Quantity = quantity;
        }

        #region public functions
        /// <summary>
        /// Make the player collect the trowables
        /// </summary>
        /// <param name="player">The player.</param>
        public void CollectThrowable(Player player)
        {
            player.CollectThrowables(ThrowableWeaponType, Quantity);            
        }
        #endregion

    }
}
