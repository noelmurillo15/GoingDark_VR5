using UnityEngine;
using UnityEngine.UI;
using MovementEffects;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GoingDark.Core.Enums;


public class TallyScreen : MonoBehaviour
{

    private int enemies = 0;
    private int asteroids = 0;
    private int[] missileCount;
    private decimal time = 0;

    private int enemyScore;
    private int asteroidScore;
    private int timeScore;
    private int deathScore = 0;

    private string finalScore;

    private Text tEnemies;
    private Text tTime;
    private Text tAsteroids;
    private Text tScore;

    private x360Controller controller;
    private MissionSystem missionSystem;
    private SystemManager systems;
    private MissileSystem missileSystem;
    private PersistentGameManager gameManager;
    private PlayerStats playerStats;

    [SerializeField]
    private PauseManager save;
    [SerializeField]
    private GameObject toContinue;
    [SerializeField]
    private GameObject tallyscreen;
    [SerializeField]
    private Text timer;

    private TimeSpan tSpan;

    #region Getters&Setters
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
    #endregion

    // Use this for initialization
    void Start()
    {
        gameManager = PersistentGameManager.Instance;
        controller = GamePadManager.Instance.GetController(0);

        missionSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionSystem>();
        systems = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        Text[] text = tallyscreen.GetComponentsInChildren<Text>();
        tEnemies = text[0];
        tAsteroids = text[1];
        tTime = text[2];
        tScore = text[3];

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

    #region Coroutines
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

                playerStats.FadeOut();
                yield return 0f;
            }
            else
                yield return 0f;
        }
    }
    #endregion

    #region Private Methods

    private void UpdateMissileCount()
    {
        if (missileSystem == null)
        {
            missileSystem = systems.GetSystemScript(SystemType.Missile) as MissileSystem;
            missileCount = missileSystem.GetMissileCount();

            // save missile amounts
            gameManager.SetBasicMissileCount(missileCount[(int)MissileType.Basic]);
            gameManager.SetEMPMissileCount(missileCount[(int)MissileType.Emp]);
            gameManager.SetShieldbreakMissileCount(missileCount[(int)MissileType.ShieldBreak]);
            gameManager.SetChromaticMissileCount(missileCount[(int)MissileType.Chromatic]);
        }
    }
    private void SetText()
    {
        enemyScore = enemies * 100;
        asteroidScore = asteroids * 10;
        timeScore = 1000 - (int)time;

        if (playerStats.DeathCount == 0)
            deathScore = 500;
        int score = CalcScore();

        tEnemies.text = "Enemies killed x " + enemies + " = " + enemyScore;
        tAsteroids.text = "Asteroids destroyed x " + asteroids + " = " + asteroidScore;
        tTime.text = "Completion time : " + string.Format("{0:D1}h:{1:D2}m:{2:D2}s", tSpan.Hours, tSpan.Minutes, tSpan.Seconds) + " = " + timeScore;
        tScore.text = "Level score : " + score + " (" + finalScore + ")";
    }

    private void TimedMissions(decimal time)
    {
        decimal temp = decimal.Round(time, 0);
        if (temp == 15 || temp == 300 || temp == 600)
            missionSystem.CheckTimedMissions((float)time);
    }

    private int CalcScore()
    {
        int score = 0;

        if (playerStats.DeathCount != 0)
            score = (enemyScore + asteroidScore + timeScore) / playerStats.DeathCount + deathScore;
        else
            score = (enemyScore + asteroidScore + timeScore) + deathScore;

        if (score >= 5000)
            finalScore = "S";
        else if (score >= 4000)
            finalScore = "A";
        else if (score >= 3000)
            finalScore = "B";
        else if (score >= 2000)
            finalScore = "C";
        else if (score >= 1000)
            finalScore = "D";
        else
            finalScore = "F";

        return score;
    }
    #endregion
}
