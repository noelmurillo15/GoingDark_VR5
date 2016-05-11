using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.Helpers;
using AGS.Core.Enums;
using AGS.Core.Interfaces;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.SkillSystem;
using AGS.Core.Systems.StatusEffectSystem;

namespace AGS.Core.Systems.CombatSkillSystem
{
    /// <summary>
    /// Any type combat skill should inherit from this. Skill states are used with mecanim behaviours
    /// </summary>
    public class CombatSkillBase : SkillBase
    {

        #region Properties
        // Constructor properties        
        public string SkillName { get; private set; }
        public bool NeedActiveTarget { get; private set; }
        public float SecondsCharging { get; private set; }
        public float SecondsRecharging { get; private set; }
        public float SecondsFiring { get; private set; }
        public CombatSkillFireType FireType { get; private set; }
        public CombatSkillAttackType AttackType { get; private set; }

        // Subscribable properties
        public ActionProperty<StatusEffectCombo> HitEffects { get; private set; } // All types of effects associated with the skill
        public ActionProperty<CombatSkillState> CombatSkillCurrentState { get; private set; } // State of the skill.
        public ActionProperty<CombatSkillStateIntention> Intention { get; private set; } // Skill intention. Typically set by input and will result in new state if state requirements are met

        public Action SkillHitAction { get; set; } // Subscribe to this to get notification of skill hits
        #endregion Properties


        /// <summary>
        /// Initializes a new instance of the <see cref="CombatSkillBase"/> class.
        /// </summary>
        /// <param name="skillName">Name of the skill.</param>
        /// <param name="needActiveTarget">if set to <c>true</c> [need active target].</param>
        /// <param name="secondsCharging">Determines how many seconds does it take to charge this skill</param>
        /// <param name="secondsRecharging">Determines how many seconds does it take to recharge this skill</param>
        /// <param name="secondsFiring">Determines how many seconds does it take to fire this skill</param>
        /// <param name="combatSkillFireType">FireType of the combat skill.</param>
        /// <param name="combatSkillAttackType">AttackType of the combat skill.</param>
        public CombatSkillBase(string skillName, bool needActiveTarget, float secondsCharging, float secondsRecharging, float secondsFiring,
            CombatSkillFireType combatSkillFireType, CombatSkillAttackType combatSkillAttackType)
        {
            SkillName = skillName;
            NeedActiveTarget = needActiveTarget;
            SecondsCharging = secondsCharging;
            SecondsRecharging = secondsRecharging;
            SecondsFiring = secondsFiring;
            FireType = combatSkillFireType;
            AttackType = combatSkillAttackType;

            HitEffects = new ActionProperty<StatusEffectCombo>();
            CombatSkillCurrentState = new ActionProperty<CombatSkillState>();
            Intention = new ActionProperty<CombatSkillStateIntention>() { Value = CombatSkillStateIntention.None };
            Intention.OnValueChanged += (sender, intention) => SetCombatSkillState(intention.Value);
        }

        #region private functions
        /// <summary>
        /// Sets the state of the combat skill based on the intention.
        /// </summary>
        /// <param name="intention">The intention.</param>
        private void SetCombatSkillState(CombatSkillStateIntention intention)
        {
            switch (intention)
            {
                case CombatSkillStateIntention.Charge:
                    CombatSkillTransitionToStateCharge();
                    break;
                case CombatSkillStateIntention.Fire:
                    CombatSkillTransitionToStateFire();
                    break;
                case CombatSkillStateIntention.SustainedFire:
                    CombatSkillTransitionToStateSustainedFire();
                    break;
            }
        }
        #endregion

        #region state transitions
        public void CombatSkillTransitionToStateIdle()
        {
            if (CombatSkillCurrentState.Value == CombatSkillState.Recharing)
            {
                CombatSkillCurrentState.Value = CombatSkillState.Idle;
            }
        }
        public void CombatSkillTransitionToStateCharge()
        {
            if (CombatSkillCurrentState.Value == CombatSkillState.Idle)
            {
                CombatSkillCurrentState.Value = CombatSkillState.Charging;
            }
        }

        public void CombatSkillTransitionToStateFire()
        {
            if (CombatSkillCurrentState.Value == CombatSkillState.Idle
                ||
                CombatSkillCurrentState.Value == CombatSkillState.Charging)
            {
                CombatSkillCurrentState.Value = CombatSkillState.Firing;
            }

        }

        public void CombatSkillTransitionToStateSustainedFire()
        {
            if (CombatSkillCurrentState.Value == CombatSkillState.Idle
                ||
                CombatSkillCurrentState.Value == CombatSkillState.Charging)
            {
                CombatSkillCurrentState.Value = CombatSkillState.SustainedFiring;
            }

        }
        public void CombatSkillTransitionToStateRecharge()
        {
            if (CombatSkillCurrentState.Value == CombatSkillState.Firing
                ||
                CombatSkillCurrentState.Value == CombatSkillState.SustainedFiring)
            {
                CombatSkillCurrentState.Value = CombatSkillState.Recharing;
            }

        }
        #endregion

        #region public functions

        /// <summary>
        /// Triggers the combat skill.
        /// </summary>
        public void TriggerCombatSkill()
        {
            if (SkillHitAction != null)
            {
                SkillHitAction();    
            }            
        }

        /// <summary>
        /// Hits the killable with all hit effects.
        /// </summary>
        /// <param name="targetKillable">The target killable.</param>
        /// <param name="hittingFromBehind">if set to <c>true</c> [hitting from behind].</param>
        public void HitKillable(KillableBase targetKillable, bool hittingFromBehind)
        {
            TriggerCombatSkill();
            if (HitEffects.Value == null) return;
            CombatHelper.ApplyResourceEffects(targetKillable, HitEffects.Value.ResourceEffects, hittingFromBehind);
            CombatHelper.ApplySuperNaturalEffects(targetKillable, HitEffects.Value.SuperNaturalEffectsEffects);
            var character = targetKillable as CharacterBase;
            if (character == null) return;
            CombatHelper.ApplyPushEffects(character, HitEffects.Value.PushEffects, hittingFromBehind);
            CombatHelper.ApplyMovementffects(character, HitEffects.Value.MovementEffects);
        }

        /// <summary>
        /// Hits any object that implements IMovable with the skills pusheffects.
        /// </summary>
        /// <param name="targetMovable">The target movable.</param>
        /// <param name="hittingFromBehind">if set to <c>true</c> [hitting from behind].</param>
        public void HitMovable(IMovable targetMovable, bool hittingFromBehind)
        {
            TriggerCombatSkill();
            if (HitEffects.Value == null) return;
            if (targetMovable == null) return;
            CombatHelper.ApplyPushEffects(targetMovable, HitEffects.Value.PushEffects);
        }
        #endregion
    }
}
