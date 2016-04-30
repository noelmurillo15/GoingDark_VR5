using UnityEngine;

public class HeadMovement : MonoBehaviour {
    //**    Attach to Main Camera   **//
    private LeapData m_leapData;
    private PlayerMovement m_playerMove;


    // Use this for initialization
    void Start() {
        m_leapData = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<LeapData>();
        m_playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update() {
        if(m_leapData.GetNumHands() == 0)
            m_playerMove.SetPlayerRotation(this.transform.rotation);
    }
}