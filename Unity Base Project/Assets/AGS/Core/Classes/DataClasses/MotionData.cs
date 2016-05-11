using UnityEngine;

namespace AGS.Core.Classes.DataClasses
{
    /// <summary>
    /// Simple class to hold velocity data. Useful for transfering velicity to an instaniated ragdoll.
    /// </summary>
    public class MotionData
    {
        public Vector3 Velocity { get; set; }
        public Vector3 AngularVelocity { get; set; }
    }


}
