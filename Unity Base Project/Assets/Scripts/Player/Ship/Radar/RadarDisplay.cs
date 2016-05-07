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
        if (!attached)
        {
            m_radar.transform.position = originWorldPos;
            m_radar.transform.localPosition = originLocalPos;
        }
    }

    void OnTriggerEnter(Collider col) {
        if (col.name == "rightPalm") {
            attached = true;
            if (m_rightPalm == null)
                m_rightPalm = col.gameObject;            
        }
    }

    void OnTriggerStay(Collider col) {
        if (col.name == "rightPalm") {
            if (m_leapData.GetIsRHandOnScreen()) {
                if (m_leapData.GetRPalmNormals().z < 0.0f)
                {
                    Vector3 pos = m_rightPalm.transform.position;
                    pos.y += 0.25f;
                    m_radar.transform.position = pos;
                }
                else
                    attached = false;
            }
        }
    }
}
