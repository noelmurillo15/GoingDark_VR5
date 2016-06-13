using UnityEngine;
using GoingDark.Core.Enums;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ArmButtons : MonoBehaviour
{

    #region Properties
    public SystemType Type { get; private set; }

    private float transition;
    private Image m_button;
    private Color original;
    private SystemManager manager;
    #endregion


    // Use this for initialization
    void Start()
    {        
        Initialize();        
        StartCoroutine(UpdateCooldowns());
    }

    void Update()
    {
        
    }    

    #region Private Methods
    void Initialize()
    {
        transition = 0f;
        m_button = GetComponent<Image>();        
        switch (transform.name)
        {
            case "EmpButton":
                Type = SystemType.Emp;
                break;
            case "CloakButton":
                Type = SystemType.Cloak;
                break;
            case "HyperdriveButton":
                Type = SystemType.Hyperdrive;
                break;
            case "MissileButton":
                Type = SystemType.Missile;
                break;
            case "WSButton":
                Type = SystemType.Laser;
                break;
            case "DecoyButton":
                Type = SystemType.Decoy;
                break;
        }
        original = m_button.color;
        manager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();
    }
    private void ActivateButton()
    {
        m_button.color = original;

        if (transform.name == "MissionLogButton")
        {
            manager.ToggleMissionLog();
            return;
        }
        else
        {
            Debug.Log("Loading Scene : " + transform.name);
            SceneManager.LoadScene(transform.name);
            return;
        }

        manager.ActivateSystem(Type);
    }
    #endregion

    #region Coroutine
    private IEnumerator UpdateCooldowns()
    {
        while (true)
        {
            CooldownCheck();
            yield return new WaitForSeconds(2);
        }
    }
    private void CooldownCheck()
    {
        if (manager.GetSystemCooldown(Type) > 0)
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
            transition = 0.1f;
            m_button.color = Color.green;
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.name == "bone3")
        {
            transition -= Time.deltaTime;
            if (transition <= 0.0f && m_button.color == Color.green)
                ActivateButton();
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.name == "bone3")
            m_button.color = original;
    }
    #endregion    
}