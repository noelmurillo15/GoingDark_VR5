using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using GoingDark.Core.Enums;

public class MissionSystem : MonoBehaviour
{
    public List<Mission> m_ActiveMissions;
    public List<Mission> m_stationMissions;
    public string filename;

    private PlayerStats m_playerStats;
    private MissionLoader m_missionLoader;
    private MissionLog m_missionLog;
    private Tutorial m_tutorial;
    private int maxMissions;

    // Use this for initialization
    void Start()
    {
        m_ActiveMissions = new List<Mission>();
        m_missionLoader = GameObject.Find("PersistentGameObject").GetComponent<MissionLoader>();
        m_missionLog = GameObject.Find("MissionLog").GetComponent<MissionLog>();
        m_playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();

        if (SceneManager.GetActiveScene().name == "Tutorial")
            m_tutorial = GameObject.Find("TutorialPref").GetComponent<Tutorial>();

        maxMissions = 4;
        m_stationMissions = m_missionLoader.LoadMissions(filename);

        // for testing
        AddActiveMission(m_stationMissions[0]);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddActiveMission(Mission mission)
    {
        mission.isActive = true;
        m_ActiveMissions.Add(mission);
    }

    /// <summary>
    /// Decrements the loot count for active scavenge missions
    /// </summary>
    void LootPickedUp()
    {
        for (int i = 0; i < m_ActiveMissions.Count; i++)
        {
            if ((m_ActiveMissions[i].type == MissionType.SCAVENGE || m_ActiveMissions[i].type == MissionType.STEALTH) && m_ActiveMissions[i].isActive)
            {
                // cannot directly modify properties of the list
                Mission mission = m_ActiveMissions[i];
                mission.objectives--;

                if (mission.objectives == 0)
                {
                    mission.completed = true;
                    m_missionLog.SendMessage("Completed", mission);

                    if (SceneManager.GetActiveScene().name == "Tutorial")
                        m_tutorial.SendMessage("MissionCompleted");
                }
                m_ActiveMissions[i] = mission;
                m_missionLog.SendMessage("UpdateInfo", m_ActiveMissions[i]);
            }
        }
    }

    void KilledEnemy(EnemyTypes type)
    {
        for (int i = 0; i < m_ActiveMissions.Count; i++)
        {
            Mission mission;
            mission = m_ActiveMissions[i];

            if (mission.enemy == EnemyTypes.ANY ||
                mission.enemy == type)
                mission.objectives--;

            m_ActiveMissions[i] = mission;
        }
    }

    void PlayerSeen()
    {
        for (int i = 0; i < m_ActiveMissions.Count; i++)
        {
            Mission mission;
            mission = m_ActiveMissions[i];
            if (mission.type == MissionType.STEALTH)
                MissionFailed(mission);
        }
    }

    public void TurnInMission(string missionName)
    {
        for (int i = 0; i < m_ActiveMissions.Count; i++)
        {
            if (m_ActiveMissions[i].missionName == missionName)
            {
                int credits = m_ActiveMissions[i].credits;
                //m_playerStats.AddCredits(credits);

                // remove turned in missions from active list and station list
                Mission temp = m_ActiveMissions[i];
                m_ActiveMissions.Remove(temp);
                m_stationMissions.Remove(temp);
                break;
            }
        }


    }

    void MissionFailed(Mission mission)
    {
        m_ActiveMissions.Remove(mission);
        m_stationMissions.Remove(mission);
        m_missionLog.SendMessage("Failed", mission);
        //m_missionMessages.SendMessage("Failed", mission.missionName);
    }

}
