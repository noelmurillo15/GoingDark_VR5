using UnityEngine;
using GD.Core.Enums;
using UnityEngine.UI;

public class ArmButtons : MonoBehaviour
{

    #region Properties
    public SystemType Type { get; private set; }
    private float delay;
    private float transition;
    private float cancelTimer;

    private Image m_button;    
    private SystemsManager manager;
    public GameObject missionLog;
    #endregion


    // Use this for initialization
    void Start()
    {
        Initialize();
        delay = 10f;
        transition = 0f;
        cancelTimer = 0f;
        m_button = GetComponent<Image>();
        manager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemsManager>();
    }

    void Update()
    {
        if (delay > 0f)
            delay -= Time.deltaTime;
        else
        {
            delay = .5f;
        }
    }

    #region Private Methods
    void Initialize()
    {
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
    }
    private void ActivateButton()
    {
        if (transform.name == "MissionLogButton")
            missionLog.SetActiveRecursively(true);
        else
            manager.SendMessage("ActivateSystem", Type);

        m_button.color = Color.white;
    }
    #endregion

    #region Collision
    public void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3")
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