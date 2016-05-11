using System;
using System.Linq;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.Interactables
{
    /// <summary>
    /// View for the Swing that contains a set number of SwingUnits.
    /// </summary>
    [Serializable]
    public class SwingView : ActionView
    {
        #region Public properties
        public Transform SwingUnitsContainer; // Put SwingUnitViews on separate GameObjects to this Transform.
        #endregion

        public Swing Swing;

        #region AGS Setup
        public override void InitializeView()
        {
            Swing = new Swing();
            SolveModelDependencies(Swing);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            if (SwingUnitsContainer == null)
            {
                SwingUnitsContainer = transform;
            }
            if (SwingUnitsContainer != null)
            {
                foreach (var swingUnitView in SwingUnitsContainer.GetComponentsInChildren<SwingUnitBaseView>())
                {
                    Swing.SwingUnits.Add(swingUnitView.SwingUnit);
                }
            }
        }
        #endregion

        #region MonoBehaviours
        public override void Update()
        {
            base.Update();
            Swing.IsStill.Value = CheckSwingIsStill();
        }
        #endregion

        #region private functions
        /// <summary>
        /// Determines if the Swing is still by checking if all SwingUnits are in idle state.
        /// </summary>
        /// <returns></returns>
        private bool CheckSwingIsStill()
        {
            return Swing.SwingUnits.All(swingUnit => swingUnit.SwingUnitCurrentState.Value == SwingUnitState.Idle);
        }
        #endregion

    }
}