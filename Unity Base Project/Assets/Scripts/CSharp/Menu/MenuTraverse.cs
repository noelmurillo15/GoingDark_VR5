using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuTraverse : MonoBehaviour
{
    x360Controller m_Controller;
    Selectable m_Button;
    Selectable[] m_arrButtons;

    // Use this for initialization
    void Start()
    {
        m_Controller = GamePadManager.Instance.GetController(0);
        m_arrButtons = gameObject.GetComponentsInChildren<Selectable>();
        Buttons();
    }

    void OnEnable()
    {
        m_Controller = GamePadManager.Instance.GetController(0);
        m_arrButtons = gameObject.GetComponentsInChildren<Selectable>();
        Buttons();
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

    void Buttons()
    {
        foreach (Selectable item in Selectable.allSelectables)
        {
            if (item.IsActive())
            {
                m_Button = item;
                m_Button.Select();
            }
        }
    }
}
