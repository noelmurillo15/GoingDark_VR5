using UnityEngine;
using XInputDotNetPure;
using System.Collections.Generic;

public struct xButton
{
    public ButtonState prevState;
    public ButtonState currState;
}

public struct xTrigger
{
    public float prevVal;
    public float curVal;
}

class xRumble
{
    public float timer;
    public float duration;
    public Vector2 intensity;

    public void Update()
    {
        timer -= Time.deltaTime;
    }
}

public class x360Controller
{
    // states
    private GamePadState m_prevState;
    private GamePadState m_currState;

    private int m_index;
    private PlayerIndex m_playerIndex;
    private List<xRumble> m_rumbleEvents;

    private Dictionary<string, xButton> map;

    private xButton A, B, X, Y;
    private xButton Up, Down, Left, Right;
    private xButton Guide;
    private xButton Start, Back;
    private xButton Thumbstick_Left, Thumbstick_Right;
    private xButton Bumper_Left, Bumper_Right;
    private xTrigger Trigger_Left, Trigger_Right;

    #region Accessors
    public int GetIndex
    {
        get { return m_index; }
    }

    public bool IsGamepadConnected
    {
        get { return m_currState.IsConnected; }
    }

    public GamePadThumbSticks.StickValue GetLeftStick()
    {
        return m_currState.ThumbSticks.Left;
    }

    public GamePadThumbSticks.StickValue GetRightStick()
    {
        return m_currState.ThumbSticks.Right;
    }

    public float GetLeftTrigger()
    {
        return m_currState.Triggers.Left;
    }

    public float GetRightTrigger()
    {
        return m_currState.Triggers.Right;
    }
    #endregion

    #region Utils
    // will always return true every frame the button is pressed
    public bool GetButton(string button)
    {
        if (map[button].currState == ButtonState.Pressed)
            return true;
        else
            return false;
    }

    // use this for menus, returns true ONCE
    public bool GetButtonDown(string button)
    {
        m_currState = GamePad.GetState(m_playerIndex);
        if (m_currState.IsConnected)
        {
            if (map[button].prevState == ButtonState.Released && map[button].currState == ButtonState.Pressed)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    // these function just like GetButtonDown on current frame
    // used for single shots, missiles etc
    public bool GetLeftTriggerTap()
    {
        if (Trigger_Left.prevVal == 0.0f && Trigger_Left.curVal >= 0.1f)
            return true;
        else
            return false;
    }

    public bool GetRightTriggerTap()
    {
        if (Trigger_Right.prevVal == 0.0f && Trigger_Right.curVal >= 0.1f)
            return true;
        else
            return false;
    }
    #endregion

    /// x360Controller constructor
    public x360Controller(int index)
    {
        m_index = index;
        m_playerIndex = (PlayerIndex)m_index;

        m_rumbleEvents = new List<xRumble>();
        map = new Dictionary<string, xButton>();
    }

    /// Sets current state of all buttons
    public void Update()
    {
        m_currState = GamePad.GetState(m_playerIndex);
        if (m_currState.IsConnected)
        {
            A.currState = m_currState.Buttons.A;
            B.currState = m_currState.Buttons.B;
            X.currState = m_currState.Buttons.X;
            Y.currState = m_currState.Buttons.Y;

            Up.currState = m_currState.DPad.Up;
            Down.currState = m_currState.DPad.Down;
            Left.currState = m_currState.DPad.Left;
            Right.currState = m_currState.DPad.Right;

            Guide.currState = m_currState.Buttons.Guide;
            Back.currState = m_currState.Buttons.Back;
            Start.currState = m_currState.Buttons.Start;

            Thumbstick_Left.currState = m_currState.Buttons.LeftStick;
            Thumbstick_Right.currState = m_currState.Buttons.RightStick;
            Bumper_Left.currState = m_currState.Buttons.LeftShoulder;
            Bumper_Right.currState = m_currState.Buttons.RightShoulder;

            Trigger_Left.curVal = m_currState.Triggers.Left;
            Trigger_Right.curVal = m_currState.Triggers.Right;

            UpdateMap();
            HandleRumble();
        }
    }

    /// Sets previous state of buttons and updates map
    public void Refresh()
    {
        m_prevState = m_currState;

        if (m_currState.IsConnected)
        {
            A.prevState = m_prevState.Buttons.A;
            B.prevState = m_prevState.Buttons.B;
            X.prevState = m_prevState.Buttons.X;
            Y.prevState = m_prevState.Buttons.Y;

            Up.prevState = m_prevState.DPad.Up;
            Down.prevState = m_prevState.DPad.Down;
            Left.prevState = m_prevState.DPad.Left;
            Right.prevState = m_prevState.DPad.Right;

            Guide.prevState = m_prevState.Buttons.Guide;
            Back.prevState = m_prevState.Buttons.Back;
            Start.prevState = m_prevState.Buttons.Start;

            Thumbstick_Left.prevState = m_prevState.Buttons.LeftStick;
            Thumbstick_Right.prevState = m_prevState.Buttons.RightStick;
            Bumper_Left.prevState = m_prevState.Buttons.LeftShoulder;
            Bumper_Right.prevState = m_prevState.Buttons.RightShoulder;

            Trigger_Left.prevVal = m_prevState.Triggers.Left;
            Trigger_Right.prevVal = m_prevState.Triggers.Right;

            UpdateMap();
        }
    }

    /// Updates values for all buttons stored in the list
    void UpdateMap()
    {
        map["A"] = A;
        map["B"] = B;
        map["X"] = X;
        map["Y"] = Y;

        map["Up"] = Up;
        map["Down"] = Down;
        map["Left"] = Left;
        map["Right"] = Right;

        map["Guide"] = Guide;
        map["Start"] = Start;
        map["Back"] = Back;

        map["LeftThumbstick"] = Thumbstick_Left;
        map["RightThumbstick"] = Thumbstick_Right;

        map["LeftBumper"] = Bumper_Left;
        map["RightBumper"] = Bumper_Right;

    }

    /// Handles all added rumble events (vibrations)
    void HandleRumble()
    {
        Vector2 power = new Vector2(0, 0);

        if (m_rumbleEvents.Count > 0)
        {
            Vector2 intensity = new Vector2(0, 0);
            for (int x = 0; x < m_rumbleEvents.Count; x++)
            {
                m_rumbleEvents[x].Update();

                if (m_rumbleEvents[x].timer > 0)
                {
                    float timeLeft = Mathf.Clamp(m_rumbleEvents[x].timer / m_rumbleEvents[x].duration, 0f, 1f);
                    power = new Vector2(Mathf.Max(m_rumbleEvents[x].intensity.x * timeLeft, power.x), Mathf.Max(m_rumbleEvents[x].intensity.y * timeLeft, power.y));

                    GamePad.SetVibration(m_playerIndex, power.x, power.y);
                }
                else
                {
                    m_rumbleEvents.Remove(m_rumbleEvents[x]);
                }
            }

        }
    }

    /// Adds a vibration on the controller
    public void AddRumble(float timer, Vector2 intensity, float duration)
    {
        xRumble mRumble = new xRumble();

        mRumble.timer = timer;
        mRumble.intensity = intensity;
        mRumble.duration = duration;
        m_rumbleEvents.Add(mRumble);
    }
}