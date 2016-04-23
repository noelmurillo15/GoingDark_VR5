using UnityEngine;
using System.Collections;

public class RadarDisplay : MonoBehaviour {

    private bool attached;
    private Vector3 originLocalPos;
    private Vector3 originWorldPos;
    private LeapData m_leapData;
    private GameObject m_rightPalm;
    private GameObject m_radar;

    // Use this for initialization
    void Start()
    {
        attached = false;        
        m_leapData = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<LeapData>();

        m_radar = transform.parent.gameObject;
        originLocalPos = m_radar.transform.localPosition;
        originWorldPos = m_radar.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "rightPalm")
        {
            Debug.Log(col.name + " triggered radar");
            attached = true;

            if (m_rightPalm == null)
                m_rightPalm = col.gameObject;            
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.name == "rightPalm")
        {
            if (m_leapData.GetIsRHandOnScreen() && m_leapData.GetNumRFingersHeld() == 0)
            {
                Vector3 pos = m_rightPalm.transform.position;
                pos.y += 0.25f;
                m_radar.transform.position = pos;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == "rightPalm")
        {
            Debug.Log(col.name + " not triggering radar");
            attached = false;
            m_radar.transform.position = originWorldPos;
            m_radar.transform.localPosition = originLocalPos;
        }
    }
}
