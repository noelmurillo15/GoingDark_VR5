using System;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterControlSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.SkillSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.CombatSkillSystem
{
    /// <summary>
    /// Base view for combat skills. Requires a mecanim animator and is responsible for setting the animotor in the correct state based on the models current state.
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(Animator))]
	public abstract class CombatSkillBaseView : SkillBaseView {


        #region Public properties
        public string SkillName;
        public bool NeedActiveTarget;
        public float SecondsCharging;
        public float SecondsReharging;
        public float SecondsFiring;
        public CombatSkillFireType FireType;
        public CombatSkillAttackType AttackType;

        public StatusEffectComboView StatusEffectComboView;
		#endregion

        // short hand access props
        public abstract CombatEntityBase OwnerCombatEntity { get; }
        public abstract Transform CombatEntityTransform { get; }
        public abstract CharacterControllerBase OwnerCharacterController { get; }
        public abstract bool IsSustainingFire1 { get; set; }
        public abstract bool IsSustainingFire2 { get; set; }
        public abstract bool Fire1 { get; }
        public abstract bool Fire2 { get; }
        public abstract bool Attack1 { get; }
        public abstract bool Attack2 { get; }

        public CombatSkillBase CombatSkill;
        
        // private fields
        private UpdatePersistantGameObject _combatSkillStateIntentionUpdate;
        private Animator _animator;

		#region AGS Setup
        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            CombatSkill = model as CombatSkillBase;
            if (CombatSkill == null) return;
            CombatSkill.HitEffects.Value = StatusEffectComboView != null ? StatusEffectComboView.StatusEffectCombo : null;
            CombatSkill.ResourceCost.Value = ResourceEffectCostCombo != null ? ResourceEffectCostCombo.ResourceEffectCombo : null;
            CombatSkill.IsEnabled.OnValueChanged += (sender, isEnabled) => OnSkillStateChanged(isEnabled.Value);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);


            CombatSkill.CombatSkillCurrentState.OnValueChanged += (sender, isEnabled) => OnCurrentStateChanged(isEnabled.Value);
        }

        #endregion
        #region MonoBehaviours
        public override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }
        #endregion
        #region private
        /// <summary>
        /// Called when [skill state changed].
        /// Only update if enabled
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void OnSkillStateChanged(bool isEnabled)
        {
            if (isEnabled)
            {
                _combatSkillStateIntentionUpdate = ComponentExtensions.SetupComponent<UpdatePersistantGameObject>(gameObject);
                _combatSkillStateIntentionUpdate.UpdateMethod = () =>
                {
                    if (transform == null) return;
                    CombatSkill.Intention.Value = SetCombatSkillStateIntention();
                };
            }
            else if (_combatSkillStateIntentionUpdate != null)
            {
                _combatSkillStateIntentionUpdate.Stop();
            }
        }
        #endregion

        #region State machine functions
        /// <summary>
        /// Sets the combat skill state intention.
        /// </summary>
        /// <returns></returns>
        protected abstract CombatSkillStateIntention SetCombatSkillStateIntention();

        /// <summary>
        /// Called when [current state changed].
        /// Sets mecanim animator in the correct state
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void OnCurrentStateChanged(CombatSkillState currentState)
        {
            /* Idle = 0
             * Charging = 1
             * Firing = 2
             * SustainedFiring = 3
             * Recharing = 4
             */
            _animator.SetInteger("CombatSkillState", (int)currentState);
        }

        /// <summary>
        /// Called when [state enter idle].
        /// </summary>
        public abstract void OnStateEnterIdle();

        /// <summary>
        /// Called when [state enter charging].
        /// </summary>
        public abstract void OnStateEnterCharging();

        /// <summary>
        /// Called when [state enter firing].
        /// </summary>
        public abstract void OnStateEnterFiring();

        /// <summary>
        /// Called when [state enter sustained firing].
        /// </summary>
        public abstract void OnStateEnterSustainedFiring();

        /// <summary>
        /// Called when [state update sustained firing].
        /// </summary>
        public abstract void OnStateUpdateSustainedFiring();

        /// <summary>
        /// Called when [state enter recharging].
        /// </summary>
        public abstract void OnStateEnterRecharging();       
        #endregion
	}
}