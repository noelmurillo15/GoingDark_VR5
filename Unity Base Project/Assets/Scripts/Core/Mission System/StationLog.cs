using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GoingDark.Core.Enums;

public class StationLog : MonoBehaviour
{

    private string SceneName;
    private Tutorial m_tutorial;
    private TutorialFlight m_tutorial2;
    private MissionSystem m_missionSystem;
    private MissionLog m_missionLog;
    private Button mAccept;
    private Button mTurnIn;
    private Button[] mButtons;
    private Text m_tMissionInfo;
    private Text m_tCredits;

    [HideInInspector]
    public Button mLastButton;
    public string filename;
    public GameObject m_pMissions;
    public GameObject m_pStationPanel;
    [SerializeField]
    private GameObject m_pMissionInfo;

    private GameObject mPlayer;

    private bool mDocked;
    // Use this for initialization
    void Start()
    {
        mDocked = false;
        SceneName = SceneManager.GetActiveScene().name;

        if (SceneManager.GetActiveScene().name == "Tutorial")
            m_tutorial = GameObject.Find("TutorialPref").GetComponent<Tutorial>();
        if (SceneManager.GetActiveScene().name == "Tutorial2")
            m_tutorial2 = GameObject.Find("TutorialPrefF").GetComponent<TutorialFlight>();

        m_missionSystem = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
        m_missionLog = GetComponent<MissionLog>();
        mPlayer = GameObject.Find("Player");

        mButtons = m_pMissions.GetComponentsInChildren<Button>();

        // for meow
        Button[] temp = m_pMissionInfo.GetComponentsInChildren<Button>();
        mAccept = temp[0];
        mTurnIn = temp[2];

        Text[] text = m_pMissionInfo.GetComponentsInChildren<Text>();
        m_tMissionInfo = text[0];
        m_tCredits = text[1];

    }

    public void Docked(bool isDocked)
    {
        if (isDocked)
        {
            Debug.Log("Opening Station Panel");
            m_pStationPanel.SetActive(true);
        }
        else
        {
            // close all station panels
            CloseStationPanels();
        }
    }

    void CloseStationPanels()
    {
        if (m_pMissionInfo.activeSelf)
            m_pMissionInfo.SetActive(false);
        else if (m_pMissions.activeSelf)
            m_pMissions.SetActive(false);
        else if (m_pStationPanel.activeSelf)
            m_pStationPanel.SetActive(false);
    }


    void ShowMissionInfo(Mission mission, string buttonName)
    {
        m_pMissions.SetActive(false);
        m_pMissionInfo.SetActive(true);

        m_tMissionInfo.text = mission.missionInfo;
        m_tCredits.text = "Reward : " + mission.credits + " credits";

        if (m_missionSystem.m_ActiveMissions.Exists(s => s.missionName == buttonName))
        {
            Mission tempMission = m_missionSystem.m_ActiveMissions.Find(s => s.missionName == buttonName);
            mAccept.gameObject.SetActive(false);

            if (tempMission.completed)
                mTurnIn.gameObject.SetActive(true);
            else
                mTurnIn.gameObject.SetActive(false);
        }
        else
        {

            mAccept.gameObject.SetActive(true);
            mTurnIn.gameObject.SetActive(false);
        }
    }

    void AddMissions(string missionName)
    {
        for (int i = 0; i < m_missionSystem.m_stationMissions.Count; i++)
        {
            if (missionName == m_missionSystem.m_stationMissions[i].missionName)
            {
                Debug.Log("AddMissions" + missionName);
                m_missionSystem.AddActiveMission(m_missionSystem.m_stationMissions[i]);
            }
        }
    }

    void TurnInMission(string buttonName)
    {
        for (int i = 0; i < m_missionSystem.m_stationMissions.Count; i++)
        {
            if (buttonName == m_missionSystem.m_stationMissions[i].missionName)
            {
                m_missionSystem.TurnInMission(buttonName);
                break;
            }
        }

    }

    void AssignInfo(Mission mission)
    {
        m_tMissionInfo.text = mission.missionInfo;
        m_tCredits.text = "Reward : " + mission.credits + " credits";
    }


    #region Public Methods
    public void OpenStationMissions()
    {
        if (m_pMissionInfo.activeSelf)
            m_pMissionInfo.SetActive(false);

        m_pStationPanel.SetActive(false);
        m_pMissions.SetActive(true);

        for (int i = 0; i < m_missionSystem.m_stationMissions.Count; i++)
        {
            mButtons[i].gameObject.name = m_missionSystem.m_stationMissions[i].missionName;
            mButtons[i].gameObject.SetActive(true);
            mButtons[i].GetComponentInChildren<Text>().text = mButtons[i].gameObject.name;
        }

    }

    void OpenStationPanel()
    {
        m_pMissionInfo.SetActive(false);
        m_pMissions.SetActive(false);
        m_pStationPanel.SetActive(true);
    }

    public void StationButtonPressed(string buttonName)
    {

        if (m_missionSystem.m_stationMissions.Exists(s => s.missionName == buttonName))
        {
            Debug.Log("Station Mission Info Open");
            Mission mission = m_missionSystem.m_stationMissions.Find(s => s.missionName == buttonName);
            ShowMissionInfo(mission, buttonName);
        }
        else if (buttonName == "Back")
        {
            if (mLastButton.name == "Back" || mLastButton.name == "AcceptMission" || mLastButton.name == "Missions" || mLastButton.name == "TurnIn")
            {
                OpenStationPanel();
            }
            else/* if (mLastButton.name != "Back")*/
            {
                OpenStationMissions();
            }
        }
        else if (buttonName == "AcceptMission")
        {
            if (SceneName == "Tutorial")
                m_tutorial.SendMessage("MissionAccepted", mLastButton.name);
            else if (SceneName == "Tutorial2")
                m_tutorial2.SendMessage("MissionAccepted", mLastButton.name);

            mAccept.gameObject.SetActive(false);
            AddMissions(mLastButton.name);
            mLastButton = mAccept;
            OpenStationMissions();
        }
        else if (buttonName == "TurnIn")
        {
            if (SceneName == "Tutorial")
                m_tutorial.SendMessage("MissionTurnedIn", mLastButton.name);
            else if (SceneName == "Tutorial2")
                m_tutorial2.SendMessage("MissionTurnedIn", mLastButton.name);

            TurnInMission(mLastButton.name);
            mLastButton = mTurnIn;
            OpenStationMissions();
        }
    }
    #endregion
}
