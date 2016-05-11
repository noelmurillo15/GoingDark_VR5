using System;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.Interactables
{
    /// <summary>
    /// UnlockableObjectView
    /// </summary>
    [Serializable]
	public class UnlockableObjectView : ActionView {
		
		#region Public properties
        // Fields and references to be set in the editor
        public bool ReLockable;
        public Transform SwitchesContainer; // Put SwitchViews on separate GameObjects to this Transform.
		#endregion

		public UnlockableObject UnlockableObject;

        #region AGS Setup
        public override void InitializeView()
        {
            UnlockableObject = new UnlockableObject(ReLockable);
            SolveModelDependencies(UnlockableObject);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            if (SwitchesContainer == null)
            {
                SwitchesContainer = transform;
            }
            if (SwitchesContainer != null)
            {
                foreach (var switchUnit in SwitchesContainer.GetComponentsInChildren<SwitchView>())
                {
                    UnlockableObject.Switches.Add(switchUnit.Switch);
                }
            }
        }

        #endregion
	}
}