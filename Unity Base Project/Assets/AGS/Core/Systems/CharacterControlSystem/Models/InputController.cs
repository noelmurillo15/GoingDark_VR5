using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using UnityEngine;

namespace AGS.Core.Systems.CharacterControlSystem
{
    /// <summary>
    /// This is the CharacterController to be used by the player. An FloatSwitch is used to determine weapon changing.
    /// </summary>
    public class InputController : CharacterControllerBase
    {

        #region Properties
        // Subscribable properties  
        public ActionProperty<InputAxis> InputAxis { get; private set; } // The main axis input
        public ActionProperty<FloatSwitch> InputSwitch { get; private set; } // This main input switch
        public ActionList<InputButton> InputButtons { get; private set; } // All owned buttons
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="InputController"/> class.
        /// </summary>
        public InputController()
        {
            InputAxis = new ActionProperty<InputAxis>();
            InputAxis.OnValueChanged += (sender, inputAxis) =>
            {
                MoveVector = inputAxis.Value.AxisPosition;
                Direction = inputAxis.Value.AxisInputDirection;
            };
            InputSwitch = new ActionProperty<FloatSwitch>();
            InputSwitch.OnValueChanged += (sender, inputSwitch) =>
                {
                    inputSwitch.Value.InputValue.OnValueChanged += (_, inputValue) =>
                    {
                        if (Mathf.Abs(inputValue.Value) < 0.01f)
                        {
                            NextWeapon.Value = false;
                            PreviousWeapon.Value = false;
                        }
                        else if (inputValue.Value < 0.01f)
                        {
                            NextWeapon.Value = true;
                            PreviousWeapon.Value = false;
                        }
                        else
                        {
                            NextWeapon.Value = false;
                            PreviousWeapon.Value = true;
                        }
                    };
                };
            InputButtons = new ActionList<InputButton>();
            InputButtons.ListItemAdded += InputButtonAdded;
        }

        #region functions
        private void InputButtonAdded(InputButton button)
        {
            switch (button.InputAxisName)
            {
                case "Jump":
                    button.IsOn.OnValueChanged += (sender, jumpButton) =>
                    {
                        Jump.Value = jumpButton.Value;
                    };
                    break;
                case "Sprint":
                    button.IsOn.OnValueChanged += (sender, sprintButton) =>
                    {
                        Sprint.Value = sprintButton.Value;
                    };
                    break;
                case "Crouch":
                    button.IsOn.OnValueChanged += (sender, crouchButton) =>
                    {
                        Crouch.Value = crouchButton.Value;
                    };
                    break;
                case "Sneak":
                    button.IsOn.OnValueChanged += (sender, sneakButton) =>
                    {
                        Sneak.Value = sneakButton.Value;
                    };
                    break;
                case "Interact":
                    button.IsOn.OnValueChanged += (sender, interactButton) =>
                    {
                        Interact.Value = interactButton.Value;
                    };
                    break;
                case "Action":
                    button.IsOn.OnValueChanged += (sender, actionButton) =>
                    {
                        Action.Value = actionButton.Value;
                    };
                    break;
                case "Aim":
                    button.IsOn.OnValueChanged += (sender, aimButton) =>
                    {
                        Aim.Value = aimButton.Value;
                    };
                    break;
                case "Fire1":
                    if (button.InputButtonType == InputButtonType.ButtonDown)
                    {
                        button.IsOn.OnValueChanged += (sender, fire1Button) =>
                        {
                            Attack1.Value = fire1Button.Value;
                        };
                    }
                    else
                    {
                        button.IsOn.OnValueChanged += (sender, fire1Button) =>
                        {
                            Fire1.Value = fire1Button.Value;
                        };    
                    }
                    
                    break;
                case "Fire2":
                    if (button.InputButtonType == InputButtonType.ButtonDown)
                    {
                        button.IsOn.OnValueChanged += (sender, fire2Button) =>
                        {
                            Attack2.Value = fire2Button.Value;
                        };
                    }
                    else
                    {
                        button.IsOn.OnValueChanged += (sender, fire2Button) =>
                        {
                            Fire2.Value = fire2Button.Value;
                        };    
                    }
                    

                    break;
                case "Fire3":
                    if (button.InputButtonType == InputButtonType.ButtonDown)
                    {
                        button.IsOn.OnValueChanged += (sender, fire3Button) =>
                        {
                            Attack3.Value = fire3Button.Value;
                        };
                    }
                    else
                    {
                        button.IsOn.OnValueChanged += (sender, fire3Button) =>
                        {
                            Fire3.Value = fire3Button.Value;
                        };    
                    }
                    
                    break;
                case "NextWeapon":
                    button.IsOn.OnValueChanged += (sender, nextWeaponButton) =>
                    {
                        NextWeapon.Value = nextWeaponButton.Value;
                    };
                    break;
                case "NextThrowable":
                    button.IsOn.OnValueChanged += (sender, nextThrowableButton) =>
                    {
                        NextThrowable.Value = nextThrowableButton.Value;
                    };
                    break;
            }
        }
        #endregion functions
    }
}
