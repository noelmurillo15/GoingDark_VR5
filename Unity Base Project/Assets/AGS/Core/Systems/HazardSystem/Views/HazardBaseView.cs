using System;
using AGS.Core.Classes.CollisionReferences;
using AGS.Core.Classes.ViewScripts;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.InteractionSystem;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Systems.InteractionSystem.Interactables;
using UnityEngine;

namespace AGS.Core.Systems.HazardSystem
{
    /// <summary>
    /// BaseHazardView requires a mecanim animator and is responsible for setting the animator in the correct state based on the models current state.
    /// This base view should only be inherited by non-area hazard views, since it requires collision detection implementation. For area hazards, collisions are handled by the AreaOfEffectView
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class HazardBaseView : ActionView
    {
        #region Public properties
        // fiels to be set in editor
        public float SecondsActive;
        public float SecondsRecharging;
        public bool DeactivateOnTrigger;
        public bool DestroyOnTrigger;
        #endregion

        public HazardBase HazardBase;
        private Animator _animator;
        protected HazardFX HazardEffects;

        #region AGS Setup
        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            HazardBase = model as HazardBase;
            if (HazardBase == null) return;
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            if (model == null) return;

            // Subscribe to the hazard state
            HazardBase.HazardCurrentState.OnValueChanged += (sender, state) => OnCurrentStateChanged(state.Value);
            // Default activation on init
            HazardBase.TransitionToStateActivate();

            // Sets up collision & trigger detection subscription with KillableBaseView components
            Action<KillableBaseView> onCollisionKillableAction = OnCollisionKillableNotification;
            foreach (var childTransform in gameObject.GetComponentsInChildren<Collider>())
            {
                childTransform.gameObject.OnCollisionActionEnterWith(onCollisionKillableAction);
                childTransform.gameObject.OnTriggerActionEnterWith(onCollisionKillableAction);
            }

            // Sets up collision & trigger detection subscription with RagdollBodyTrigger components
            Action<MovableObjectBaseView> onCollisionMovableNotification = OnCollisionMovableNotification;
            foreach (var childTransform in gameObject.GetComponentsInChildren<Collider>())
            {
                childTransform.gameObject.OnCollisionActionEnterWith(onCollisionMovableNotification);
                childTransform.gameObject.OnTriggerActionEnterWith(onCollisionMovableNotification);
            }

            // Sets up collision & trigger detection subscription with RagdollBodyTrigger components
            Action<RagdollBodyTrigger> onCollisionRagdollNotification = OnCollisionRagdollNotification;
            foreach (var childTransform in gameObject.GetComponentsInChildren<Collider>())
            {
                childTransform.gameObject.OnCollisionActionEnterWith(onCollisionRagdollNotification);
                childTransform.gameObject.OnTriggerActionEnterWith(onCollisionRagdollNotification);
            }

            if (HazardEffects != null)
            {
                HazardEffects.ParticlesSystemsDoneAction += () => Destroy(gameObject);    
            }
            
        }

        #endregion
        #region MonoBehaviours
        public override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
            HazardEffects = GetComponent<HazardFX>();
        }
        #endregion
        #region State machine functions
        /// <summary>
        /// Called when [current state changed].
        /// Sets mecanim animator in the correct state
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void OnCurrentStateChanged(HazardState currentState)
        {
            /* Active == 0,
             * Recharging == 1
             * Inactive == 2 */
            _animator.SetInteger("HazardState", (int)currentState);
        }

        /// <summary>
        /// Called when [state enter active].
        /// </summary>
        public abstract void OnStateEnterActive();

        /// <summary>
        /// Called when [state enter recharging].
        /// </summary>
        public abstract void OnStateEnterRecharging();

        /// <summary>
        /// Called when [state enter inactive].
        /// </summary>
        public abstract void OnStateEnterInactive();
        #endregion

        #region collision notifications
        /// <summary>
        /// Called when [collision killable notification].
        /// </summary>
        /// <param name="killableView">The killable view.</param>
        protected abstract void OnCollisionKillableNotification(KillableBaseView killableView);

        /// <summary>
        /// Called when [collision movable notification].
        /// </summary>
        /// <param name="obj">The object.</param>
        protected abstract void OnCollisionMovableNotification(MovableObjectBaseView obj);

        /// <summary>
        /// Called when [collision ragdoll notification].
        /// </summary>
        /// <param name="ragdollBody">The ragdoll body.</param>
        protected abstract void OnCollisionRagdollNotification(MonoBehaviour ragdollBody);

        #endregion
        #region public functions
        /// <summary>
        /// Activates this hazard.
        /// </summary>
        public virtual void Activate()
        {
            HazardBase.TransitionToStateActivate();
        }

        /// <summary>
        /// Deactivates this hazard.
        /// </summary>
        public virtual void Deactivate()
        {
            HazardBase.TransitionToStateDeactivate();
        }

        /// <summary>
        /// Recharges this hazard.
        /// </summary>
        public virtual void Recharge()
        {
            HazardBase.TransitionToStateRecharge();
        }
        #endregion
    }
}