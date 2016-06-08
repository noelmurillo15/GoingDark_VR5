using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using GD.Core.Enums;

public class MissionLog : MonoBehaviour
{
    // need all these public for stupid gameobjects not found when inactive >=(
    public GameObject[] buttons;
    public GameObject m_pMissionButtonPanel;
    public GameObject m_pMissionInfo;
    public GameObject m_pMissionMessage;

    private MissionSystem m_missionSystem;
    private Text m_tMissionInfo;
    private Text m_tObjectives;


    private int buttonNum;

    void OnAwake()
    {
        AssignButtonName();
    }
    // Use this for initialization
    void Start()
    {
        m_missionSystem = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();

        Text[] text = m_pMissionInfo.GetComponentsInChildren<Text>();
        m_tMissionInfo = text[0];
        m_tObjectives = text[1];

        ActiveMissions();
    }

    void UpdateInfo(Mission mission)
    {
        for (int i = 0; i < m_missionSystem.m_ActiveMissions.Count; i++)
        {
            if (m_missionSystem.m_ActiveMissions[i].missionName == mission.missionName)
            {
                m_tObjectives.text = "Objectives Left : " + mission.objectives;
                if (mission.completed)
                    m_tObjectives.text = "Return to the Station to turn in your mission";
            }
        }
    }

    void ActiveButtons()
    {
        for (int i = 0; i < m_missionSystem.m_ActiveMissions.Count; i++)
        {
            buttons[i].SetActive(true);
        }
    }

    void TogglePanel()
    {
        Debug.Log("Opened Mission Panel");
        AssignButtonName();
        ActiveMissions();
        m_pMissionButtonPanel.SetActive(!m_pMissionButtonPanel.activeSelf);
    }

    void ActiveMissions()
    {
        for (int i = 0; i < m_missionSystem.m_ActiveMissions.Count; i++)
            buttons[i].gameObject.SetActive(true);
    }

    void AssignButtonName()
    {
        for (int i = 0; i < m_missionSystem.m_ActiveMissions.Count; i++)
        {
            buttons[i].gameObject.name = m_missionSystem.m_ActiveMissions[i].missionName;
            Debug.Log(buttons[i].gameObject.name);
            buttons[i].GetComponentInChildren<Text>().text = buttons[i].gameObject.name;
        }
    }

    void Failed(Mission mission)
    {
        ActiveMissions();
        m_pMissionMessage.SetActive(true);
        m_pMissionMessage.SendMessage("Failed", mission.missionName);
    }

    void Completed(Mission mission)
    {
        m_pMissionMessage.SetActive(true);
        m_pMissionMessage.SendMessage("Completed", mission.missionName);
    }

    void AssignInfo(Mission mission)
    {
        m_tMissionInfo.text = mission.missionInfo;

        if (mission.completed)
            m_tObjectives.text = "Return to station to turn in missions";
        else
            m_tObjectives.text = "Objectives left : " + mission.objectives;
    }


    #region Public Methods

    public void ButtonPressed(string buttonName)
    {
        if (m_missionSystem.m_ActiveMissions.Exists(s => s.missionName == buttonName))
        {
            Mission mission = m_missionSystem.m_ActiveMissions.Find(s => s.missionName == buttonName);
            Debug.Log("Missionlog Mission Info Open");

            AssignInfo(mission);
            m_pMissionButtonPanel.SetActive(false);
            m_pMissionInfo.SetActive(true);
        }
        else if (buttonName == "CloseLog")
        {
            m_pMissionButtonPanel.SetActive(false);
            m_tObjectives.gameObject.SetActive(false);
        }
        else if (buttonName == "Back")
        {
            m_pMissionInfo.SetActive(false);
            m_pMissionButtonPanel.SetActive(true);
        }
    }

    #endregion

}
