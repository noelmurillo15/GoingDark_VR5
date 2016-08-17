using UnityEngine;
using GoingDark.Core.Enums;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

    #region Properties
    public GameDifficulty Difficulty;

    private List<IEnemy> enemies = new List<IEnemy>();

    private int creditMultiplier;

    private Transform PlayerPosition;
    private MissionSystem missionSystem;
    private ObjectPoolManager poolmanager;
    private TallyScreen tallyscreen;
    #endregion


    void Start()
    {
        switch (PlayerPrefs.GetString("Difficulty"))
        {
            case "Easy":
                Difficulty = GameDifficulty.Easy;
                creditMultiplier = 1;
                break;
            case "Medium":
                Difficulty = GameDifficulty.Normal;
                creditMultiplier = 2;
                break;
            case "Hard":
                Difficulty = GameDifficulty.Hard;
                creditMultiplier = 3;
                break;
            case "Nightmare":
                Difficulty = GameDifficulty.Nightmare;
                creditMultiplier = 5;
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
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    #region Accessors
    public GameDifficulty GetGameDifficulty()
    {
        return Difficulty;
    }
    public Transform GetPlayerTransform()
    {
        return PlayerPosition;
    }
    #endregion    

    #region Modifiers
    public void AddEnemy(IEnemy enemy)
    {
        enemies.Add(enemy);
    }
    public void RemoveEnemy(IEnemy enemy)
    {
        GameObject explosive = poolmanager.GetEnemyExplosion();

        if (explosive != null)
        {
            explosive.transform.parent = missionSystem.transform;
            explosive.transform.position = enemy.transform.position;
            explosive.SetActive(true);
        }

        RandomAmmoDrop(enemy.transform.position);

        if(missionSystem != null)
            missionSystem.KilledEnemy(enemy.GetEnemyType());

        int creds = 0;         
        switch (enemy.GetEnemyType())
        {
            case EnemyTypes.Basic:
                creds += 20 * creditMultiplier;
                break;
            case EnemyTypes.Droid:
                creds += 5 * creditMultiplier;
                break;
            case EnemyTypes.SquadLead:
                creds += 50 * creditMultiplier;
                break;
            case EnemyTypes.JetFighter:
                creds += 10 * creditMultiplier;
                break;
            case EnemyTypes.Transport:
                creds += 250 * creditMultiplier;
                break;
            case EnemyTypes.Trident:
                creds += 15 * creditMultiplier;
                break;
            case EnemyTypes.Tank:
                creds += 300 * creditMultiplier;
                break;
            case EnemyTypes.FinalBoss:
                creds += 500 * creditMultiplier;
                break;
        }
        PlayerPosition.SendMessage("UpdateCredits", creds);

        if (tallyscreen != null)
            tallyscreen.EnemiesKilled += 1;

        enemies.Remove(enemy);
    }

    public void AllEnemiesPatrol()
    {
        for (int x = 0; x < enemies.Count; x++)
            enemies[x].GetStateManager().SetEnemyTarget(null);
    }

    public void PlayerSeen()
    {
        if(missionSystem != null)
            missionSystem.PlayerSeen();
    }
    public void SendAlert(Vector3 enemypos)
    {
        object[] tempStorage = new object[2];
        tempStorage[0] = PlayerPosition.position;
        tempStorage[1] = enemypos;
        BroadcastMessage("BroadcastAlert", tempStorage);
    }
    void RandomAmmoDrop(Vector3 _pos)
    {
        if (Random.Range(1, 2) == 1)
        {
            GameObject go = poolmanager.GetAmmoDrop();
            go.transform.position = _pos;
            go.transform.rotation = Quaternion.identity;
            go.SetActive(true);
        }
    }
    #endregion
}