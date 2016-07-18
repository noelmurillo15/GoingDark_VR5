using UnityEngine;
using UnityEngine.SceneManagement;

public class StationLog : MonoBehaviour
{

    private string SceneName;
    private MissionSystem m_missionSystem;
    private MissionLog m_missionLog;
    private int m_stationID;

    private bool mDocked = false;
    // Use this for initialization
    void Start()
    {
        SceneName = SceneManager.GetActiveScene().name;
        m_missionSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionSystem>();
        m_missionLog = GameObject.Find("MissionLog").GetComponent<MissionLog>();
    }

    public void Docked(bool isDocked, int stationID)
    {
        if (isDocked)
        {
            m_stationID = stationID;
            if (stationID == 1)
            {
                TurnInMission();
            }
            else if (stationID == 0)
            {
                if (m_missionSystem.m_ActiveMissions.Count == 0)
                {
                    m_missionSystem.AddActiveMission(m_missionSystem.m_PrimaryMissions[0]);
                    Debug.Log("Added primary mission " + m_missionSystem.m_PrimaryMissions[0].missionName);
                    m_missionLog.NewMission(m_missionSystem.m_ActiveMissions[0]);
                    if (SceneManager.GetActiveScene().name == "Level1")
                    {
                        Debug.Log(m_missionSystem.m_PrimaryMissions[0].type);
                    }
                    m_missionSystem.m_PrimaryMissions.RemoveAt(0);
                    
                }
            }
        }
    }

    void TurnInMission()
    {
        for (int i = 0; i < m_missionSystem.m_ActiveMissions.Count; i++)
        {
            if (!m_missionSystem.m_ActiveMissions[i].isOptional && m_missionSystem.m_ActiveMissions[i].completed)
            {
                m_missionSystem.TurnInMission(m_missionSystem.m_ActiveMissions[i]);
            }
        }
    }
}
