﻿using UnityEngine;
using System.Collections;

public class Thrusters : MonoBehaviour {
    //**    Attach to Thruster  **//
    public bool inRange;
    public float offset;
    public float percentage;
    public HandBehavior m_palm;
    public PlayerMovement m_playerMove;

    private GameObject speedBarColor1;
    private GameObject speedBarColor2;


    // Use this for initialization
    void Start()
    {
        inRange = false;
        offset = 0.0044f;
        percentage = 0.0f;
        speedBarColor1 = GameObject.Find("SpeedColor1");
        speedBarColor2 = GameObject.Find("SpeedColor2");
        m_playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
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
        {
            m_playerMove.DecreaseSpeed();
            UpdateSpeedGauge();
        }
        else if (transform.localPosition.z > -0.004f)
        {
            m_playerMove.IncreaseSpeed(percentage);
            UpdateSpeedGauge();
        }        
    }

    private void UpdateSpeedGauge()
    {
        float percentage = m_playerMove.GetSpeed() / m_playerMove.GetMaxSpeed();

        Vector3 newScale;
        newScale.x = speedBarColor1.transform.localScale.x;
        newScale.y = percentage * 0.001f;
        newScale.z = speedBarColor1.transform.localScale.z;

        speedBarColor1.transform.localScale = newScale;
        speedBarColor2.transform.localScale = newScale;

        Vector3 newPos = speedBarColor1.transform.localPosition;
        float offset = (percentage * 0.00456f) - 0.00456f;
        newPos.z = offset;
        speedBarColor1.transform.localPosition = newPos;

        newPos = speedBarColor2.transform.localPosition;
        newPos.z = offset;
        speedBarColor2.transform.localPosition = newPos;
    }

    #region Collision Detection
    void OnTriggerEnter(Collider col)
    {
        if (col.name == "leftPalm")
            inRange = true;
    }
    void OnTriggerExit(Collider col)
    {
        if (col.name == "leftPalm")
            inRange = false;
    }
    #endregion
}