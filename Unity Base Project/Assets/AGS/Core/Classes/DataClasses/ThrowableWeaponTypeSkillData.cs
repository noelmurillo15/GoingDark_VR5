using System;
using AGS.Core.Enums;
using UnityEngine;

namespace AGS.Core.Classes.DataClasses
{
    /// <summary>
    /// This class holds skill data for different ThrowableWeaponTypes
    /// </summary>
    [Serializable]
    public class ThrowableWeaponTypeSkillData
    {
        public ThrowableWeaponType ThrowableWeaponType;
        public ThrowableWeaponThrowingType ThrowingType;
        public float ChargeTimer;
        public float RechargeTimer;
        public Vector3 ThrowingSpeed;
    }
}
