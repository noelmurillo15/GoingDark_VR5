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
    public List<Mission> m_PrimaryMissions;

    private PlayerStats m_playerStats;
    private MissionLoader m_missionLoader;
    private MissionLog m_missionLog;
    private MissionTracker m_missionTracker;
    private SystemManager m_systemManager;
    private MissileSystem m_missileSystem;
    private MessageScript messages;

    private int m_stationID;
    private int portalIndex = 0;
    private int numEnemies = 0;
    private string SceneName;

    GameObject[] portals;

    SaveGame save;

    private TallyScreen tallyscreen;
    private Mission MainMission;
    #region Getters
    public List<Mission> GetActiveMissions()
    {
        return m_ActiveMissions;
    }

    public Mission GetMainMission()
    {
        return MainMission;
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        SceneName = SceneManager.GetActiveScene().name;
        m_missionLoader = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionLoader>();
        m_missionTracker = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionTracker>();
        m_systemManager = GameObject.Find("Devices").GetComponent<SystemManager>();
        messages = GameObject.Find("PlayerCanvas").GetComponent<MessageScript>();

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

        m_ActiveMissions = new List<Mission>();
        m_CompletedMissions = new List<Mission>();
        m_PrimaryMissions = new List<Mission>();

        tallyscreen = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TallyScreen>();
        m_missionLog = GameObject.Find("Missions").GetComponent<MissionLog>();

        m_PrimaryMissions = m_missionLoader.LoadMissions(SceneName);

        if (SceneName != "Level1")
            Timing.RunCoroutine(AddMissions());
    }

    #region Private Methods

    void MissionFailed(Mission mission)
    {
        if (m_ActiveMissions.Count != 0)
            m_missionTracker.UpdateInfo(m_ActiveMissions[0], true);
        else
            m_missionTracker.missionTracker.SetActive(false);
        m_missionLog.UpdateButtons(mission.missionName);

        m_ActiveMissions.Remove(mission);
        m_missionLog.Failed(mission);
        Timing.RunCoroutine(Wait(3.0f));
    }

    void AddAllMissions()
    {
        for (int i = 0; i < m_PrimaryMissions.Count; i++)
        {
            Mission mission = m_PrimaryMissions[i];
            if (mission.type == MissionType.Elimination)
                mission.objectives = numEnemies;
            AddActiveMission(mission);
        }
        m_PrimaryMissions.Clear();
    }
    #endregion

    #region Coroutines
    IEnumerator<float> AddMissions()
    {
        yield return Timing.WaitForSeconds(3.0f);
        AddAllMissions();
        m_missionLog.SetNames();
    }

    IEnumerator<float> Wait(float time)
    {
        yield return Timing.WaitForSeconds(time);
        m_playerStats.GoToStation();
    }

    #endregion

    #region Public Methods
    public void ControlPointTaken()
    {
        for (int i = 0; i < m_ActiveMissions.Count; i++)
        {
            if (m_ActiveMissions[i].type == MissionType.ControlPoint)
            {
                Mission mission = m_ActiveMissions[i];
                mission.objectives--;
                m_ActiveMissions[i] = mission;

                if (m_ActiveMissions[i].objectives == 0)
                    TurnInMission(m_ActiveMissions[i]);

                m_missionTracker.UpdateInfo(mission);

            }
        }
    }

    public void CheckTimedMissions(float time)
    {
        for (int i = 0; i < m_ActiveMissions.Count; i++)
        {
            if (m_ActiveMissions[i].missionTimer > 0f)
            {
                if (m_ActiveMissions[i].missionTimer <= time) // time ran out, fail
                {
                    MissionFailed(m_ActiveMissions[i]);
                }
            }
        }

    }

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

        if (!mission.isOptional)
            MainMission = mission;

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
                AddActiveMission(temp);
                for (int i = 0; i < m_PrimaryMissions.Count; i++)
                {
                    if (m_PrimaryMissions[i].type == temp.type)
                        m_PrimaryMissions.RemoveAt(i);
                }
                m_missionTracker.ShowTracker();
                m_missionTracker.UpdateInfo(m_ActiveMissions[0], true);
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
        // gives the player the blueprint associated with the mission
        CheckForBlueprint(mission);


        // portals for tutorial
        if (SceneName == "Level1")
        {
            portals[portalIndex].SetActive(false);
            if ((portalIndex + 1) <= 2)
                portals[portalIndex + 1].SetActive(true);
            portalIndex++;
        }

        Mission tempMission = m_ActiveMissions.Find(x => x.missionName == mission.missionName);
        // update buttons in mission log
        m_missionLog.UpdateButtons(tempMission.missionName);

        m_CompletedMissions.Add(tempMission);
        m_ActiveMissions.Remove(tempMission);

        // update tracker to next mission
        if (m_ActiveMissions.Count != 0)
            m_missionTracker.UpdateInfo(m_ActiveMissions[0], true);

        // only go back to station in Level 1
        if (SceneName == "Level1")
        {
            m_missionTracker.missionTracker.SetActive(false);
            Timing.RunCoroutine(Wait(3.0f));
        }

        // done with all missions
        if ((m_ActiveMissions.Count == 0 && m_PrimaryMissions.Count == 0) || (!mission.isOptional && SceneName != "Level1")) // no missions left or mission is primary
        {
            m_missionTracker.missionTracker.SetActive(false);
            m_missionLog.TurnOffPanel();

            Invoke("BeginTally", 2f);
        }
    }

    void BeginTally()
    {        
        m_playerStats.GoToStation();
        tallyscreen.ActivateTallyScreen();
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

    #region Utils

    void CheckForBlueprint(Mission mission)
    {
        if (mission.blueprint != "")
        {
            if (mission.blueprint.Contains("System"))
                GiveSystem(mission.blueprint);
        }
    }

    void GiveSystem(string name)
    {
        if (name.Contains("Emp"))
        {
            m_systemManager.InitializeDevice(SystemType.Emp);
            messages.Init("Emp");
        }
        else if (name.Contains("Hyperdrive"))
        {
            m_systemManager.InitializeDevice(SystemType.Hyperdrive);
            messages.Init("Hyperdrive");
        }

        Timing.RunCoroutine(WaitMsg());
    }

    IEnumerator<float> WaitMsg()
    {
        yield return Timing.WaitForSeconds(3.0f);
        messages.InitDown();
    }
    #endregion
}