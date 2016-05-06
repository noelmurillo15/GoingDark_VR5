using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArmButtons : MonoBehaviour {

    private float transition;
    private float cancelTimer;

    private Image m_button;
    private ArmSettings m_armSettings;
    private GameObject m_missionLog;

    // Use this for initialization
    void Start() {
        transition = 0.0f;
        cancelTimer = 0.0f;
        m_button = null;     

        if (m_armSettings == null)
            m_armSettings = GameObject.Find("leftForearm").GetComponent<ArmSettings>();

        if (m_missionLog == null)
            m_missionLog = GameObject.Find("ButtonObject");
    }

    #region Collision
    public void OnTriggerEnter(Collider col) {
        if (col.name == "bone3" && col.transform.parent.name == "rightIndex")
        {
            if(m_button == null)
                m_button = GetComponent<Image>();

            transition = 0.25f;
            cancelTimer = 1.25f;
            m_button.color = Color.grey;

            if (transform.name == "HyperDriveButton")
                if (m_armSettings.HyperDriveCooldown() > 0.0f)
                    m_button.color = Color.red;

            if (transform.name == "CloakButton")
                if (m_armSettings.CloakCooldown() > 0.0f)
                    m_button.color = Color.red;
        }        
    }

    public void OnTriggerStay(Collider col) {
        if (col.name == "bone3" && col.transform.parent.name == "rightIndex")
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
        if (col.name == "bone3" && col.transform.parent.name == "rightIndex")
            if (m_button.color == Color.green)
                ActivateButton();   
    }
    #endregion

    private void ActivateButton() {
        m_button.color = Color.white;

        if (transform.name == "CloseSettings")
            m_armSettings.CloseSettings();

        else if (transform.name == "CloakButton")
            m_armSettings.SetCloak();

        else if (transform.name == "HyperDriveButton")
            m_armSettings.InitializeHyperDrive();

        else if (transform.name == "MissileButton")
            m_armSettings.FireMissile();

        else if (transform.name == "MissionLogButton")
        {
            m_missionLog.SetActiveRecursively(true);
            m_armSettings.CloseSettings();
        }

        else
        {
            Debug.Log("Switching Scene : " + transform.name);
            SceneManager.LoadScene(transform.name);
        }
    }
}