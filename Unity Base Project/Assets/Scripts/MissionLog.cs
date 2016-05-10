using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class MissionLog : MonoBehaviour
{

    // need all these public for stupid gameobjects not found when inactive >=(
    public GameObject[] buttons;
    public GameObject[] infoPanels;
    public GameObject[] missionButtons;
    // public GameObject[] turnInButtons;
    public Image[] checkmarks;
    public GameObject missionButtonPanel;

    private MissionSystem.Mission[] missions;

    private MissionSystem m_missionSystem;
    private bool atStation;
    // Use this for initialization
    void Start()
    {

        atStation = false;
        // will load different missions for different levels
        m_missionSystem = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
        // load level 1 missions
        LoadMissions();
    }

    // Update is called once per frame
    void Update()
    {


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

    /// <summary>
    /// Checks what mission log panels are active and turn them off
    /// num = panel to not turn off
    /// </summary>
    /// <param name="num"></param>
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

        Text[] text;
        for (int i = 0; i < 4; i++)
        {
            buttons[i].GetComponentInChildren<Text>().text = missions[i].missionName;
            checkmarks[i].gameObject.SetActive(false);
            text = infoPanels[i].GetComponentsInChildren<Text>();
            text[0].text = missions[i].missionInfo;
            text[1].text = "Objectives Left : " + missions[i].objectives;
        }
    }

    /// <summary>
    /// Marks mission as completed
    /// </summary>
    /// <param name="missionNum"></param>
    void CompletedMission(int missionNum)
    {
        missions[missionNum - 1].completed = true;
        missions[missionNum - 1].isActive = false;
        checkmarks[missionNum - 1].gameObject.SetActive(true);
        Text[] text = infoPanels[missionNum - 1].GetComponentsInChildren<Text>();
        text[1].text = "Return to station to turn in missions";
    }

    /// <summary>
    /// Updates objective count for set mission
    /// </summary>
    /// <param name="m"></param>
    void UpdateObjectCount(MissionSystem.Mission m)
    {
        for (int i = 0; i < 4; i++)
        {
            if (missions[i].missionName == m.missionName)
            {
                Text[] text;
                missions[i].objectives = m.objectives;
                text = infoPanels[i].GetComponentsInChildren<Text>();
                text[1].text = "Objectives Left : " + missions[i].objectives;
                Debug.Log("Updated objective count");
            }
        }
    }


    /// <summary>
    /// Starts mission depending on the name of the parent of the clicked button
    /// </summary>
    /// <param name="parentName"></param>
    void AcceptMission(string parentName)
    {
        if (parentName == "Mission1_InfoPanel")
        {
            if (!missions[0].completed)
            {
                missions[0].isActive = true;
                checkmarks[0].gameObject.SetActive(true);
            }
            else
            {
                m_missionSystem.SendMessage("TurnIn", 1);
                buttons[0].GetComponent<Button>().interactable = false;
                buttons[0].GetComponent<BoxCollider>().enabled = false;
            }
        }
        else if (parentName == "Mission2_InfoPanel")
        {
            if (!missions[1].completed)
            {
                missions[1].isActive = true;
                checkmarks[1].gameObject.SetActive(true);
            }
            else
                m_missionSystem.SendMessage("TurnIn", 2);
        }
        else if (parentName == "Mission3_InfoPanel")
        {
            if (!missions[2].completed)
            {
                missions[2].isActive = true;
                checkmarks[2].gameObject.SetActive(true);
            }
            else
            {
                m_missionSystem.SendMessage("TurnIn", 3);
            }
        }
        else if (parentName == "Mission4_InfoPanel")
        {
            if (!missions[3].completed)
            {
                missions[3].isActive = true;
                checkmarks[3].gameObject.SetActive(true);
            }
            else
            {
                m_missionSystem.SendMessage("TurnIn", 4);

            }
        }
    }

    /// <summary>
    /// Sets all text and buttons for completed missions to Turn In
    /// </summary>
    void AtStation()
    {
        for (int i = 0; i < 4; i++)
        {
            if (missions[i].completed)
            {
                missionButtons[i].gameObject.SetActive(true);
                missionButtons[i].GetComponentInChildren<Text>().text = "Turn In";

            }
        }
    }

    /// <summary>
    /// Resets objects and text in mission log for completed missions
    /// </summary>
    void LeavingStation()
    {
        for (int i = 0; i < 4; i++)
        {
            if (missions[i].completed)
            {
                missionButtons[i].gameObject.SetActive(false);
                Text[] text = infoPanels[i].GetComponentsInChildren<Text>();
                text[1].text = "Return to station to turn in missions";

            }
        }
    }

}
