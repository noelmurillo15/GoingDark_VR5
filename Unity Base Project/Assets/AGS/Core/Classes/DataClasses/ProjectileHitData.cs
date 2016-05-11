using AGS.Core.Enums;
using UnityEngine;

namespace AGS.Core.Classes.DataClasses
{
    /// <summary>
    /// ProjectileHitData stores the location of where a projectile his something, and also the direction it was fired in
    /// </summary>
    public class ProjectileHitData
    {
        public Transform HitTransform { get; set; }
        public Vector3 ProjectileDirection { get; set; }
    }
}
