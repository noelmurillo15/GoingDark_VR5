﻿using UnityEngine;
using UnityEngine.UI;

public class LaserOverheat : MonoBehaviour {
    
    private Image LaserGauge;
    public GameObject smoke;
    public bool overheat;
    public float MaxAmount = 100.0f;
    public float CurrentAmount = 0.0f;


    void Start()
    {
        overheat = false;
        CurrentAmount = MaxAmount;
        LaserGauge = GetComponent<Image>();
        //smoke = GameObject.Find("Overheat");
        //smoke.SetActive(false);
    }

    void Update()
    {
        if(!overheat)
            UpdateGauge(Time.deltaTime * 20f);
        else
        {
            if (CurrentAmount > 95f)
                Reset();
            UpdateGauge(Time.deltaTime * 15f);
        }
    }

    public bool GetOverheat()
    {
        return overheat;
    }

    void SetOverheat(bool _boolean)
    {

        overheat = _boolean;
        //if (overheat)
        //{
        //    smoke.SetActive(true);
        //}
        //else
        //    smoke.SetActive(false);
    }

    public void UpdateGauge(float DamageTaken)
    {
        CurrentAmount += DamageTaken;
        if (CurrentAmount > 100f)
            CurrentAmount = 100f;
        else if(CurrentAmount < 0f)
        {
            SetOverheat(true);
        }

        float C_Shield = CurrentAmount / MaxAmount;
        if (!overheat)
            LaserGauge.color = Color.Lerp(Color.red, Color.cyan, C_Shield);
        else
            LaserGauge.color = Color.red;
        C_Shield *= .5f;
        SetHealth(C_Shield);
    }

    void SetHealth(float NewShield)
    {
        LaserGauge.fillAmount = NewShield;
    }

    public void Reset()
    {
        SetOverheat(false);
        CurrentAmount = MaxAmount;
        SetHealth(((CurrentAmount / MaxAmount) * 0.5f));
    }
}
