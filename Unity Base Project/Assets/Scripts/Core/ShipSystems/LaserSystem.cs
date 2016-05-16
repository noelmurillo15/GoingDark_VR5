﻿using UnityEngine;

public class LaserSystem : ShipDevice
{
    private float reload;
    private GameObject Laser;
    private GameObject gun1;
    private GameObject gun2;
    // Use this for initialization
    void Start()
    {
        Debug.Log("Starting Lasers");
        reload = 0f;
        maxCooldown = 10f;
        Laser = Resources.Load<GameObject>("LaserBeam");
        gun1 = GameObject.Find("Gun1");
        gun2 = GameObject.Find("Gun2");
    }

    // Update is called once per frame
    void Update()
    {
        if (reload > 0f)
            reload -= Time.deltaTime;

        UpdateCooldown();

        if (Activated)
            ShootGun();
    }

    public void ShootGun()
    {
        Debug.Log("Shooting Lasers");
        if (Laser == null)
            return;

        if (reload <= 0f)
        {
            Instantiate(Laser, gun1.transform.position, gun1.transform.rotation);
            Instantiate(Laser, gun2.transform.position, gun2.transform.rotation);
            reload = 1f;
        }
    }
}