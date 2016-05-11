using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;

namespace AGS.Core.Classes.DataClasses
{
    /// <summary>
    /// This data class holds the current count for a given ThrowableWeaponType
    /// </summary>
    [Serializable]
    public class ThrowableWeaponStash
    {
        public ThrowableWeaponType ThrowableWeaponType;
        public ActionProperty<int> Count = new ActionProperty<int>();
    }
}
