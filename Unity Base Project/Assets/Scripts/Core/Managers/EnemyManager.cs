using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    
    #region Properties
    public Transform PlayerPosition { get; private set; }
    private MissionSystem missionSystem;

    private List<EnemyBehavior> enemies = new List<EnemyBehavior>();
    private List<Transform> enemypositions = new List<Transform>();
    #endregion

    void Awake()
    {
        InvokeRepeating("CheckEnemies", 10f, 5f);          
        missionSystem = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
    }

    void Start()
    {
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    public void AddEnemy(EnemyBehavior enemy)
    {
        enemypositions.Add(enemy.MyTransform);
        enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyBehavior enemy)
    {
        enemypositions.Remove(enemy.MyTransform);
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

    public void TargetDestroyed()
    {
        BroadcastMessage("BroadcastWin");
    }
}