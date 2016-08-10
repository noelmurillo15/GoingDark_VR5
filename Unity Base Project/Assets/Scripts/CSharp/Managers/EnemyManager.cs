using UnityEngine;
using GoingDark.Core.Enums;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

    #region Properties
    public GameDifficulty Difficulty;

    private List<EnemyStateManager> enemies = new List<EnemyStateManager>();

    private Transform PlayerPosition;
    private MissionSystem missionSystem;
    private SystemManager systemManager;
    private ObjectPoolManager poolmanager;
    private TallyScreen tallyscreen;
    #endregion


    void Start()
    {
        switch (PlayerPrefs.GetString("Difficulty"))
        {
            case "Easy":
                Difficulty = GameDifficulty.Easy;
                break;
            case "Medium":
                Difficulty = GameDifficulty.Normal;
                break;
            case "Hard":
                Difficulty = GameDifficulty.Hard;
                break;
            case "Nightmare":
                Difficulty = GameDifficulty.Nightmare;
                break;
            default:
                Debug.Log("Enemy Manager could not get Level Difficulty");
                Difficulty = GameDifficulty.Easy;
                break;
        }
        Debug.Log("Game Difficulty : " + Difficulty.ToString());

        poolmanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();
        missionSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionSystem>(); 
        tallyscreen = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TallyScreen>();
        systemManager = GameObject.Find("Devices").GetComponent<SystemManager>();
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating("CheckEnemies", 10f, 10f);
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
    #endregion    

    #region Modifiers
    public void AddEnemy(EnemyStateManager enemy)
    {
        enemies.Add(enemy);
    }
    public void RemoveEnemy(EnemyStateManager enemy)
    {
        GameObject explosive = poolmanager.GetEnemyExplosion();

        if(missionSystem != null)
            explosive.transform.parent = missionSystem.transform;
        explosive.transform.position = enemy.transform.position;
        explosive.SetActive(true);

        RandomAmmoDrop(enemy.transform.position);

        if(missionSystem != null)
            missionSystem.KilledEnemy(enemy.Type);

        if(tallyscreen != null)
            tallyscreen.EnemiesKilled += 1;

        enemies.Remove(enemy);
    }


    public void PlayerSeen()
    {
        if(missionSystem != null)
            missionSystem.PlayerSeen();
    }
    public void SendAlert(Vector3 enemypos)
    {
        AudioManager.instance.RaiseBattleMusic();
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

    #region Invoked Methods
    void CheckEnemies()
    {
        if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].State == EnemyStates.Attack)
                    return;
            }
            AudioManager.instance.LowerBattleMusic();
        }
    }
    #endregion
}