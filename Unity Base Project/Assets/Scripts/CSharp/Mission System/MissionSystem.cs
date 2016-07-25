using UnityEngine;
using GoingDark.Core.Enums;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using MovementEffects;

public class MissionSystem : MonoBehaviour
{
    public List<Mission> m_ActiveMissions;
    public List<Mission> m_CompletedMissions;
    public List<Mission> m_SecondaryMissions;
    public List<Mission> m_PrimaryMissions;
    public Mission m_MainMission;
    public string[] filename;

    private PlayerStats m_playerStats;
    private MissionLoader m_missionLoader;
    private MissionLog m_missionLog;
    private MissionTracker m_missionTracker;
    private int m_stationID;
    private int portalIndex = 0;
    private int numEnemies = 0;
    private string SceneName;
    private bool addMissions = false;

    GameObject spawner;
    GameObject[] portals;

    private TallyScreen tallyscreen;

    // Use this for initialization
    void Start()
    {
        SceneName = SceneManager.GetActiveScene().name;
        m_missionLoader = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionLoader>();
        m_missionTracker = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionTracker>();

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");
        numEnemies = temp.Length;

        m_playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        if (SceneName == "Level1")
        {
            portals = new GameObject[3];
            portals[0] = GameObject.Find("FlightPortal");
            portals[1] = GameObject.Find("StealthPortal");
            portals[2] = GameObject.Find("CombatPortal");

            portals[1].SetActive(false);
            portals[2].SetActive(false);
        }
        else
        {
            // add all missions at start of level -> choose which one tracks
            addMissions = true;
        }

        m_ActiveMissions = new List<Mission>();
        m_CompletedMissions = new List<Mission>();
        m_SecondaryMissions = new List<Mission>();
        m_PrimaryMissions = new List<Mission>();

        tallyscreen = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TallyScreen>();
        m_missionLog = GameObject.Find("Missions").GetComponent<MissionLog>();

        m_MainMission = m_missionLoader.LoadMission(filename[0]);
        m_PrimaryMissions = m_missionLoader.LoadMissions(filename[1]);

        if (SceneName != "Level1")
            Timing.RunCoroutine(AddMissions());

    }

    void MissionFailed(Mission mission)
    {
        m_missionTracker.missionTracker.SetActive(false);
        m_ActiveMissions.Remove(mission);
        m_missionLog.Failed(mission);
        Debug.Log("Failed mission, return to portals");
        Timing.RunCoroutine(Wait(3.0f));
    }

    IEnumerator<float> AddMissions()
    {
        Debug.Log("Coroutine");
        yield return Timing.WaitForSeconds(3.0f);
        AddAllMissions();
        m_missionLog.SetNames();
    }

    #region Private Methods

    IEnumerator<float> DelayMission(float duration)
    {
        m_missionTracker.missionTracker.SetActive(false);
        yield return Timing.WaitForSeconds(duration);
        AddActiveMission(m_MainMission);
        m_missionTracker.missionTracker.SetActive(true);
        m_missionTracker.UpdateInfo(m_ActiveMissions[0]);

        yield return Timing.WaitForSeconds(1.0f);
        if (SceneName == "Level1")
            spawner.SetActive(true);
    }

    void AddAllMissions()
    {
        for (int i = 0; i < m_PrimaryMissions.Count; i++)
        {
            Mission mission = m_PrimaryMissions[i];
            if (mission.type == MissionType.Elimination)
                mission.objectives = numEnemies;
            AddActiveMission(mission);
            Debug.Log("Added mission" + mission.missionName);
        }
        m_PrimaryMissions.Clear();
    }
    #endregion

    #region Public Methods

    public void LootPickedUp()
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

                    // automatic turn in
                    TurnInMission(mission);
                }
                m_missionTracker.UpdateInfo(mission);
            }
        }
    }
    public void AddActiveMission(Mission mission)
    {
        m_missionTracker.missionTracker.SetActive(true);
        mission.isActive = true;
        m_ActiveMissions.Add(mission);
        m_missionTracker.UpdateInfo(m_ActiveMissions[0]);
        m_missionLog.NewMission(m_ActiveMissions[0]);
    }

    public void EnteredPortal(string name)
    {
        if (m_ActiveMissions.Count == 0)
        {
            MissionType type = Convert(name);
            if (m_PrimaryMissions.Count != 0)
            {
                Mission temp = m_PrimaryMissions.Find(x => x.type == type);
                string mName = temp.missionName;
                AddActiveMission(temp);
                for (int i = 0; i < m_PrimaryMissions.Count; i++)
                {
                    if (m_PrimaryMissions[i].type == temp.type)
                        m_PrimaryMissions.RemoveAt(i);
                }
                m_missionTracker.ShowTracker();
            }
        }
    }

    public void KilledEnemy(EnemyTypes type)
    {
        for (int i = 0; i < m_ActiveMissions.Count; i++)
        {
            Mission mission;
            mission = m_ActiveMissions[i];

            // eliminate all threats
            if (mission.type == MissionType.Elimination)
            {
                Debug.Log("Killed enemy");

                mission.objectives--;
                m_ActiveMissions[i] = mission;

                if (mission.objectives == 0)
                {
                    mission.completed = true;
                    m_missionLog.Completed(mission);
                    TurnInMission(mission);
                }
                m_missionTracker.UpdateInfo(mission);
            }
            else if (mission.enemy == EnemyTypes.Any ||
                mission.enemy == type && mission.type != MissionType.Elimination)
            {
                mission.objectives--;
                m_ActiveMissions[i] = mission;

                if (mission.objectives == 0)
                {
                    mission.completed = true;
                    m_ActiveMissions[i] = mission;

                    m_missionLog.Completed(mission);

                    TurnInMission(mission);
                }
                m_missionTracker.UpdateInfo(mission);

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
                m_PrimaryMissions.Add(mission);
                MissionFailed(mission);
            }
        }
    }

    // automatic turn in for missions, specific for primary/secondary
    public void TurnInMission(Mission mission)
    {
        if (SceneName == "Level1")
        {
            portals[portalIndex].SetActive(false);
            if ((portalIndex + 1) <= 2)
                portals[portalIndex + 1].SetActive(true);
            portalIndex++; 
        }

        m_missionTracker.missionTracker.SetActive(false);
        Mission tempMission = m_ActiveMissions.Find(x => x.missionName == mission.missionName);

        if (tempMission.missionName == m_MainMission.missionName)
            m_MainMission.completed = true;

        string temp = TypeToString(tempMission.type);
        m_CompletedMissions.Add(tempMission);
        m_ActiveMissions.Remove(tempMission);

        m_playerStats.SaveData.Credits += tempMission.credits;
        Timing.RunCoroutine(Wait(3.0f));

        // done with all missions
        if (m_ActiveMissions.Count == 0 && m_PrimaryMissions.Count == 0)
        {
            tallyscreen.ActivateTallyScreen();
        }
    }

    IEnumerator<float> Wait(float time)
    {
        yield return Timing.WaitForSeconds(time);
        m_playerStats.GoToStation();
    }

    #endregion

    #region Tutorial

    MissionType Convert(string name)
    {
        MissionType type = MissionType.Scavenge;
        if (name == "CombatPortal")
            type = MissionType.Combat;
        else if (name == "StealthPortal")
            type = MissionType.Stealth;
        else if (name == "FlightPortal")
            type = MissionType.Scavenge;

        return type;
    }

    string TypeToString(MissionType type)
    {
        string temp = "";

        if (type == MissionType.Combat)
            temp = "CombatPortal";
        else if (type == MissionType.Scavenge)
            temp = "FlightPortal";
        else if (type == MissionType.Stealth)
            temp = "StealthPortal";

        return temp;
    }
    #endregion

}