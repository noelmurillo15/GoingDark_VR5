using UnityEngine;
using GoingDark.Core.Enums;

public class IEnemy : MonoBehaviour
{

    #region Properties
    public EnemyTypes Type = EnemyTypes.None;
    public EnemyDifficulty Level = EnemyDifficulty.NONE;
    public Impairments Debuff = Impairments.None;

    public int Health;
    public int MissileCount;

    public MovementProperties MoveData;
    public ShieldProperties ShieldData;

    Rigidbody rb;

    private GameObject explosion;
    private GameObject ammoDrop;
    private GameObject stunned;

    private EnemyManager manager;
    public Transform MyTransform { get; private set; }
    #endregion


    public virtual void Initialize()
    {
        Health = 0;
        MissileCount = 0;
        MyTransform = transform;
        LoadEnemyData();
        explosion = Resources.Load<GameObject>("Projectiles/Explosions/EnemyExplosion");
        ammoDrop = Resources.Load<GameObject>("AmmoDrop");
        stunned = transform.GetChild(1).gameObject;
        stunned.SetActive(false);
        rb = GetComponent<Rigidbody>();


        manager = transform.parent.GetComponent<EnemyManager>();
        manager.AddEnemy(MyTransform.GetComponent<EnemyBehavior>());
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
            Debug.Log("Enemy shield was Hit");
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

        Debug.Log("Enemy was Hit(Missile)");
        missile.Kill();

        Health--;
        if (Health <= 0)
            Kill();
    }

    void Hit()
    {
        Debug.Log("Enemy was Hit()");

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
        Instantiate(explosion, transform.position, Quaternion.identity);
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
    public void IncreaseSpeed()
    {
        if (MoveData.Speed < (MoveData.MaxSpeed * MoveData.Boost))
            MoveData.Speed += Time.deltaTime * MoveData.Acceleration;
        else if (MoveData.Speed > (MoveData.MaxSpeed * MoveData.Boost) + .5f)
            DecreaseSpeed();
    }
    public void DecreaseSpeed()
    {
        if (MoveData.Speed > 0.0f)
            MoveData.Speed -= Time.deltaTime * MoveData.Acceleration * 4f;
        else
            MoveData.Speed = 0.0f;
    }
    #endregion

    #region Private Methods
    void LoadEnemyData()
    {
        if (Level == EnemyDifficulty.NONE)
        {
            Debug.LogError("Enemy's does not have a difficulty");
            return;
        }

        MoveData.Speed = 0f;
        MoveData.Boost = 1f;
        switch (transform.name)
        {
            case "Droid":
                SetEnemyType(EnemyTypes.Droid);
                switch (Level)
                {
                    case EnemyDifficulty.EASY:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = .5f;
                        MoveData.MaxSpeed = 100f;
                        Health = 1;
                        break;
                    case EnemyDifficulty.MED:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 1f;
                        MoveData.MaxSpeed = 120f;
                        Health = 2;
                        break;
                    case EnemyDifficulty.HARD:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 1f;
                        MoveData.MaxSpeed = 160f;
                        Health = 3;
                        break;
                    case EnemyDifficulty.NIGHTMARE:
                        break;
                }
                break;
            case "Trident":
                SetEnemyType(EnemyTypes.Trident);
                MissileCount = 20;
                switch (Level)
                {
                    case EnemyDifficulty.EASY:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 95f;
                        Health = 2;
                        break;
                    case EnemyDifficulty.MED:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 120f;
                        Health = 3;
                        break;
                    case EnemyDifficulty.HARD:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 160f;
                        Health = 4;
                        break;
                    case EnemyDifficulty.NIGHTMARE:
                        break;
                }
                break;
            case "BasicEnemy":
                SetEnemyType(EnemyTypes.Basic);
                MissileCount = 20;
                ShieldData.Initialize(transform.GetChild(0).gameObject);
                switch (Level)
                {
                    case EnemyDifficulty.EASY:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 90f;
                        Health = 3;
                        break;
                    case EnemyDifficulty.MED:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 120f;
                        Health = 4;
                        break;
                    case EnemyDifficulty.HARD:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 180f;
                        Health = 5;
                        break;
                    case EnemyDifficulty.NIGHTMARE:
                        break;
                }
                break;
            case "Transport":
                SetEnemyType(EnemyTypes.Transport);
                switch (Level)
                {
                    case EnemyDifficulty.EASY:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 120f;
                        Health = 3;
                        break;
                    case EnemyDifficulty.MED:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 140f;
                        Health = 4;
                        break;
                    case EnemyDifficulty.HARD:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 200f;
                        Health = 5;
                        break;
                    case EnemyDifficulty.NIGHTMARE:
                        break;
                }
                break;
            case "Boss":
                SetEnemyType(EnemyTypes.Boss);
                ShieldData.Initialize(transform.GetChild(0).gameObject);
                MissileCount = 1000;
                MoveData.Acceleration = 15f;
                MoveData.RotateSpeed = 5f;
                MoveData.MaxSpeed = 60f;
                Health = 10;
                break;


            default:
                Debug.LogError("Invalid Enemy Name : " + transform.name);
                break;
        }
    }
    #endregion 
}