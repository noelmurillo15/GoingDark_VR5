using UnityEngine;
using GoingDark.Core.Enums;

public class IEnemy : MonoBehaviour
{

    #region Properties
    public Transform Target { get; protected set; }

    public EnemyTypes Type = EnemyTypes.None;
    public GameDifficulty Level = GameDifficulty.Easy;
    public Impairments Debuff = Impairments.None;

    public int Health;
    public int MissileCount;

    public MovementProperties MoveData;
    public ShieldProperties ShieldData;

    private GameObject ammoDrop;
    private GameObject stunned;

    public EnemyManager manager;
    public Transform MyTransform { get; private set; }
    #endregion


    public virtual void Initialize()
    {
        Target = null;
        Health = 0;
        MissileCount = 0;
        MyTransform = transform;

        if (transform.name.Contains("Droid"))
            transform.name = "Droid";
        else if (transform.name.Contains("BasicEnemy"))
            transform.name = "BasicEnemy";
        
        ammoDrop = Resources.Load<GameObject>("AmmoDrop");
        stunned = transform.GetChild(1).gameObject;
        stunned.SetActive(false);


        Invoke("AddToManager", 1f);
    }

    #region Accessors
    public MovementProperties GetMoveData()
    {
        return MoveData;
    }
    public ShieldProperties GetShieldData()
    {
        return ShieldData;
    }
    #endregion

    #region Modifiers
    public void SetEnemyType(EnemyTypes _type)
    {
        Type = _type;
    }
    #endregion

    #region Msg Functions

    void AddToManager()
    {
        manager = transform.parent.GetComponent<EnemyManager>();
        manager.AddEnemy(MyTransform.GetComponent<EnemyBehavior>());
        GetComponent<EnemyCollision>().SetManagerRef(manager);

        Level = manager.Difficulty;

        LoadEnemyData();
    }
    void EMPHit()
    {
        stunned.SetActive(true);

        Debuff = Impairments.Stunned;
        Invoke("ResetDebuff", 5f);

        if (Type == EnemyTypes.Droid)
            Invoke("Kill", 5f);
    }

    void ShieldHit()
    {    
        if (ShieldData.GetShieldActive())
        {
            ShieldData.TakeDamage();
        }
        else
            Hit();        
    }

    void Hit(Missile missile)
    {
        if (ShieldData.GetShieldActive())
        {
            missile.Deflect();
            return;
        }

        missile.Kill();

        Health--;
        if (Health <= 0)
            Kill();
    }

    void Hit()
    {
        Health--;
        if (Health <= 0)
            Kill();
    }    

    private bool RandomChance()
    {
        float wDrop = Random.Range(1, 3);
        if (wDrop == 1)
            return true;

        return false;
    }
    void Kill()
    {
        Debug.Log("Enemy has been Destroyed");
        GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>().KilledEnemy(Type);
        GameObject explosive = manager.GetEnemyExplosion();
        explosive.transform.position = transform.position;        
        explosive.SetActive(true);
        if (RandomChance())
            Instantiate(ammoDrop, transform.position, Quaternion.identity);
        manager.RemoveEnemy(MyTransform.GetComponent<EnemyBehavior>());
        Destroy(gameObject);
    }
    #endregion

    #region Public Methods
    public void ResetDebuff()
    {
        SetSpeedBoost(1f);
        Debuff = Impairments.None;
        stunned.SetActive(false);
    }
    public void StopMovement()
    {
        MoveData.Speed = 0f;
    }
    public void DecreaseMissileCount()
    {
        MissileCount--;
    }
    public void SetSpeedBoost(float newBoost)
    {
        MoveData.Boost = newBoost;
    }
    #endregion

    #region Private Methods
    void LoadEnemyData()
    {
        switch (Type)
        {
            case EnemyTypes.Basic:
                ShieldData.Initialize(transform.GetChild(0).gameObject);
                MissileCount = 20;
                switch (Level)
                {
                    case GameDifficulty.Easy:
                        MoveData.Set(0f, .5f, 60f, 2f, 10f);
                        Health = 2;
                        break;
                    case GameDifficulty.Normal:
                        MoveData.Set(0f, .5f, 90f, 1.8f, 15f);
                        Health = 3;
                        break;
                    case GameDifficulty.Hard:
                        MoveData.Set(0f, .5f, 110f, 1.6f, 25f);
                        Health = 5;
                        break;
                    case GameDifficulty.Nightmare:
                        MoveData.Set(0f, .5f, 150f, 1.4f, 40f);
                        Health = 8;
                        break;
                }
                break;
            case EnemyTypes.Droid:
                switch (Level)
                {
                    case GameDifficulty.Easy:
                        MoveData.Set(0f, .5f, 110f, 1f, 10f);
                        Health = 1;
                        break;
                    case GameDifficulty.Normal:
                        MoveData.Set(0f, .5f, 120f, .8f, 20f);
                        Health = 2;
                        break;
                    case GameDifficulty.Hard:
                        MoveData.Set(0f, .5f, 160f, .7f, 40f);
                        Health = 3;
                        break;
                    case GameDifficulty.Nightmare:
                        MoveData.Set(0f, .5f, 200f, .6f, 50f);
                        Health = 5;
                        break;
                }
                break;
            case EnemyTypes.Transport:
                ShieldData.Initialize(transform.GetChild(0).gameObject);
                switch (Level)
                {
                    case GameDifficulty.Easy:
                        MoveData.Set(0f, .5f, 120f, 3f, 10f);
                        Health = 3;
                        break;
                    case GameDifficulty.Normal:
                        MoveData.Set(0f, .5f, 150f, 2.5f, 15f);
                        Health = 4;
                        break;
                    case GameDifficulty.Hard:
                        MoveData.Set(0f, .5f, 200f, 2f, 25f);
                        Health = 5;
                        break;
                    case GameDifficulty.Nightmare:
                        MoveData.Set(0f, .5f, 250f, 1.8f, 30f);
                        break;
                }
                break;
            case EnemyTypes.Trident:
                ShieldData.Initialize(transform.GetChild(0).gameObject);
                MissileCount = 20;
                switch (Level)
                {
                    case GameDifficulty.Easy:
                        MoveData.Set(0f, .5f, 80f, 1.8f, 10f);
                        Health = 1;
                        break;
                    case GameDifficulty.Normal:
                        MoveData.Set(0f, .5f, 95f, 1.5f, 18f);
                        Health = 2;
                        break;
                    case GameDifficulty.Hard:
                        MoveData.Set(0f, .5f, 120f, 1.2f, 30f);
                        Health = 4;
                        break;
                    case GameDifficulty.Nightmare:
                        MoveData.Set(0f, .5f, 160f, 1f, 45f);
                        Health = 4;
                        break;
                }                
                break;
            case EnemyTypes.Boss:
                ShieldData.Initialize(transform.GetChild(0).gameObject);
                MoveData.Set(0f, .5f, 50f, 5f, 10f);
                MissileCount = 1000;
                Health = 50;
                break;
            default:
                Debug.LogError("Enemy's Tag doesn't match");
                break;
        }     
    }
    #endregion 
}