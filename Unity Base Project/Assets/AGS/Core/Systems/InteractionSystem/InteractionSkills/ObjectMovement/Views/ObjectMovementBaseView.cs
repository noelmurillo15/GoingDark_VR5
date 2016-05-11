using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.InteractionSystem.Base;
using AGS.Core.Systems.InteractionSystem.Interactables;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.InteractionSkills.ObjectMovement
{
    /// <summary>
    /// ObjectMovementBaseView requires a mecanim animator and is responsible for setting the animator in the correct state based on the models current state.
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class ObjectMovementBaseView : InteractionSkillBaseView
    {
        #region Public properties
        // Fields to be set in the editor
        public float PickUpSpeed;
        public MovableObjectWeight MaxPushWeight;
        public MovableObjectWeight MaxCarryWeight;
        public float OffsetHorizontalLow;
        public float OffsetHorizontalHeavy;
        public float PushSpeedNormal;
        public float PullSpeedNormal;
        public Vector2 ThrowSpeedNormal;
        public float PushSpeedLight;
        public float PullSpeedLight;
        public Vector2 ThrowSpeedLight;
        public float PushSpeedHeavy;
        public float PullSpeedHeavy;
        public Vector2 ThrowSpeedHeavy;
        public float PushSpeedMassive;
        public Vector2 ThrowSpeedTiny;
        public float HeavyCarryRelativeHeight;
		#endregion

        protected Vector2 ActiveThrowSpeed;
        /// <summary>
        /// Convenience property. Gets the current movable object.
        /// </summary>
        /// <value>
        /// The current movable object.
        /// </value>
        public MovableObject CurMovableObject
        {
            get
            {
                return CurrentInteractableTarget as MovableObject;
            }
        }

        public ObjectMovement ObjectMovement;
        protected float PushSpeed { get; set; }
        protected float PullSpeed { get; set; }

        private UpdatePersistantGameObject _objectMovementIntentionUpdater;
        public bool FromCarrying { get; set; }
        public bool ImmediateCarry { get; set; }
        private Animator _animator;
        private Vector3 _targetPosition;
        private float _smoothness;

        #region AGS Setup
        public override void InitializeView()
        {
            ObjectMovement = new ObjectMovement(ApproachMargin, OffsetVertical, OffsetHorizontal, PickUpSpeed,
                MaxPushWeight, MaxCarryWeight, OffsetHorizontalLow, OffsetHorizontalHeavy, PushSpeedNormal, PullSpeedNormal, ThrowSpeedNormal,
                PushSpeedLight, PullSpeedLight, ThrowSpeedLight, PushSpeedHeavy, PullSpeedHeavy, ThrowSpeedHeavy, PushSpeedMassive, ThrowSpeedTiny, HeavyCarryRelativeHeight);       
            SolveModelDependencies(ObjectMovement);
        }

		public override void InitializeActionModel(ActionModel model)
        {
			base.InitializeActionModel(model);
            ObjectMovement.IsEnabled.OnValueChanged += (sender, isEnabled) => OnSkillStateChanged(isEnabled.Value);                 
        }
        #endregion
        #region MonoBehaviours
        public override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }
        #endregion

        #region State machine functions
        /// <summary>
        /// Sets the object movement state intention.
        /// Intention input should always be implemented in the child view since controls differs from game to game.
        /// </summary>
        /// <returns></returns>
        protected abstract ObjectMovementIntention SetObjectMovementStateIntention();

        /// <summary>
        /// Called when [current state changed].
        /// Sets mecanim animator in the correct state
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void OnCurrentStateChanged(ObjectMovementState currentState)
        {
            /* ObjectMovementStates 
             * Idle == 0
             * Approaching == 1
             * Grabbing == 2
             * Releasing == 3
             * Carrying == 4
             * Throwing == 5 */
            _animator.SetInteger("MovableObjectState", (int)currentState);
        }

        /// <summary>
        /// Called when [state enter idle].
        /// </summary>
        public override void OnStateEnterIdle()
        {
            if (FromCarrying)
            {
                // If character has been carrying - reset character height
                var resetHeightTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Reset height timer");
                resetHeightTimer.TimerMethod = ResetOriginalHeight;
                resetHeightTimer.Invoke(0.2f);
            }
        }

        /// <summary>
        /// Called when [state enter approaching].
        /// </summary>
        public override void OnStateEnterApproaching()
        {
            if (ImmediateCarry)
            {
                ObjectMovement.Intention.Value = ObjectMovementIntention.Carry;
                return;
            }
            OwnerCharacter.UsePhysics(false);
            _smoothness = 3f * Time.deltaTime;
            _targetPosition = GetTargetPosition();
        }

        /// <summary>
        /// Called when [state update approaching].
        /// </summary>
        public override void OnStateUpdateApproaching()
        {
            if (CurMovableObject == null)
            {
                ObjectMovement.TransitionToStateRelease();
                return;
            }
            
            // Move towards target until approach margin is reached
            MoveTowardsInteractableTarget(_targetPosition, _smoothness);
            
            if (ReachedTargetPosition(_targetPosition, ObjectMovement.ApproachMargin))
            {
                if (CurMovableObject != null)
                {
                    CurMovableObject.Grab(OwnerCharacter);
                    ObjectMovement.TransitionToStateInteract();
                }
                else
                {
                    ObjectMovement.TransitionToStateRelease(); // Something went wrong. Movable may have been moved. Release.
                }
            }
        }

        /// <summary>
        /// Called when [state enter releasing].
        /// </summary>
        public override void OnStateEnterReleasing()
        {
            OwnerCharacter.UsePhysics(true);
            ImmediateCarry = false;
            if (CurMovableObject == null)
                return;
            
            // Tell the movable that its being released
            CurMovableObject.Release();
        }

        public virtual void OnStateUpdateReleasing()
        {
            // Transition to idle when movable is properly released
            if (CurMovableObject != null)
            {
                if (CurMovableObject.MovableObjectCurrentState.Value == MovableObjectState.Idle)
                {
                    ObjectMovement.OwnerInteractionSkills.Value.ForceClear();
                    ObjectMovement.TransitionToStateIdle();
                }
                
            }
            else
            {
                ObjectMovement.TransitionToStateIdle();
            }
        }

        /// <summary>
        /// Called when [state enter grabbing].
        /// </summary>
        public virtual void OnStateEnterGrabbing()
        {
            OwnerCharacter.UsePhysics(true);
            PushSpeed = 0;
            PullSpeed = 0;
            // We have different speeds for different movable sizes
            switch (CurMovableObject.Weight)
            {
                case MovableObjectWeight.Light:
                    PushSpeed = ObjectMovement.PushSpeedLight;
                    PullSpeed = ObjectMovement.PullSpeedLight;
                    break;
                case MovableObjectWeight.Medium:
                    PushSpeed = ObjectMovement.PushSpeedNormal;
                    PullSpeed = ObjectMovement.PullSpeedNormal;
                    break;
                case MovableObjectWeight.Heavy:
                    PushSpeed = ObjectMovement.PushSpeedHeavy;
                    PullSpeed = 0;
                    break;
                case MovableObjectWeight.Massive:
                    PushSpeed = ObjectMovement.PushSpeedMassive;
                    PullSpeed = 0;
                    break;
            }
        }

        /// <summary>
        /// Called when [state update grabbing].
        /// </summary>
        public virtual void OnStateUpdateGrabbing()
        {
            if (CurMovableObject == null)
            {
                // Lost movable object
                ObjectMovement.TransitionToStateRelease();
                return;
            }
            if (ObjectMovement.MaxPushWeight.Value < CurMovableObject.Weight)
            {
                // Character can no longer push this movable. May happen if character had a temporary strength increase that runs out while pushing.
                ObjectMovement.TransitionToStateRelease();
                return;
            }
        }

        /// <summary>
        /// Called when [state enter throwing].
        /// </summary>
        public virtual void OnStateEnterThrowing()
        {
            ActiveThrowSpeed = Vector2.zero;
            // We have different throwing speeds for different movable sizes
            switch (CurMovableObject.Weight)
            {
                case MovableObjectWeight.Tiny:
                    ActiveThrowSpeed = ObjectMovement.ThrowSpeedTiny;
                    break;
                case MovableObjectWeight.Light:
                    ActiveThrowSpeed = ObjectMovement.ThrowSpeedLight;
                    break;
                case MovableObjectWeight.Medium:
                    ActiveThrowSpeed = ObjectMovement.ThrowSpeedNormal;
                    break;
                case MovableObjectWeight.Heavy:
                    ActiveThrowSpeed = ObjectMovement.ThrowSpeedHeavy;
                    break;
            }
        }

        /// <summary>
        /// Called when [state enter carrying].
        /// </summary>
        public virtual void OnStateEnterCarrying()
        {
            OwnerCharacter.UsePhysics(true);
            //ObjectMovementBaseView.CurMovableObject.Release();

            if (ObjectMovement.MaxCarryWeight.Value < CurMovableObject.Weight)
            {
                // Character cannot carry this object
                ObjectMovement.TransitionToStateRelease();
                return;
            }

            FromCarrying = true;
            SetCarryingHeight();
            // Tell the movable that we are picking it up!
            CurMovableObject.PickUp(OwnerCharacter);
        }

        /// <summary>
        /// Called when [state update carrying].
        /// </summary>
        public virtual void OnStateUpdateCarrying()
        {
            if (ObjectMovement.MaxCarryWeight.Value < CurMovableObject.Weight)
            {
                // Character can no longer carry this movable. May happen if character had a temporary strength increase that runs out while carrying.
                ObjectMovement.TransitionToStateRelease();
            }
        }
        #endregion

        #region private functions
        /// <summary>
        /// Called when [skill state changed].
        /// Only subscribe to ObjectMovementCurrentState and only run the intention update method if skill is enabled.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void OnSkillStateChanged(bool isEnabled)
        {
            if (isEnabled)
            {
                _objectMovementIntentionUpdater = ComponentExtensions.SetupComponent<UpdatePersistantGameObject>(gameObject);
                _objectMovementIntentionUpdater.UpdateMethod = () =>
                {
                    ObjectMovement.Intention.Value = SetObjectMovementStateIntention();
                };
                ObjectMovement.ObjectMovementCurrentState.OnValueChanged += (sender, state) => OnCurrentStateChanged(state.Value);
            }
            else
            {
                if (_objectMovementIntentionUpdater != null)
                {
                    _objectMovementIntentionUpdater.Stop();
                }
                ObjectMovement.ObjectMovementCurrentState.OnValueChanged -= OnObjectMovementDisabled;
            }
        }

        /// <summary>
        /// Called when [object movement disabled].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="state">The <see cref="ActionPropertyEventArgs{T}"/> instance containing the event data.</param>
        private void OnObjectMovementDisabled(object sender, ActionPropertyEventArgs<ObjectMovementState> state)
        {
            OnCurrentStateChanged(state.Value);
        }

        #endregion
        #region abstract functions
        /// <summary>
        /// Resets the original height of the character.
        /// </summary>
        public abstract void ResetOriginalHeight();

        /// <summary>
        /// Sets the carry height of the character.
        /// </summary>
        public abstract void SetCarryingHeight();

        /// <summary>
        /// Moves the character together with movable object.
        /// </summary>
        /// <param name="curSpeed">The current speed.</param>
        public abstract void MoveWithObject(float curSpeed);
        #endregion
    }
}