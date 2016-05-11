using System.Collections.Generic;
using AGS.Core.Enums;
using UnityEngine;

namespace AGS.Core.Classes.DataClasses
{
    /// <summary>
    /// Simple class used by CheckHitBelow to set angles and normals
    /// </summary>
    public class HitAngleAndNormal
    {
        public float HitAngleForward { get; set; }
        public float SlopeAngle { get; set; }
        public Vector3 GroundNormal { get; set; }
        public HitObstacle HitObstacle { get; set; }

        public override int GetHashCode()
        {
            int hash = 100;
            hash += HitAngleForward.GetHashCode();
            hash += SlopeAngle.GetHashCode();
            hash += GroundNormal.GetHashCode();
            hash += HitObstacle.GetHashCode();
            return hash;
        }
    }

    /// <summary>
    /// Compares instances of HitAngleAndNormal 
    /// </summary>
    public class HitAngleAndNormalComparer : IEqualityComparer<HitAngleAndNormal>
    {
        public bool Equals(HitAngleAndNormal x, HitAngleAndNormal y)
        {
            if (Mathf.Abs(x.HitAngleForward - y.HitAngleForward) < 0.01f && x.HitObstacle == y.HitObstacle)
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(HitAngleAndNormal obj)
        {
            int hash = 100 + obj.GetHashCode();
            return hash;

        }
    }
}
