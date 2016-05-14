using UnityEngine;
using GD.Core.Enums;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArmButtons : MonoBehaviour
{

    #region Properties
    private bool initialized;
    private float delay;
    private float transition;
    private float cancelTimer;

    private Image m_button;
    private ShipSystems Systems;
    private ArmSettings settings;
    
    private MissileSystem m_shootObj;
      

    #endregion


    // Use this for initialization
    void Start()
    {
        delay = .5f;
        transition = 0.0f;
        cancelTimer = 0.0f;
        m_button = null;
        initialized = false;
        m_shootObj = GameObject.Find("Devices").GetComponentInChildren<MissileSystem>();
    }

    void Update()
    {
        if (initialized)
        {
            if (delay > 0f)
                delay -= Time.deltaTime;
            else
            {
                UpdateCooldowns();
                delay = .5f;
            }
        }
    }

    void Initialize()
    {
        if (m_button == null)
            m_button = GetComponent<Image>();
        if (settings == null)
            settings = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<ArmSettings>();
        if (Systems == null)
            Systems = GameObject.Find("Devices").GetComponent<ShipSystems>();
        


        initialized = true;
    }

    void UpdateCooldowns()
    {
        switch (transform.name)
        {
            case "HyperDriveButton":
                if (Systems.HyperDrive.Cooldown > 0.0f)
                    m_button.color = Color.red;
                else
                    m_button.color = Color.white;
                break;

            case "CloakButton":
                if (Systems.Cloak.Cooldown > 0.0f)
                    m_button.color = Color.red;
                else
                    m_button.color = Color.white;
                break;
            case "EmpButton":
                if (Systems.Emp.Cooldown > 0.0f)
                    m_button.color = Color.red;
                else
                    m_button.color = Color.white;
                break;

            case "MissileButton":
                if (Systems.Missiles.Cooldown > 0.0f)
                    m_button.color = Color.red;
                else
                    m_button.color = Color.white;
                break;
        }
    }

    #region Collision
    public void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3")
        {
            Initialize();
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

    private void ActivateButton()
    {
        switch (transform.name)
        {
            case "CloseSettings":
                settings.CloseSettings();
                return;

            case "CloseLog":
                Systems.WeaponSelect.SetActive(false);
                Debug.Log("close message");
                break;

            case "Homing Missile":
                Debug.Log("Homing message");
                m_shootObj.MissileSelect(MissileType.BASIC);
                Systems.WeaponSelect.SetActive(false);
                break;
            case "EMP Missile":
                Debug.Log("EMP message");
                m_shootObj.MissileSelect(MissileType.EMP);
                Systems.WeaponSelect.SetActive(false);
                break;
            case "Chromatic Missile":
                Debug.Log("Chromatic message");
                m_shootObj.MissileSelect(MissileType.CHROMATIC);
                Systems.WeaponSelect.SetActive(false);
                break;
            case "ShieldBreaker Missile":
                Debug.Log("ShieldBreak message");
                m_shootObj.MissileSelect(MissileType.SHIELDBREAKER);
                Systems.WeaponSelect.SetActive(false);
                break;

            case "CloakButton":
                Systems.Cloak.Activate(!Systems.Cloak.Activated);
                break;

            case "HyperDriveButton":
                Systems.HyperDrive.HyperDriveInitialize();
                break;

            case "MissileButton":
                Systems.Missiles.FireMissile();
                break;

            case "MissionLogButton":
                Systems.MissionLog.SetActiveRecursively(true);
                break;

            case "ToggleRadarButton":
                Systems.ToggleDeviceStatus(SystemType.RADAR);
                break;

            case "EmpButton":
                Systems.Emp.SetEmpActive(true);
                break;

            case "DecoyButton":
                Systems.Decoy.LeaveDecoy();
                break;
            case "WeaponSelectButton":
                Systems.WeaponSelect.SetActiveRecursively(true);
                break;

            default:
                Debug.Log("Switching Scene : " + transform.name);
                SceneManager.LoadScene(transform.name);
                break;
        }
        m_button.color = Color.white;

        if (settings.Active)
            settings.CloseSettings();
    }
}