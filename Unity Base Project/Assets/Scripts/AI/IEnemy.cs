﻿using UnityEngine;
using GoingDark.Core.Enums;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(EnemyStateManager))]
[RequireComponent(typeof(EnemyCollision))]
[RequireComponent(typeof(EnemyMovement))]
public class IEnemy : MonoBehaviour {

    #region Properties
    [SerializeField]
    private EnemyTypes Type;
    [SerializeField]
    private GameObject stunned;
    [SerializeField]
    private bool hasShield;

    private float multiplier = 0f;

    private Impairments Debuff;
    private HealthProperties HealthData;
    private ShieldProperties ShieldData;

    private EnemyManager manager;
    private EnemyMovement movement;
    private EnemyCollision collision;
    private EnemyStateManager statemanager;
    #endregion


    void Awake()
    {             
        stunned.SetActive(false);
        Debuff = Impairments.None;

        movement = GetComponent<EnemyMovement>();
        collision = GetComponent<EnemyCollision>();
        statemanager = GetComponent<EnemyStateManager>();        

        switch (PlayerPrefs.GetString("Difficulty"))
        {
            case "Easy":
                multiplier = 1f;
                break;
            case "Medium":
                multiplier = 1.5f;
                break;
            case "Hard":
                multiplier = 2f;
                break;
            case "Nightmare":
                multiplier = 3f;
                break;
        }
        Invoke("LoadEnemyData", .5f);
    }

    #region Accessors        
    public float GetDifficultyMultiplier()
    {
        return multiplier;
    }
    public EnemyTypes GetEnemyType()
    {
        return Type;
    }
    public EnemyManager GetManager()
    {
        return manager;
    }
    public HealthProperties GetHealthData()
    {
        return HealthData;
    }
    public ShieldProperties GetShieldData()
    {
        return ShieldData;
    }
    public EnemyStateManager GetStateManager()
    {
        return statemanager;
    }
    public EnemyMovement GetEnemyMovement()
    {
        return movement;
    }
    public EnemyCollision GetEnemyCollision()
    {
        return collision;
    }
    public Impairments GetDebuffData()
    {
        return Debuff;
    }
    #endregion

    #region Debuffs
    void ResetDebuff()
    {
        Debuff = Impairments.None;
        stunned.SetActive(false);
    }
    #endregion

    #region Damage Calls 
    void EMPHit()
    {
        stunned.SetActive(true);
        Debuff = Impairments.Stunned;
        Invoke("ResetDebuff", 5f);
    }
    void SplashDmg()
    {
        if (Type != EnemyTypes.FinalBoss)
        {
            if (hasShield && ShieldData.GetShieldActive())
                ShieldData.Damage(ShieldData.MaxHealth * .1f);
            else
                HealthData.Damage(HealthData.MaxHealth * .1f);
        }
    }
    public void CrashHit(float _speed)
    {
        if (Type != EnemyTypes.FinalBoss)
        {
            if (hasShield && ShieldData.GetShieldActive())
                ShieldData.Damage(_speed * ShieldData.MaxHealth * .5f);
            else
                HealthData.Damage(_speed * HealthData.MaxHealth * .5f);
        }      
    }        
    public void MissileHit(MissileProjectile missile)
    {
        if (hasShield && ShieldData.GetShieldActive())
        {
            if (Type != EnemyTypes.FinalBoss)
            {
                if (missile.Type == MissileType.ShieldBreak)
                {
                    ShieldData.Damage(100f);
                    missile.Kill();
                }
                else if (missile.Type == MissileType.Emp)
                {
                    EMPHit();
                    missile.Kill();
                }
                else
                    missile.Deflect();
            }
            else
            {
                missile.Deflect();
            }                   
        }
        else
        {
            HealthData.Damage(missile.GetBaseDmg());
            missile.Kill();
        }
    }
    public void LaserDmg(LaserProjectile laser)
    {
        if (hasShield && ShieldData.GetShieldActive())
        {
            if (Type != EnemyTypes.FinalBoss)
                ShieldData.Damage(laser.GetBaseDmg());            
        }
        else
            HealthData.Damage(laser.GetBaseDmg());        
        
        laser.Kill();
    }

    public void Kill()
    {
        if (GetComponent<EnemyTrail>() != null)        
            GetComponent<EnemyTrail>().Kill();

        manager.RemoveEnemy(this);
        Destroy(gameObject);
    }
    #endregion

    #region Private Methods
    void LoadEnemyData()
    {        
        if (hasShield)
            ShieldData = new ShieldProperties(transform.GetChild(0).gameObject, 100f * multiplier);

        switch (Type)
        {
            case EnemyTypes.Droid:
                HealthData = new HealthProperties(5 * multiplier, transform); break;
            case EnemyTypes.JetFighter:
                HealthData = new HealthProperties(10 * multiplier, transform); break;
            case EnemyTypes.Trident:
                HealthData = new HealthProperties(15 * multiplier, transform); break;
            case EnemyTypes.Basic:
                HealthData = new HealthProperties(20 * multiplier, transform); break;
            case EnemyTypes.SquadLead:
                HealthData = new HealthProperties(30 * multiplier, transform); break;
            case EnemyTypes.Transport:
                HealthData = new HealthProperties(50 * multiplier, transform); break;
            case EnemyTypes.Tank:
                HealthData = new HealthProperties(90 * multiplier, transform); break;
            case EnemyTypes.FinalBoss:
                HealthData = new HealthProperties(150 * multiplier, transform); break;
        }
        manager = transform.root.GetComponent<EnemyManager>();

        movement.Initialize();
        collision.Initialize();
        movement.LoadEnemyData(multiplier);

        statemanager.SetEnemyTarget(null);
        manager.AddEnemy(this);
    }
    #endregion 
}