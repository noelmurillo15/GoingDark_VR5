using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{

    #region Properties
    private float delay;
    private float transition;
    private float cancelTimer;

    private Image m_button;
    #endregion


    // Use this for initialization
    void Start()
    {
        transition = 0f;
        cancelTimer = 0f;
        m_button = GetComponent<Image>();
    }

    void Update()
    {

    }

    #region Private Methods
    private void ActivateButton()
    {
        AudioManager.instance.PlayMenuGood();

        switch (transform.name)
        {
            case "Exit":
                Application.Quit();
                break;


            default:
                Debug.Log("Loading Scene : " + transform.name);
                SceneManager.LoadScene(transform.name);
                break;
        }
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