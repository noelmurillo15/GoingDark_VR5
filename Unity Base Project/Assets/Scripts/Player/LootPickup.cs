﻿using UnityEngine;
using System.Collections;

public class LootPickup : MonoBehaviour {


    [SerializeField]
    private LootCounter lootCounter;

    private bool collected;
    private float timer;
    private GameObject messages;

    // Use this for initialization
    void Start () {
        collected = false;
        timer = 2.0f;
        messages = GameObject.Find("Screen");
    }
	
	// Update is called once per frame
	void Update () {

	    if (collected)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
                gameObject.SetActive(false);
            messages.SendMessage("LootPickUp");

        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            if (lootCounter.lootCounter > 0)
            {
                lootCounter.lootCounter -= 1;
                collected = true;
                if (lootCounter.lootCounter == 0)
                {
                    messages.SendMessage("Win");
                }
            }
            
        }
    }
}
