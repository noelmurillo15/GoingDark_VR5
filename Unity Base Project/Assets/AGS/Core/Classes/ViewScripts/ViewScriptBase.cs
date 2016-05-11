using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScripts are meant to be used as supplement to ActionViews, to provide modularity, reusability, testability, readability and reduce file length.
    /// ViewScripts need a reference to a GameView, and will operate on their own to provide extra functionality. A useful example is to separate FX or animation logic from the GameView.
    /// </summary>
    public abstract class ViewScriptBase : MonoBehaviour
    {

        public ActionView ViewReference;

        public virtual void Awake() { }

        public virtual void Start()
        {
            if (ViewReference == null)
            {
                ViewReference = GetComponent<ActionView>();
            }
            if (ViewReference == null) return;
            ViewReference.ViewReady.OnValueChanged += (sender, viewReady) =>
            {
                if (!viewReady.Value) return;
                SetupModelBindings();
            };
        }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }

        /// <summary>
        /// Setups the model bindings. Called when View is ready
        /// </summary>
        protected virtual void SetupModelBindings() { }
    }
}
