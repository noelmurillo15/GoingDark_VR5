using UnityEngine;
using GoingDark.Core.Enums;

public class IEnemy : MonoBehaviour
{

    #region Properties
    public Transform Target { get; protected set; }

    public EnemyTypes Type = EnemyTypes.None;
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
        LoadEnemyData();
    }
    
    void EMPHit()
    {
        Debug.Log("Enemy Was Emp'd");
        stunned.SetActive(true);
        Debuff = Impairments.Stunned;
        Invoke("ResetDebuff", 3f);
        if (Type == EnemyTypes.Droid)
            Invoke("Kill", 3f);
    }
    void ShieldHit(float _val)
    {
        Debug.Log("Enemy Took Shield Dmg");
        ShieldData.TakeDamage(_val);        
    }
    public void Hit(Missile missile)
    {        
        if (ShieldData.GetShieldActive())
        {
            switch (missile.Type)
            {
                case MissileType.Basic:
                    missile.Deflect();
                    break;
                case MissileType.Emp:
                    EMPHit();
                    ShieldHit(20f);
                    missile.Kill();
                    break;
                case MissileType.Chromatic:
                    missile.Deflect();
                    break;
                case MissileType.ShieldBreak:
                    ShieldHit(100f);
                    missile.Kill();
                    break;
            }
        }
        else
        {
            switch (missile.Type)
            {
                case MissileType.Basic:
                    Damage(5);
                    break;
                case MissileType.Emp:
                    Damage(1);
                    EMPHit();
                    break;
                case MissileType.ShieldBreak:
                    Damage(1);
                    break;
                case MissileType.Chromatic:
                    Damage(10);
                    break;
            }
            missile.Kill();
        }
    }
    public void Hit(LaserProjectile laser)
    {
        if (ShieldData.GetShieldActive())
        {
            ShieldHit(5f);
        }
        else
        {
            Damage(1);
        }
        laser.Kill();
    }    
    void Damage(int _val)
    {
        Health -= _val;
        if (Health <= 0)
            Kill();
    }
    void Kill()
    {
        Debug.Log("Enemy has been Destroyed");
        GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>().KilledEnemy(Type);
        GameObject explosive = manager.GetEnemyExplosion();
        explosive.transform.position = transform.position;        
        explosive.SetActive(true);

        if (manager.RandomChance())
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
        switch (manager.Difficulty)
        {
            #region Easy
            case GameDifficulty.Easy:
                switch (Type)
                {
                    case EnemyTypes.Basic:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 60f, 2f, 10f);
                        MissileCount = 10;
                        Health = 10;
                        break;
                    case EnemyTypes.Droid:
                        MoveData.Set(0f, .5f, 110f, 1f, 10f);
                        Health = 5;
                        break;
                    case EnemyTypes.Transport:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 120f, 3f, 10f);
                        Health = 15;
                        break;
                    case EnemyTypes.Trident:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 80f, 1.8f, 10f);
                        MissileCount = 10;
                        Health = 10;
                        break;
                    case EnemyTypes.Boss:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 50f, 5f, 10f);
                        MissileCount = 1000;
                        Health = 25;
                        break;
                }
                break;
            #endregion
            #region Normal
            case GameDifficulty.Normal:
                switch (Type)
                {
                    case EnemyTypes.Basic:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 90f, 1.8f, 15f);
                        MissileCount = 10;
                        Health = 25;
                        break;
                    case EnemyTypes.Droid:
                        MoveData.Set(0f, .5f, 120f, .8f, 20f);
                        Health = 10;
                        break;
                    case EnemyTypes.Transport:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 150f, 2.5f, 15f);
                        Health = 30;
                        break;
                    case EnemyTypes.Trident:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MissileCount = 10;
                        MoveData.Set(0f, .5f, 95f, 1.5f, 18f);
                        Health = 20;
                        break;
                    case EnemyTypes.Boss:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MissileCount = 1000;
                        MoveData.Set(0f, .5f, 60f, 4f, 15f);
                        Health = 60;
                        break;
                }
                break;
            #endregion
            #region Hard
            case GameDifficulty.Hard:
                switch (Type)
                {
                    case EnemyTypes.Basic:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MissileCount = 10;
                        MoveData.Set(0f, .5f, 110f, 1.6f, 25f);
                        Health = 50;
                        break;
                    case EnemyTypes.Droid:
                        MoveData.Set(0f, .5f, 160f, .7f, 40f);
                        Health = 15;
                        break;
                    case EnemyTypes.Transport:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 200f, 2f, 25f);
                        Health = 75;
                        break;
                    case EnemyTypes.Trident:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MissileCount = 10;
                        MoveData.Set(0f, .5f, 120f, 1.2f, 30f);
                        Health = 40;
                        break;
                    case EnemyTypes.Boss:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 50f, 5f, 10f);
                        MissileCount = 1000;
                        Health = 25;
                        break;
                }
                break;
            #endregion
            #region Nightmare
            case GameDifficulty.Nightmare:
                switch (Type)
                {
                    case EnemyTypes.Basic:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MissileCount = 10;
                        MoveData.Set(0f, .5f, 150f, 1.4f, 40f);
                        Health = 100;
                        break;
                    case EnemyTypes.Droid:
                        MoveData.Set(0f, .5f, 200f, .6f, 50f);
                        Health = 30;
                        break;
                    case EnemyTypes.Transport:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 250f, 1.8f, 30f);
                        Health = 120;
                        break;
                    case EnemyTypes.Trident:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MissileCount = 10;
                        MoveData.Set(0f, .5f, 160f, 1f, 45f);
                        Health = 80;
                        break;
                    case EnemyTypes.Boss:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 50f, 5f, 10f);
                        MissileCount = 1000;
                        Health = 25;
                        break;
                }
                break;
            #endregion
        }

        //    case EnemyTypes.Boss:
        //        switch (Level)
        //        {
        //            case GameDifficulty.Hard:
        //                MoveData.Set(0f, .5f, 80f, 3f, 20f);
        //                Health = 125;
        //                break;
        //            case GameDifficulty.Nightmare:
        //                MoveData.Set(0f, .5f, 120f, 2f, 30f);
        //                Health = 200;
        //                break;
        //        }
        //        break;
        //    default:
        //        Debug.LogError("Enemy's Tag doesn't match");
        //        break;
        //}     
    }
    #endregion 
}