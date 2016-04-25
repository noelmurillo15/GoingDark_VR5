using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    GameObject health1;
    GameObject health2;
    GameObject health3;
    PlayerShipData player;
    public float timer;

    // Use this for initialization
    void Start()
    {
        timer = 30.0f;

        if (health1 == null)
            health1 = GameObject.Find("Health1");
        if (health2 == null)
            health2 = GameObject.Find("Health2");
        if (health3 == null)
            health3 = GameObject.Find("Health3");

        player = GameObject.Find("BattleShip").GetComponent<PlayerShipData>();
        health1.gameObject.GetComponent<Renderer>().material.color = Color.green;
        health2.gameObject.GetComponent<Renderer>().material.color = Color.green;
        health3.gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0.0f)
            timer -= Time.deltaTime;

        playerLife();

        if (timer <= 0.0f && player.GetHitCount() != 0)
        {
            player.DecreaseHitCount();
            timer = 30;
        }
    }

    void playerLife()
    {
        if (player.GetHitCount() >= 1)
            health1.gameObject.GetComponent<Renderer>().material.color = Color.red;
        else
            health1.gameObject.GetComponent<Renderer>().material.color = Color.green;

        if (player.GetHitCount() >= 2)
            health2.gameObject.GetComponent<Renderer>().material.color = Color.red;
        else
            health2.gameObject.GetComponent<Renderer>().material.color = Color.green;

        if (player.GetHitCount() >= 3)
            health3.gameObject.GetComponent<Renderer>().material.color = Color.red;
        else
            health3.gameObject.GetComponent<Renderer>().material.color = Color.green;
    }
}
