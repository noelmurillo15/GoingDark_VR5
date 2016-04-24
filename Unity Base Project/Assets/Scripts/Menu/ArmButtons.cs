using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArmButtons : MonoBehaviour {

    private float transition;
    private float cancelTimer;

    private Image m_button;
    private ArmSettings m_armSettings;
   

    // Use this for initialization
    void Start() {
        transition = 0.0f;
        cancelTimer = 0.0f;
        m_button = GetComponent<Image>();

        if (m_armSettings == null)
            m_armSettings = GameObject.Find("leftForearm").GetComponent<ArmSettings>();
    }

    #region Collision
    public void OnTriggerEnter(Collider col) {
        if (col.name == "bone3" && col.transform.parent.name == "rightIndex") {
            transition = 0.25f;
            cancelTimer = 1.25f;
            m_button.color = Color.grey;            
        }
    }

    public void OnTriggerStay(Collider col) {
        if (col.name == "bone3" && col.transform.parent.name == "rightIndex") {
            if (cancelTimer > 0.0f) {
                transition -= Time.deltaTime;
                cancelTimer -= Time.deltaTime;

                if (transition <= 0.0f)
                    m_button.color = Color.green;
            }
            else
                m_button.color = Color.red;            
        }
    }

    public void OnTriggerExit(Collider col) {
        if(m_button.color == Color.green)
            if (col.name == "bone3" && col.transform.parent.name == "rightIndex")
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

        else if (transform.name == "MonitorPower")
            m_armSettings.MonitorPower();

        else
        {
            Debug.Log("Switching Scene : " + transform.name);
            SceneManager.LoadScene(transform.name);
        }
    }
}