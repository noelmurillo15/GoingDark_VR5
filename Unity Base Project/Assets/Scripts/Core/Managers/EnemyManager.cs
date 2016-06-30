﻿using UnityEngine;
using GoingDark.Core.Enums;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{

    #region Properties
    public GameDifficulty Difficulty;

    public Transform PlayerPosition { get; private set; }
    private MissionSystem missionSystem;

    private CloakSystem pCloak;
    private SystemManager systemManager;
    private MissionSystem missions;

    private List<EnemyBehavior> enemies = new List<EnemyBehavior>();

    private ObjectPooling ammopool = new ObjectPooling();
    private ObjectPooling laserpool = new ObjectPooling();
    private ObjectPooling missilepool = new ObjectPooling();
    private ObjectPooling explosionpool = new ObjectPooling();

    private GameObject explosions;
    private GameObject projectiles;
    #endregion

    void Awake()
    {
        explosions = GameObject.Find("Explosions");
        projectiles = GameObject.Find("Projectiles");

        InvokeRepeating("CheckEnemies", 10f, 5f);          
        missionSystem = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
    }

    void Start()
    {
        ammopool.Initialize(Resources.Load<GameObject>("AmmoDrop"), 10, projectiles);
        laserpool.Initialize(Resources.Load<GameObject>("Projectiles/Lasers/EnemyLaser"), 30, projectiles);
        missilepool.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/EnemyMissile"), 30, projectiles);
        explosionpool.Initialize(Resources.Load<GameObject>("Projectiles/Explosions/EnemyExplosion"), 40, explosions);

        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        systemManager = GameObject.Find("Devices").GetComponent<SystemManager>();

        missions = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();

        pCloak = systemManager.GetSystemScript(SystemType.Cloak) as CloakSystem;
    }

    #region Accessors
    public CloakSystem GetPlayerCloak()
    {
        return pCloak;
    }
    public GameObject GetAmmoDrop()
    {
        return ammopool.GetPooledObject();
    }
    public GameObject GetEnemyLaser()
    {
        return laserpool.GetPooledObject();
    }
    public GameObject GetEnemyMissile()
    {
        return missilepool.GetPooledObject();
    }
    public GameObject GetEnemyExplosion()
    {
        return explosionpool.GetPooledObject();
    }    
    #endregion

    #region Modifiers
    public void AddEnemy(EnemyBehavior enemy)
    {
        enemies.Add(enemy);
    }
   
    public void RemoveEnemy(EnemyBehavior enemy)
    {
        RandomAmmoDrop(enemy.MyTransform.position);
        missions.KilledEnemy(enemy.Type);
        enemies.Remove(enemy);
    }

    public void SendAlert(Vector3 enemypos)
    {
        AudioManager.instance.StartCoroutine("RaiseBattleMusic");
        object[] tempStorage = new object[2];
        tempStorage[0] = PlayerPosition.position;
        tempStorage[1] = enemypos;
        BroadcastMessage("BroadcastAlert", tempStorage);
    }

    private void CheckEnemies()
    {
        if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                    if (enemies[i].uniqueAi.enabled)
                        return;                
            }
            AudioManager.instance.StartCoroutine("LowerBattleMusic");
        }
        else
        {
            Debug.Log("You WIN");
        }
    }

    public void RandomAmmoDrop(Vector3 _pos)
    {
        GameObject go = null;
        if (Random.Range(1, 3) == 1)
        {
            go = GetAmmoDrop();
            go.transform.position = _pos;
            go.transform.rotation = Quaternion.identity;
            go.SetActive(true);
        }                   
    }

    public void TargetDestroyed()
    {
        BroadcastMessage("BroadcastWin");
    }
    #endregion
}