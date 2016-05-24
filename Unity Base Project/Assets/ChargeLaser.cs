using UnityEngine;
using System.Collections;

public class ChargeLaser : MonoBehaviour
{
    public float delay = 1.25f;
    private GameObject laser;
    // Use this for initialization
    void Start()
    {
        laser = Resources.Load<GameObject>("ChargedShot");
    }

    // Update is called once per frame
    void Update()
    {
        delay -= Time.deltaTime;
        if (delay <= 0.0f)
        {
            Instantiate(laser, transform.position, transform.rotation);
            enabled = false;
        }
    }
}
