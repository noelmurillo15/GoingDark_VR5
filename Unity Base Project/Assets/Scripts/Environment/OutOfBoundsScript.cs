﻿using UnityEngine;

public class OutOfBoundsScript : MonoBehaviour {

    private GameObject messages;

    // Use this for initialization
    void Start () {
        Debug.Log("OutOfBounds Script Start");
        messages = GameObject.Find("WarningMessages");
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player")) {
            Debug.Log("Player in playable area");
            messages.SendMessage("ManualPilot");
            col.SendMessage("InBounds");
        }

        if (col.CompareTag("Enemy") || col.CompareTag("TransportShip"))
        {
            Debug.Log("Enemy is in Playable Area");
            col.SendMessage("InBounds");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("Player Out Of Bounds");
            messages.SendMessage("AutoPilot");
            col.SendMessage("OutOfBounds");
        }
        if (col.CompareTag("Enemy") || col.CompareTag("TransportShip"))
        {
            Debug.Log("Enemy Out Of Bounds");
            col.SendMessage("OutOfBounds");
        }
    }
}