﻿using UnityEngine;

[RequireComponent(typeof(EnemyBehavior))]
public class TransportShipAI : MonoBehaviour {
    //**    Attach to Transport Prefab  **//
    private float padding;
    public float cloakTimer;

    public Material transMat;
    public Material opaqueMat;

    private GameObject mesh1;
    private GameObject mesh2;

    private EnemyBehavior ai;


    // Use this for initialization
    void Start () {       
        mesh1 = transform.GetChild(0).gameObject;
        mesh2 = transform.GetChild(1).gameObject;

        ai = GetComponent<EnemyBehavior>();
    }
	
	// Update is called once per frame
	void Update () {
        if (padding > 0f)
            padding -= Time.deltaTime;

        if (cloakTimer > 0.0f)
            cloakTimer -= Time.deltaTime;
        else {
            if(cloakTimer < 0.0f)
                CloakOff();

            cloakTimer = 0.0f;
        }
    }

    void CloakOn() {
        if (cloakTimer <= 0.0f) {
            cloakTimer = 30.0f;
            mesh1.GetComponent<Renderer>().material = transMat;
            mesh2.GetComponent<Renderer>().material = transMat;
        }
    }

    void CloakOff() {
        ai.ChangeBehavior();
        mesh1.GetComponent<Renderer>().material = opaqueMat;
        mesh2.GetComponent<Renderer>().material = opaqueMat;
    }

    void OnTriggerEnter(Collider col) {
        if (col.tag == "Player")
            CloakOn();
    }

    public float GetCloakTimer()
    {
        return cloakTimer;
    }
}