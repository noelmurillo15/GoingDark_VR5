using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.GUISystem
{
    /// <summary>
    /// Base UI text class used for both screen space and world space texts.
    /// </summary>
    public abstract class DynamicScreenTextBase : ActionModel
    {
        #region Properties
        // Action properties
        public ActionProperty<string> Text { get; set; } // Text of this screen text
        public ActionProperty<Color> Color { get; set; } // Color of this screen text
        public ActionProperty<float> LifetimeSeconds { get; set; } // Life time of this screen text
        public ActionProperty<string> PrefabName { get; set; } // Prefab name of this screen text
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicScreenTextBase"/> class.
        /// </summary>
        protected DynamicScreenTextBase()
        {
            Text = new ActionProperty<string>();
            Color = new ActionProperty<Color>();
            LifetimeSeconds = new ActionProperty<float>();
            PrefabName = new ActionProperty<string>();
        }

        /// <summary>
        /// Extra constructor for creating DynamicScreenTexts with preset values during runtime.
        /// Initializes a new instance of the <see cref="DynamicScreenTextBase"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        /// <param name="lifeTime">The life time.</param>
        /// <param name="prefabName">Name of the prefab.</param>
        protected DynamicScreenTextBase(string text, Color color, float lifeTime, string prefabName)
        {
            Text = new ActionProperty<string> { Value = text };
            Color = new ActionProperty<Color>() { Value = color };
            LifetimeSeconds = new ActionProperty<float>() { Value = lifeTime };
            PrefabName = new ActionProperty<string>() { Value = prefabName };

        }
    }
}
