using System;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.CharacterControlSystem
{
    /// <summary>
    /// Override this class to create a specific input switch. Only GetInputValue needs to be implemented.
    /// </summary>
    [Serializable]
    public abstract class FloatSwitchBaseView : ActionView
	{

        public FloatSwitch FloatSwitch;
        #region AGS Setup
        public override void InitializeView()
        {
            FloatSwitch = new FloatSwitch();
            SolveModelDependencies(FloatSwitch);
        }

        #endregion

        #region MonoBehaviours

        public override void Update()
        {
            base.Update();
            FloatSwitch.InputValue.Value = GetInputValue();
        }

        #endregion

        /// <summary>
        /// Gets the switch input value.
        /// </summary>
        /// <returns></returns>
        public abstract float GetInputValue();
	}
}