using System;
using System.Linq;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.CharacterControlSystem;
using AGS.Core.Systems.CombatSkillSystem;
using AGS.Core.Systems.WeaponSystem;
using UnityEngine;
using AGS.Core.Systems.AimingSystem;

namespace AGS.Core.Systems.CharacterSystem
{
    /// <summary>
    /// Base class for any killable object that can also fight (but not move). Inherit from this to create for example turrets, defense towers or similar.
    /// </summary>
    public abstract class CombatEntityBase : KillableBase
    {

        #region Properties
        // Constructor properties
        public float TurnSpeed { get; set; }

        // Subscribable properties
        public ActionProperty<Aiming> Aiming { get; set; }
        //public ActionProperty<bool> Aiming { get; set; } // Is the combat entity aiming?
        //public ActionProperty<Vector3> AimTarget { get; set; } // The position the combat entity is aiming at.
        public ActionProperty<KillableBase> Target { get; set; } // Current killable target. Some skills require that a target is set.
        public ActionProperty<float> DistanceToTarget { get; set; } // Current distance to Target
        public ActionProperty<bool> TargetIsInRange { get; set; } // Is target in range? Dependent on CurrentWeapon range
        public ActionProperty<CombatMoveSet> ActiveCombatMoveSet { get; set; } // This changes with CurrentWeapon
        public ActionProperty<EquipableWeaponBase> CurrentWeapon { get; set; } // Current equipped weapon
        public ActionProperty<CharacterControllerBase> CharacterController { get; set; } // Owned controller

        public ActionList<CombatMoveSet> CombatMoveSets { get; set; } // Owned combat move sets
        public ActionList<EquipableWeaponBase> Weapons { get; set; } // Owned weapons

        // Subscribable actions
        public Action TurnAroundAction { get; set; }
        public Action<Vector3> TurnInDirectionAction { get; set; }
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="CombatEntityBase" /> class.
        /// </summary>
        /// <param name="transform">The CombatEntityBase transform.</param>
        /// <param name="name">CombatEntityBase name. Is instantiating ragdolls upon death, this need to match Ragdoll prefix. A Killable with the name John will try to instantiate JohnRagdoll</param>
        /// <param name="turnSpeed">The turn speed.</param>
        protected CombatEntityBase(Transform transform, string name, float turnSpeed)
            : base(transform, name)
        {
            TurnSpeed = turnSpeed;
            Aiming = new ActionProperty<Aiming>();
            Aiming.OnValueChanged += (sender, aimingSystem) =>
            {
                aimingSystem.Value.OwnerCombatEntity.Value = this;
            };
            Target = new ActionProperty<KillableBase>();
            DistanceToTarget = new ActionProperty<float>();
            TargetIsInRange = new ActionProperty<bool>();
            TargetIsInRange.OnValueChanged += (sender, isInRange) =>
            {
                if (!isInRange.Value
                    &&
                    Target != null)
                {
                    SetTarget(null);
                }
            };
            ActiveCombatMoveSet = new ActionProperty<CombatMoveSet>();
            ActiveCombatMoveSet.OnValueChanged += (sender, activeMoveSet) => OnActiveCombatMoveSetChanged(activeMoveSet.Value);
            CurrentWeapon = new ActionProperty<EquipableWeaponBase>();
            CurrentWeapon.OnValueChanged += (sender, currentWeapon) => OnCurrentWeaponChanged(currentWeapon.Value);

            CharacterController = new ActionProperty<CharacterControllerBase>();
            CharacterController.OnValueChanged += (sender, characterController) =>
            {
                characterController.Value.OwnerCombatEntity.Value = this;
            };
            CombatMoveSets = new ActionList<CombatMoveSet>();
            Weapons = new ActionList<EquipableWeaponBase>();

            CombatMoveSets.ListItemAdded += CombatMoveSetAdded;
            Weapons.ListItemAdded += WeaponAdded;
        }

        #region skill enablers
        /// <summary>
        /// Enables combat skills for active move set.
        /// </summary>
        protected void EnableCombatSkills()
        {
            if (ActiveCombatMoveSet.Value != null)
            {
                ActiveCombatMoveSet.Value.Enable();
            }
        }

        /// <summary>
        /// Disables combat skills for active move set.
        /// </summary>
        protected void DisableCombatSkills()
        {
            if (ActiveCombatMoveSet.Value != null)
            {
                ActiveCombatMoveSet.Value.Disable();
            }
        }
        #endregion

        #region private functions
        /// <summary>
        /// Called when [active combat move set changed].
        /// Disable all other movesets upon notification and enable new
        /// </summary>
        /// <param name="combatMoveSet">The combat move set.</param>
        protected virtual void OnActiveCombatMoveSetChanged(CombatMoveSet combatMoveSet)
        {
            if (combatMoveSet == null) return;
            foreach (var moveSet in CombatMoveSets.Where(x => x != combatMoveSet))
            {
                moveSet.Disable();
            }
            combatMoveSet.Enable();
        }

        /// <summary>
        /// Called when [current weapon changed].
        /// Disable all other weapon upon notification and enable new weapon. Also sets ActiveCombatMoveSet according to new weapons moveset.
        /// </summary>
        /// <param name="weapon">The weapon.</param>
        private void OnCurrentWeaponChanged(EquipableWeaponBase weapon)
        {
            if (weapon == null) return;
            foreach (var equipableWeaponBase in Weapons.Where(x => x.Enabled.Value))
            {
                equipableWeaponBase.Disable();
            }
            weapon.Enable();
            var newMoveSet = CombatMoveSets.FirstOrDefault(x => x.CombatMoveSetType == weapon.CombatMoveSetType);
            if (newMoveSet != null)
            {
                ActiveCombatMoveSet.Value = newMoveSet;
            }
            else
            {
                Debug.LogError("Missing CombatMoveSet for current weapon");
            }
        }

        /// <summary>
        /// List add notification. If ActiveCombatMoveSet is null, set ActiveCombatMoveSet to listadd
        /// </summary>
        /// <param name="combatMoveSetAdd">The combat move set add.</param>
        private void CombatMoveSetAdded(CombatMoveSet combatMoveSetAdd)
        {
            if (combatMoveSetAdd == null) return;
            combatMoveSetAdd.OwnerCombatEntity.Value = this;
            if (ActiveCombatMoveSet.Value == null)
            {
                ActiveCombatMoveSet.Value = combatMoveSetAdd;
            }
        }

        /// <summary>
        /// List add notification. Always set CurrentWeapon to listadd. This automatically equips new weapons upon pick up
        /// </summary>
        /// <param name="weapon">The weapon.</param>
        private void WeaponAdded(EquipableWeaponBase weapon)
        {
            weapon.OwnerCombatEntity.Value = this;
            CurrentWeapon.Value = weapon;
        }
        #endregion functions

        #region public functions
        /// <summary>
        /// Sets the target.
        /// </summary>
        /// <param name="target">The target.</param>
        public virtual void SetTarget(KillableBase target)
        {
            Target.Value = target;
        }

        /// <summary>
        /// Turns the character around.
        /// </summary>
        public void TurnAround()
        {
            if (TurnAroundAction != null)
            {
                TurnAroundAction();
            }
        }

        /// <summary>
        /// Turns the character in the given direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        public void TurnInDirection(Vector3 direction)
        {
            if (TurnInDirectionAction != null)
            {
                TurnInDirectionAction(direction);
            }
        }
        #endregion
    }
}
