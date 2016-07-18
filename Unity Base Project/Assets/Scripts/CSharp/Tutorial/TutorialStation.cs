using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialStation : MonoBehaviour {
    private Tutorial m_tutorial;
    private TutorialFlight m_tutorial2;
    private string m_sceneName;

    // Use this for initialization
    void Start () {
        
        m_tutorial2 = GameObject.Find("TutorialPref").GetComponent<TutorialFlight>();
    
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {

            m_tutorial2.SendMessage("EnterStation");

        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            m_tutorial2.SendMessage("ExitStation");

        }

    }
}
