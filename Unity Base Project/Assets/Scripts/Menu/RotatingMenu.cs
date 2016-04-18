using UnityEngine;
using System.Collections;

public class RotatingMenu : MonoBehaviour {

    private float velocity;
    private LeapData m_leapData;


    // Use this for initialization
    void Start() { 
        if (m_leapData == null)
            m_leapData = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<LeapData>();
    }

    // Update is called once per frame
    void Update() {
        if (velocity > 0.2f)
            velocity -= 20 * Time.deltaTime;
        else if (velocity < -0.2f)
            velocity += 20 * Time.deltaTime;
        else
            velocity = 0.0f;

    
        if (m_leapData.GetLPalmVelocity().x > 500.0f && m_leapData.GetLPalmNormals().x > 0.0f)
            velocity = -m_leapData.GetLPalmVelocity().x * Time.deltaTime * 0.50f;
        else if (m_leapData.GetLPalmVelocity().x < -500.0f && m_leapData.GetLPalmNormals().x < 0.0f)
            velocity = -m_leapData.GetLPalmVelocity().x * Time.deltaTime * 0.50f;
        else if (m_leapData.GetRPalmVelocity().x < -500.0f && m_leapData.GetRPalmNormals().x < 0.0f)
            velocity = -m_leapData.GetRPalmVelocity().x * Time.deltaTime * 0.50f;


        this.transform.Rotate(0.0f, velocity, 0.0f);
    }
}