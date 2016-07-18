using UnityEngine;
using UnityEngine.SceneManagement;
using GoingDark.Core.Enums;

public class SpaceStation : MonoBehaviour
{

    public Mission[] stationMissions;

    private MissionSystem m_missionSystem;
    private MissionLoader m_missionLoader;
    private AudioSource sound;
    private StationLog m_stationLog;

    public int m_stationID;

    // Use this for initialization
    void Start()
    {
        sound = GetComponent<AudioSource>();

        stationMissions = new Mission[4];
        m_stationLog = GameObject.Find("MissionLog").GetComponent<StationLog>();
        m_missionSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionSystem>();
        m_missionLoader = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionLoader>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider col)
    {
        // if player entered the space station, let them turn in missions
        if (col.transform.tag == "Player")
        {
            m_stationLog.Docked(true, m_stationID);
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            m_stationLog.Docked(false, m_stationID);
        }

    }
}
