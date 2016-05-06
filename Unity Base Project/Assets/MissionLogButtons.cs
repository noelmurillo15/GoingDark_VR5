using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionLogButtons : MonoBehaviour
{


    private Image m_button;
    private float transition;
    private float cancelTimer;
    private GameObject missionLog;
    private GameObject buttonObject;
   // private MissionLog missionLogScript;
    // Use this for initialization
    void Start()
    {
        m_button = GetComponent<Image>();
        missionLog = GameObject.Find("MissionLog");
        buttonObject = GameObject.Find("ButtonObject");
     //   missionLogScript = missionLog.GetComponent<MissionLog>();

        transition = 0.0f;
        cancelTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Collision
    public void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3")
        {
            if (col.transform.parent.name == "leftIndex" || col.transform.parent.name == "rightIndex")
            {
                transition = 0.05f;
                cancelTimer = 1.0f;
                if (transform.name == "CloseLog")
                    m_button.CrossFadeColor(Color.green, 0.1f, false, false);
                else
                    m_button.CrossFadeColor(Color.blue, 0.1f, false, false);

                AudioManager.instance.PlayMenuGood();
            }
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.name == "bone3")
        {
            if (col.transform.parent.name == "leftIndex" || col.transform.parent.name == "rightIndex")
            {
                transition -= Time.deltaTime;
                cancelTimer -= Time.deltaTime;

                if (transition <= 0.0f)
                {
                    m_button.CrossFadeColor(Color.white, 0.01f, false, false);
                    if (transform.name == "CloseLog")
                        m_button.color = Color.green;
                    else
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
            if (col.transform.parent.name == "leftIndex" || col.transform.parent.name == "rightIndex")
            {
                if (m_button.color == Color.blue || m_button.color == Color.green)
                {
                    ClickButton();
                    m_button.color = Color.white;
                }
                else
                {
                    AudioManager.instance.PlayMenuBad();
                    m_button.color = Color.white;
                    m_button.CrossFadeColor(Color.white, 0.01f, false, false);
                }
            }
        }
    }
    #endregion


    private void ClickButton()
    {
        if (transform.name == "CloseLog")
        {
            buttonObject.SetActive(false);
            missionLog.SendMessage(transform.name);
        }
        else
            missionLog.SendMessage(transform.name);
    }

}
