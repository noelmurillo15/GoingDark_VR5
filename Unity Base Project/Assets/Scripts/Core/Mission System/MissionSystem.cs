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
    public List<Mission> m_CompletedMissions;
    public string filename;

    private PlayerStats m_playerStats;
    private MissionLoader m_missionLoader;
    private MissionLog m_missionLog;
    private Tutorial m_tutorial;
    private TutorialFlight m_tutorial2;
    private int maxMissions;

    // Use this for initialization
    void Start()
    {
        m_ActiveMissions = new List<Mission>();
        m_CompletedMissions = new List<Mission>();
        m_missionLoader = GameObject.Find("PersistentGameObject").GetComponent<MissionLoader>();
        m_missionLog = GameObject.Find("MissionLog").GetComponent<MissionLog>();
        m_playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        if (SceneManager.GetActiveScene().name == "Tutorial")
            m_tutorial = GameObject.Find("TutorialPref").GetComponent<Tutorial>();
        if (SceneManager.GetActiveScene().name == "Tutorial2")
            m_tutorial2 = GameObject.Find("TutorialPrefF").GetComponent<TutorialFlight>();
        maxMissions = 4;
        m_stationMissions = m_missionLoader.LoadMissions(filename);

        // for testing
        //AddActiveMission(m_stationMissions[0]);
    }

    public void AddActiveMission(Mission mission)
    {
        mission.isActive = true;
        Debug.Log("AddActiveMission " + mission.missionName);
        m_ActiveMissions.Add(mission);
    }

    /// <summary>
    /// Decrements the loot count for active scavenge missions
    /// </summary>
    void LootPickedUp()
    {
        for (int i = 0; i < m_ActiveMissions.Count; i++)
        {
            if ((m_ActiveMissions[i].type == MissionType.Scavenge || m_ActiveMissions[i].type == MissionType.Stealth) && m_ActiveMissions[i].isActive)
            {
                // cannot directly modify properties of the list
                Mission mission = m_ActiveMissions[i];
                mission.objectives--;

                if (mission.objectives == 0)
                {
                    mission.completed = true;
                    m_missionLog.SendMessage("Completed", mission);

                    if (SceneManager.GetActiveScene().name == "Tutorial")
                        m_tutorial.SendMessage("MissionCompleted", mission.missionName);
                    if (SceneManager.GetActiveScene().name == "Tutorial2")
                        m_tutorial2.SendMessage("MissionCompleted", mission.missionName);
                }
                m_ActiveMissions[i] = mission;
                m_missionLog.SendMessage("UpdateInfo", m_ActiveMissions[i]);
            }
        }
    }

    public void KilledEnemy(EnemyTypes type)
    {
        for (int i = 0; i < m_ActiveMissions.Count; i++)
        {
            Mission mission;
            mission = m_ActiveMissions[i];

            if (mission.enemy == EnemyTypes.Any ||
                mission.enemy == type)
            {
                mission.objectives--;
                if (mission.objectives <= 0)
                {
                    mission.completed = true;
                    m_missionLog.Completed(mission);
                    m_CompletedMissions.Add(mission);
                }
            }

            m_ActiveMissions[i] = mission;
        }
    }

    public void PlayerSeen()
    {
        for (int i = 0; i < m_ActiveMissions.Count; i++)
        {
            Mission mission;
            mission = m_ActiveMissions[i];
            if (mission.type == MissionType.Stealth)
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
                Debug.Log("Credits before mission : " + m_playerStats.SaveData.Credits);
                m_playerStats.SaveData.Credits += credits;
                Debug.Log("Added " + credits + "credits. Player has : " + m_playerStats.SaveData.Credits);
                // remove turned in missions from active list and station list

                Mission temp = m_ActiveMissions[i];
                int station = m_stationMissions.FindIndex(x => x.missionName == temp.missionName);
                m_stationMissions.RemoveAt(station);
                m_ActiveMissions.RemoveAt(i);

                //m_CompletedMissions.Add(temp);
                break;
            }
        }


    }

    void MissionFailed(Mission mission)
    {
        m_ActiveMissions.Remove(mission);
        m_missionLog.Failed(mission);
    }

}
