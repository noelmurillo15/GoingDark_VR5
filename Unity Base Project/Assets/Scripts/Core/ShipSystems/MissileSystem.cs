﻿using UnityEngine;
using GD.Core.Enums;
using UnityEngine.UI;

public class MissileSystem : ShipDevice
{

    #region Properties
    public int Count { get; private set; }

    //  Missile Types
    private GameObject basic;
    private GameObject emp;
    private GameObject chromatic;
    private GameObject shieldbreak;
    private GameObject selectedMissile;

    

    private bool missSwitch;
    private float missTimer;

    //  Missile Display
    private Text textCount;
    #endregion


    void Start()
    {
        missSwitch = false;
        missTimer = 5;
        //Debug.Log("Initializing Missiles");
        Count = 10;
        maxCooldown = 1f;

        
        basic = Resources.Load<GameObject>("Missiles/BasicMissile");
        emp = Resources.Load<GameObject>("Missiles/EmpMissile");
        chromatic = Resources.Load<GameObject>("Missiles/ChromaticMissile");
        shieldbreak = Resources.Load<GameObject>("Missiles/ShieldBreakMissile");

        textCount = GameObject.Find("MissileCounter").GetComponent<Text>();
        textCount.text = Count.ToString();
        selectedMissile = basic;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F) && Cooldown <= 0F)
            Activate();

        if (Activated)
            LaunchMissile();
    }

    public void LaunchMissile()
    {
        if (selectedMissile == null)
        {
            Debug.Log("Missile Gameobject not attached");
            return;
        }

        if (Count > 0)
        {
            Count--;
            textCount.text = Count.ToString();
            Activated = false;
            GameObject go = Instantiate(selectedMissile, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z - 1f), transform.rotation) as GameObject;
            AudioManager.instance.PlayMissileLaunch();
            Debug.Log("Missile has been launched");
        }
    }

    public void AddMissile()
    {
        int rand = Random.Range(2, 5);
        Count += rand;
        textCount.text = Count.ToString();
        Debug.Log(rand + " Missiles Added");
    }  
    
    
    public void WeaponSelect(MissileType type)
    {
        switch (type)
        {
            case MissileType.EMP:
                selectedMissile = emp;
                break;
            case MissileType.BASIC:
                selectedMissile = basic;
                break;
            case MissileType.CHROMATIC:
                selectedMissile = chromatic;
                break;
            case MissileType.SHIELDBREAKER:
                selectedMissile = shieldbreak;
                break;
        }
    }  
}
