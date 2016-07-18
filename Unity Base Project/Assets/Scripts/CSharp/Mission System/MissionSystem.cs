using UnityEngine;
using GoingDark.Core.Enums;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MissionSystem : MonoBehaviour
{
    public List<Mission> m_ActiveMissions;
    public List<Mission> m_CompletedMissions;
    public List<Mission> m_SecondaryMissions;
    public List<Mission> m_PrimaryMissions;
    public string[] filename;

    private PlayerStats m_playerStats;
    private MissionLoader m_missionLoader;
    private MissionLog m_missionLog;
    private TutorialFlight m_tutorial2;
    private int maxMissions;
    private int m_stationID;

    // private GameObject m_Boss;
    private string SceneName;

    // Use this for initialization
    void Start()
    {
        SceneName = SceneManager.GetActiveScene().name;

        m_ActiveMissions = new List<Mission>();
        m_CompletedMissions = new List<Mission>();
        m_SecondaryMissions = new List<Mission>();
        m_PrimaryMissions = new List<Mission>();

        m_missionLoader = GameObject.Find("PersistentGameObject").GetComponent<MissionLoader>();
        m_missionLog = GameObject.Find("MissionLog").GetComponent<MissionLog>();
        m_playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        if (SceneName == "Level1")
        {
            m_tutorial2 = GameObject.Find("TutorialPref").GetComponent<TutorialFlight>();
            m_PrimaryMissions = m_missionLoader.LoadMissions(filename[0]);
        }
        else
        {
            m_PrimaryMissions = m_missionLoader.LoadMissions(filename[0]);
            m_SecondaryMissions = m_missionLoader.LoadMissions(filename[1]);
        }
        maxMissions = 4;
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
            if ((m_ActiveMissions[i].type == MissionType.Scavenge || m_ActiveMissions[i].type == MissionType.Stealth) && m_ActiveMissions[i].isActive)
            {
                // cannot directly modify properties of the list
                Mission mission = m_ActiveMissions[i];
                mission.objectives--;
                m_ActiveMissions[i] = mission;
                if (mission.objectives == 0)
                {
                    mission.completed = true;
                    m_missionLog.Completed(mission);
                    m_ActiveMissions[i] = mission;

                    if (SceneManager.GetActiveScene().name == "Level1")
                        m_tutorial2.SendMessage("MissionCompleted", mission.type);

                    // automatic turn in
                    if (mission.isOptional || (!mission.isOptional && m_PrimaryMissions.Count > 0))
                        TurnInMission(mission);
                    else
                        m_missionLog.TurnInLastMission();
                }
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
                m_ActiveMissions[i] = mission;

                if (mission.objectives <= 0)
                {
                    mission.completed = true;
                    m_ActiveMissions[i] = mission;

                    m_missionLog.Completed(mission);

                    if (mission.isOptional || (!mission.isOptional && m_PrimaryMissions.Count > 0))
                        TurnInMission(mission);
                    else
                        m_missionLog.TurnInLastMission();

                }
            }

        }
    }

    public void PlayerSeen()
    {
        for (int i = 0; i < m_ActiveMissions.Count; i++)
        {
            Mission mission;
            mission = m_ActiveMissions[i];
            if (mission.type == MissionType.Stealth)
            {
                if (mission.isOptional)
                    MissionFailed(mission);
                else
                {
                    m_PrimaryMissions.Add(mission);
                    MissionFailed(mission);
                }
            }
        }
    }

    // automatic turn in for missions, specific for primary/secondary
    public void TurnInMission(Mission mission)
    {
        Mission tempMission = m_ActiveMissions.Find(x => x.missionName == mission.missionName);
        if (!mission.isOptional) // primary mission
        {
            m_CompletedMissions.Add(tempMission);
            if (SceneManager.GetActiveScene().name == "Level1")
                m_tutorial2.SendMessage("MissionTurnedIn", tempMission.type);
            m_ActiveMissions.Remove(tempMission);
            Debug.Log("Turned in primary");
            StartNextMission();
        }
        else  // secondary mission
        {
            m_CompletedMissions.Add(tempMission);
            Debug.Log("Turned in secondary");
            m_ActiveMissions.Remove(tempMission);

        }

        m_playerStats.SaveData.Credits += tempMission.credits;
        Debug.Log("Credits : " + m_playerStats.SaveData.Credits);
    }

    void StartNextMission()
    {
        if (m_PrimaryMissions.Count > 0)
        {
            AddActiveMission(m_PrimaryMissions[0]);
            Debug.Log("Added primary : " + m_PrimaryMissions[0].missionName);
            m_missionLog.NewMission(m_PrimaryMissions[0]);
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                m_tutorial2.SendMessage("MissionAccepted", m_PrimaryMissions[0].type);
            }
            m_PrimaryMissions.RemoveAt(0);
         }
    }

    void MissionFailed(Mission mission)
    {
        m_ActiveMissions.Remove(mission);
        m_missionLog.Failed(mission);
    }
}