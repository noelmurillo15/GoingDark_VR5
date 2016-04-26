using UnityEngine;
using System.Collections;

public class TestingThruster : MonoBehaviour {
    //**        Attach to Thruster Prefab       **//

    public bool inRange;
    public float offset;
    public float percentage;
    public ControlPanel cpanel;
    public TestingHandBehavior m_palm;
    public JoyStickMovement m_playerMove;


    // Use this for initialization
    void Start()
    {
        inRange = false;
        offset = 0.0044f;
        percentage = 0.0f;
        cpanel = GameObject.Find("ControlPanel").GetComponent<ControlPanel>();
        m_playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<JoyStickMovement>();
    }

    // Update is called once per frame
    void Update() {
        if (inRange) {
            if (m_palm.GetisLHandClosed() && m_palm.GetIsLeftHandIn()) {
                Vector3 velocity;
                velocity = m_palm.GetLPalmVelocity();
                velocity.z = velocity.y;
                velocity.x = 0.0f;
                velocity.y = 0.0f;

                if(transform.localPosition.z > -offset)
                    percentage = (transform.localPosition.z + offset) / (offset * 2);

                if ((transform.localPosition.z + (velocity.z * Time.deltaTime * 0.0008f)) > -offset  &&
                    (transform.localPosition.z + (velocity.z * Time.deltaTime * 0.0008f)) < offset)
                    transform.localPosition += (velocity * Time.deltaTime * 0.0008f);                
            }
        }

        if (transform.localPosition.z < -0.004f)
            m_playerMove.DecreaseSpeed();
        else if (transform.localPosition.z > -0.004f)
            m_playerMove.IncreaseSpeed(percentage);

        cpanel.UpdateSpeedGauge();
    }

    #region Collision Detection
    void OnTriggerEnter(Collider col)
    {
        if (col.name == "leftPalm" || col.name == "bone1" || col.name == "bone2" || col.name == "bone3")
            inRange = true;
    }
    void OnTriggerExit(Collider col)
    {
        if (col.name == "leftPalm" || col.name == "bone1" || col.name == "bone2" || col.name == "bone3")
            inRange = false;
    }
    #endregion
}