using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class MissionSystem : MonoBehaviour
{
    public enum MissionType { COMBAT, SCAVENGE, STEALTH };
    public enum EnemyType { BASIC_ENEMY, KAMIKAZE, TRANSPORT, ANY, NONE };

    public string filename;

    public struct Mission
    {
        public string missionName;
        public string missionInfo;

        public bool completed;
        public bool isOptional;
        public bool spotted;
        public bool isActive;

        public float missionTimer;

        public int credits;
        public int objectives;

        public MissionType type;
        public EnemyType enemy;
    }

    //public Mission[,] m_LevelMissions;

    //public List<Mission> m_ActiveMissions;

    private PlayerStats m_playerStats;
    public Dictionary<int, Mission> m_ActiveMissions;

    private MissionLoader m_missionLoader;
    private MissionLog m_missionLog;
    public Mission[] m_stationMissions;

    private int maxMissions;
    // Use this for initialization
    void Start()
    {
        // m_ActiveMissions = new List<Mission>();
        m_ActiveMissions = new Dictionary<int, Mission>();
        m_missionLoader = GameObject.Find("PersistentGameObject").GetComponent<MissionLoader>();
        m_missionLog = GameObject.Find("MissionLog").GetComponent<MissionLog>();
        m_playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();

        maxMissions = 4;
        m_stationMissions = m_missionLoader.LoadMissions(filename);

        // m_ActiveMissions.Add(m_stationMissions[0]);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddActiveMission(int key, Mission mission)
    {
        mission.isActive = true;
        m_ActiveMissions.Add(key, mission);
        //m_ActiveMissions.Add(mission);
    }

    /// <summary>
    /// Decrements the loot count for active scavenge missions
    /// </summary>
    void LootPickedUp()
    {
        for (int i = 0; i < m_ActiveMissions.Count; i++)
        {
            if (m_ActiveMissions[i].type == MissionType.SCAVENGE && m_ActiveMissions[i].isActive)
            {
                // cannot directly modify properties of the list
                Mission mission = m_ActiveMissions[i];
                mission.objectives--;

                if (mission.objectives == 0)
                {
                    mission.completed = true;
                    if (SceneManager.GetActiveScene().name == "Tutorial")
                    {
                        GameObject.Find("TutorialPref").GetComponent<Tutorial>().SendMessage("MissionCompleted");
                    }
                }
                m_ActiveMissions[i] = mission;
                m_missionLog.SendMessage("UpdateInfo", m_ActiveMissions[i]);
            }
        }
    }

    public void TurnInMission(int key)
    {
        int credits = m_ActiveMissions[key].credits;
        Debug.Log("Gave player mission credits");
        //m_playerStats.AddCredits(credits);
    }
}
