using System;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.GUISystem
{
    /// <summary>
    /// DynamicScreenTextView
    /// </summary>
    [Serializable]
	public abstract class DynamicScreenTextView : ActionView {
		
		#region Public properties
		public string Text;
        public Color Color;
        public float LifetimeSeconds;
		#endregion

        public DynamicScreenTextBase DynamicScreenText;

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            DynamicScreenText = model as DynamicScreenTextBase;
        }

	}
}