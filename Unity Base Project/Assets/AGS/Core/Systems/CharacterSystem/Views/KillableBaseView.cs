using System;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.CharacterSystem
{
    /// <summary>
    /// BaseView for killable objects
    /// </summary>
    [Serializable]
    public abstract class KillableBaseView : ActionView
    {

        #region Public properties
        // Fields to be set in the editor
        public string Name;

        // References to be set in the editor
        public Transform CharacterResourcesContainer; // Put any DamageableResourceViews on separate child GameObjects to this transform
        #endregion

        public KillableBase Killable;

        #region AGS Setup
        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            Killable = model as KillableBase;
            if (Killable == null || CharacterResourcesContainer == null) return;
            foreach (var damageableResourceView in CharacterResourcesContainer.GetComponentsInChildren<DamageableResourceView>())
            {
                Killable.Resources.Add(damageableResourceView.DamageableResource);
            }
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            Killable.ProximityModifierFunc += GetProximityModifier;
        }
        #endregion

        #region abstract functions
        /// <summary>
        /// Gets the effective proximity modifier calculated from this Killable to a Transform.
        /// </summary>
        /// <param name="effectOrigin">The effect origin.</param>
        /// <returns></returns>
        protected abstract float GetProximityModifier(Transform effectOrigin);
        #endregion
    }
}