using UnityEngine;
using GoingDark.Core.Enums;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

    #region Properties
    public GameDifficulty Difficulty;

    public Transform PlayerPosition { get; private set; }

    private List<EnemyStateManager> enemies = new List<EnemyStateManager>();

    private ObjectPoolManager poolmanager;

    private CloakSystem pCloak;
    private MissionSystem missionSystem;
    private SystemManager systemManager;
    #endregion

    void Awake()
    {
        PlayerPosition = GameObject.Find("PlayerTutorial").transform;
        systemManager = GameObject.Find("Devices").GetComponent<SystemManager>();
        missionSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionSystem>();
        poolmanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();
    }

    void Start()
    {
        

        pCloak = systemManager.GetSystemScript(SystemType.Cloak) as CloakSystem;
        InvokeRepeating("CheckEnemies", 20f, 5f);
    }

    #region Accessors
    public CloakSystem GetPlayerCloak()
    {
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
        if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].State == EnemyStates.Attack)
                    return;
            }
            AudioManager.instance.StartCoroutine("LowerBattleMusic");
        }
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
            go = poolmanager.GetAmmoDrop();
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