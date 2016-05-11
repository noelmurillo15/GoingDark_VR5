using UnityEngine;

namespace AGS.Core.Systems.GUISystem
{
    /// <summary>
    /// A world space "floating text". To be used for damage numbers etc.
    /// </summary>
    public class FloatingText : DynamicScreenTextBase
    {
        #region Properties
        // Constructor properties
        public Vector3 SpawnPosition { get; set; }
        public Vector3 SpawnRotation { get; set; }

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingText"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        /// <param name="lifeTime">The life time.</param>
        /// <param name="prefabName">Name of the prefab.</param>
        /// <param name="spawnPos">The spawn position.</param>
        /// <param name="spawnRot">The spawn rotation.</param>
        public FloatingText(string text, Color color, float lifeTime, string prefabName, Vector3 spawnPos, Vector3 spawnRot)
            : base(text, color, lifeTime, prefabName)
        {
            SpawnPosition = spawnPos;
            SpawnRotation = spawnRot;
        }
    }
}
