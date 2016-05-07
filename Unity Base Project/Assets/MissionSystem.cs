using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class MissionSystem : MonoBehaviour {


    private enum MissionRewards { EASY = 50, NORMAL = 100, HARD = 150};
    public enum MissionType { COMBAT, SCAVENGE, STEALTH};
    public enum EnemyType { ENEMY, KAMIKAZE, TRANSPORT, ANY, NONE};

    public struct Mission
    {
        public string missionName;
        public string missionInfo;
        
        public bool completed;
        public bool isOptional;
        public bool spotted;

        public float missionTimer;
       
        public int credits;
        public int objectives;
       
        public MissionType type;
        public EnemyType enemy;
    }

    /// <summary>
    /// Dictionary for storing missions for all levels.
    /// Key : Level name, value : LevelInformation
    /// </summary>
    public Dictionary<string, Mission[]> MissionStorage;

    private Mission[] m_combat;
    private Mission[] m_stealth;
    private Mission[] m_scavenge;

    private Mission[] level1;

    
	// Use this for initialization
	void Start () {
        Debug.Log("Mission Start");
        MissionStorage = new Dictionary<string, Mission[]>();
        m_combat = new Mission[3];
        m_stealth = new Mission[1];
        m_scavenge = new Mission[3];
        level1 = new Mission[4];
        // Load in all the missions with IO
        LoadMissions();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void CombatMissions()
    {
        // set all missions to incomplete
        for (int i = 0; i < m_combat.Length; i++)
        {
            m_combat[i].completed = false;
            // all combat missions are optional
            m_combat[i].isOptional = true;
            m_combat[i].type = MissionType.COMBAT;
        }

        // this is ugly, but will work for now
        m_combat[0].missionName = "Kill them Dead";
        m_combat[0].missionInfo = "Kill 3 enemies";
        m_combat[0].credits = (int)MissionRewards.HARD;
        m_combat[0].enemy = EnemyType.ANY;
        m_combat[0].missionTimer = 0.0f;
        m_combat[0].objectives = 3;

        m_combat[1].missionName = "It's Mine Now";
        m_combat[1].missionInfo = "Destroy a transport ship and steal its cargo";
        m_combat[1].credits = (int)MissionRewards.EASY;
        m_combat[1].enemy = EnemyType.TRANSPORT;
        m_combat[1].missionTimer = 0.0f;
        m_combat[1].objectives = 1;

        m_combat[2].missionName = "BOOM";
        m_combat[2].missionInfo = "Locate and destroy one of the Kamikaze ships";
        m_combat[2].credits = (int)MissionRewards.HARD;
        m_combat[2].enemy = EnemyType.KAMIKAZE;
        m_combat[2].missionTimer = 0.0f;
        m_combat[2].objectives = 1;

    }

    void StealthMissions()
    {
        // set all missions to incomplete
        for (int i = 0; i < m_stealth.Length; i++)
        {
            m_stealth[i].completed = false;
            m_stealth[i].type = MissionType.STEALTH;
            m_stealth[i].enemy = EnemyType.NONE;
            m_stealth[i].isOptional = true;
        }

        m_stealth[0].missionName = "Silent Thief";
        m_stealth[0].missionInfo = "Locate 3 objectives without getting seen by the enemy fleet";
        m_stealth[0].missionTimer = 0.0f;
        m_stealth[0].spotted = false;
        m_stealth[0].credits = (int)MissionRewards.HARD;


    }

    void ScavengeMissions()
    {
        // set all missions to incomplete
        for (int i = 0; i < m_stealth.Length; i++)
        {
            m_scavenge[i].completed = false;
            m_scavenge[i].type = MissionType.SCAVENGE;
            m_scavenge[i].enemy = EnemyType.NONE;
        }

        m_scavenge[0].missionName = "Fast and Furious";
        m_scavenge[0].missionInfo = "Collect 4 objectives in 2 minutes";
        m_scavenge[0].credits = (int)MissionRewards.NORMAL;
        m_scavenge[0].isOptional = true;
        m_scavenge[0].missionTimer = 120.0f;
        m_scavenge[0].objectives = 4;

        m_scavenge[1].missionName = "Scavenger";
        m_scavenge[1].missionInfo = "Collect 4 objectives";
        m_scavenge[1].credits = (int)MissionRewards.EASY;
        m_scavenge[1].isOptional = false;
        m_scavenge[1].missionTimer = 0.0f;
        m_scavenge[1].objectives = 4;

        m_scavenge[2].missionName = "Hoarder";
        m_scavenge[2].missionInfo = "Collect 10 objectives";
        m_scavenge[2].credits = (int)MissionRewards.HARD;
        m_scavenge[2].isOptional = false;
        m_scavenge[2].missionTimer = 0.0f;
        m_scavenge[2].objectives = 10;

    }

    void LoadMissions()
    {
        ScavengeMissions();
        CombatMissions();
        StealthMissions();

        level1[0] = m_scavenge[1];
        level1[1] = m_combat[0];
        level1[2] = m_combat[2];
        level1[3] = m_stealth[0];

        Debug.Log("Loading mission");
        MissionStorage.Add("Level1", level1);
    }

    public Mission[] GetMissionsByLevel(string levelName)
    {
        Debug.Log("Level Name : " + levelName);
        return MissionStorage[levelName];
    }

}
