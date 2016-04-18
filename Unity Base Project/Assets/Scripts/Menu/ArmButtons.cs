using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArmButtons : MonoBehaviour {

    private Image m_button;
    private float transition;
    private float cancelTimer;
    private ArmSettings m_armSettings;


    // Use this for initialization
    void Start()
    {
        transition = 0.0f;
        cancelTimer = 0.0f;
        m_button = GetComponent<Image>();

        if (m_armSettings == null)
            m_armSettings = GameObject.Find("leftForearm").GetComponent<ArmSettings>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Collision
    public void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3")
        {
            if (col.transform.parent.name == "rightIndex")
            {
                transition = 0.1f;
                cancelTimer = 1.25f;
                m_button.CrossFadeColor(Color.green, 0.1f, false, false);
            }
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.name == "bone3")
        {
            if (col.transform.parent.name == "rightIndex")
            {
                transition -= Time.deltaTime;
                cancelTimer -= Time.deltaTime;

                if (transition <= 0.0f)
                {
                    m_button.CrossFadeColor(Color.white, 0.01f, false, false);
                    m_button.color = Color.green;
                }

                if (cancelTimer <= 0.0f)
                    m_button.color = Color.red;
            }
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.name == "bone3")
        {
            if (col.transform.parent.name == "rightIndex")
            {
                if (m_button.color == Color.green)
                    TransitionScene();
                else
                {
                    m_button.color = Color.white;
                    m_button.CrossFadeColor(Color.white, 0.01f, false, false);
                }
            }
        }
    }
    #endregion

    private void TransitionScene()
    {
        if (transform.name == "CloseSettings")
            m_armSettings.CloseSettings();
        else
        {
            Debug.Log("Switching Scene : " + transform.name);
            SceneManager.LoadScene(transform.name);
        }
    }
}