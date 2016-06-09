using UnityEngine;
using UnityEngine.SceneManagement;
using GoingDark.Core.Enums;

public class SpaceStation : MonoBehaviour
{

    public Mission[] stationMissions;

    private MissionSystem m_missionSystem;
    private MissionLoader m_missionLoader;
    private AudioSource sound;
    private Tutorial m_tutorial;
    private StationLog m_stationLog;
    private string m_sceneName;

    // Use this for initialization
    void Start()
    {
        sound = GetComponent<AudioSource>();

        stationMissions = new Mission[4];
        m_stationLog = GameObject.Find("MissionLog").GetComponent<StationLog>();
        m_missionSystem = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
        m_missionLoader = GameObject.Find("PersistentGameObject").GetComponent<MissionLoader>();
        m_sceneName = SceneManager.GetActiveScene().name;

        //if (m_sceneName == "Tutorial")
        //    m_tutorial = GameObject.Find("TutorialPref").GetComponent<Tutorial>();
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
            Debug.Log("Collided with station");
            m_stationLog.Docked(true);
            //if (m_sceneName == "Tutorial")
            //    m_tutorial.SendMessage("EnterStation");
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            m_stationLog.Docked(false);
            //m_stationLog.SendMessage("Docked", false);
            //if (m_sceneName == "Tutorial")
            //    m_tutorial.SendMessage("ExitStation");
        }

    }
}
