﻿using UnityEngine;
using UnityEngine.UI;

public class MissionLogButtons : MonoBehaviour
{


    private Image m_button;
    private float transition;
    private float cancelTimer;
    private MissionLog missionLog;
    private GameObject buttonObject;
    // Use this for initialization

    void Start()
    {
        m_button = GetComponent<Image>();
        missionLog = GameObject.Find("MissionLog").GetComponent<MissionLog>();
        buttonObject = GameObject.Find("MissionPanel");

        transition = 0.0f;
        cancelTimer = 0.0f;
    }

    // Update is called once per frame

    #region Collision
    public void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3")
        {
            if (col.transform.parent.name == "index")
            {
                Debug.Log("Missionlog enter");
                transition = 0.25f;
                cancelTimer = 1.25f;
                m_button.CrossFadeColor(Color.blue, 0.1f, false, false);
            }
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.name == "bone3")
        {
            if (col.transform.parent.name == "index")
            {
                transition -= Time.deltaTime;
                cancelTimer -= Time.deltaTime;

                if (transition <= 0.0f)
                {
                    m_button.CrossFadeColor(Color.white, 0.01f, false, false);
                    m_button.color = Color.blue;
                }

                if (cancelTimer <= 0.0f)
                {
                    m_button.color = Color.red;
                }
            }
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.name == "bone3")
        {
            if (col.transform.parent.name == "index")
            {
                if (m_button.color == Color.blue || m_button.color == Color.green)
                {
                    m_button.color = Color.white;
                }
                else
                {
                    m_button.color = Color.white;
                    m_button.CrossFadeColor(Color.white, 0.01f, false, false);
                }
            }
        }
    }
    #endregion


}