using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuTraverse : MonoBehaviour {


    x360Controller m_Controller;
    Selectable m_Button;
    Selectable[] m_arrButtons;

	// Use this for initialization
	void Start () {

        m_Controller = GamePadManager.Instance.GetController(0);
        m_arrButtons = gameObject.GetComponentsInChildren<Selectable>();
	}

    void OnEnable()
    {
        m_arrButtons = gameObject.GetComponentsInChildren<Selectable>();
        m_Button = m_arrButtons[0];
        CheckActiveButtons(m_Button);
        m_Button.Select();
    }
	
	// Update is called once per frame
	void Update () {

        Selectable tempButton = m_Button;
        if (m_Controller.GetButtonDown("Up"))
        {
            tempButton = m_Button.FindSelectableOnUp();
        }

        if (m_Controller.GetButtonDown("Down"))
        {
            tempButton = m_Button.FindSelectableOnDown();
        }

        if (m_Controller.GetButtonDown("A"))
        {
            m_Button.GetComponent<Button>().onClick.Invoke();
        }

        if (tempButton.IsActive())
        {
            m_Button = tempButton;
        }

        m_Button.Select();
	}

    void CheckActiveButtons(Selectable button)
    {
        if (!button.IsActive())
        {
            for (int i = 0; i < m_arrButtons.Length; i++)
            {
                if (m_arrButtons[i].IsActive())
                {
                    m_Button = m_arrButtons[i];
                    break;
                }

            }
        }
    }
}
