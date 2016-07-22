using UnityEngine;
using GoingDark.Core.Enums;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

    #region Properties
    public GameDifficulty Difficulty;
    public Transform PlayerPosition { get; private set; }

    private List<EnemyStateManager> enemies = new List<EnemyStateManager>();

    private CloakSystem pCloak;
    private MissionSystem missionSystem;
    private SystemManager systemManager;
    private ObjectPoolManager poolmanager;
    #endregion


    void Start()
    {
        missionSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionSystem>();
        poolmanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();
        systemManager = GameObject.Find("Devices").GetComponent<SystemManager>();
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("CheckEnemies", 20f, 5f);
    }

    #region Accessors
    public Transform GetPlayerTransform()
    {
        return PlayerPosition;
    }
    public ObjectPoolManager GetObjectPoolManager()
    {
        return poolmanager;
    }
    public CloakSystem GetPlayerCloak()
    {
        if (systemManager == null)
        {
            Debug.LogError("System Manager was not found @ start... Finding it again");
            systemManager = GameObject.Find("Devices").GetComponent<SystemManager>();
        }

        if (pCloak == null)
        {
            Debug.LogError("Player Cloak was not found @ start... Finding it again");
            pCloak = systemManager.GetSystemScript(SystemType.Cloak) as CloakSystem;
        }

        return pCloak;
    }
    #endregion    

    #region Modifiers
    public void AddEnemy(EnemyStateManager enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyStateManager enemy)
    {
        GameObject explosive = poolmanager.GetEnemyExplosion();
        explosive.transform.parent = missionSystem.transform;
        explosive.transform.position = enemy.transform.position;
        explosive.SetActive(true);

        RandomAmmoDrop(enemy.transform.position);
        missionSystem.KilledEnemy(enemy.Type);

        enemies.Remove(enemy);
    }

    public void PlayerSeen()
    {
        missionSystem.PlayerSeen();
    }

    public void SendAlert(Vector3 enemypos)
    {
        AudioManager.instance.StartCoroutine("RaiseBattleMusic");
        object[] tempStorage = new object[2];
        tempStorage[0] = PlayerPosition.position;
        tempStorage[1] = enemypos;
        BroadcastMessage("BroadcastAlert", tempStorage);
    }

    public void RandomAmmoDrop(Vector3 _pos)
    {
        GameObject go = null;
        if (Random.Range(1, 3) == 1)
        {
            go = poolmanager.GetAmmoDrop();
            go.transform.position = _pos;
            go.transform.rotation = Quaternion.identity;
            go.SetActive(true);
        }
    }
    #endregion

    #region Msg Calls
    void CheckEnemies()
    {
        if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].State == EnemyStates.Attack)
                    return;
            }
            AudioManager.instance.StartCoroutine("LowerBattleMusic");
        }
    }
    #endregion
}