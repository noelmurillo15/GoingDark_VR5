using System;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.CharacterControlSystem
{
    /// <summary>
    /// InputButtons need to have the same InputAxisName as the corresponding Key map in Project Settings/Input.
    /// Override this class to create input buttons. Only GetButton() and GetButtonDown() needs to be implemented
    /// </summary>
    [Serializable]
    public abstract class InputButtonBaseView : ActionView
    {

        #region Public properties
        // Fields to be set in the editor
        public string InputAxisName;
        public InputButtonType ButtonType;
        #endregion

        public InputButton InputButton;

        private UpdatePersistantGameObject _buttonUpdater; // component for updating the button value


        #region AGS Setup
        public override void InitializeView()
        {
            InputButton = new InputButton(InputAxisName, ButtonType);
            SolveModelDependencies(InputButton);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            _buttonUpdater = ComponentExtensions.SetupComponent<UpdatePersistantGameObject>(gameObject);
            switch (InputButton.InputButtonType)
            {
                case InputButtonType.Button:
                    _buttonUpdater.UpdateMethod = () =>
                    {
                        _buttonUpdater.UpdateMethod = () =>
                        {
                            InputButton.IsOn.Value = GetButton();
                        };
                    };
                    break;
                case InputButtonType.ButtonDown:
                    _buttonUpdater.UpdateMethod = () =>
                    {
                        InputButton.IsOn.Value = GetButtonDown();
                    }; break;
                case InputButtonType.ButtonUp:
                    _buttonUpdater.UpdateMethod = () =>
                    {
                        InputButton.IsOn.Value = GetButtonUp();
                    };
                    break;
                case InputButtonType.DoubleTap:
                    _buttonUpdater.UpdateMethod = () =>
                    {
                        InputButton.IsOn.Value = GetButtonDoubleTap();
                    };
                    break;
                case InputButtonType.DoubleTapAndHold:
                    _buttonUpdater.UpdateMethod = () =>
                    {
                        InputButton.IsOn.Value = GetButtonDoubleTapAndHold();
                    };
                    break;
                default:
                    _buttonUpdater.UpdateMethod = () =>
                    {
                        InputButton.IsOn.Value = GetButton();
                    };
                    break;

            }
        }
        #endregion

        #region MonoBehaviours

        public override void OnDestroy()
        {
            _buttonUpdater.Stop();
            base.OnDestroy();
        }

        #endregion

        /// <summary>
        /// For setting up GetButton as defined by Unity.
        /// </summary>
        /// <returns></returns>
        protected abstract bool GetButton();
        /// <summary>
        /// For setting up GetButtonDown as defined by Unity.
        /// </summary>
        /// <returns></returns>
        protected abstract bool GetButtonDown();
        /// <summary>
        /// For setting up GetButtonUp as defined by Unity.
        /// </summary>
        /// <returns></returns>
        protected abstract bool GetButtonUp();
        /// <summary>
        /// For setting up a double tap button.
        /// </summary>
        /// <returns></returns>
        protected abstract bool GetButtonDoubleTap();
        /// <summary>
        /// For setting up a double tap and hold button.
        /// </summary>
        /// <returns></returns>
        protected abstract bool GetButtonDoubleTapAndHold();
    }
}