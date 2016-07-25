using UnityEngine;
using UnityEngine.UI;
using GoingDark.Core.Enums;

public class MissionLog : MonoBehaviour
{
    // message shit, fuck it
    public GameObject m_pMissionMessage;
    private MissionSystem m_missionSystem;

    [SerializeField]
    private GameObject missionLog;
    private Button[] buttons;
    // Use this for initialization
    void Start()
    {
        buttons = missionLog.GetComponentsInChildren<Button>();
        m_missionSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionSystem>();
    }

    #region Public Methods
    public void SetNames()
    {
        int num = 0;
        for (int i = 0; i < m_missionSystem.m_ActiveMissions.Count; i++)
        {
            string temp = m_missionSystem.m_ActiveMissions[i].missionName;
            buttons[i].transform.name = temp;
            buttons[i].GetComponentInChildren<Text>().text = temp;
            if (!buttons[i].gameObject.activeSelf)
                buttons[i].gameObject.SetActive(true);
            num = i;
        }
    }

    public void UpdateButtons(string name)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].name == name)
                buttons[i].gameObject.SetActive(false);
        }
    }

    public void UpdateButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i >= m_missionSystem.m_ActiveMissions.Count && buttons[i].name != "Close")
                buttons[i].gameObject.SetActive(false);
        }
    }


    public void TurnOffPanel()
    {
        missionLog.SetActive(false);
    }

    public void TurnOnPanel()
    {
        missionLog.SetActive(true);
    }
    #endregion

    #region Messages

    public void Failed(Mission mission)
    {
        m_pMissionMessage.SetActive(true);
        m_pMissionMessage.SendMessage("Failed", mission.missionName);
    }

    public void Completed(Mission mission)
    {
        m_pMissionMessage.SetActive(true);
        m_pMissionMessage.SendMessage("Completed", mission.missionName);
    }

    public void NewMission(Mission mission)
    {
        m_pMissionMessage.SetActive(true);
        m_pMissionMessage.GetComponent<MissionMessages>().NewMission(mission.missionName, mission.missionInfo);
    }

    public void TurnInLastMission()
    {
        m_pMissionMessage.SetActive(true);
        m_pMissionMessage.SendMessage("TurnInLastMission");
    }

    #endregion


}
