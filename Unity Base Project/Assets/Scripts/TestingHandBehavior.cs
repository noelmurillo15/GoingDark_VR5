﻿using UnityEngine;
using System.Collections;

public class TestingHandBehavior : MonoBehaviour {

    public bool jsAttached;
    public LeapData m_leapData;

	// Use this for initialization
	void Start () {
        jsAttached = false;
        m_leapData = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<LeapData>();
	}

    void Awake() {
        jsAttached = false;
        if(m_leapData == null)
            m_leapData = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<LeapData>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void SetJSAttached(bool boolean) {
        jsAttached = boolean;
    }

    public bool GetisHandClosed() {
        if (m_leapData.GetNumRFingersHeld() <= 1)
            return true;

        return false;
    }

    public Vector3 GetPalmVelocity() {
        Vector3 velocity;
        velocity = m_leapData.GetRPalmVelocity();
        velocity /= 10.0f;

        if (velocity.x < 12.0f && velocity.x > -12.0f)
            velocity.x = 0.0f;

        if (velocity.y < 12.0f && velocity.y > -12.0f)
            velocity.y = 0.0f;

        return velocity;
    }
}