using AGS.Core.Enums;
using AGS.Core.Systems.CombatSkillSystem;
using AGS.Core.Systems.WeaponSystem;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that subscribes to a Weapon and sets weapon animator parameters
    /// </summary>
    public class WeaponAnimations : ViewScriptBase
    {
        private Animator _animator;

        private WeaponBaseView _weaponBaseView;
        private WeaponBase _weapon;

        public override void Awake()
        {
            base.Awake();
            _animator = gameObject.GetComponentInChildren<Animator>();
        }

        protected override void SetupModelBindings()
        {
            if (_animator == null) return;
            if (ViewReference != null)
            {
                _weaponBaseView = ViewReference as WeaponBaseView;
                if (_weaponBaseView == null) return;

                _weapon = _weaponBaseView.Weapon;

            }
            if (_weapon == null) return;
            if (_weapon.OwnerCombatEntity.Value != null)
            {
                OnOwnerCombatEntityChanged();
            }
            else
            {
                _weapon.OwnerCombatEntity.OnValueChanged += (sender, ownerCombatEntity) => OnOwnerCombatEntityChanged();
            }
        }

        /// <summary>
        /// Called when [owner combat entity changed].
        /// </summary>
        private void OnOwnerCombatEntityChanged()
        {
            _weapon.OwnerCombatEntity.Value.ActiveCombatMoveSet.OnValueChanged +=
                (sender, activeMoveSet) => OnActiveMoveSetChanged(activeMoveSet.Value);

            if (_weapon.OwnerCombatEntity.Value.CharacterController.Value == null) return;
            _weapon.OwnerCombatEntity.Value.CharacterController.Value.Fire1.OnValueChanged +=
                (sender, fire1) => OnFire1Changed(fire1.Value);
            _weapon.OwnerCombatEntity.Value.CharacterController.Value.Fire2.OnValueChanged +=
                (sender, fire2) => OnFire2Changed(fire2.Value);
            _weapon.OwnerCombatEntity.Value.CharacterController.Value.Fire3.OnValueChanged +=
                (sender, fire3) => OnFire3Changed(fire3.Value);
        }

        /// <summary>
        /// Called when [active move set changed].
        /// </summary>
        /// <param name="combatMoveSet">The combat move set.</param>
        private void OnActiveMoveSetChanged(CombatMoveSet combatMoveSet)
        {
            if (combatMoveSet == null) return;

            if (_animator.isInitialized)
            {
                _animator.SetInteger("CombatMoveSet", (int) combatMoveSet.CombatMoveSetType);
            }
            combatMoveSet.CombatEntityPosition.OnValueChanged += (sender, position) =>
            {
                if (_animator.isInitialized)
                {
                    _animator.SetInteger("CombatMovePosition", (int) position.Value);
                }
            };
            combatMoveSet.Enabled.OnValueChanged += (sender, isEnabled) =>
            {
                if (_animator.isInitialized)
                {
                    _animator.SetBool("CombatEnabled", isEnabled.Value);
                }                
            };
            combatMoveSet.ComboTimeoutAction += () =>
            {
                if (_animator.isInitialized)
                {
                    _animator.SetTrigger("ComboTimeout");
                    _animator.ResetTrigger("FastAttack");
                    _animator.ResetTrigger("StrongAttack");
                }
            };

            combatMoveSet.CombatEntityPosition.OnValueChanged += (sender, position) =>
            {
                if (_animator.isInitialized)
                {
                    _animator.SetInteger("CombatPosition", (int) position.Value);
                }
            };
            combatMoveSet.CombosExecuted.OnValueChanged += (sender, combosExecuted) =>
                {
                    if (_animator == null || !_animator.isInitialized) return;
                    _animator.SetInteger("CombatCombosExecuted", combosExecuted.Value);
                };


            combatMoveSet.ActiveCombatMove.OnValueChanged += (sender, activeCombatMove) => OnActiveCombatMoveChanged(activeCombatMove.Value);
        }

        /// <summary>
        /// Called when [active combat move changed].
        /// </summary>
        /// <param name="activeCombatMove">The active combat move.</param>
        private void OnActiveCombatMoveChanged(CombatMove activeCombatMove)
        {
            if (_animator == null || !_animator.isInitialized) return;
            
            _animator.ResetTrigger("ComboTimeout");
            if (activeCombatMove == null)
            {
                _animator.SetInteger("CombatCombosExecuted", 0);
                _animator.ResetTrigger("FastAttack");
                _animator.ResetTrigger("StrongAttack");
                return;
            }
            if (activeCombatMove.FireType == CombatSkillFireType.OneShot)
            {
                    switch (activeCombatMove.AttackType)
                    {
                        case CombatSkillAttackType.Primary:
                            _animator.SetTrigger("FastAttack");
                            break;
                        case CombatSkillAttackType.Secondary:
                            _animator.SetTrigger("StrongAttack");
                            break;
                    }
            }
            else
            {
                activeCombatMove.OutOfResources.OnValueChanged +=
                    (sender, value) => _animator.SetBool("OutOfSustainedResources", value.Value);
            }

        }

        /// <summary>
        /// Called when [fire1 changed].
        /// </summary>
        /// <param name="fire1">if set to <c>true</c> [fire1].</param>
        private void OnFire1Changed(bool fire1)
        {
            if (_animator.isInitialized)
            {
                _animator.SetBool("Fire1", fire1);
            }
        }

        /// <summary>
        /// Called when [fire2 changed].
        /// </summary>
        /// <param name="fire2">if set to <c>true</c> [fire2].</param>
        private void OnFire2Changed(bool fire2)
        {
            if (_animator.isInitialized)
            {
                _animator.SetBool("Fire2", fire2);
            }
        }

        /// <summary>
        /// Called when [fire3 changed].
        /// </summary>
        /// <param name="fire3">if set to <c>true</c> [fire3].</param>
        private void OnFire3Changed(bool fire3)
        {
            if (_animator.isInitialized)
            {
                _animator.SetBool("Fire3", fire3);
            }
        }
    }
}
