﻿using UnityEngine;
using System.Collections;

public class TestingHandBehavior : MonoBehaviour {

    public LeapData m_leapData;

	// Use this for initialization
	void Start () {
        m_leapData = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<LeapData>();
	}

    void Awake() {
        if(m_leapData == null)
            m_leapData = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<LeapData>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public bool GetisRHandClosed() {        
        if (m_leapData.GetNumRFingersHeld() < 2)
            return true;

        return false;
    }
    public bool GetisLHandClosed() {      
        if (m_leapData.GetNumLFingersHeld() < 2)
            return true;

        return false;
    }
    public bool GetIsLeftHandIn()
    {
        return m_leapData.GetIsLHandOnScreen();
    }

    public bool GetIsRightHandIn()
    {
        return m_leapData.GetIsRHandOnScreen();
    }

    public Vector3 GetRPalmVelocity() {
        Vector3 velocity;
        velocity = m_leapData.GetRPalmVelocity();
        velocity /= 10.0f;

        if (velocity.x < 12.0f && velocity.x > -12.0f)
            velocity.x = 0.0f;

        if (velocity.y < 12.0f && velocity.y > -12.0f)
            velocity.y = 0.0f;

        return velocity;
    }
    public Vector3 GetLPalmVelocity()
    {
        Vector3 velocity;
        velocity = m_leapData.GetLPalmVelocity();
        velocity /= 10.0f;

        if (velocity.x < 12.0f && velocity.x > -12.0f)
            velocity.x = 0.0f;

        if (velocity.y < 12.0f && velocity.y > -12.0f)
            velocity.y = 0.0f;

        if (velocity.z < 12.0f && velocity.y > -12.0f)
            velocity.z = 0.0f;

        return velocity;
    }
}