using System;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.CharacterControlSystem
{
    /// <summary>
    /// Add this class to the Player, reference the specific input classes to be used, and then reference this class from the PlayerView. This class is platform independent and is used by all player controllers.
    /// </summary>
    [Serializable]
    public class InputControllerView : CharacterControllerBaseView
    {
		
		#region Public properties
        // References to be set in the editor
		public InputAxisBaseView InputAxisBaseView;
        public FloatSwitchBaseView FloatSwitchBaseView;
        public Transform InputButtonsContainer;
		#endregion

        public InputController InputController;
		

        #region AGS Setup
        public override void InitializeView()
        {
            InputController = new InputController();
            SolveModelDependencies(InputController);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            InputController.InputAxis.Value = InputAxisBaseView.InputAxis;
            if (FloatSwitchBaseView != null)
            {
                InputController.InputSwitch.Value = FloatSwitchBaseView.FloatSwitch;    
            }
            
            foreach (var inputButtonBaseView in InputButtonsContainer.GetComponentsInChildren<InputButtonBaseView>())
            {
                InputController.InputButtons.Add(inputButtonBaseView.InputButton);
            }
        }
        #endregion
    }
}