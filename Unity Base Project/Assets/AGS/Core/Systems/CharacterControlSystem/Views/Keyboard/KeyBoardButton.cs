using UnityEngine;

namespace AGS.Core.Systems.CharacterControlSystem.Keyboard
{
    /// <summary>
    /// For mouse & keyboard buttons
    /// </summary>
    public class KeyBoardButton : InputButtonBaseView
    {
        public string KeyCodeOverride;
        public float DoubleTapSpeedSeconds;
        private float _tapTimer;
        private bool _tappedOnce;
        private bool _tappedTwice;

        /// <summary>
        /// For setting up GetButton as defined by Unity.
        /// </summary>
        /// <returns></returns>
        protected override bool GetButton()
        {
            return string.IsNullOrEmpty(KeyCodeOverride) ? Input.GetButton(InputButton.InputAxisName) : Input.GetKey(KeyCodeOverride);
        }

        /// <summary>
        /// For setting up GetButtonDown as defined by Unity.
        /// </summary>
        /// <returns></returns>
        protected override bool GetButtonDown()
        {
            return string.IsNullOrEmpty(KeyCodeOverride) ? Input.GetButtonDown(InputButton.InputAxisName) : Input.GetKeyDown(KeyCodeOverride);
        }

        /// <summary>
        /// For setting up GetButtonUp as defined by Unity.
        /// </summary>
        /// <returns></returns>
        protected override bool GetButtonUp()
        {
            return string.IsNullOrEmpty(KeyCodeOverride) ? Input.GetButtonUp(InputButton.InputAxisName) : Input.GetKeyUp(KeyCodeOverride);
        }

        /// <summary>
        /// For setting up a double tap button.
        /// Returns true once when the player taps the button twice within the DoubleTapSpeedSeconds
        /// </summary>
        /// <returns></returns>
        protected override bool GetButtonDoubleTap()
        {
            if (string.IsNullOrEmpty(KeyCodeOverride) ? Input.GetButtonUp(InputButton.InputAxisName) : Input.GetKeyUp(KeyCodeOverride))
            {
                _tapTimer = Time.time;
                _tappedOnce = true;
                return false;
            }
            if (string.IsNullOrEmpty(KeyCodeOverride) ? Input.GetButtonDown(InputButton.InputAxisName) : Input.GetKeyDown(KeyCodeOverride) && _tappedOnce)
            {
                if ((Time.time - _tapTimer) < DoubleTapSpeedSeconds)
                {
                    _tapTimer = Time.time;
                    _tappedOnce = false;
                    return true;
                }
            }
            if ((Time.time - _tapTimer) >= DoubleTapSpeedSeconds)
            {
                _tapTimer = Time.time;
                _tappedOnce = false;
            }
            return false;
        }

        /// <summary>
        /// For setting up a double tap and hold button.
        /// Returns true when the player taps the button twice within the DoubleTapSpeedSeconds and stays true while the player holds the button down
        /// </summary>
        /// <returns></returns>
        protected override bool GetButtonDoubleTapAndHold()
        {
            if (string.IsNullOrEmpty(KeyCodeOverride) ? Input.GetButtonDown(InputButton.InputAxisName) : Input.GetKeyDown(KeyCodeOverride) && !_tappedOnce)
            {
                _tapTimer = Time.time;
                _tappedOnce = true;
                _tappedTwice = false;
                return false;
            }

            if (string.IsNullOrEmpty(KeyCodeOverride) ? Input.GetButtonDown(InputButton.InputAxisName) : Input.GetKeyDown(KeyCodeOverride) && _tappedOnce)
            {
                if ((Time.time - _tapTimer) < DoubleTapSpeedSeconds)
                {
                    _tapTimer = Time.time;
                    _tappedOnce = false;
                    _tappedTwice = true;
                    return true;

                }

            }
            if (string.IsNullOrEmpty(KeyCodeOverride) ? Input.GetButton(InputButton.InputAxisName) : Input.GetKey(KeyCodeOverride) && _tappedTwice)
            {
                return true;
            }
            if ((Time.time - _tapTimer) >= DoubleTapSpeedSeconds)
            {
                _tapTimer = Time.time;
                _tappedOnce = false;
            }
            _tappedTwice = false;
            return false;
        }
    }
}
