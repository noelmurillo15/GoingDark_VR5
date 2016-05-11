using System;
using System.Linq;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Classes.ViewScripts;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterControlSystem;
using AGS.Core.Systems.CombatSkillSystem;
using AGS.Core.Systems.WeaponSystem;
using UnityEngine;
using AGS.Core.Systems.AimingSystem;

namespace AGS.Core.Systems.CharacterSystem
{
    /// <summary>
    /// Base view for a static fighting GameObject.
    /// </summary>
    [Serializable]
    public abstract class CombatEntityBaseView : KillableBaseView
    {
        #region Public properties
        // Fields to be set in the editor
        public float TurnSpeed;

        // References to be set in the editor
        public CharacterControllerBaseView CharacterControllerBaseView; // Owned character controller view
        public EquipableWeaponBaseView StartingWeaponView; // Optional starting weapon reference
        public AimingBaseView AimingSystem; // Reference to a AimingBaseView. Optional

        public Transform WeaponsContainer; // Reference to the WeaponsContainer that should contain any weapons that CombatEnity should own from start
        public EquipableWeaponBaseView[] WeaponViews; // Any other weapons that is not found withing the WeaponsContainer
        public Transform CombatMoveSetsContainer; // Place any combat move set that combat entity should have from start on separate child GameObjects to CombatMoveSetsContainer
        #endregion

        public CombatEntityBase CombatEntity;

        private UpdateTemporaryGameObject _targetTemporaryUpdate;
        private bool _justSwitchedWeapon;

        #region AGS Setup
        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            CombatEntity = model as CombatEntityBase;
            if (CombatEntity == null) return;
            CombatEntity.CharacterController.Value = CharacterControllerBaseView != null ? CharacterControllerBaseView.CharacterController : null;
            CombatEntity.Aiming.Value = AimingSystem != null ? AimingSystem.Aiming : null;
            CombatEntity.TurnAroundAction += TurnAroundNotification;
            CombatEntity.TurnInDirectionAction += TurnInDirectionNotification;
            if (CombatMoveSetsContainer != null)
            {
                foreach (var combatMoveSetView in CombatMoveSetsContainer.GetComponentsInChildren<CombatMoveSetView>())
                {
                    CombatEntity.CombatMoveSets.Add(combatMoveSetView.CombatMoveSet);
                }
            }
            if (WeaponsContainer != null)
            {
                foreach (var equipableWeaponBaseView in WeaponsContainer.GetComponentsInChildren<EquipableWeaponBaseView>())
                {
                    CombatEntity.Weapons.Add(equipableWeaponBaseView.EquipableWeapon);
                }
            }

            foreach (var weaponView in WeaponViews)
            {
                CombatEntity.Weapons.Add(weaponView.EquipableWeapon);
            }

            if (StartingWeaponView != null)
            {
                CombatEntity.CurrentWeapon.Value = StartingWeaponView.EquipableWeapon;
            }
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);

            CombatEntity.Target.OnValueChanged += (sender, target) => OnActiveTargetChanged(target.Value);
        }

        #endregion

        #region MonoBehaviours
        public override void Update()
        {
            base.Update();
            CheckWeaponSwitching();
        }
        #endregion

        #region private functions
        /// <summary>
        /// Called when [active target changed].
        /// Gets a notification when ActiveTarget has changed and sets up a temporary update to calculate targets distance and IsInRange boolean
        /// Removes temp update if target is set to null
        /// </summary>
        /// <param name="target">The target.</param>
        private void OnActiveTargetChanged(KillableBase target)
        {
            if (target != null)
            {
                _targetTemporaryUpdate = ComponentExtensions.AddComponentOnEmptyChild<UpdateTemporaryGameObject>(gameObject, "Active target update");
                _targetTemporaryUpdate.UpdateMethod = () =>
                {
                    if (CombatEntity.Transform == null) return;
                    if (target.Transform == null)
                    {
                        // Target may have been destroyed                     
                        return;
                    }
                    CombatEntity.DistanceToTarget.Value = Vector3.Distance(CombatEntity.Transform.position, target.Transform.position);
                    CombatEntity.TargetIsInRange.Value = CheckTargetIsInRange(CombatEntity.DistanceToTarget.Value);
                };
            }
            else
            {
                if (_targetTemporaryUpdate != null)
                {
                    _targetTemporaryUpdate.Stop();
                }
                CombatEntity.TargetIsInRange.Value = false;
            }
        }

        /// <summary>
        /// Checks if the target is in range.
        /// </summary>
        /// <param name="distanceToTarget">The distance to target.</param>
        /// <returns></returns>
        private bool CheckTargetIsInRange(float distanceToTarget)
        {
            if (distanceToTarget < 0f)
            {
                return false;
            }
            if (CombatEntity.CurrentWeapon.Value != null)
            {
                return distanceToTarget <= CombatEntity.CurrentWeapon.Value.Range;
            }
            return false;
        }

        /// <summary>
        /// Switches weapons based on charactercontroller variables
        /// </summary>
        private void CheckWeaponSwitching()
        {
            if (_justSwitchedWeapon) return;
            if (CombatEntity == null || CombatEntity.CharacterController.Value == null) return;
            if (CombatEntity.ActiveCombatMoveSet.Value == null) return;
            if (CombatEntity.ActiveCombatMoveSet.Value.ActiveCombatMove.Value != null) return; // Dont allow weaponswitching in middle of attack
            if (CombatEntity.CharacterController.Value.NextWeapon.Value)
            {
                NextWeapon();
            }
            else if (CombatEntity.CharacterController.Value.PreviousWeapon.Value)
            {
                PreviousWeapon();
            }
        }

        /// <summary>
        /// Selects next weapon in the Weapons list.
        /// </summary>
        private void NextWeapon()
        {
            var curWeaponIndex = CombatEntity.Weapons.IndexOf(CombatEntity.CurrentWeapon.Value);
            CombatEntity.CurrentWeapon.Value = curWeaponIndex == CombatEntity.Weapons.Count - 1 ? CombatEntity.Weapons.FirstOrDefault() : CombatEntity.Weapons[curWeaponIndex + 1];
            WeaponSwitchThrottle();
        }

        /// <summary>
        /// Selects previous weapon in the Weapons list.
        /// </summary>
        private void PreviousWeapon()
        {
            var curWeaponIndex = CombatEntity.Weapons.IndexOf(CombatEntity.CurrentWeapon.Value);
            CombatEntity.CurrentWeapon.Value = curWeaponIndex == 0 ? CombatEntity.Weapons.LastOrDefault() : CombatEntity.Weapons[curWeaponIndex - 1];
            WeaponSwitchThrottle();
        }

        /// <summary>
        /// Sets up a short timer to prevent spamming weapon switching
        /// </summary>
        private void WeaponSwitchThrottle()
        {
            _justSwitchedWeapon = true;
            var weaponSwitchThrottle = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Weapon switch throttle");
            weaponSwitchThrottle.TimerMethod = () => _justSwitchedWeapon = false;
            weaponSwitchThrottle.Invoke(0.5f);
        }

        #endregion

        #region protected functions
        /// <summary>
        /// Called when the combat entity turns around.
        /// </summary>
        protected virtual void TurnAroundNotification()
        {
            // Turn around function was executed
            TurnAround();
        }

        /// <summary>
        /// Called when the combat entity turns in the given direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        protected virtual void TurnInDirectionNotification(Vector3 direction)
        {
            TurnInDirection(direction);
        }

        /// <summary>
        /// Turns the combat entity around 180 degrees.
        /// </summary>
        protected abstract void TurnAround();

        /// <summary>
        /// Turn the combat entity in the given direction.
        /// </summary>
        protected abstract void TurnInDirection(Vector3 degrees);

        /// <summary>
        /// Rotates combat entity in the specified direction
        /// </summary>
        /// <param name="direction">The direction.</param>
        protected abstract void RotateTowardsDirectionNotification(Direction direction);
        #endregion
    }
}