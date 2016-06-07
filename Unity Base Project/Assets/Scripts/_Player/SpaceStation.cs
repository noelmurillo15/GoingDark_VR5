﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceStation : MonoBehaviour
{

    public MissionSystem.Mission[] stationMissions;

    private MissionSystem m_missionSystem;
    private MissionLog m_missionLog;
    private MissionLoader m_missionLoader;
    private SystemManager SystemData;


    // Use this for initialization
    void Start()
    {
        SystemData = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();
        stationMissions = new MissionSystem.Mission[4];
        m_missionLog = GameObject.Find("MissionLog").GetComponent<MissionLog>();
        m_missionSystem = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
        m_missionLoader = GameObject.Find("PersistentGameObject").GetComponent<MissionLoader>();
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
            //m_missionLog.SendMessage("Docked", true);
            //if (SceneManager.GetActiveScene().name == "Tutorial")
            //    GameObject.Find("TutorialPref").GetComponent<Tutorial>().SendMessage("EnterStation");

            AudioManager.instance.PlayShipRepair();
            SystemData.FullSystemRepair();
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            //m_missionLog.SendMessage("Docked", false);
            //if (SceneManager.GetActiveScene().name == "Tutorial")
            //    GameObject.Find("TutorialPref").GetComponent<Tutorial>().SendMessage("ExitStation");
        }
    }
}
