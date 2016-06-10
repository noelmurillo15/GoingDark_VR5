using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    
    #region Properties
    private  List<GameObject> enemies = new List<GameObject>();

    public Transform PlayerPosition { get; private set; }

    private MissionSystem missionSystem;
    #endregion

    void Awake()
    {
        InvokeRepeating("CheckEnemies", 5f, 5f);          
        missionSystem = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
    }

    void Start()
    {
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void SendAlert(Vector3 enemypos)
    {
        Debug.Log("Alert has been sent");
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
                if (enemies[i] != null)
                {
                    if (enemies[i].GetComponent<EnemyBehavior>().uniqueAi.enabled)
                        return;
                }
                else
                {
                    enemies.Remove(enemies[i]);
                    enemies.Sort();
                    CheckEnemies();
                    return;
                }
            }
            AudioManager.instance.StartCoroutine("LowerBattleMusic");
        }
    }

    public void TargetDestroyed()
    {
        BroadcastMessage("BroadcastWin");
    }
}