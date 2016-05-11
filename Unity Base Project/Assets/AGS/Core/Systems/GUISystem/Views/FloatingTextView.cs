using System;
using UnityEngine;

namespace AGS.Core.Systems.GUISystem
{
    /// <summary>
    /// FloatingTextViews normally has their model already created from code during runtime.
    /// </summary>
    [Serializable]
    public class FloatingTextView : DynamicScreenTextView
    {
        public FloatingText FloatingText;

        #region AGS Setup

        public override void InitializeView()
        {
            if (FloatingText == null)
            {
                FloatingText = new FloatingText("No text set", Color.white, 1f, "None", Vector3.zero, Vector3.zero);    
            }
            SolveModelDependencies(FloatingText);
        }

        #endregion 
	}
}