using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArmButtons : MonoBehaviour {

    #region Properties
    private bool initialized;
    private float delay;
    private float transition;
    private float cancelTimer;

    private Image m_button;
    private ShipDevices devices;
    private ArmSettings settings;
    #endregion


    // Use this for initialization
    void Start() {
        delay = .5f;
        transition = 0.0f;
        cancelTimer = 0.0f;                

        m_button = null;
        initialized = false;
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
        if (devices == null)
            devices = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<ShipDevices>();

        initialized = true;
    }

    void UpdateCooldowns()
    {
        switch (transform.name)
        {
            case "HyperDriveButton":
                if (devices.HyperDrive.Cooldown > 0.0f)
                    m_button.color = Color.red;
                else
                    m_button.color = Color.white;
                break;

            case "CloakButton":
                if (devices.Cloak.Cooldown > 0.0f)
                    m_button.color = Color.red;
                else
                    m_button.color = Color.white;
                break;
            case "EmpButton":
                if (devices.Emp.Cooldown > 0.0f)
                    m_button.color = Color.red;
                else
                    m_button.color = Color.white;
                break;

            case "MissileButton":
                if (devices.MissileLaunch.Cooldown > 0.0f)
                    m_button.color = Color.red;
                else
                    m_button.color = Color.white;
                break;
        }
    }

    #region Collision
    public void OnTriggerEnter(Collider col) {
        if (col.name == "bone3")
        {
            Initialize();
            transition = 0.1f;
            cancelTimer = 1.5f;
            m_button.color = Color.grey;                     
        }        
    }

    public void OnTriggerStay(Collider col) {
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

    public void OnTriggerExit(Collider col) {        
        if (col.name == "bone3")
            if (m_button.color == Color.green)
                ActivateButton();   
    }
    #endregion

    private void ActivateButton() {        
        switch (transform.name)
        {
            case "CloseSettings":
                settings.CloseSettings();
                return; 

            case "CloakButton":
                devices.Cloak.Activate(!devices.Cloak.Activated);
                break;

            case "HyperDriveButton":
                devices.HyperDrive.HyperDriveInitialize();
                break;

            case "MissileButton":
                devices.MissileLaunch.FireMissile();
                break;

            case "MissionLogButton":
                devices.MissionLog.SetActiveRecursively(true);
                break;

            case "ToggleRadarButton":
                devices.Radar.SetActive(!devices.Radar.activeSelf);
                break;

            case "EmpButton":
                devices.Emp.SetEmpActive(true);
                break;

            case "DecoyButton":
                devices.Decoy.LeaveDecoy();
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