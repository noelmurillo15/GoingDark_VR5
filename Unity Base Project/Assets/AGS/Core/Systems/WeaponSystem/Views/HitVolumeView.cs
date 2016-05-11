using System;
using System.Collections.Generic;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.CollisionReferences;
using AGS.Core.Enums;
using AGS.Core.Interfaces;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.CombatSkillSystem;
using AGS.Core.Systems.RagdollSystem;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Systems.InteractionSystem.Interactables;
using UnityEngine;

namespace AGS.Core.Systems.WeaponSystem
{
    /// <summary>
    /// HitVolumeView has a collider that is activated based on active combat move state and hit volume index.
    /// Multiple targets can be hit while active, the HitVolumeView tracks all targets it has hit so that it only hits each target once.
    /// </summary>
    [Serializable]
    public class HitVolumeView : ActionView
    {
        #region Public properties
        // Fields to be se in the editor
        public GameObject ParentGameObject; // A reference to the GameObject that holds weapon view.
        public HitVolumeIndex HitVolumeIndex;
        #endregion

        private List<IDamageable> _killablesAlreadyHit; // Record of IDamageables that has been hit while active
        private List<IMovable> _moveablesAlreadyHit; // Record of IMovables that has been hit while active
        private Collider _collider;

        public HitVolume HitVolume;

        private CombatMoveSet _activeMoveSet;
        private CombatMove _activeCombatMove;

        /// <summary>
        /// Convenience property. Gets the owner equipable weapon.
        /// </summary>
        /// <value>
        /// The owner equipable weapon.
        /// </value>
        protected EquipableWeaponBase OwnerEquipableWeapon
        {
            get
            {
                if (_ownerEquipableWeapon != null) return _ownerEquipableWeapon;
                if (HitVolume.OwnerEquipableWeapon != null)
                {
                    _ownerEquipableWeapon = HitVolume.OwnerEquipableWeapon.Value;
                }
                return _ownerEquipableWeapon;
            }
        }
        private EquipableWeaponBase _ownerEquipableWeapon;

        /// <summary>
        /// Convenience property. Gets the current combat move.
        /// </summary>
        /// <value>
        /// The current combat move.
        /// </value>
        protected CombatMove CurrentCombatMove
        {
            get
            {

                if (HitVolume.OwnerEquipableWeapon != null
                    &&
                    HitVolume.OwnerEquipableWeapon.Value.OwnerCombatEntity != null
                    &&
                    HitVolume.OwnerEquipableWeapon.Value.OwnerCombatEntity.Value.ActiveCombatMoveSet != null
                    &&
                    HitVolume.OwnerEquipableWeapon.Value.OwnerCombatEntity.Value.ActiveCombatMoveSet.Value.ActiveCombatMove != null)
                {
                    return HitVolume.OwnerEquipableWeapon.Value.OwnerCombatEntity.Value.ActiveCombatMoveSet.Value.ActiveCombatMove.Value;
                }
                return null;
            }
        }
        #region AGS Setup
        public override void InitializeView()
        {
            HitVolume = new HitVolume(transform, HitVolumeIndex);
            SolveModelDependencies(HitVolume);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            HitVolume.OwnerEquipableWeapon.OnValueChanged += (sender, ownerEquipableWeapon) =>
                {
                    if (ParentGameObject != null)
                    {
                        transform.position = ParentGameObject.transform.position;
                        transform.parent = ParentGameObject.transform;
                    }


                    ownerEquipableWeapon.Value.OwnerCombatEntity.OnValueChanged +=
                        (ownerCombatEntitySender, ownerCombatEntity) =>
                        {
                            ownerCombatEntity.Value.ActiveCombatMoveSet.OnValueChanged +=
                             (activeCombatMoveSetSender, activeMoveSet) =>
                             {
                                 if (gameObject.activeSelf) // Is this gameObject active in the scene?
                                 {
                                     _activeMoveSet = activeMoveSet.Value;
                                     // Subscribe to ActiveCombatMove
                                     _activeMoveSet.ActiveCombatMove.OnValueChanged += OnActiveCombatMoveChanged;
                                 }

                             };
                        };

                };


        }

        #endregion
        #region MonoBehaviours
        public override void Awake()
        {
            base.Awake();

            _collider = transform.GetComponent<Collider>();
            _killablesAlreadyHit = new List<IDamageable>();
            _moveablesAlreadyHit = new List<IMovable>();

            // Setup a collission enter notification with a KillableBaseView
            Action<KillableBaseView> addDamageableTargetAction = AddDamageableTarget;
            gameObject.OnCollisionActionEnterWith(addDamageableTargetAction);
            gameObject.OnTriggerActionEnterWith(addDamageableTargetAction);

            // Setup a collission enter notification with a RagdollBodyTrigger
            Action<RagdollBodyTrigger> addRagdollTargetAction = AddRagdollTarget;
            gameObject.OnCollisionActionEnterWith(addRagdollTargetAction);
            gameObject.OnTriggerActionEnterWith(addRagdollTargetAction);

            // Setup a collission enter notification with a MovableObjectBaseView
            Action<MovableObjectBaseView> addMovableTargetAction = AddMovableTarget;
            gameObject.OnCollisionActionEnterWith(addMovableTargetAction);
            gameObject.OnTriggerActionEnterWith(addMovableTargetAction);
        }

        void OnEnable()
        {
            if (_activeMoveSet == null) return;
            // Subscribe to ActiveCombatMove
            _activeMoveSet.ActiveCombatMove.OnValueChanged += OnActiveCombatMoveChanged;
        }
        void OnDisable()
        {
            if (_activeMoveSet == null) return;
            // Unsubscribe to ActiveCombatMove.
            _activeMoveSet.ActiveCombatMove.OnValueChanged -= OnActiveCombatMoveChanged;
        }
        #endregion

        #region private functions
        /// <summary>
        /// Called when [active combat move changed].
        /// </summary>
        /// <param name="activeMoveSetSender">The active move set sender.</param>
        /// <param name="activeCombatMove">The <see cref="ActionPropertyEventArgs{T}"/> instance containing the event data.</param>
        private void OnActiveCombatMoveChanged(object activeMoveSetSender, ActionPropertyEventArgs<CombatMove> activeCombatMove)
        {
            if (_activeCombatMove != null)
            {
                // If we alreade have a cached reference to active combat move, we unsubcribe to its skill change before we reset the cached reference
                _activeCombatMove.CombatSkillCurrentState.OnValueChanged -= OnCombatSkillCurrentStateChanged;
            }
            if (activeCombatMove.Value == null)
            {
                // If the activeCombatMove is null we also null the CurrentHitVolume, disable the collider, and clear hit reference lists
                if (OwnerEquipableWeapon.CurrentHitVolume.Value == HitVolume)
                {
                    OwnerEquipableWeapon.CurrentHitVolume.Value = null;
                }
                _collider.enabled = false;
                _killablesAlreadyHit.Clear();
                _moveablesAlreadyHit.Clear();
                return;
            }
            // Cache active combat move and subscribe to its state
            _activeCombatMove = activeCombatMove.Value;
            _activeCombatMove.CombatSkillCurrentState.OnValueChanged += OnCombatSkillCurrentStateChanged;
        }

        /// <summary>
        /// Called when [combat skill current state changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="state">The <see cref="ActionPropertyEventArgs{CombatSkillState}"/> instance containing the event data.</param>
        private void OnCombatSkillCurrentStateChanged(object sender, ActionPropertyEventArgs<CombatSkillState> state)
        {
            OnCurrentStateChanged(state.Value);
        }

        /// <summary>
        /// Called when [current state changed].
        /// </summary>
        /// <param name="state">The state.</param>
        private void OnCurrentStateChanged(CombatSkillState state)
        {
            if (_collider == null) return;
            if (_activeCombatMove.HitVolumeIndex != HitVolume.HitVolumeIndex) return;
            if (state == CombatSkillState.Firing || state == CombatSkillState.SustainedFiring)
            {
                // Set this to owner weapons current hit volume, and enable collider
                OwnerEquipableWeapon.CurrentHitVolume.Value = HitVolume;
                _collider.enabled = true;
            }

        }

        /// <summary>
        /// Adds the damageable target.
        /// </summary>
        /// <param name="killableView">The killable view.</param>
        private void AddDamageableTarget(KillableBaseView killableView)
        {
            if (_activeCombatMove.CombatSkillCurrentState.Value != CombatSkillState.Firing
                &&
                _activeCombatMove.CombatSkillCurrentState.Value != CombatSkillState.SustainedFiring) return;
            if (_killablesAlreadyHit.Contains(killableView.Killable))
            {
                return;
            }
            // Hit the killable and add it to _killablesAlreadyHit
            OwnerEquipableWeapon.HitKillable(killableView.Killable);
            _killablesAlreadyHit.Add(killableView.Killable);
        }

        /// <summary>
        /// Adds the ragdoll target.
        /// </summary>
        /// <param name="ragdollBody">The ragdoll body.</param>
        private void AddRagdollTarget(MonoBehaviour ragdollBody)
        {
            var ragdollView = ragdollBody.GetComponentInParent<RagdollView>();
            if (ragdollView.Ragdoll == null) return;
            AddIMoveableTarget(ragdollView.Ragdoll);
        }

        /// <summary>
        /// Adds the movable target.
        /// </summary>
        /// <param name="movableView">The movable view.</param>
        private void AddMovableTarget(MovableObjectBaseView movableView)
        {
            AddIMoveableTarget(movableView.MovableObject);
        }

        /// <summary>
        /// Adds the i moveable target.
        /// </summary>
        /// <param name="iMovable">The i movable.</param>
        private void AddIMoveableTarget(IMovable iMovable)
        {
            if (CurrentCombatMove == null
                ||
                (CurrentCombatMove.CombatSkillCurrentState.Value != CombatSkillState.Firing
                &&
                CurrentCombatMove.CombatSkillCurrentState.Value != CombatSkillState.SustainedFiring)) return;
            if (_moveablesAlreadyHit.Contains(iMovable))
            {
                return;
            }
            // Hit the iMovable and add it to _moveablesAlreadyHit
            OwnerEquipableWeapon.HitMovable(iMovable);
            _moveablesAlreadyHit.Add(iMovable);
        }
        #endregion
    }
}