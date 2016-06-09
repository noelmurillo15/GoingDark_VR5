﻿using UnityEngine;

public class LaserSystem : ShipDevice
{
    #region Properties
    private GameObject gun1;
    private GameObject gun2;
    private GameObject burst1;
    private GameObject burst2;
    private GameObject EnergyBar;
    private float size = 0.05f;
    private float BarAmount;
    private bool overheat;
    private float OverheatTimer;
    #endregion


    // Use this for initialization
    void Start()
    {
        maxCooldown = 1f;
        gun1 = GameObject.Find("Gun1");
        gun2 = GameObject.Find("Gun2");
        burst1 = gun1.transform.GetChild(0).gameObject;
        burst2 = gun2.transform.GetChild(0).gameObject;
        overheat = false;
        BarAmount = 100.0f;
        EnergyBar = transform.GetChild(3).gameObject;
        UpdateEnergyGauge(0.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!overheat)
        {
            if(BarAmount < 100.0f)
                BarAmount += Time.fixedDeltaTime*5.0f;
            


            if (Input.GetKey(KeyCode.L))
                Activate();

            if (Input.GetAxisRaw("RTrigger") > 0f)
                Activate();

            if (Activated)
                ShootGun();

            UpdateEnergyGauge(0.0f);
        }
        else
        {
            if(OverheatTimer>0.0f)
                OverheatTimer -= Time.fixedDeltaTime;
            else
            {
                BarAmount += Time.fixedDeltaTime*10.0f;
                UpdateEnergyGauge(0.0f);
                if(BarAmount>=100.0f)
                {
                    BarAmount = 100.0f;
                    overheat = false;
                }
            }
        }
    }

    public void ShootGun()
    {
        UpdateEnergyGauge(18.0f);

        burst1.SetActive(true);
        burst2.SetActive(true);
        DeActivate();
    }

    public void UpdateEnergyGauge(float amount)
    {
        BarAmount -= amount;
        if(BarAmount<= 0.0f)
        {
            overheat = true;
            BarAmount = 0.0f;
            OverheatTimer = 5.0f;
        }
        Vector3 newScale;
        newScale = EnergyBar.transform.localScale;
        newScale.y = size * (BarAmount * 0.01f);
        EnergyBar.transform.localScale = newScale;
        Vector3 newPos = EnergyBar.transform.localPosition;
        float offset = ((BarAmount * 0.01f) * 0.00456f) - 0.00456f;
        newPos.x = offset;
        EnergyBar.transform.localPosition = newPos;
    }
    
}