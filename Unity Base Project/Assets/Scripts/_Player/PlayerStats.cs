﻿using UnityEngine;
using GD.Core.Enums;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour {
    //**    Attach to Player    **//

    //  Health
    public int hitCount;
    //  Weapons
    public int numMissiles;
    //  Credits
    public int numCredits;
    //  Movement
    private MovementStats MoveData;

    private ShipDevices devices;

    //  Shields
    private bool shieldOn;
    private float shieldTimer;
    private int shieldHealth;
    private GameObject shield;

    // Use this for initialization
    void Start () {      
        hitCount = 0;
        MoveData.Speed = 0f;
        MoveData.Boost = 1f;
        MoveData.MaxSpeed = 50f;
        MoveData.RotateSpeed = 20f;
        MoveData.Acceleration = 5f;
        numCredits = PlayerPrefs.GetInt("Credits", 100);
        numMissiles = PlayerPrefs.GetInt("MissleCount", 10);
        devices = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<ShipDevices>();

        // shield defaults
        shieldOn = true;
        shieldTimer = 0.0f;
        shieldHealth = 3;
        shield = GameObject.FindGameObjectWithTag("Shield");
    }
	
	// Update is called once per frame
	void Update () {
        // shield cooldown and reactivate
        if (shieldTimer > 0.0f)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0.0f)
            {
                shieldOn = true;
                shield.SetActive(shieldOn);
            }
        }
    }

    #region Accessors
    public bool GetShield()
    {
        return shieldOn;
    }
    public int GetHitCount()
    {
        return hitCount;
    }
    public int GetNumMissiles()
    {
        return numMissiles;
    }           
    public ShipDevices GetDevices()
    {
        return devices;
    }
    public MovementStats GetMoveData()
    {
        return MoveData;
    }
    #endregion

    #region Modifiers
    public void StopMovement()
    {
        MoveData.Speed = 0f;
    }
    public void IncreaseSpeed()
    {
        if (MoveData.Speed < (MoveData.MaxSpeed * MoveData.Boost))
            MoveData.Speed += Time.deltaTime * MoveData.Acceleration;
        else if (MoveData.Speed > (MoveData.MaxSpeed * MoveData.Boost) + .5f)
            DecreaseSpeed();
    }
    public void DecreaseSpeed()
    {
        if (MoveData.Speed > 0.0f)
            MoveData.Speed -= Time.deltaTime * MoveData.Acceleration * 4f;
        else
            MoveData.Speed = 0.0f;
    }
    public void DecreaseMissileCount()
    {
        numMissiles--;
    }
    public void IncreaseHitCount()
    {
        hitCount++;
    }
    public void DecreaseHitCount()
    {
        hitCount--;
    }    
    #endregion

    #region Msg Functions
    public void EMPHit()
    {
        Debug.Log("EMP has affected Player's Systems");
    }

    public void Hit()
    {
        if (shieldOn)
        {
            shieldHealth--;
            if (shieldHealth == 0)
            {
                shieldHealth = 100;
                shieldOn = false;
                shield.SetActive(shieldOn);
                shieldTimer = 10.0f;
            }
        }
        else
        {
            IncreaseHitCount();
            PlayerHealth m_Health = GameObject.Find("Health").GetComponent<PlayerHealth>();
            m_Health.UpdatePlayerHealth();
        }
        AudioManager.instance.PlayHit();

        if (hitCount > 2)
            Kill();
    }

    public void EnvironmentalDMG()
    {
        if (shieldOn)
        {
            shieldHealth--;
            if (shieldHealth == 0)
            {
                shieldHealth = 100;
                shieldOn = false;
                shield.SetActive(shieldOn);
                shieldTimer = 10.0f;
            }
        }
        else
        {
            IncreaseHitCount();
            PlayerHealth m_Health = GameObject.Find("Health").GetComponent<PlayerHealth>();
            m_Health.UpdatePlayerHealth();
        }

        if (hitCount > 2)
            Kill();
    }

    public void Kill()
    {
        Debug.Log("Destroyed Player Ship");
        SceneManager.LoadScene("GameOver");
    }
    #endregion    
}