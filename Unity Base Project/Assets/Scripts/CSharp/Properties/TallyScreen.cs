using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TallyScreen : MonoBehaviour {

    private int enemies = 0;
    private int asteroids = 0;
    private decimal time = 0;

    private Text tEnemies;
    private Text tTime;
    private Text tAsteroids;

    private x360Controller controller;

    public GameObject toContinue;
    public GameObject tallyscreen;

    public int EnemiesKilled
    {
        get{ return enemies; }
        set { enemies = value; }
    }

    public int AsteroidsDestroyed
    {
        get { return asteroids; }
        set { asteroids = value; }
    }

    // Use this for initialization
    void Start () {
        controller = GamePadManager.Instance.GetController(0);

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
        tTime.text = "Completion time : " + decimal.Round(time, 2);
    }
}
