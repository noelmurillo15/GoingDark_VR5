﻿using UnityEngine;

public class OutOfBoundsScript : MonoBehaviour
{

    private GameObject messages;
    private GameObject Player;
    private float DisplayDuration;

    // Use this for initialization
    void Start()
    {
        DisplayDuration = 10.0f;
        messages = GameObject.Find("PlayerCanvas");
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
            col.SendMessage("InBounds");
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Enemy"))
            col.SendMessage("OutOfBounds", transform.position);        
    }

    void SendWarning()
    {
        messages.SendMessage("AutoPilot");
    }
    void DisableWarning()
    {
        messages.SendMessage("ManualPilot");
    }

    void EnableAutoPilot()
    {
        Player.SendMessage("OutOfBounds", Vector3.zero);
    }
}