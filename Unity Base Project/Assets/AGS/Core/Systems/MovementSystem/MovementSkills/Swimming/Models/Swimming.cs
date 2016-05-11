using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.HazardSystem;
using AGS.Core.Systems.MovementSystem.Base;
using UnityEngine;

namespace AGS.Core.Systems.MovementSystem.MovementSkills.Swimming
{
    /// <summary>
    /// Swimming skill is needed for character movement in Fluids
    /// </summary>
    public class Swimming : MovementSkillBase
    {
        #region Properties
        // Constructor properties
        public float SwimSpeed { get; private set; }
        public float SurfaceJumpSpeedUp { get; private set; }
        public float SurfaceJumpSpeedForward { get; private set; }
        public float SecondsBetweenStrokes { get; set; }

        // Subscribable properties
        public ActionProperty<Vector3> CurrentSwimSpeed { get; private set; } // Current swim velocity
        public ActionProperty<Vector3> CurrentSurfaceSpeed { get; private set; } // Current swim velocity
        public ActionProperty<bool> OnSurface { get; private set; } // Is the character on the surface of the Fluid?
        public ActionProperty<Fluid> CurrentFluid { get; private set; } // Reference to the current Fluid
        public ActionProperty<bool> JustSplashed { get; private set; } // Did the character just enter CurrentFluid?
        public ActionProperty<SwimmingState> SwimmingCurrentState { get; private set; } // swimming state machine. Partially dependent on Intention
        public ActionProperty<SwimmingIntention> Intention { get; private set; } // The intention value handles the characters "intention". It could, but is not required to, change the SwimmingCurrentState

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="Swimming"/> class.
        /// </summary>
        /// <param name="swimSpeed">The power of the swim stroke.</param>
        /// <param name="surfaceJumpSpeedUp">The surface jump speed upwards.</param>
        /// /// <param name="surfaceJumpSpeedForward">The surface jump speed forward.</param>
        /// <param name="secondsBetweenStrokes">Time window for how often a stroke is allowed.</param>
        public Swimming(float swimSpeed, float surfaceJumpSpeedUp, float surfaceJumpSpeedForward, float secondsBetweenStrokes)
        {
            SwimSpeed = swimSpeed;
            CurrentSwimSpeed = new ActionProperty<Vector3>();
            CurrentSurfaceSpeed = new ActionProperty<Vector3>();
            OnSurface = new ActionProperty<bool>() { Value = false };
            CurrentFluid = new ActionProperty<Fluid>();
            SurfaceJumpSpeedUp = surfaceJumpSpeedUp;
            SurfaceJumpSpeedForward = surfaceJumpSpeedForward;
            SecondsBetweenStrokes = secondsBetweenStrokes;
            JustSplashed = new ActionProperty<bool>() { Value = false };
            SwimmingCurrentState = new ActionProperty<SwimmingState>();
            Intention = new ActionProperty<SwimmingIntention>();
            Intention.OnValueChanged += (sender, state) => SetSwimmingState(state.Value);
            IsEnabled.OnValueChanged += (sender, isEnabled) =>
            {
                if (!isEnabled.Value)
                {
                    // Upon disable, set intention to none to make view ready for intentin change as soon as skill is enabled again
                    Intention.Value = SwimmingIntention.None;
                }
            };
        }
        #region state transitions
        /// <summary>
        /// Transitions to state out of water.
        /// </summary>
        public void TransitionToStateOutOfFluid()
        {
            SwimmingCurrentState.Value = SwimmingState.OutOfWater;
        }

        /// <summary>
        /// Transitions to state in water.
        /// </summary>
        public void TransitionToStateInFluid()
        {
            SwimmingCurrentState.Value = SwimmingState.InWater;
        }

        /// <summary>
        /// Transitions to state do stroke.
        /// </summary>
        public void TransitionToStateDoStroke()
        {
            if (SwimmingCurrentState.Value == SwimmingState.InWater)
            {
                SwimmingCurrentState.Value = SwimmingState.DoingStroke;
            }

        }

        /// <summary>
        /// Transitions to state surface jump.
        /// </summary>
        public void TransitionToStateSurfaceJump()
        {
            SwimmingCurrentState.Value = SwimmingState.SurfaceJumping;
        }

        #endregion
        #region private functions
        /// <summary>
        /// Sets the state of the swimming based on intention.
        /// </summary>
        /// <param name="intention">The intention.</param>
        private void SetSwimmingState(SwimmingIntention intention)
        {
            if (intention == SwimmingIntention.SurfaceJump)
            {
                if (OwnerMovementSkills.Value != null)
                {
                    OwnerMovementSkills.Value.OwnerCharacter.Value.VerticalVelocityLocked.Value = true;
                }

                TransitionToStateSurfaceJump();
            }
            else
            {
                if (intention == SwimmingIntention.DoStroke)
                {
                    if (OwnerMovementSkills.Value != null)
                    {
                        OwnerMovementSkills.Value.OwnerCharacter.Value.VerticalVelocityLocked.Value = false;
                    }
                    TransitionToStateDoStroke();
                }
            }
        }

        #endregion
        #region public functions
        /// <summary>
        /// Set character to start swimming.
        /// </summary>
        /// <param name="fluid">The fluid.</param>
        public void StartSwimming(Fluid fluid)
        {
            OwnerMovementSkills.Value.OwnerCharacter.Value.VerticalVelocityLocked.Value = false;
            CurrentFluid.Value = fluid;
            TransitionToStateInFluid();
        }

        /// <summary>
        /// Set character to be out of the fluid.
        /// </summary>
        public void OutOfFluid()
        {
            CurrentFluid.Value = null;
            TransitionToStateOutOfFluid();
        }

        /// <summary>
        /// Sets the current swim speed.
        /// </summary>
        /// <param name="currentSwimSpeed">The swim speed.</param>
        public void SetCurrentSwimSpeed(Vector3 currentSwimSpeed)
        {
            CurrentSwimSpeed.Value = currentSwimSpeed;
        }

        /// <summary>
        /// Sets the surface jump speed.
        /// </summary>
        /// <param name="currentSurfaceJumpSpeed">The current surface jump speed.</param>
        public void SetSurfaceJumpSpeed(Vector3 currentSurfaceJumpSpeed)
        {
            CurrentSurfaceSpeed.Value = currentSurfaceJumpSpeed;
        }
        #endregion public functions
    }
}
