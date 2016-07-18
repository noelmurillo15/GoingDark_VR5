using UnityEngine;
using GoingDark.Core.Enums;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

    #region Properties
    public GameDifficulty Difficulty;

    public Transform PlayerPosition { get; private set; }

    private List<EnemyStateManager> enemies = new List<EnemyStateManager>();

    private ObjectPooling ammopool = new ObjectPooling();
    private ObjectPooling laserpool = new ObjectPooling();
    private ObjectPooling missilepool = new ObjectPooling();
    private ObjectPooling explosionpool = new ObjectPooling();

    private CloakSystem pCloak;
    private MissionSystem missionSystem;
    private SystemManager systemManager;

    private GameObject explosions;
    private GameObject projectiles;
    #endregion

    void Awake()
    {
        explosions = GameObject.Find("Explosions");
        projectiles = GameObject.Find("Projectiles");
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        systemManager = GameObject.Find("Devices").GetComponent<SystemManager>();
        missionSystem = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
    }

    void Start()
    {
        ammopool.Initialize(Resources.Load<GameObject>("AmmoDrop"), 10, projectiles);
        laserpool.Initialize(Resources.Load<GameObject>("Projectiles/Lasers/EnemyLaser"), 30, projectiles);
        missilepool.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/EnemyMissile"), 30, projectiles);
        explosionpool.Initialize(Resources.Load<GameObject>("Projectiles/Explosions/EnemyExplosion"), 40, explosions);

        pCloak = systemManager.GetSystemScript(SystemType.Cloak) as CloakSystem;
        InvokeRepeating("CheckEnemies", 20f, 5f);
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
    public void AddEnemy(EnemyStateManager enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyStateManager enemy)
    {
        RandomAmmoDrop(enemy.MyTransform.position);
        missionSystem.KilledEnemy(enemy.Type);
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
        //if (enemies.Count > 0)
        //{
        //    for (int i = 0; i < enemies.Count; i++)
        //    {
        //        if (enemies[i].uniqueAi.enabled)
        //            return;
        //    }
        //    AudioManager.instance.StartCoroutine("LowerBattleMusic");
        //}
        //else
        //{
        //    Debug.Log("You WIN");
        //}
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