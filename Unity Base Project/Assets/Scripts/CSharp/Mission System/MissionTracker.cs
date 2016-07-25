﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GoingDark.Core.Enums;
using MovementEffects;
using UnityEngine.SceneManagement;

public class MissionTracker : MonoBehaviour
{

    // main mission
    private Text name;
    private Text info;
    private Text credits;
    // tracking current mission
    private Text trackerName;
    private Text trackerInfo;
    private Text trackerObjectives;

    private GameObject continueText;
    private GameObject missionBox;

    private MissionSystem missionSystem;
    private x360Controller controller;
    private SystemManager systemManager;
    private MissionLog missionLog;
    private string SceneName;

    public GameObject missionTracker;

    // Use this for initialization
    void Start()
    {

        controller = GamePadManager.Instance.GetController(0);
        SceneName = SceneManager.GetActiveScene().name;
        //missionTracker = GameObject.Find("MissionTracker");
        missionBox = GameObject.Find("MissionBox");
        continueText = GameObject.Find("PressToContinue");
        missionSystem = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
        systemManager = GameObject.Find("Devices").GetComponent<SystemManager>();
        missionLog = GameObject.Find("Missions").GetComponent<MissionLog>();
        // turn off after they are found
        missionTracker.SetActive(false);
        continueText.SetActive(false);
        missionBox.SetActive(false);

        AssignText();
        Timing.RunCoroutine(ShowMain());
    }

    void Update()
    {
        if (controller.GetButtonDown("Start"))
        {
            if (missionSystem.m_ActiveMissions.Count > 0 && SceneName != "Level1")
            {
                missionLog.TurnOnPanel();
                systemManager.SendMessage("MessageUp", true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            missionSystem.LootPickedUp();
            //missionSystem.KilledEnemy(EnemyTypes.Any);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            missionSystem.KilledEnemy(EnemyTypes.Any);
        }
    }


    IEnumerator<float> ShowMain()
    {
        yield return Timing.WaitForSeconds(2.0f);
        missionBox.SetActive(true);
        systemManager.SendMessage("MessageUp", true);
        yield return Timing.WaitForSeconds(3.0f);
        continueText.SetActive(true);

        while (true)
        {
            if (controller.GetButtonDown("A") || Input.GetKeyDown(KeyCode.A))
            {
                missionBox.SetActive(false);
                systemManager.SendMessage("MessageUp", false);
                //missionSystem.KilledEnemy(EnemyTypes.Droid);
                yield return 0f;
            }
            else
                yield return 0f;
        }
    }

    public void ShowTracker()
    {
        missionTracker.SetActive(true);
        // set info for tracker
        UpdateInfo(missionSystem.m_ActiveMissions[0]);
    }

    private void AssignText()
    {
        Text[] temp = missionBox.GetComponentsInChildren<Text>();
        info = temp[1];

        Text[] temp2 = missionTracker.GetComponentsInChildren<Text>();
        trackerName = temp2[0];
        trackerInfo = temp2[1];
        trackerObjectives = temp2[2];

        info.text = MissionInfo(SceneManager.GetActiveScene().name);
    }

    public void UpdateInfo(Mission mission)
    {
        if (trackerName.text == "MissionName" || trackerName.text == mission.missionName)
        {
            trackerInfo.text = mission.missionInfo;
            trackerName.text = mission.missionName;
            trackerObjectives.text = "Objectives Left : " + mission.objectives;
        }
    }

    public void UpdateInfo(Mission mission, bool changed)
    {
        if (changed)
        {
            trackerInfo.text = mission.missionInfo;
            trackerName.text = mission.missionName;
            trackerObjectives.text = "Objectives Left : " + mission.objectives;
        }
    }

    private string MissionInfo(string level)
    {
        string info = "";
        switch (level)
        {
            case "Level1":
                {
                    info = "We have found traces of alien activity on the outskirts of our sector. We are not sure who or what it is, and we need you to scout them out. Collect any valuables they left behind and destroy any remaining ships. Go through one of the portals when you are ready.";
                    break;
                }
            case "Level2":
                {
                    info = "We have found traces of alien activity on the outskirts of our sector. We are not sure who or what it is, and we need you to scout them out. Collect any valuables they left behind and destroy any remaining ships. Go through one of the portals when you are ready.";
                    break;
                }
            default:
                break;
        }

        return info;
    }

}
