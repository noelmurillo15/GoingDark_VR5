using UnityEngine;
using System.Collections;

public class TestingHandBehavior : MonoBehaviour {

    public bool jsAttached;
    public GameObject joystick;
    public LeapData m_leapData;

    public Vector3 velocity;

	// Use this for initialization
	void Start () {
        jsAttached = false;
        joystick = GameObject.Find("JoyStick");
        m_leapData = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<LeapData>();
	}

    void Awake()
    {
        jsAttached = false;
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void SetJSAttached(bool boolean)
    {
        jsAttached = boolean;
    }

    public bool GetisHandClosed()
    {
        if (m_leapData.GetNumRFingersHeld() <= 1)
            return true;

        return false;
    }

    public Vector3 GetPalmVelocity()
    {
        velocity = m_leapData.GetRPalmVelocity();
        velocity /= 10.0f;

        if (velocity.x < 12.0f && velocity.x > -12.0f)
            velocity.x = 0.0f;

        if (velocity.y < 12.0f && velocity.y > -12.0f)
            velocity.y = 0.0f;

        return velocity;
    }
}