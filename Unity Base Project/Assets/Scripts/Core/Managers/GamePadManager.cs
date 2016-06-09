using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePadManager : MonoBehaviour
{

    public int m_numGamepads = 1;

    private List<x360Controller> m_GamepadList;
    private static GamePadManager m_pInstance;

    public static GamePadManager Instance
    {
        get
        {
            // instance does not exist
            if (m_pInstance == null)
            {
                Debug.Log("No instance of GamePadManager currently exists");
                return null;
            }

            return m_pInstance;
        }
    }
    

    // called from RefreshGamePads for GetButtonDown functionality
    public void Refresh()
    {
        for (int i = 0; i < m_GamepadList.Count; i++)
        {
            m_GamepadList[i].Refresh();
        }
    }

    void Awake()
    {
        if (m_pInstance != null && m_pInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            m_pInstance = this;
            DontDestroyOnLoad(gameObject);
            m_numGamepads = Mathf.Clamp(m_numGamepads, 1, 4);
            m_GamepadList = new List<x360Controller>();

            for (int i = 0; i < m_numGamepads; i++)
            {
                m_GamepadList.Add(new x360Controller(i));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < m_GamepadList.Count; i++)
        {
            m_GamepadList[i].Update();
        }
    }

    /// <summary>
    /// Returns gamepad connected at that index, null if there is none
    /// </summary>
    /// <param name="controllerIndex"></param>
    /// <returns></returns>
    public x360Controller GetController(int controllerIndex)
    {
        for (int i = 0; i < m_GamepadList.Count; i++)
        {
            if (m_GamepadList[i].GetIndex == controllerIndex)
            {
                return m_GamepadList[i];
            }
        }

        // no valid gamepad index found
        return null;
    }

    /// <summary>
    /// Returns number of connected gamepads
    /// </summary>
    /// <returns></returns>
    public int NumConnected()
    {
        int temp = 0;
        for (int i = 0; i < m_GamepadList.Count; i++)
        {
            if (m_GamepadList[i].IsGamepadConnected)
            {
                temp += 1;
            }
        }

        return temp;
    }

    /// <summary>
    /// Returns true if any button is pressed on ANY gamepad
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public bool GetAnyButton(string button)
    {
        for (int i = 0; i < m_GamepadList.Count; i++)
        {
            if (m_GamepadList[i].IsGamepadConnected && m_GamepadList[i].GetButton(button))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns true if button is pressed on any gamepad this frame
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public bool GetAnyButtonDown(string button)
    {
        for (int i = 0; i < m_GamepadList.Count; i++)
        {
            if (m_GamepadList[i].IsGamepadConnected && m_GamepadList[i].GetButtonDown(button))
            {
                return true;
            }
        }

        return false;
    }
}
