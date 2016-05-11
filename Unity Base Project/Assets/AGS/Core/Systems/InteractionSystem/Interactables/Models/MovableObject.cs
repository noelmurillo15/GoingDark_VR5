using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Interfaces;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.Interactables
{
    /// <summary>
    /// Movables implement IMovable and thus are movable by push effects etc
    /// </summary>
    public class MovableObject : InteractableBase, IMovable
    {
        #region Properties
        // Constructor properties
        public MovableObjectWeight Weight { get; private set; } // This 
        public float SecondsReInteract { get; private set; } // 

        // Subscribable properties
        public ActionProperty<bool> IsGrounded { get; private set; }
        public Vector3 ThrowForce { get; private set; } // Speed to throw this object
        public ActionProperty<MovableObjectState> MovableObjectCurrentState { get; private set; } // Current state of the object
        public ActionProperty<AdvancedCharacterBase> ConnectedCharacter { get; private set; } // Reference to the character that is currently handling this object

        // Subscrible to notify view of an applied push effect
        public Action<PushEffect, bool> PushEffectAction { get; set; }
        public Func<bool> IsStillFunc { get; set; } 
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="MovableObject"/> class.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="weight">The weight determines how a character is handling the object.</param>
        /// <param name="secondsReInteract">Determines how fast a character can interact with this object again after releasing it.</param>
        public MovableObject(Transform transform, MovableObjectWeight weight, float secondsReInteract)
            : base(transform)
        {

            SecondsReInteract = secondsReInteract;
            IsGrounded = new ActionProperty<bool>();
            MovableObjectCurrentState = new ActionProperty<MovableObjectState>() { Value = MovableObjectState.Idle };
            Weight = weight;
            ConnectedCharacter = new ActionProperty<AdvancedCharacterBase>();
        }


        #region state transitions
        /// <summary>
        /// Transitions to state idle.
        /// </summary>
        public void TransitionToStateIdle()
        {
            if (MovableObjectCurrentState.Value == MovableObjectState.Grabbed
                ||
                MovableObjectCurrentState.Value == MovableObjectState.Carried
                ||
                MovableObjectCurrentState.Value == MovableObjectState.Thrown)
            {
                MovableObjectCurrentState.Value = MovableObjectState.Idle;
                //// Set up the timer for when the object should transition to idle and be interactable again
                //var releaseTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Movable Object release timer");
                //releaseTimer.TimerMethod = () => MovableObjectCurrentState.Value = MovableObjectState.Idle;
                //releaseTimer.Invoke(SecondsReInteract);
            }

        }

        /// <summary>
        /// Transitions to state grab.
        /// </summary>
        public void TransitionToStateGrab()
        {
            if (MovableObjectCurrentState.Value == MovableObjectState.Idle)
            {
                MovableObjectCurrentState.Value = MovableObjectState.Grabbed;
            }
        }

        /// <summary>
        /// Transitions to state pick up.
        /// </summary>
        public void TransitionToStatePickUp()
        {
            if (MovableObjectCurrentState.Value == MovableObjectState.Idle
                ||
                MovableObjectCurrentState.Value == MovableObjectState.Grabbed)
            {
                MovableObjectCurrentState.Value = MovableObjectState.PickedUp;
            }
        }

        /// <summary>
        /// Transitions to state carry.
        /// </summary>
        public void TransitionToStateCarry()
        {
            if (MovableObjectCurrentState.Value == MovableObjectState.PickedUp)
            {
                MovableObjectCurrentState.Value = MovableObjectState.Carried;
            }
        }

        /// <summary>
        /// Transitions to state throw.
        /// </summary>
        public void TransitionToStateThrow()
        {
            if (MovableObjectCurrentState.Value == MovableObjectState.Carried)
            {
                MovableObjectCurrentState.Value = MovableObjectState.Thrown;
            }
        }
        #endregion

        #region public functions

        /// <summary>
        /// Connect with the specified advanced character.
        /// </summary>
        /// <param name="advancedCharacter">The advanced character.</param>
        public void Grab(AdvancedCharacterBase advancedCharacter)
        {
            ConnectedCharacter.Value = advancedCharacter;
            TransitionToStateGrab();
        }

        /// <summary>
        /// Releases this movable object.
        /// </summary>
        public void Release()
        {
            TransitionToStateIdle();
        }

        /// <summary>
        /// Picks up this movable object.
        /// </summary>
        /// <param name="advancedCharacter">The advanced character.</param>
        public void PickUp(AdvancedCharacterBase advancedCharacter)
        {
            ConnectedCharacter.Value = advancedCharacter;
            TransitionToStatePickUp();
        }

        /// <summary>
        /// Carries this movable object.
        /// </summary>
        public void Carry()
        {
            TransitionToStateCarry();
        }

        /// <summary>
        /// Throws this movable object with specified throw force.
        /// </summary>
        /// <param name="throwForce">The throw force.</param>
        public void Throw(Vector3 throwForce)
        {
            ThrowForce = throwForce;
            TransitionToStateThrow();
        }
        #endregion
		/// <summary>
		/// Applies the push effect.
		/// Notify subscribers that movable got a push effect
		/// </summary>
		/// <param name="pushEffect">The push effect.</param>
		public void ApplyPushEffect(PushEffect pushEffect)
		{
			ApplyPushEffect(pushEffect, false);
			
		}

        /// <summary>
        /// Applies the push effect.
        /// Notify subscribers that movable got a push effect
        /// </summary>
        /// <param name="pushEffect">The push effect.</param>
        /// <param name="hitFromBehind">if set to <c>true</c> [hit from behind].</param>
        public void ApplyPushEffect(PushEffect pushEffect, bool hitFromBehind)
        {
            
            if (PushEffectAction != null)
            {
                PushEffectAction(pushEffect, hitFromBehind);    
            }
            
        }

        /// <summary>
        /// Applies the movement effect.
        /// </summary>
        /// <param name="movementEffect">The movement effect.</param>
        public void ApplyMovementEffect(MovementEffect movementEffect)
        {
            // Dummy. Implement if need to apply movement effects to movables (not very likely any need for a hasted or stunned crate =) )
        }
    }
}
