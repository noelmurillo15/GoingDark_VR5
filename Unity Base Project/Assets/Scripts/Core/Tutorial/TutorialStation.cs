using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialStation : MonoBehaviour {
    private Tutorial m_tutorial;
    private TutorialFlight m_tutorial2;
    private string m_sceneName;

    // Use this for initialization
    void Start () {
        m_sceneName = SceneManager.GetActiveScene().name;
        switch (m_sceneName)
        {
            case "Tutorial":
                m_tutorial = GameObject.Find("TutorialPref").GetComponent<Tutorial>();
                break;
            case "Tutorial2":
                m_tutorial2 = GameObject.Find("TutorialPrefF").GetComponent<TutorialFlight>();
                break;
            default:
                break;
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        switch (m_sceneName)
        {
            case "Tutorial":
                m_tutorial.SendMessage("EnterStation");
                break;
            case "Tutorial2":
                m_tutorial2.SendMessage("EnterStation");
                break;
            default:
                break;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        switch (m_sceneName)
        {
            case "Tutorial":
                m_tutorial.SendMessage("ExitStation");
                break;
            case "Tutorial2":
                m_tutorial2.SendMessage("ExitStation");
                break;
            default:
                break;
        }

    }
}
