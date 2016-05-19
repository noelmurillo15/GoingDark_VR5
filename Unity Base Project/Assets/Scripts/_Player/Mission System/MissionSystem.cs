using UnityEngine;
using UnityEngine.SceneManagement;


public class MissionSystem : MonoBehaviour
{
    private enum MissionRewards { EASY = 50, NORMAL = 100, HARD = 150 };
    public enum MissionType { COMBAT, SCAVENGE, STEALTH };
    public enum EnemyType { BASIC_ENEMY, KAMIKAZE, TRANSPORT, ANY, NONE };

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


        public int Objectives
        {
            get { return objectives; }
            set { objectives = value; }
        }

    }

    public Mission[] m_LevelMissions;
    private MissionLog m_missionLog;

    // Use this for initialization
    void Start()
    {
        m_missionLog = GameObject.Find("MissionLog").GetComponent<MissionLog>();
        //m_LevelMissions = gameObject.GetComponent<MissionLoader>().LoadMissions();
        Debug.Log("Mission Received : " + m_LevelMissions[0].missionName);
        // Load in all the missions with IO
    }

    // Update is called once per frame
    void Update()
    {

    }


    void LootPickedUp()
    {
        for (int i = 0; i < 4; i++)
        {
            if (m_LevelMissions[i].type == MissionType.SCAVENGE && m_LevelMissions[i].isActive)
            {
                m_LevelMissions[i].objectives--;
                m_missionLog.SendMessage("UpdateObjectCount", m_LevelMissions[i]);
                Debug.Log("Updated Objective count in System");

                if (m_LevelMissions[i].objectives == 0)
                {
                    m_missionLog.SendMessage("CompletedMission", i + 1);
                    Debug.Log("Mission Completed");
                }
            }


        }
    }

    /// <summary>
    /// Kill message recieved from Enemy ship
    /// </summary>
    void BASIC_ENEMY()
    {

    }

    /// <summary>
    /// Kill message recieved from Transport ship
    /// </summary>
    void TRANSPORT()
    {

    }

    /// <summary>
    /// Kill message recieved from Kamikaze ship
    /// </summary>
    void KAMIKAZE()
    {

    }


    /// <summary>
    ///  Handles turn in of a mission
    /// </summary>
    /// <param name="index"></param>
    void TurnIn(int index)
    {
        string name = SceneManager.GetActiveScene().name;
        PlayerStats stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        stats.numCredits += m_LevelMissions[index - 1].credits;
    }

}
