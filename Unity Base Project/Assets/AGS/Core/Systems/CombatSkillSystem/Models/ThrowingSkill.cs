using System;
using System.Linq;
using AGS.Core.Base;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.DataClasses;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.SkillSystem;
using UnityEngine;

namespace AGS.Core.Systems.CombatSkillSystem
{
    /// <summary>
    /// ThrowingSkill can be used by CharacterBase class.
    /// </summary>
    public class ThrowingSkill : SkillBase
    {
        #region Properties
        // Constructor properties
        public Transform ThrowingSkillMuzzle { get; set; }
        public ThrowableWeaponTypeSkillData[] ThrowableWeaponTypeData;

        // Subscribable properties
        public ActionProperty<CharacterBase> OwnerCharacter; // reference to ThrowingSkill owner

        private ThrowableWeaponStash _throwableWeaponStash; // this is based on OwnerCharacters current active ThrowableWeaponType

        /// <summary>
        /// Gets a value indicating whether [out of throwables].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [out of throwables]; otherwise, <c>false</c>.
        /// </value>
        public bool OutOfThrowables
        {
            get
            {
                _throwableWeaponStash = OwnerCharacter.Value.ThrowableWeaponStashes.FirstOrDefault(x => x.ThrowableWeaponType == OwnerCharacter.Value.ActiveThrowableType.Value);
                if (_throwableWeaponStash == null) return true;
                return _throwableWeaponStash.Count.Value <= 0;
            }
        }

        // Subscribe to the following actions to get notified of ThrowingSkill events
        public Action BeginThrowAction { get; set; }
        public Action EndThrowAction { get; set; }
        public Action<ThrowableWeaponType, ThrowableWeaponThrowingType, Vector3> ReleaseThrowableAction { get; set; }
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowingSkill"/> class.
        /// </summary>
        /// <param name="throwingSkillMuzzle">Marks where throwable weapons should be released.</param>
        /// <param name="throwableWeaponTypeData">Holds the data of throwingskill variables of differenct ThrowableWeaponTypes.</param>
        public ThrowingSkill(Transform throwingSkillMuzzle, ThrowableWeaponTypeSkillData[] throwableWeaponTypeData)
        {
            ThrowingSkillMuzzle = throwingSkillMuzzle;
            OwnerCharacter = new ActionProperty<CharacterBase>();
            ThrowableWeaponTypeData = throwableWeaponTypeData;
        }

        #region private
        /// <summary>
        /// Gets the throwingskill data of the current active throwable type.
        /// </summary>
        /// <value>
        /// The active throwable skill data.
        /// </value>
        private ThrowableWeaponTypeSkillData ActiveThrowableSkillData
        {
            get
            {
                var activeSkillData =
                    ThrowableWeaponTypeData.FirstOrDefault(
                        x => x.ThrowableWeaponType == OwnerCharacter.Value.ActiveThrowableType.Value);
                if (activeSkillData == null)
                {
                    Debug.LogError("Missing skill data for active throwable type");
                }
                return activeSkillData;
            }
        }

        /// <summary>
        /// Notify subscribers (at least the view should be subscribing to the ReleaseThrowableAction) that character is releasing a throwable
        /// </summary>
        private void ReleaseThrowable()
        {
            ApplyResourceCost(OwnerCharacter.Value);
            if (ReleaseThrowableAction != null)
            {
                ReleaseThrowableAction(ActiveThrowableSkillData.ThrowableWeaponType, ActiveThrowableSkillData.ThrowingType, ActiveThrowableSkillData.ThrowingSpeed);
            }
            var rechargeTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Throwing skill recharge timer");
            rechargeTimer.TimerMethod = () =>
            {
                if (EndThrowAction != null)
                {
                    EndThrowAction();
                }
            };
            rechargeTimer.Invoke(ActiveThrowableSkillData.RechargeTimer);
            TimerComponents.Add(rechargeTimer);
        }
        #endregion

        #region public functions
        /// <summary>
        /// BeginThrowForward can be called from animation event to time when to release the throwable straight forward. If ChargeTimer == 0, call this event exactly when throwable should be released
        /// </summary>
        public void BeginThrowForward()
        {
            if (ActiveThrowableSkillData.ThrowingType == ThrowableWeaponThrowingType.Arc) return;
            if (ActiveThrowableSkillData == null) return;
            if (ActiveThrowableSkillData.ChargeTimer > 0)
            {
                var chargeTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Throwing skill charge timer");
                chargeTimer.TimerMethod = ReleaseThrowable;
                chargeTimer.Invoke(ActiveThrowableSkillData.ChargeTimer);
                TimerComponents.Add(chargeTimer);
            }
            else
            {
                ReleaseThrowable();
            }

        }

        /// <summary>
        /// BeginThrowArc can be called from animation event to time when to release the throwable in an arc. If ChargeTimer == 0, call this event exactly when throwable should be released
        /// </summary>
        public void BeginThrowArc()
        {
            if (ActiveThrowableSkillData.ThrowingType == ThrowableWeaponThrowingType.Forward) return;
            if (ActiveThrowableSkillData == null) return;
            if (ActiveThrowableSkillData.ChargeTimer > 0)
            {
                var chargeTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Throwing skill charge timer");
                chargeTimer.TimerMethod = ReleaseThrowable;
                chargeTimer.Invoke(ActiveThrowableSkillData.ChargeTimer);
                TimerComponents.Add(chargeTimer);
            }
            else
            {
                ReleaseThrowable();
            }

        }

        #endregion
    }
}
