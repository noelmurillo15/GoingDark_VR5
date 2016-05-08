using UnityEngine;

public class HeadMovement : MonoBehaviour {
    //**    Attach to Main Camera   **//
    private LeapData m_leapData;
    private GameObject m_Player;


    // Use this for initialization
    void Start() {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_leapData = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<LeapData>();
    }

    // Update is called once per frame
    void Update() {
        if(m_leapData.GetNumHands() == 0)
            m_Player.transform.rotation = Quaternion.Slerp(m_Player.transform.rotation, transform.rotation, Time.deltaTime * 0.5f);
    }
}