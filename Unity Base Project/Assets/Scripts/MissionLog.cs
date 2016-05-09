using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class MissionLog : MonoBehaviour {

    public GameObject[] buttons;
    public GameObject[] infoPanels;
    public Image[] checkmarks;
    public GameObject missionButtonPanel;
    
    private MissionSystem.Mission[] missions;
    //private Dictionary<int, Mission> MissionList;


    private MissionSystem m_missionSystem;
    //Mission[] m_mission;
	// Use this for initialization
	void Start () {

        //MissionList = new Dictionary<int, Mission>();

        // will load different missions for different levels
        m_missionSystem = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
        // load level 1 missions
        //TestMissions();
        LoadMissions();
    }

    // Update is called once per frame
    void Update () {


    }

    void Mission1()
    {
        infoPanels[0].SetActive(!infoPanels[0].activeSelf);
        CheckActivePanels(0);
    }

    void Mission2()
    {
        infoPanels[1].SetActive(!infoPanels[1].activeSelf);
        CheckActivePanels(1);
    }

    void Mission3()
    {
        infoPanels[2].SetActive(!infoPanels[2].activeSelf);
        CheckActivePanels(2);
    }

    void Mission4()
    {
        infoPanels[3].SetActive(!infoPanels[3].activeSelf);
        CheckActivePanels(3);
    }

    void CloseLog()
    {
        for (int i = 0; i < 4; i++)
            infoPanels[i].SetActive(false);
    }

    void CheckActivePanels(int num)
    {
        if (infoPanels[0].activeSelf && num != 0)
            infoPanels[0].SetActive(false);
        if (infoPanels[1].activeSelf && num != 1)
            infoPanels[1].SetActive(false);
        if (infoPanels[2].activeSelf && num != 2)
            infoPanels[2].SetActive(false);
        if (infoPanels[3].activeSelf && num != 3)
            infoPanels[3].SetActive(false);
    }

    /// <summary>
    ///  Loads missions for current level
    /// </summary>
    public void LoadMissions()
    {
        missions = m_missionSystem.GetMissionsByLevel("Level1");
        for (int i = 0; i < 4; i++)
        {
            buttons[i].GetComponentInChildren<Text>().text = missions[i].missionName;
            checkmarks[i].gameObject.SetActive(false);
            infoPanels[i].GetComponentInChildren<Text>().text = missions[i].missionInfo;
        }
    }

}
