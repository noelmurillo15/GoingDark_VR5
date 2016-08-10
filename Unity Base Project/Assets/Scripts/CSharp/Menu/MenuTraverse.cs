using UnityEngine;
using UnityEngine.UI;

public class MenuTraverse : MonoBehaviour
{
    x360Controller m_Controller;

    Selectable[] m_AllSelectables;
    Selectable m_Selectable;

    // Use this for initialization
    void Start()
    {
        m_Controller = GamePadManager.Instance.GetController(0);
    }

    void OnEnable()
    {        
        m_AllSelectables = gameObject.GetComponentsInChildren<Selectable>(true);
        m_Selectable = m_AllSelectables[0];
        CheckActiveButtons();
    }

    // Update is called once per frame
    void Update()
    {
        Selectable tempButton = m_Selectable;

        if (m_Controller.GetButtonDown("Up"))
        {
            tempButton = m_Selectable.FindSelectableOnUp();
        }
        if (m_Controller.GetButtonDown("Down"))
        {
            tempButton = m_Selectable.FindSelectableOnDown();
        }
        if (m_Controller.GetButtonDown("Left"))
        {
            tempButton = m_Selectable.FindSelectableOnLeft();
        }
        if (m_Controller.GetButtonDown("Right"))
        {
            tempButton = m_Selectable.FindSelectableOnRight();
        }
        if (m_Controller.GetButtonDown("A"))
        {
            m_Selectable.GetComponent<Button>().onClick.Invoke();
        }
        if (m_Controller.GetButtonDown("B") && m_AllSelectables.Length > 0)
        {
            for(int x = 0; x < m_AllSelectables.Length; x++)
            {
                if(m_AllSelectables[x].name == "Back")
                    m_AllSelectables[x].GetComponent<Button>().onClick.Invoke();
            }
        }


        if (tempButton)
        {
            if (tempButton.gameObject.activeSelf)
            {
                m_Selectable = tempButton;
            }
        }
        m_Selectable.Select();
    }

    void CheckActiveButtons()
    {
        if (!m_Selectable.gameObject.activeSelf)
        {
            for (int i = 0; i < m_AllSelectables.Length; i++)
            {
                if (m_AllSelectables[i].gameObject.activeSelf)
                {
                    m_Selectable = m_AllSelectables[i];
                    break;
                }
            }
        }
    }
}
