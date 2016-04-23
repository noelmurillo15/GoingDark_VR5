﻿using UnityEngine;
using System.Collections;

public class TestingJoyStick : MonoBehaviour {

    public bool inRange;
    public bool palmAttached;

    public Transform m_handPos;
    public TestingHandBehavior m_palm;
    public JoyStickMovement m_playerMove;

    public Quaternion originalRotation;


	// Use this for initialization
	void Start () {
        inRange = false;
        m_handPos = null;
        palmAttached = false;
        originalRotation = transform.localRotation;                  
        m_playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<JoyStickMovement>();
    }

    // Update is called once per frame
    void Update() {

        if (inRange)
        {
            if (m_palm.GetisRHandClosed() && m_palm.GetIsRightHandIn())
            {
                Vector3 velocity;
                velocity = m_palm.GetRPalmVelocity();
                velocity.z = velocity.x;
                velocity.x = velocity.y;
                velocity.y = 0.0f;
                transform.Rotate((velocity * 1.5f) * Time.deltaTime);
            }
            else if (!m_palm.GetisRHandClosed())
            {
                m_playerMove.turnRateZero();
                transform.localRotation = originalRotation;
            }
        }

        if (transform.localEulerAngles.x > 1.0f && transform.localEulerAngles.x < 90.0f)
            m_playerMove.goDown();
        else if (transform.localEulerAngles.x > 90.0f && transform.localEulerAngles.x < 360.0f)
            m_playerMove.goUp();

        if (transform.localEulerAngles.z > 1.0f && transform.localEulerAngles.z < 90.0f)
            m_playerMove.TurnLeft();
        else if (transform.localEulerAngles.z > 90.0f && transform.localEulerAngles.z < 360.0f)
            m_playerMove.TurnRight();
    }

    void OnTriggerEnter(Collider col) {
        if (col.name == "leftPalm" || col.name == "bone1" || col.name == "bone2" || col.name == "bone3")
            inRange = true;
    }

    void OnTriggerExit(Collider col) {
        if (col.name == "leftPalm" || col.name == "bone1" || col.name == "bone2" || col.name == "bone3")
            inRange = false;
    }
}