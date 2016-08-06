﻿using UnityEngine;
using UnityEngine.UI;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GoingDark.Core.Enums;


public class TallyScreen : MonoBehaviour
{

    private int enemies = 0;
    private int asteroids = 0;
    private decimal time = 0;

    private Text tEnemies;
    private Text tTime;
    private Text tAsteroids;

    private int[] missileCount;

    private x360Controller controller;
    private MissionSystem missionSystem;
    private SystemManager systems;
    private MissileSystem missileSystem;


    public GameObject toContinue;
    public GameObject tallyscreen;

    public Text timer;


    public int EnemiesKilled
    {
        get { return enemies; }
        set { enemies = value; }
    }

    public int AsteroidsDestroyed
    {
        get { return asteroids; }
        set { asteroids = value; }
    }

    // Use this for initialization
    void Start()
    {
        controller = GamePadManager.Instance.GetController(0);
        missionSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionSystem>();
        systems = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();

        Text[] text = tallyscreen.GetComponentsInChildren<Text>();
        tEnemies = text[0];
        tAsteroids = text[1];
        tTime = text[2];

        tallyscreen.SetActive(false);
    }

    void Update()
    {
        if (!tallyscreen.activeSelf)
            time += (decimal)Time.deltaTime;

        timer.text = decimal.Round(time, 2).ToString();

        TimedMissions(time);
    }

    public void ActivateTallyScreen()
    {
        Timing.RunCoroutine(PressButton());
    }

    private void UpdateMissileCount()
    {
        if (missileSystem == null)
        {
            missileSystem = systems.GetSystemScript(SystemType.Missile) as MissileSystem;
            missileCount = missileSystem.GetMissileCount();

            PlayerPrefs.SetInt("BasicMissileCount", missileCount[(int)MissileType.Basic]);
            PlayerPrefs.SetInt("EMPMissileCount", missileCount[(int)MissileType.Emp]);
            PlayerPrefs.SetInt("ShieldbreakMissileCount", missileCount[(int)MissileType.ShieldBreak]);
            PlayerPrefs.SetInt("ChromaticMissileCount", missileCount[(int)MissileType.Chromatic]);
        }
    }

    private IEnumerator<float> PressButton()
    {
        SetText();
        tallyscreen.SetActive(true);
        toContinue.SetActive(true);
        while (true)
        {
            if (controller.GetButtonDown("A"))
            {
                UpdateMissileCount();
                tallyscreen.SetActive(false);
                SceneManager.LoadScene("LevelSelect");//, LoadSceneMode.Single);
                yield return 0f;
            }
            else
                yield return 0f;
        }
    }

    private void SetText()
    {
        tEnemies.text = "Enemies killed : " + enemies;
        tAsteroids.text = "Asteroids destroyed : " + asteroids;
        tTime.text = "Completion time : " + decimal.Round(time, 2);
    }

    void TimedMissions(decimal time)
    {
        decimal temp = decimal.Round(time, 0);
        if (temp == 15 || temp == 300 || temp == 600)
            missionSystem.CheckTimedMissions((float)time);
    }
}
