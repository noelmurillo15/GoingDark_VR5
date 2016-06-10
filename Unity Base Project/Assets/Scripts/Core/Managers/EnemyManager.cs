﻿using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    
    #region Properties
    private  List<GameObject> enemies = new List<GameObject>();
    #endregion

    void Awake()
    {
       InvokeRepeating("CheckEnemies", 5f, 5f);    
    }
    
    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }


    public void FoundTarget(Vector3 _target, Vector3 enemypos)
    {
        AudioManager.instance.StartCoroutine("RaiseBattleMusic");
        object[] tempStorage = new object[2];
        tempStorage[0] = _target;
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