using System;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.MovingEnvironmentSystem
{
    /// <summary>
    /// Add this View to a GameObject that should draw path points
    /// </summary>
    [Serializable]
    public class EnvironmentPathView : ActionView
    {
        public EnvironmentPathType PathType;
        public Transform PointsContainer; // Add EnvironmentPathPointViews to separate child GameObjects to this Transform
        
        public EnvironmentPath EnvironmentPath;

        #region AGS Setup
        public override void InitializeView()
        {
            EnvironmentPath = new EnvironmentPath(PathType);
            SolveModelDependencies(EnvironmentPath);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            EnvironmentPath = model as EnvironmentPath;
            if (EnvironmentPath == null) return;
            foreach (var pathPointBaseView in PointsContainer.GetComponentsInChildren<EnvironmentPathPointView>())
            {
                EnvironmentPath.Points.Add(pathPointBaseView.EnvironmentPathPoint);
            }
        }
        #endregion
        

    }
}
