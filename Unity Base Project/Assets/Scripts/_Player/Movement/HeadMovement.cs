using UnityEngine;

public class HeadMovement : MonoBehaviour {
    //**    Attach to Main Camera   **//

    #region Properties
    private LeapData m_leapData;
    private GameObject m_Player;
    private Transform MyTransform;
    #endregion


    // Use this for initialization
    void Start() {
        MyTransform = transform;
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_leapData = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<LeapData>();
    }

    // Update is called once per frame
    void Update() {
        if(m_leapData.GetNumHands() == 0)
            m_Player.transform.rotation = Quaternion.Slerp(m_Player.transform.rotation, MyTransform.rotation, Time.deltaTime * 0.5f);
    }
}