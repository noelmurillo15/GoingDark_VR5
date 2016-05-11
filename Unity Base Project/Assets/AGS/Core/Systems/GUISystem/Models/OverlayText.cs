namespace AGS.Core.Systems.GUISystem
{
    /// <summary>
    /// Used for screen space or camera space overlay text. To be used for anything but world space texts.
    /// </summary>
    public class OverlayText : DynamicScreenTextBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OverlayText"/> class.
        /// </summary>
        /// <param name="prefabName">Name of the prefab.</param>
        /// <param name="text">The text.</param>
        public OverlayText(string prefabName, string text)
        {
            PrefabName.Value = prefabName;
            Text.Value = text;
            LifetimeSeconds.Value = 0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OverlayText"/> class.
        /// </summary>
        /// <param name="prefabName">Name of the prefab.</param>
        /// <param name="text">The text.</param>
        /// <param name="lifeTime">The life time.</param>
        public OverlayText(string prefabName, string text, float lifeTime)
        {
            PrefabName.Value = prefabName;
            Text.Value = text;
            LifetimeSeconds.Value = lifeTime;
        }
    }
}
