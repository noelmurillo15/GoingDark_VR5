using System;

namespace AGS.Core.Systems.GUISystem
{
    /// <summary>
    /// OverlayTextViews normally has their model already created from code during runtime.
    /// </summary>
    [Serializable]
    public class OverlayTextView : DynamicScreenTextView
    {
        public OverlayText OverlayText;

        #region AGS Setup

        public override void InitializeView()
        {
            if (OverlayText == null)
            {
                OverlayText = new OverlayText("No text set", "None");
            }
            SolveModelDependencies(OverlayText);
        }

        #endregion
    }
}