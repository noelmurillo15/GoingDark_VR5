using UnityEngine;

public class MenuEnvironment : MonoBehaviour {

    private GameObject m_Ship;
    private GameObject m_Thrusters;

    // Use this for initialization
    void Start() {
        if(m_Ship == null)
            m_Ship = GameObject.FindGameObjectWithTag("PlayerShip");

        if(m_Thrusters == null)
            m_Thrusters = GameObject.Find("Environment/Afterburner");
    }

    // Update is called once per frame
    void Update () {
        m_Ship.transform.Translate(0.0f, 0.0f, 12.0f * Time.deltaTime);
        m_Thrusters.transform.Translate(0.0f, 0.0f, -12.0f * Time.deltaTime);
    }
}
