﻿using UnityEngine;

public class LootPickup : MonoBehaviour
{

    private MissionSystem mission;
    private PlayerStats playerStats;

    // Use this for initialization
    void Start()
    {
        mission = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionSystem>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Player"))
        {
            mission.LootPickedUp();
            playerStats.UpdateCredits(100);
            AudioManager.instance.PlayCollect();
            Destroy(gameObject);
        }
    }
}