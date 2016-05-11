using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.Interactables
{
    /// <summary>
    /// SwingUnitView for Rigidbody swings
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class SwingUnitView : SwingUnitBaseView
    {
        private Rigidbody _rigidbody;

        #region MonoBehaviours
        public override void Awake()
        {
            base.Awake();
            _rigidbody = transform.GetComponent<Rigidbody>();
        }
        #endregion

        #region State machine functions
        /// <summary>
        /// Called when [state enter idle].
        /// </summary>
        public override void OnStateEnterIdle()
        {
            // Do nothing
        }

        /// <summary>
        /// Called when [state update idle].
        /// </summary>
        public override void OnStateUpdateIdle()
        {
            // Check if swing is not still and should be swinging naturally
            if (!IsStill())
            {
                SwingUnit.TransitionToStateSwingNatural();
            }
        }

        /// <summary>
        /// Called when [state enter reducing speed].
        /// </summary>
        public override void OnStateEnterReducingSpeed()
        {
            // Do nothing
        }

        /// <summary>
        /// Called when [state update reducing speed].
        /// </summary>
        public override void OnStateUpdateReducingSpeed()
        {
            // Check if swing is still and should be idle
            if (IsStill())
            {
                SwingUnit.TransitionToStateIdle();
            }
            // Constantly reduce swing speed
            ReduceSpeed(0.9f);
        }

        /// <summary>
        /// Called when [state enter swinging natural].
        /// </summary>
        public override void OnStateEnterSwingingNatural()
        {
            // Do nothing
        }

        /// <summary>
        /// Called when [state update swinging natural].
        /// </summary>
        public override void OnStateUpdateSwingingNatural()
        {
            // Check if swing is still and should be idle
            if (IsStill())
            {
                SwingUnit.TransitionToStateIdle();
            }
        }
        #endregion

        #region private functions
        /// <summary>
        /// Determines whether this instance is still.
        /// </summary>
        /// <returns></returns>
        private bool IsStill()
        {
            if (_rigidbody == null) return false;
            var vel = _rigidbody.velocity.magnitude;
            return Mathf.Abs(vel) < 0.1f && Mathf.Abs(Vector3.Distance(transform.position, StartPos)) < 0.25f;
        }

        /// <summary>
        /// Adds the swing force.
        /// </summary>
        /// <param name="forceToAdd">The force to add.</param>
        protected override void AddSwingForce(Vector3 forceToAdd)
        {
            _rigidbody.AddForce(forceToAdd);
        }

        /// <summary>
        /// Reduces the speed.
        /// </summary>
        /// <param name="reduceFactor">The reduce factor.</param>
        private void ReduceSpeed(float reduceFactor)
        {
            _rigidbody.velocity *= reduceFactor;

        }
        #endregion
    }
}
