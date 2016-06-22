using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StationButtons : MonoBehaviour
{

    private Image m_button;
    private float transition;
    private float cancelTimer;
    private StationLog stationLog;

    void Start()
    {
        m_button = GetComponent<Image>();
        stationLog = GameObject.Find("MissionLog").GetComponent<StationLog>();

        transition = 0.0f;
        cancelTimer = 0.0f;
    }

    #region Collision
    public void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3")
        {
            if (col.transform.parent.name == "index")
            {
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
                    ClickButton();
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

    public void ClickButton()
    {

        if (transform.name == "Missions")
        {
            //stationLog.SendMessage("OpenStationMissions");
            stationLog.OpenStationMissions();
        }
        else if (transform.name == "Repair")
        {

        }
        else
        {
            //stationLog.SendMessage("ButtonPressed", transform.name);
            stationLog.StationButtonPressed(transform.name);
        }

        stationLog.mLastButton = gameObject.GetComponent<Button>();
    }
}
