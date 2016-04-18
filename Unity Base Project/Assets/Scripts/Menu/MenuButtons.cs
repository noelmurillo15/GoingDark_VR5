using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuButtons : MonoBehaviour {

    private Image m_button;
    private float transition;
    private float cancelTimer;
    private PlayerData playerData;
    private EMP emp;
    
    // Use this for initialization
    void Start() {
        transition = 0.0f;
        cancelTimer = 0.0f;
        m_button = GetComponent<Image>();     
                            
    }

    // Update is called once per frame
    void Update() {
        
    }

    #region Collision
    public void OnTriggerEnter(Collider col) {
        if (col.name == "bone3") {
            if (col.transform.parent.name == "leftIndex" || col.transform.parent.name == "rightIndex") {
                transition = 0.1f;
                cancelTimer = 1.25f;
                m_button.CrossFadeColor(Color.green, 0.1f, false, false);
            }
        }
    }

    public void OnTriggerStay(Collider col) {
        if (col.name == "bone3") {
            if (col.transform.parent.name == "leftIndex" || col.transform.parent.name == "rightIndex") {
                transition -= Time.deltaTime;
                cancelTimer -= Time.deltaTime;
                
                if (transition <= 0.0f) {
                    m_button.CrossFadeColor(Color.white, 0.01f, false, false);
                    m_button.color = Color.green;
                }

                if (cancelTimer <= 0.0f)
                    m_button.color = Color.red;
            }
        }
    }

    public void OnTriggerExit(Collider col) {
        if (col.name == "bone3") {
            if (col.transform.parent.name == "leftIndex" || col.transform.parent.name == "rightIndex") {
                if (m_button.color == Color.green)
                    TransitionScene();
                else {
                    m_button.color = Color.white;
                    m_button.CrossFadeColor(Color.white, 0.01f, false, false);
                }
            }
        }
    }
    #endregion
    
    private void TransitionScene() {
        if (transform.name == "Exit") {
#if UNITY_EDITOR
            Debug.Log("Exiting Editor...");
            EditorApplication.isPlaying = false;
#else
            Debug.Log("Exiting Application...");
            Application.Quit(); //    does not work in editor only for builds
#endif
        }
        else if (transform.name == "EMPButton")
        {
            if (emp.GetEmpCooldown() <= 0.0f)
            {
                emp.SetEmpActive(true);
            }
        }
        else if (transform.name == "CloakButton")
        {
            if (playerData.GetCloakCooldown() <= 0.0f && playerData.isCloaked == false)
            {
                playerData.SetCloaked(true);
                m_button.GetComponent<Text>().text = "Uncloak";
            }
            else if (playerData.isCloaked)
            {
                playerData.SetCloaked(false);
                m_button.GetComponent<Text>().text = "Cloak";
            }
        }
        else {
            Debug.Log("Switching Scene : " + transform.name);
            SceneManager.LoadScene(transform.name);
        }
    }
}