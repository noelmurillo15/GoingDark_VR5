using System;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterControlSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.CombatSkillSystem
{
    /// <summary>
    /// Not yet implemented
    /// </summary>
    [Serializable]
    public class CombatAbilityView : CombatSkillBaseView
    {
		
		#region Public properties
        public AreaOfEffectView AreaOfEffectView;
		#endregion

		public CombatAbility CombatAbility;
		
		#region AGS Setup
        public override void InitializeView()
        {
            CombatAbility = new CombatAbility(SkillName, NeedActiveTarget, SecondsCharging, SecondsReharging, SecondsFiring, FireType, AttackType);
            SolveModelDependencies(CombatAbility);
        }

        public override CombatEntityBase OwnerCombatEntity
        {
            get { throw new NotImplementedException(); }
        }

        public override Transform CombatEntityTransform
        {
            get { throw new NotImplementedException(); }
        }

        public override CharacterControllerBase OwnerCharacterController
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsSustainingFire1 { get; set; }
        public override bool IsSustainingFire2 { get; set; }

        public override bool Fire1
        {
            get { throw new NotImplementedException(); }
        }

        public override bool Fire2
        {
            get { throw new NotImplementedException(); }
        }

        public override bool Attack1
        {
            get { throw new NotImplementedException(); }
        }

        public override bool Attack2
        {
            get { throw new NotImplementedException(); }
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            CombatAbility.AreaOfEffect.Value = AreaOfEffectView != null ? AreaOfEffectView.AreaOfEffect : null;
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
        }
        #endregion

        #region state machine functions
        protected override CombatSkillStateIntention SetCombatSkillStateIntention()
        {
            throw new NotImplementedException();
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