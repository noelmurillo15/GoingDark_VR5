using UnityEngine;
using UnityEngine.UI;

public class MenuTraverse : MonoBehaviour
{
    x360Controller m_Controller;

    Selectable[] m_Buttons;
    Selectable m_Button;

    // Use this for initialization
    void OnEnable()
    {
        m_Controller = GamePadManager.Instance.GetController(0);
        m_Buttons = gameObject.GetComponentsInChildren<Selectable>();
        m_Button = m_Buttons[0];
        CheckActiveButtons();
    }

    // Update is called once per frame
    void Update()
    {
        Selectable tempButton = m_Button;
        if (m_Controller.GetButtonDown("Up"))
        {
            tempButton = m_Button.FindSelectableOnUp();
        }

        if (m_Controller.GetButtonDown("Down"))
        {
            tempButton = m_Button.FindSelectableOnDown();
        }
        if (m_Controller.GetButtonDown("Left"))
        {
            tempButton = m_Button.FindSelectableOnLeft();
        }

        if (m_Controller.GetButtonDown("Right"))
        {
            tempButton = m_Button.FindSelectableOnRight();
        }
        if (m_Controller.GetButtonDown("A"))
        {
            m_Button.GetComponent<Button>().onClick.Invoke();
        }

        if (tempButton)
        {
            if (tempButton.gameObject.activeSelf)
            {
                m_Button = tempButton;
            }
        }
        m_Button.Select();
    }

    void CheckActiveButtons()
    {
        if (!m_Button.gameObject.activeSelf)
        {
            for (int i = 0; i < m_Buttons.Length; i++)
            {
                if (m_Buttons[i].gameObject.activeSelf)
                {
                    m_Button = m_Buttons[i];
                    break;
                }
            }
        }
    }
}
