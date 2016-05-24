using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionLog : MonoBehaviour
{

    // need all these public for stupid gameobjects not found when inactive >=(
    public GameObject[] buttons;
    public Image[] checkmarks;
    public GameObject missionButtonPanel;
    public Text missionInfo;
    public Text objectives;

    // station missions
    public GameObject[] stationButtons;
    public GameObject stationMissionPanel;
    public Text stationInfo;

    private MissionSystem m_missionSystem;

    private int buttonNum;
    private bool[] turnedIn;
    // Use this for initialization
    void Start()
    {
        m_missionSystem = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
        turnedIn = new bool[4];
        buttonNum = 0;

        //missionInfo.text = "Active Missions";

        ActiveMissions();
        AssignMissionInfo("MissionLog");
        AssignMissionInfo("Station");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateInfo(MissionSystem.Mission mission)
    {
        for (int i = 0; i < m_missionSystem.m_ActiveMissions.Count; i++)
        {
            if (m_missionSystem.m_ActiveMissions[i].missionName == mission.missionName)
            {
                objectives.text = "Objectives Left : " + mission.objectives;
                if (mission.completed)
                {
                    objectives.text = "Return to the Station to turn in your mission";
                }
            }
        }
    }

    void Docked(bool isDocked)
    {
        if (isDocked)
        {
            missionButtonPanel.SetActive(false);
            stationMissionPanel.SetActive(true);
        }
        else
            stationMissionPanel.SetActive(false);
    }

    void TogglePanel()
    {
        Debug.Log("Opened Mission Panel");
        ActiveMissions();
        AssignMissionInfo("MissionLog");
        missionButtonPanel.SetActive(!missionButtonPanel.activeSelf);
    }

    void ActiveMissions()
    {
        for (int i = 0; i < m_missionSystem.m_ActiveMissions.Count; i++)
            buttons[i].gameObject.SetActive(true);
    }

    void AssignMissionInfo(string name)
    {
        if (name == "MissionLog")
        {
            for (int i = 0; i < m_missionSystem.m_ActiveMissions.Count; i++)
            {
                buttons[i].GetComponentInChildren<Text>().text = m_missionSystem.m_ActiveMissions[i].missionName;
            }
        }
        else if (name == "Station")
        {
            for (int i = 0; i < 4; i++)
            {
                stationButtons[i].GetComponentInChildren<Text>().text = m_missionSystem.m_stationMissions[i].missionName;
            }
        }
    }

    void ToggleButtons(bool toggle, string name)
    {
        if (name == "Station")
        {
            for (int i = 0; i < 4; i++)
            {
                if (!turnedIn[i])
                    stationButtons[i].SetActive(toggle);
            }
        }
        else if (name == "MissionLog")
        {
            for (int i = 0; i < m_missionSystem.m_ActiveMissions.Count; i++)
            {
                buttons[i].SetActive(toggle);
            }
        }
    }


    #region Button Messages

    void ButtonPressed(string buttonName)
    {
        string obj = "Objectives left : ";
        switch (buttonName)
        {
            case "Mission1":
                {
                    ToggleButtons(false, "MissionLog");
                    missionInfo.text = m_missionSystem.m_ActiveMissions[0].missionInfo;
                    objectives.text = obj + m_missionSystem.m_ActiveMissions[0].objectives;
                    objectives.gameObject.SetActive(true);
                    missionInfo.gameObject.SetActive(true);
                    buttons[5].SetActive(true);
                    buttons[4].SetActive(false);
                    break;
                }
            case "Mission2":
                {
                    ToggleButtons(false, "MissionLog");
                    missionInfo.text = m_missionSystem.m_ActiveMissions[1].missionInfo;
                    objectives.text = obj + m_missionSystem.m_ActiveMissions[1].objectives;
                    objectives.gameObject.SetActive(true);
                    missionInfo.gameObject.SetActive(true);
                    buttons[5].SetActive(true);
                    buttons[4].SetActive(false);
                    break;
                }
            case "Mission3":
                {
                    ToggleButtons(false, "MissionLog");
                    missionInfo.text = m_missionSystem.m_ActiveMissions[2].missionInfo;
                    objectives.text = obj + m_missionSystem.m_ActiveMissions[2].objectives;
                    objectives.gameObject.SetActive(true);
                    missionInfo.gameObject.SetActive(true);
                    buttons[5].SetActive(true);
                    buttons[4].SetActive(false);
                    break;
                }
            case "Mission4":
                {
                    ToggleButtons(false, "MissionLog");
                    missionInfo.text = m_missionSystem.m_ActiveMissions[3].missionInfo;
                    objectives.text = obj + m_missionSystem.m_ActiveMissions[3].objectives;
                    objectives.gameObject.SetActive(true);
                    missionInfo.gameObject.SetActive(true);
                    buttons[5].SetActive(true);
                    buttons[4].SetActive(false);
                    break;
                }
            case "CloseLog":
                {
                    missionButtonPanel.SetActive(false);
                    missionInfo.text = "Active Missions";
                    objectives.gameObject.SetActive(false);
                    ActiveMissions();
                    break;
                }
            case "Back":
                {
                    buttons[5].SetActive(false);
                    buttons[4].SetActive(true);
                    missionInfo.text = "Active Missions";
                    ToggleButtons(true, "MissionLog");
                    break;
                }
            case "StationMission1":
                {
                    buttonNum = 0;
                    ToggleButtons(false, "Station");
                    stationInfo.text = m_missionSystem.m_stationMissions[0].missionInfo;
                    stationInfo.gameObject.SetActive(true);
                    // close
                    stationButtons[4].SetActive(false);
                    CheckIfCompleted(buttonNum);
                    // back
                    stationButtons[6].SetActive(true);
                    break;
                }
            case "StationMission2":
                {
                    buttonNum = 1;
                    ToggleButtons(false, "Station");
                    stationInfo.text = m_missionSystem.m_stationMissions[1].missionInfo;
                    stationInfo.gameObject.SetActive(true);
                    // accept button
                    stationButtons[4].SetActive(false);
                    CheckIfCompleted(buttonNum);
                    stationButtons[6].SetActive(true);
                    break;
                }
            case "StationMission3":
                {
                    buttonNum = 2;
                    ToggleButtons(false, "Station");
                    stationInfo.text = m_missionSystem.m_stationMissions[2].missionInfo;
                    stationInfo.gameObject.SetActive(true);
                    // accept button
                    stationButtons[4].SetActive(false);
                    CheckIfCompleted(buttonNum);
                    stationButtons[6].SetActive(true);
                    break;
                }
            case "StationMission4":
                {
                    buttonNum = 3;
                    ToggleButtons(false, "Station");
                    stationInfo.text = m_missionSystem.m_stationMissions[3].missionInfo;
                    stationInfo.gameObject.SetActive(true);
                    // accept button
                    stationButtons[4].SetActive(false);
                    CheckIfCompleted(buttonNum);
                    stationButtons[6].SetActive(true);
                    break;
                }
            case "StationCloseLog":
                {
                    buttonNum = 0;
                    stationMissionPanel.SetActive(false);
                    ToggleButtons(true, "Station");
                    break;
                }
            case "StationBack":
                {
                    stationInfo.gameObject.SetActive(false);
                    ToggleButtons(true, "Station");
                    stationButtons[4].SetActive(true);
                    // accept button
                    stationButtons[5].SetActive(false);
                    stationButtons[6].SetActive(false);
                    objectives.gameObject.SetActive(false);
                    break;
                }
            case "StationAcceptMission":
                {
                    m_missionSystem.AddActiveMission(buttonNum, m_missionSystem.m_stationMissions[buttonNum]);
                    stationInfo.gameObject.SetActive(false);
                    ToggleButtons(true, "Station");
                    stationButtons[4].SetActive(true);
                    // accept button
                    stationButtons[5].SetActive(false);
                    stationButtons[6].SetActive(false);
                    if (SceneManager.GetActiveScene().name == "Tutorial")
                    {
                        GameObject.Find("TutorialPref").GetComponent<Tutorial>().SendMessage("MissionAccepted");
                    }
                    break;
                }
            case "StationTurnIn":
                {
                    m_missionSystem.TurnInMission(buttonNum);
                    ToggleButtons(true, "Station");
                    stationButtons[buttonNum].SetActive(false);
                    stationButtons[4].SetActive(true);
                    stationButtons[6].SetActive(false);
                    stationButtons[7].SetActive(false);
                    stationInfo.gameObject.SetActive(false);
                    turnedIn[buttonNum] = true;
                    if (SceneManager.GetActiveScene().name == "Tutorial")
                    {
                        GameObject.Find("TutorialPref").GetComponent<Tutorial>().SendMessage("MissionTurnedIn");
                    }
                    break;
                }
            default:
                break;
        }
    }

    #endregion

    void CheckIfCompleted(int missionNum)
    {
        if (m_missionSystem.m_ActiveMissions.ContainsKey(missionNum))
        {
            if (m_missionSystem.m_ActiveMissions[missionNum].completed)
            {
                stationButtons[7].SetActive(true);
            }
        }
        else
        {
            stationButtons[5].SetActive(true);
        }


    }
}
