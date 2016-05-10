using UnityEngine;

public class RadarDisplay : MonoBehaviour
{

    private bool attached;
    private bool SetInPlace;
    private Vector3 originLocalPos;
    private Vector3 originWorldPos;
    private LeapData m_leapData;
    private GameObject m_rightPalm;
    private GameObject m_radar;
    Vector3 SetPos;
    // Use this for initialization
    void Start()
    {
        attached = false;
        SetInPlace = false;
        m_leapData = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<LeapData>();

        m_radar = transform.parent.gameObject;
        originLocalPos = m_radar.transform.localPosition;
        originWorldPos = m_radar.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (!attached && !SetInPlace)
        {
            m_radar.transform.position = originWorldPos;
            m_radar.transform.localPosition = originLocalPos;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "rightPalm")
        {
            attached = true;
            if (m_rightPalm == null)
                m_rightPalm = col.gameObject;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.name == "rightPalm")
        {
            if (m_leapData.GetIsRHandOnScreen())
            {

                if (m_leapData.GetRPalmNormals().z < 0.0f)
                {
                    Vector3 pos = m_rightPalm.transform.position;
                    pos.y += 0.25f;
                    m_radar.transform.position = pos;
                    SetPos = pos;
                    SetInPlace = false;
                    if (m_leapData.GetNumRFingersHeld() < 1.0f)
                    {
                        m_radar.transform.position = SetPos;
                        SetInPlace = true;
                    }
                }
                else
                    attached = false;
            }
        }
    }
}
