using UnityEngine;
using GoingDark.Core.Enums;
using UnityEngine.UI;
using System.Collections;

public class QuickSlot : MonoBehaviour
{

    #region Properties
    public SystemType Type;

    private float transition;
    private Image m_button;
    private Color original;
    private SystemManager manager;
    #endregion


    // Use this for initialization
    void Start()
    {
        transition = 0f;
        m_button = GetComponent<Image>();
        original = m_button.color;

        manager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();

        StartCoroutine(UpdateCooldowns());
    }

    void ActivateButton()
    {        
        if (Type != SystemType.NONE)
        {
            manager.ActivateSystem(Type);
            return;
        }
        if (Type == SystemType.RADAR || Type == SystemType.SHIELD)
        {
            manager.ToggleSystem(Type);
            return;
        }
        if (Type == SystemType.NONE)
            Debug.LogError("Quickslot button " + transform.name + " has type of None");
    }

    #region Coroutine
    private IEnumerator UpdateCooldowns()
    {
        while (true)
        {
            CooldownCheck();
            yield return new WaitForSeconds(1);
        }
    }
    private void CooldownCheck()
    {
        if (manager.GetSystemCooldown(Type))
            m_button.color = Color.red;
        else
            m_button.color = original;
    }
    #endregion

    #region Collision
    public void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3" && m_button.color == original)
        {
            if (transition == 0f)
                transition = 1f;

            m_button.color = Color.green;
        }
    }
    public void OnTriggerStay(Collider col)
    {
        if (col.name == "bone3")
        {
            transition -= Time.deltaTime;
            if (transition <= 0.0f)
                m_button.color = Color.red;
        }
    }
    public void OnTriggerExit(Collider col)
    {
        if (col.name == "bone3" && m_button.color != Color.red)
            ActivateButton();

        transition = 0f;
        m_button.color = original;
    }
    #endregion 
}