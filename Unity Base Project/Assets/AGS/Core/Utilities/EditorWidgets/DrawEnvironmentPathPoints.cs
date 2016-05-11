using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AGS.Core.Utilities.EditorWidgets
{
    /// <summary>
    /// This script draws a path between Transforms in the editor scene view
    /// </summary>
    public class DrawEnvironmentPathPoints : MonoBehaviour
    {

        public List<Transform> EnvironmentPathPoints;

        public void OnDrawGizmos()
        {
            if (EnvironmentPathPoints == null || EnvironmentPathPoints.Count < 2)
                return;

            var nonNullPoints = EnvironmentPathPoints.Where(t => t != null).ToList();

            if (nonNullPoints.Count < 2)
                return;
            foreach (var point in nonNullPoints)
            {
                var previousPointIndex = nonNullPoints.IndexOf(point) - 1;
                if (previousPointIndex < 0) continue;

                var previousPoint = nonNullPoints[previousPointIndex];
                Gizmos.DrawLine(previousPoint.transform.position, point.transform.position);
            }
        }
    }
}