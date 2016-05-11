﻿using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    ShootObject missile;
    bool collected;
    // Use this for initialization
    void Start()
    {
        collected = false;
        missile = GameObject.Find("MissileLaunch").GetComponent<ShootObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collected)
            Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            missile.SendMessage("AddMissile");
            collected = true;            
        }
    }
}