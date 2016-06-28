using UnityEngine;
using GoingDark.Core.Enums;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MissionSystem : MonoBehaviour
{
    public List<Mission> m_ActiveMissions;
    public List<Mission>[] m_stationMissions;
    public List<Mission> m_CompletedMissions;
    public string[] filename;

    private PlayerStats m_playerStats;
    private MissionLoader m_missionLoader;
    private MissionLog m_missionLog;
    private Tutorial m_tutorial;
    private TutorialFlight m_tutorial2;
    private int maxMissions;
    private int m_stationID;

    private GameObject m_Boss;

    // Use this for initialization
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Tutorial2")
        {
            m_Boss = GameObject.Find("Boss");
            m_Boss.SetActive(false);
        }
        m_ActiveMissions = new List<Mission>();
        m_CompletedMissions = new List<Mission>();
        m_missionLoader = GameObject.Find("PersistentGameObject").GetComponent<MissionLoader>();
        m_missionLog = GameObject.Find("MissionLog").GetComponent<MissionLog>();
        m_playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        m_stationMissions = new List<Mission>[2];

        if (SceneManager.GetActiveScene().name == "Tutorial")
            m_tutorial = GameObject.Find("TutorialPref").GetComponent<Tutorial>();

        if (SceneManager.GetActiveScene().name == "Tutorial2")
        {
            m_tutorial2 = GameObject.Find("TutorialPrefF").GetComponent<TutorialFlight>();
            m_stationMissions[0] = m_missionLoader.LoadMissions(filename[0]);
        }
        else
        {
            m_stationMissions[0] = m_missionLoader.LoadMissions("Level1_1");
            m_stationMissions[1] = m_missionLoader.LoadMissions("Level1_2");

        }

        maxMissions = 4;

    }

    public void AddActiveMission(Mission mission)
    {
        mission.isActive = true;
        if (mission.enemy == EnemyTypes.Boss)
        {
        if (SceneManager.GetActiveScene().name != "Tutorial2")
                m_Boss.SetActive(true);
            Debug.Log("Boss Spawned");
        }
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

                if (mission.objectives == 0 && !mission.completed)
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
                    // m_CompletedMissions.Add(mission);
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

    public void TurnInMission(string missionName, int stationID)
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
                Mission[] container = new Mission[4];
                m_ActiveMissions.CopyTo(container);
                int index = m_stationMissions[stationID].FindIndex(x => x.missionName == temp.missionName);
                m_stationMissions[stationID].RemoveAt(index);
                m_ActiveMissions.RemoveAt(i);

                m_CompletedMissions.Add(container[i]);
                break;
            }
        }

        if (m_CompletedMissions.Count == 3)
        {
            m_stationMissions[stationID].Add(m_missionLoader.LoadMission("Level1_Boss"));
        }


    }

    void MissionFailed(Mission mission)
    {
        m_ActiveMissions.Remove(mission);
        m_missionLog.Failed(mission);
        if (SceneManager.GetActiveScene().name == "Tutorial2")
            m_tutorial2.SendMessage("MissionFailed", mission.missionName);
    }

}
