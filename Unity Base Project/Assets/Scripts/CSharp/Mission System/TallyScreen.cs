using UnityEngine;
using UnityEngine.UI;
using MovementEffects;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GoingDark.Core.Enums;
using GoingDark.Core.Utility;


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

    [SerializeField]
    private PauseManager save;

    public GameObject toContinue;
    public GameObject tallyscreen;

    public Text timer;
    TimeSpan tSpan;

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

        tSpan = TimeSpan.FromSeconds((double)time);
        timer.text = string.Format("{0:D1}h:{1:D2}m:{2:D2}s", tSpan.Hours, tSpan.Minutes, tSpan.Seconds);
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
                save.AutoSave();
                SceneManager.LoadScene("LevelSelect");
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
        tTime.text = "Completion time : " + string.Format("{0:D1}h:{1:D2}m:{2:D2}s", tSpan.Hours, tSpan.Minutes, tSpan.Seconds);
    }

    void TimedMissions(decimal time)
    {
        decimal temp = decimal.Round(time, 0);
        if (temp == 15 || temp == 300 || temp == 600)
            missionSystem.CheckTimedMissions((float)time);
    }
}
