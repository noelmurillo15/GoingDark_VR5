using System;
using AGS.Core.Classes.ViewScripts;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CombatSkillSystem;
using AGS.Core.Systems.WeaponSystem;
using UnityEngine;

namespace AGS.Core.Systems.CharacterSystem
{
    /// <summary>
    /// Base Player View. Like normal implementation of AdvancedCharacterBaseView plus TargetingSystem
    /// </summary>
    [Serializable]
    public abstract class PlayerBaseView : AdvancedCharacterBaseView
    {
        public Player Player;
        public TargetingSystem TargetingSystem; // Reference to a TargetingSystem ViewScript. Ignore if not in use

        #region AGS Setup
        public override void InitializeView()
        {
            Player = new Player(transform, Name, Speed, TurnSpeed, SkinWidth, SkinCorrectionRays, SlopeLimitMoving, SlopeLimitSliding);
            SolveModelDependencies(Player);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            Player.CollectWeaponAction += CollectWeapon;
        }
        #endregion

        #region MonoBehaviours
        public override void Awake()
        {
            base.Awake();
            TargetingSystem = GetComponent<TargetingSystem>();
        }
        #endregion

        /// <summary>
        /// Collects the weapon. Instaniates weapon and moveset.
        /// </summary>
        /// <param name="equipableWeaponPrefab">The equipable weapon prefab.</param>
        /// <param name="weaponMoveSetPrefab">The weapon move set prefab.</param>
        private void CollectWeapon(UnityEngine.Object equipableWeaponPrefab, UnityEngine.Object weaponMoveSetPrefab)
        {
            if (weaponMoveSetPrefab != null)
            {
                AddMoveSet(weaponMoveSetPrefab);
            }
            if (equipableWeaponPrefab != null)
            {
                AddWeapon(equipableWeaponPrefab);
            }

        }

        /// <summary>
        /// Adds the weapon.
        /// </summary>
        /// <param name="equipableWeaponPrefab">The equipable weapon prefab.</param>
        private void AddWeapon(UnityEngine.Object equipableWeaponPrefab)
        {
            var equipableWeaponGO = Instantiate(equipableWeaponPrefab) as GameObject;
            var quipableWeaponBaseView = equipableWeaponGO.GetComponent<EquipableWeaponBaseView>();
            if (WeaponsContainer != null && quipableWeaponBaseView != null)
            {
                equipableWeaponGO.transform.parent = WeaponsContainer.transform;
                if (quipableWeaponBaseView.ViewReady.Value)
                {
                    CombatEntity.Weapons.Add(quipableWeaponBaseView.EquipableWeapon);
                }
                else
                {
                    quipableWeaponBaseView.ViewReady.OnValueChanged += (sender, e) =>
                    {
                        if (!e.Value) return;
                        CombatEntity.Weapons.Add(quipableWeaponBaseView.EquipableWeapon);
                    };
                }
            }
        }

        /// <summary>
        /// Adds the move set.
        /// </summary>
        /// <param name="weaponMoveSetPrefab">The weapon move set prefab.</param>
        private void AddMoveSet(UnityEngine.Object weaponMoveSetPrefab)
        {
            var combatMoveSetGO = Instantiate(weaponMoveSetPrefab) as GameObject;
            var combatMoveSetView = combatMoveSetGO.GetComponent<CombatMoveSetView>();
            if (CombatMoveSetsContainer != null && combatMoveSetView != null)
            {
                combatMoveSetGO.transform.parent = CombatMoveSetsContainer.transform;
                if (combatMoveSetView.ViewReady.Value)
                {
                    CombatEntity.CombatMoveSets.Add(combatMoveSetView.CombatMoveSet);
                }
                else
                {
                    combatMoveSetView.ViewReady.OnValueChanged += (sender, e) =>
                    {
                        if (!e.Value) return;
                        CombatEntity.CombatMoveSets.Add(combatMoveSetView.CombatMoveSet);
                    };
                }
            }
        }
    }
}