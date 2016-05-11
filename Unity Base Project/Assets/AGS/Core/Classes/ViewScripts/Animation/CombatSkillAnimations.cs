using System;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.AimingSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.CombatSkillSystem;
using AGS.Core.Systems.WeaponSystem;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that subscribes to CombatSkills and sets animator parameters
    /// </summary>
    public class CombatSkillAnimations : ViewScriptBase
    {
        private Animator _animator;

        private CombatEntityBaseView _combatEntityBaseView;
        private CombatEntityBase _combatEntity;
        private UpdatePersistantGameObject _aimYUpdater;
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
                _combatEntityBaseView = ViewReference as CombatEntityBaseView;
                if (_combatEntityBaseView == null) return;

                _combatEntity = _combatEntityBaseView.CombatEntity;

            }
            if (_combatEntity == null) return;

            _combatEntity.ActiveCombatMoveSet.OnValueChanged += (sender, activeMoveSet) => OnActiveMoveSetChanged(activeMoveSet.Value);
            _combatEntity.CurrentWeapon.OnValueChanged += (sender, currentWeapon) => OnCurrentWeaponChanged(currentWeapon.Value);
            if (_combatEntity.Aiming.Value != null)
            {
                _combatEntity.Aiming.Value.AimingCurrentState.OnValueChanged += (sender, aimingState) => OnAimingStateChanged(aimingState.Value);
            }
            
            if (_combatEntity.CharacterController.Value == null) return;
            _combatEntity.CharacterController.Value.Fire1.OnValueChanged += (sender, fire1) => OnFire1Changed(fire1.Value);
            _combatEntity.CharacterController.Value.Fire2.OnValueChanged += (sender, fire2) => OnFire2Changed(fire2.Value);
            _combatEntity.CharacterController.Value.Fire3.OnValueChanged += (sender, fire3) => OnFire3Changed(fire3.Value);
        }

        /// <summary>
        /// Called when [aiming state changed].
        /// </summary>
        /// <param name="aimingState">State of the aiming.</param>
        private void OnAimingStateChanged(AimingStateMachineState aimingState)
        {
            if (_aimYUpdater != null)
            {
                _aimYUpdater.Stop();
            }
            if (aimingState == AimingStateMachineState.Aiming)
            {
                _animator.SetBool("Aim", true);

                _aimYUpdater = ComponentExtensions.SetupComponent<UpdatePersistantGameObject>(gameObject);
                _aimYUpdater.UpdateMethod = () =>
                {
                    _animator.SetFloat("AimY", Input.mousePosition.y / Screen.height);
                };
            }
            else if (aimingState == AimingStateMachineState.LockedOnTarget)
            {
                if (_combatEntity.CurrentWeapon.Value != null)
                {

                        _animator.SetBool("Aim", true);
                        _aimYUpdater = ComponentExtensions.SetupComponent<UpdatePersistantGameObject>(gameObject);
                        _aimYUpdater.UpdateMethod = () =>
                        {
                            if (_combatEntity.Target.Value == null) {
                                _aimYUpdater.Stop();
                            }
                            else
                            {
                                var targetCenter = _combatEntity.Target.Value.Transform.GetComponent<Collider>().bounds.center;
                                var distance = targetCenter - (_combatEntityBaseView.transform.position + Vector3.up * _combatEntityBaseView.GetComponent<Collider>().bounds.max.y);
                                var projectedAim = distance.normalized.y;
                                if (projectedAim == 0)
                                {
                                    projectedAim = 0.5f;
                                }
                                else
                                {
                                    projectedAim = 0.5f + projectedAim / 2f;
                                }


                                _animator.SetFloat("AimY", projectedAim);
                            }

                        };
                    
                }
            }
            else
            {
                _animator.SetBool("Aim", false);
            }
        }


        /// <summary>
        /// Called when [active move set changed].
        /// </summary>
        /// <param name="combatMoveSet">The combat move set.</param>
        private void OnActiveMoveSetChanged(CombatMoveSet combatMoveSet)
        {
            if (_animator == null) return;
            if (combatMoveSet == null) return;
            _animator.SetInteger("CombatMoveSet", (int)combatMoveSet.CombatMoveSetType);
            combatMoveSet.Enabled.OnValueChanged += (sender, isEnabled) => _animator.SetBool("CombatEnabled", isEnabled.Value);
            combatMoveSet.ComboTimeoutAction += () =>
            {
                _animator.SetTrigger("ComboTimeout");
                _animator.ResetTrigger("FastAttack");
                _animator.ResetTrigger("StrongAttack");
            };

            combatMoveSet.CombatEntityPosition.OnValueChanged += (sender, position) => _animator.SetInteger("CombatPosition", (int)position.Value);
            combatMoveSet.CombosExecuted.OnValueChanged += (sender, combosExecuted) =>
                {
                    if (_animator == null) return;
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
            if (_animator == null) return;

            if (activeCombatMove == null)
            {
                _animator.ResetTrigger("FastAttack");
                _animator.ResetTrigger("StrongAttack");
                return;
            }

            if (activeCombatMove.CombatMoveType == CombatMoveType.SingleHit
                ||
                activeCombatMove.CombatMoveType == CombatMoveType.ComboFinisher)
            {
                _animator.ResetTrigger("ComboTimeout");
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
        /// Called when [current weapon changed].
        /// </summary>
        /// <param name="equipableWeapon">The equipable weapon.</param>
        private void OnCurrentWeaponChanged(EquipableWeaponBase equipableWeapon)
        {
            _animator.SetBool("HoldWeapon", equipableWeapon.WeaponGripLeftHand != null || equipableWeapon.WeaponGripRightHand != null);
        }

        /// <summary>
        /// Called when [fire1 changed].
        /// </summary>
        /// <param name="fire1">if set to <c>true</c> [fire1].</param>
        private void OnFire1Changed(bool fire1)
        {
            _animator.SetBool("Fire1", fire1);
        }

        /// <summary>
        /// Called when [fire2 changed].
        /// </summary>
        /// <param name="fire2">if set to <c>true</c> [fire2].</param>
        private void OnFire2Changed(bool fire2)
        {
            _animator.SetBool("Fire2", fire2);
        }

        /// <summary>
        /// Called when [fire3 changed].
        /// </summary>
        /// <param name="fire3">if set to <c>true</c> [fire3].</param>
        private void OnFire3Changed(bool fire3)
        {
            _animator.SetBool("Fire3", fire3);
        }
    }
}
