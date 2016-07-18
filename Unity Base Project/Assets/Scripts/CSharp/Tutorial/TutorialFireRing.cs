﻿using UnityEngine;

public class TutorialFireRing : MonoBehaviour {
    public GameObject prevRing;
    private BoxCollider box;
    private bool buffer;
    private GameObject particle;
    private TutorialFlight tutorial;
	// Use this for initialization
	void Start () {
        box = GetComponent<BoxCollider>();
        buffer = false;
        particle = transform.FindChild("SpiralParticles").gameObject;
        particle.SetActive(false);
        box.enabled = false;
        tutorial = GameObject.Find("TutorialGuidance").GetComponent<TutorialFlight>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!prevRing && !buffer)
        {
            buffer = true;
            box.enabled = true;
            particle.SetActive(true);
        }
	}

    void OnBecameVisible()
    {
        enabled = true;
    }

    void OnBecameInvisible()
    {
        enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            AudioManager.instance.PlayCollect();
            tutorial.SendMessage("AddRingCount");
            Destroy(gameObject);
        }
    }
}
