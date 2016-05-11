using AGS.Core.Enums;
using UnityEngine;

namespace AGS.Core.Classes.Helpers
{
    /// <summary>
    /// Converts AGS ForceTypes into Unity forces
    /// </summary>
    public static class ForceConverter
    {
        /// <summary>
        /// Transforms the force type to unity ForceMode.
        /// </summary>
        /// <param name="forceType">Type of the force.</param>
        /// <returns></returns>
        public static ForceMode ForceTypeToUnityForceMode(ForceType forceType)
        {
            switch (forceType)
            {
                case ForceType.Normal:
                    return ForceMode.Force;
                case ForceType.Impulse:
                    return ForceMode.Impulse;
                default:
                    return ForceMode.Force;

            }
        }

        /// <summary>
        /// Transforms the force type to unity ForceMode2D.
        /// </summary>
        /// <param name="forceType">Type of the force.</param>
        /// <returns></returns>
        public static ForceMode2D ForceTypeToUnityForceMod2D(ForceType forceType)
        {
            switch (forceType)
            {
                case ForceType.Normal:
                    return ForceMode2D.Force;
                case ForceType.Impulse:
                    return ForceMode2D.Impulse;
                default:
                    return ForceMode2D.Force;

            }
        }
    }

}
