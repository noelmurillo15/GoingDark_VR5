using UnityEngine;
using GD.Core.Enums;
using UnityEngine.UI;
using System.Collections;

public class ArmButtons : MonoBehaviour
{

    #region Properties
    public SystemType Type { get; private set; }

    private float transition;
    private float cancelTimer;
    private Image m_button;
    private Color original;

    public GameObject missionLog;
    private SystemsManager manager;
    #endregion


    // Use this for initialization
    void Start()
    {
        Initialize();        
        manager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemsManager>();
    }

    void Update()
    {
        UpdateCooldowns();
    }

    void UpdateCooldowns()
    {
        Debug.Log("Checking Cooldown");
        if (manager.GetSystemCooldown(Type) > 0f)
            m_button.color = Color.red;
        else
            m_button.color = original;

        
    }

    #region Private Methods
    void Initialize()
    {
        transition = 0f;
        cancelTimer = 0f;
        m_button = GetComponent<Image>();        
        switch (transform.name)
        {
            case "EmpButton":
                Type = SystemType.EMP;
                break;
            case "CloakButton":
                Type = SystemType.CLOAK;
                break;
            case "HyperdriveButton":
                Type = SystemType.HYPERDRIVE;
                break;
            case "MissileButton":
                Type = SystemType.MISSILES;
                break;
            case "DecoyButton":
                Type = SystemType.DECOY;
                break;
            case "RadarButton":
                Type = SystemType.RADAR;
                break;


            default:
                Debug.Log("Button Not Initialized : " + transform.name);
                break;
        }
        original = m_button.color;
    }
    private void ActivateButton()
    {
        if (transform.name != "MissionLogButton")
            manager.SendMessage("ActivateSystem", Type);
        else
            missionLog.SetActiveRecursively(true);
            

        m_button.color = original;
    }
    #endregion

    #region Collision
    public void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3" && m_button.color == original)
        {
            transition = 0.1f;
            cancelTimer = 1.5f;
            m_button.color = Color.grey;
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.name == "bone3")
        {
            if (cancelTimer > 0.0f)
            {
                transition -= Time.deltaTime;
                cancelTimer -= Time.deltaTime;

                if (transition <= 0.0f && m_button.color == Color.grey)
                    m_button.color = Color.green;
            }
            else
                m_button.color = Color.red;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.name == "bone3")
            if (m_button.color == Color.green)
                ActivateButton();
    }
    #endregion    
}