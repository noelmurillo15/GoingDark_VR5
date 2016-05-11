using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.InteractionSystem.Base;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.InteractionSkills.ObjectMovement
{
    /// <summary>
    /// Main workload of the ObjectMovement model is to provide the object movement state machine and handle the object movement intention of the character.
    /// There are lots of properties for determining speeds for interaction with different sized movables.
    /// </summary>
    public class ObjectMovement : InteractionSkillBase
    {
        #region Properties
        // Constructor properties
        public float OffsetHorizontalLow { get; private set; }
        public float OffsetHorizontalHeavy { get; private set; }
        public float PushSpeedNormal { get; private set; }
        public float PullSpeedNormal { get; private set; }
        public Vector2 ThrowSpeedNormal { get; private set; }
        public float PushSpeedLight { get; private set; }
        public float PullSpeedLight { get; private set; }
        public Vector2 ThrowSpeedLight { get; private set; }
        public float PushSpeedHeavy { get; private set; }
        public float PullSpeedHeavy { get; private set; }
        public Vector2 ThrowSpeedHeavy { get; private set; }
        public float PushSpeedMassive { get; private set; }
        public Vector2 ThrowSpeedTiny { get; private set; }
        public float HeavyCarryRelativeHeight { get; private set; }
        public float PickUpSpeed { get; private set; }

        // Subscribable properties
        public ActionProperty<MovableObjectWeight> MaxPushWeight { get; private set; } // Objects with bigger sizes than this value cannot be pushed. Perfect to override if gaining super strength or similar.
        public ActionProperty<MovableObjectWeight> MaxCarryWeight { get; private set; } // Objects with bigger sizes than this value cannot be carried. Perfect to override if gaining super strength or similar.
        public ActionProperty<ObjectMovementState> ObjectMovementCurrentState { get; private set; } // object movement state machine. Partially dependent on Intention
        public ActionProperty<ObjectMovementIntention> Intention { get; private set; } // The intention value handles the characters "intention". It could, but is not required to, change the ObjectMovementCurrentState

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectMovement"/> class.
        /// </summary>
        /// <param name="approachMargin">The approach margin.</param>
        /// <param name="offsetVertical">The offset vertical.</param>
        /// <param name="offsetHorizontal">The offset horizontal.</param>
        /// <param name="pickUpSpeed">The pick up speed.</param>
        /// <param name="maxPushWeight">The maximum push weight.</param>
        /// <param name="maxCarryWeight">The maximum carry weight.</param>
        /// <param name="offsetHorizontalLow">Approach offset for low.</param>
        /// <param name="offsetHorizontalHeavy">Approach offset for heavy.</param>
        /// <param name="pushSpeedNormal">The push speed normal.</param>
        /// <param name="pullSpeedNormal">The pull speed normal.</param>
        /// <param name="throwSpeedNormal">The throw speed normal.</param>
        /// <param name="pushSpeedLight">The push speed light.</param>
        /// <param name="pullSpeedLight">The pull speed light.</param>
        /// <param name="throwSpeedLight">The throw speed light.</param>
        /// <param name="pushSpeedHeavy">The push speed heavy.</param>
        /// <param name="pullSpeedHeavy">The pull speed heavy.</param>
        /// <param name="throwSpeedHeavy">The throw speed heavy.</param>
        /// <param name="pushSpeedMassive">The push speed massive.</param>
        /// <param name="throwSpeedTiny">The throw speed tiny.</param>
        /// <param name="heavyCarryRelativeHeight">If carrying something heavy on shoulders or on back, it is often natural for a character to bend down a little</param>
        public ObjectMovement(float approachMargin, float offsetVertical, float offsetHorizontal,
            float pickUpSpeed, MovableObjectWeight maxPushWeight, MovableObjectWeight maxCarryWeight,
            float offsetHorizontalLow, float offsetHorizontalHeavy, float pushSpeedNormal, float pullSpeedNormal, Vector2 throwSpeedNormal,
            float pushSpeedLight, float pullSpeedLight, Vector2 throwSpeedLight, float pushSpeedHeavy, float pullSpeedHeavy, Vector2 throwSpeedHeavy,
            float pushSpeedMassive, Vector2 throwSpeedTiny, float heavyCarryRelativeHeight)
            : base(approachMargin, offsetVertical, offsetHorizontal)
        {
            PickUpSpeed = pickUpSpeed;
            OffsetHorizontalLow = offsetHorizontalLow;
            OffsetHorizontalHeavy = offsetHorizontalHeavy;
            PushSpeedNormal = pushSpeedNormal;
            PullSpeedNormal = pullSpeedNormal;
            ThrowSpeedNormal = throwSpeedNormal;
            PushSpeedLight = pushSpeedLight;
            PullSpeedLight = pullSpeedLight;
            ThrowSpeedLight = throwSpeedLight;
            PushSpeedHeavy = pushSpeedHeavy;
            PullSpeedHeavy = pullSpeedHeavy;
            ThrowSpeedHeavy = throwSpeedHeavy;
            PushSpeedMassive = pushSpeedMassive;
            ThrowSpeedTiny = throwSpeedTiny;
            HeavyCarryRelativeHeight = heavyCarryRelativeHeight;
            MaxPushWeight = new ActionProperty<MovableObjectWeight>() { Value = maxPushWeight };
            MaxCarryWeight = new ActionProperty<MovableObjectWeight>() { Value = maxCarryWeight };
            ObjectMovementCurrentState = new ActionProperty<ObjectMovementState>();
            Intention = new ActionProperty<ObjectMovementIntention>();
            Intention.OnValueChanged += (sender, intention) => SetObjectMovementState(intention.Value);
            IsEnabled.OnValueChanged += (sender, isEnabled) =>
            {
                if (!isEnabled.Value)
                {
                    // Upon disable, set intention to none to make view ready for intentin change as soon as skill is enabled again
                    Intention.Value = ObjectMovementIntention.None;
                }
            };
        }
        #region private functions
        /// <summary>
        /// Sets the state of the object movement based on intention.
        /// </summary>
        /// <param name="intention">The intention.</param>
        private void SetObjectMovementState(ObjectMovementIntention intention)
        {
            switch (intention)
            {
                case ObjectMovementIntention.Grab:
                    TransitionToStateApproach();
                    break;
                case ObjectMovementIntention.Release:
                    TransitionToStateRelease();
                    break;
                case ObjectMovementIntention.Carry:
                    TransitionToStateCarry();
                    break;
                case ObjectMovementIntention.Throw:
                    TransitionToStateThrow();
                    break;
            }
        }
        #endregion

        #region state transitions
        /// <summary>
        /// Transitions to state idle.
        /// </summary>
        public override void TransitionToStateIdle()
        {
            if (ObjectMovementCurrentState.Value == ObjectMovementState.Releasing)
            {
                ObjectMovementCurrentState.Value = ObjectMovementState.Idle;
                OwnerInteractionSkills.Value.ForceClear();
            }
            
        }

        /// <summary>
        /// Transitions to state approach.
        /// </summary>
        public override void TransitionToStateApproach()
        {
            if (ObjectMovementCurrentState.Value == ObjectMovementState.Idle)
            {
                ObjectMovementCurrentState.Value = ObjectMovementState.Approaching;
            }
        }

        /// <summary>
        /// Transitions to state interact.
        /// </summary>
        public override void TransitionToStateInteract()
        {
            if (ObjectMovementCurrentState.Value == ObjectMovementState.Approaching)
            {
                ObjectMovementCurrentState.Value = ObjectMovementState.Grabbing;
            }
        }

        /// <summary>
        /// Transitions to state release.
        /// </summary>
        public override void TransitionToStateRelease()
        {
            if (ObjectMovementCurrentState.Value != ObjectMovementState.Idle)
            {
                ObjectMovementCurrentState.Value = ObjectMovementState.Releasing;
            }
        }

        /// <summary>
        /// Transitions to state carry.
        /// </summary>
        public void TransitionToStateCarry()
        {
            if (ObjectMovementCurrentState.Value == ObjectMovementState.Approaching
                ||
                ObjectMovementCurrentState.Value == ObjectMovementState.Grabbing)
            {
                ObjectMovementCurrentState.Value = ObjectMovementState.Carrying;
            }
        }

        /// <summary>
        /// Transitions to state throw.
        /// </summary>
        public void TransitionToStateThrow()
        {
            if (ObjectMovementCurrentState.Value == ObjectMovementState.Carrying)
            {
                ObjectMovementCurrentState.Value = ObjectMovementState.Throwing;
            }
        }
        

        #endregion
    }
}
