using UnityEngine;
using System.Collections;

public class ChargeLaser : MonoBehaviour
{
    public float delay = 1.25f;
    public GameObject laser;
    private Transform leapcam;
    private GameObject environment;

    // Use this for initialization
    void Start()
    {
        environment = GameObject.Find("Environment");
        leapcam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        //laser = Resources.Load<GameObject>("ChargedShot");
    }

    // Update is called once per frame
    void Update()
    {
        delay -= Time.deltaTime;
        if (delay <= 0.0f)
        {
            GameObject go = Instantiate(laser, transform.position, leapcam.transform.rotation) as GameObject;
            enabled = false;
        }
    }
}
