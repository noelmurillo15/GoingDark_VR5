using System;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using UnityEngine;
using AGS.Core.Systems.CharacterControlSystem;

namespace AGS.Core.Systems.AimingSystem
{
    /// <summary>
    /// BaseView for aiming views
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class AimingBaseView : ActionView
    {
        public Aiming Aiming;

        private Animator _animator;

        /// <summary>
        /// Convenience property. Gets the owner combat entity.
        /// </summary>
        /// <value>
        /// The owner character controller.
        /// </value>
        protected CombatEntityBase OwnerCombatEntity
        {
            get
            {
                if (_ownerCombatEntity != null) return _ownerCombatEntity;
                if (Aiming.OwnerCombatEntity.Value != null)
                {
                    _ownerCombatEntity = Aiming.OwnerCombatEntity.Value;
                }
                return _ownerCombatEntity;
            }
        }
        private CombatEntityBase _ownerCombatEntity;

        /// <summary>
        /// Convenience property. Gets the owner player.
        /// </summary>
        /// <value>
        /// The owner player.
        /// </value>
        protected Player OwnerPlayer
        {
            get
            {
                if (_player != null) return _player;
                if (OwnerCombatEntity != null)
                {
                    var player = OwnerCombatEntity as Player;
                    if (player != null)
                    {
                        _player = player;
                    }
                }
                return _player;
            }
        }
        private Player _player;

        /// <summary>
        /// Convenience property. Gets the owner character controller.
        /// </summary>
        /// <value>
        /// The owner character controller.
        /// </value>
        protected CharacterControllerBase OwnerCharacterController
        {
            get
            {
                if (_ownerCharacterController != null) return _ownerCharacterController;
                if (Aiming.OwnerCombatEntity.Value != null
                    &&
                    Aiming.OwnerCombatEntity.Value.CharacterController.Value != null)
                {
                    _ownerCharacterController = Aiming.OwnerCombatEntity.Value.CharacterController.Value;
                }
                return _ownerCharacterController;
            }
        }
        private CharacterControllerBase _ownerCharacterController;

        #region AGS Setup
        public override void InitializeView()
        {
            Aiming = new Aiming();
            SolveModelDependencies(Aiming);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            // Subscribe to Aiming current state
            Aiming.AimingCurrentState.OnValueChanged += (sender, aiState) => OnCurrentStateChanged(aiState.Value);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);

        }
        #endregion

        #region MonoBehaviours

        public override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        public override void Update()
        {
            base.Update();
            Aiming.Intention.Value = SetAimingStateIntention();
        }
        #endregion

        #region State machine functions
        /// <summary>
        /// Sets the aiming state intention.
        /// </summary>
        /// <returns></returns>
        protected abstract AimingStateIntention SetAimingStateIntention();

        /// <summary>
        /// Called when [current state changed].
        /// Sets mecanim animator in the correct state
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void OnCurrentStateChanged(AimingStateMachineState currentState)
        {
            /* State ID's
             * Idle == 0
             * Aiming == 1
             * LockedOnTarget == 2 */
            _animator.SetInteger("AimingStateMachineState", (int)currentState);
        }

        /// <summary>
        /// Called when [state enter idle].
        /// </summary>
        public virtual void OnStateEnterIdle() { }

        /// <summary>
        /// Called when [state enter aiming].
        /// </summary>
        public virtual void OnStateEnterAiming() { }

        /// <summary>
        /// Called when [state update aiming].
        /// </summary>
        public virtual void OnStateUpdateAiming() { }

        /// <summary>
        /// Called when [state enter locked on target].
        /// </summary>
        public virtual void OnStateEnterLockedOnTarget() { }

        /// <summary>
        /// Called when [state update locked on target].
        /// </summary>
        public virtual void OnStateUpdateLockedOnTarget() { }
        #endregion
    }
}