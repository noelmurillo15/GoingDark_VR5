using System;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterControlSystem;
using AGS.Core.Systems.CharacterSystem;
using UnityEngine;

namespace AGS.Core.Systems.CombatSkillSystem
{
    /// <summary>
    /// One CombatMoveView per combat move
    /// </summary>
    [Serializable]
    public class CombatMoveView : CombatSkillBaseView
    {
        #region Public properties
        // props to be set in the editor
        public float ComboChainTimer;
        public CombatMoveType CombatMoveType;
        public CombatMovePosition RequiredPosition;
        public HitVolumeIndex HitVolumeIndex;
        #endregion

        public CombatMove CombatMove;

        #region short hand easy access props
        public override CombatEntityBase OwnerCombatEntity
        {
            get { return CombatMove.OwnerCombatMoveSet.Value.OwnerCombatEntity.Value; }
        }
        public override Transform CombatEntityTransform
        {
            get { return CombatMove.OwnerCombatMoveSet.Value.OwnerCombatEntity.Value.Transform; }
        }
        public override CharacterControllerBase OwnerCharacterController
        {
            get
            {
                if (CombatMove.OwnerCombatMoveSet.Value != null
                    &&
                    CombatMove.OwnerCombatMoveSet.Value.OwnerCombatEntity.Value != null
                    &&
                    CombatMove.OwnerCombatMoveSet.Value.OwnerCombatEntity.Value.CharacterController.Value != null)
                {
                    return CombatMove.OwnerCombatMoveSet.Value.OwnerCombatEntity.Value.CharacterController.Value;
                }
                return null;
            }
        }
        public override bool IsSustainingFire1 { get; set; }
        public override bool IsSustainingFire2 { get; set; }
        public override bool Fire1
        {
            get
            {
                if (OwnerCharacterController == null) return false;
                return OwnerCharacterController.Fire1.Value
                       &&
                       CombatMove.FireType == CombatSkillFireType.Sustained
                       &&
                       CombatMove.AttackType == CombatSkillAttackType.Primary;
            }
        }
        public override bool Fire2
        {
            get
            {
                if (OwnerCharacterController == null) return false;
                return OwnerCharacterController.Fire2.Value
                       &&
                       CombatMove.FireType == CombatSkillFireType.Sustained
                       &&
                       CombatMove.AttackType == CombatSkillAttackType.Secondary;
            }
        }
        public override bool Attack1
        {
            get
            {
                if (OwnerCharacterController == null) return false;
                return OwnerCharacterController.Attack1.Value
                       &&
                       CombatMove.FireType == CombatSkillFireType.OneShot
                       &&
                       CombatMove.AttackType == CombatSkillAttackType.Primary;
            }
        }
        public override bool Attack2
        {
            get
            {
                if (OwnerCharacterController == null) return false;
                return OwnerCharacterController.Attack2.Value
                       &&
                       CombatMove.FireType == CombatSkillFireType.OneShot
                       &&
                       CombatMove.AttackType == CombatSkillAttackType.Secondary;
            }
        }
        #endregion

        #region AGS Setup
        public override void InitializeView()
        {
            CombatMove = new CombatMove(SkillName, NeedActiveTarget, SecondsCharging, SecondsReharging, SecondsFiring,
            FireType, AttackType, ComboChainTimer, CombatMoveType, RequiredPosition, HitVolumeIndex);
            SolveModelDependencies(CombatMove);
        }
        #endregion
        #region state machine functions
        protected override CombatSkillStateIntention SetCombatSkillStateIntention()
        {
            if (CombatMove.IsEnabled.Value == false
                ||
                CombatMove.CombatSkillCurrentState.Value != CombatSkillState.Idle)
            {
                return CombatSkillStateIntention.None;
            }
            if (Fire1 || Fire2)
            {
                CombatMove.CheckSupplyContinuousResourceEffects(OwnerCombatEntity);
                if (!CombatMove.OutOfResources.Value)
                {
                    IsSustainingFire1 = Fire1;
                    IsSustainingFire2 = Fire2;
                    return CombatMove.SecondsCharging > 0f
                        ? CombatSkillStateIntention.Charge
                        : CombatSkillStateIntention.SustainedFire;
                }
            }
            if (Attack1 || Attack2)
            {
                CombatMove.CheckSupplyResourceEffects(OwnerCombatEntity);
                if (!CombatMove.OutOfResources.Value)
                {
                    IsSustainingFire1 = false;
                    IsSustainingFire2 = false;
                    return CombatMove.SecondsCharging > 0f ? CombatSkillStateIntention.Charge : CombatSkillStateIntention.Fire;
                }
            }
            return CombatSkillStateIntention.None;
        }

        /// <summary>
        /// Called when [state enter idle].
        /// </summary>
        public override void OnStateEnterIdle()
        {
            // Do nothing
        }

        /// <summary>
        /// Called when [state enter charging].
        /// </summary>
        public override void OnStateEnterCharging()
        {
            // sets up a timer and then transitions to SustainedFire or Fire depending on input
            var stateCountDownTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Charging State CountDown Timer");
            stateCountDownTimer.TimerMethod = () =>
            {
                if (IsSustainingFire1 || IsSustainingFire2)
                {
                    CombatSkill.CombatSkillTransitionToStateSustainedFire();
                }
                else
                {
                    CombatSkill.CombatSkillTransitionToStateFire();
                }
            };
            stateCountDownTimer.Invoke(CombatSkill.SecondsCharging);
        }

        /// <summary>
        /// Called when [state enter firing].
        /// </summary>
        public override void OnStateEnterFiring()
        {
            if(OwnerCombatEntity.CurrentWeapon.Value != null)
            {
                OwnerCombatEntity.CurrentWeapon.Value.FireWeapon();
            }
            // Apply resource cost and set up a timer for transitioning to recharge state
            CombatSkill.ApplyResourceCost(OwnerCombatEntity);
            if (CombatSkill.SecondsFiring > 0)
            {
                var stateCountDownTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Firing State CountDown Timer");
                stateCountDownTimer.TimerMethod = () => CombatSkill.CombatSkillTransitionToStateRecharge();
                stateCountDownTimer.Invoke(CombatSkill.SecondsFiring);
            }
            else
            {
                CombatSkill.CombatSkillTransitionToStateRecharge();
            }
        }

        /// <summary>
        /// Called when [state enter sustained firing].
        /// </summary>
        public override void OnStateEnterSustainedFiring()
        {
            // activate continuous resource costs
            CombatSkill.ActivateContinuousResourceCosts(OwnerCombatEntity);
        }

        /// <summary>
        /// Called when [state update sustained firing].
        /// </summary>
        public override void OnStateUpdateSustainedFiring()
        {
            // check resources
            CombatSkill.CheckSupplyContinuousResourceEffects(OwnerCombatEntity);
            if (CombatSkill.OutOfResources.Value)
            {
                CombatSkill.CombatSkillTransitionToStateRecharge();
            }
            // check for fire button release
            if (IsSustainingFire1 && !Fire1)
            {
                CombatSkill.CombatSkillTransitionToStateRecharge();
            }
            if (IsSustainingFire2 && Fire2)
            {
                CombatSkill.CombatSkillTransitionToStateRecharge();
            }
        }

        /// <summary>
        /// Called when [state enter recharging].
        /// </summary>
        public override void OnStateEnterRecharging()
        {
            // Deactivate any continuous resource costs for sustained fire
            if (IsSustainingFire1 || IsSustainingFire2)
            {
                CombatSkill.DeactivateContinuousResourceCosts();
            }
            IsSustainingFire1 = false;
            IsSustainingFire2 = false;

            // set up timer for transitioning to idle state
            if (CombatSkill.SecondsRecharging > 0)
            {
                var stateCountDownTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Recharging State CountDown Timer");
                stateCountDownTimer.TimerMethod = () => CombatSkill.CombatSkillTransitionToStateIdle();
                stateCountDownTimer.Invoke(CombatSkill.SecondsRecharging);
            }
            else
            {
                CombatSkill.CombatSkillTransitionToStateIdle();
            }
        }
        #endregion
    }
}