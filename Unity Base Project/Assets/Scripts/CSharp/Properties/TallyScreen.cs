using UnityEngine;
using UnityEngine.UI;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TallyScreen : MonoBehaviour
{

    private int enemies = 0;
    private int asteroids = 0;
    private decimal time = 0;

    private Text tEnemies;
    private Text tTime;
    private Text tAsteroids;

    private x360Controller controller;
    private MissionSystem missionSystem;

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

    IEnumerator<float> PressButton()
    {
        SetText();
        yield return Timing.WaitForSeconds(5.0f);
        tallyscreen.SetActive(true);
        toContinue.SetActive(true);
        while (true)
        {
            if (controller.GetButtonDown("A"))
            {
                Debug.Log("Pressed A");
                tallyscreen.SetActive(false);
                // send to level select.... ?
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

    //private string CheckLevel()
    //{
    //    string name = SceneManager.GetActiveScene().name;
    //    string ret = "";
    //    switch (name)
    //    {
    //        case "Level1":
    //            {

    //                break;
    //            }
    //        case "Level2":
    //            {
    //                break;
    //            }
    //        case "Level3":
    //            {
    //                break;
    //            }
    //        default:
    //            break;
    //    }
    //}

    void TimedMissions(decimal time)
    {
        decimal temp = decimal.Round(time, 0);
        if (temp == 15 || temp == 300 || temp == 600)
            missionSystem.CheckTimedMissions((float)time);
    }
}
