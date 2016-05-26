using UnityEngine;
using GD.Core.Enums;

public class IEnemy : MonoBehaviour
{

    #region Properties
    public EnemyTypes Type = EnemyTypes.NONE;
    public EnemyDifficulty Level = EnemyDifficulty.NONE;
    public Impairments Debuff = Impairments.NONE;

    public int Health;
    public int MissileCount;
    private MovementStats MoveData;
    private GameObject explosion;
    private GameObject ammoDrop;

    public Transform MyTransform { get; private set; }    
    #endregion


    public virtual void Initialize()
    {
        Health = 0;
        MissileCount = 0;
        MyTransform = transform;
        LoadEnemyData();
        explosion = Resources.Load<GameObject>("EnemyExplosion");
        ammoDrop = Resources.Load<GameObject>("AmmoDrop");
    }

    #region Accessors
    public MovementStats GetMoveData()
    {
        return MoveData;
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
        Debug.Log("Enemy has been stunned by Emp");
        Debuff = Impairments.STUNNED;
        Invoke("ResetDebuff", 10f);
        if (Type == EnemyTypes.KAMIKAZE)        
            Hit();        
    }    

    void ShieldHit()
    {
        Debug.Log("Enemy Shield Hit");
    }

    void Hit()
    {
        Debug.Log("Enemy was Hit");
        Health--;
        if (Health <= 0)
            Kill();
    }

    void Kill()
    {
        Debug.Log("Enemy has been Destroyed");
        Instantiate(explosion, transform.position, Quaternion.identity);
        Instantiate(ammoDrop, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    #endregion

    #region Public Methods
    public void ResetDebuff()
    {
        SetSpeedBoost(.5f);
        Debuff = Impairments.NONE;
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
        if(Level == EnemyDifficulty.NONE)
        {
            Debug.LogError("Enemy's does not have a difficulty");
            return;
        }
        
        MoveData.Speed = 0f;
        MoveData.Boost = 1f;
        switch (transform.name)
        {
            case "Droid":
                SetEnemyType(EnemyTypes.KAMIKAZE);
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
                SetEnemyType(EnemyTypes.TRIDENT);
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
                SetEnemyType(EnemyTypes.BASIC);
                MissileCount = 20;
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
                SetEnemyType(EnemyTypes.TRANSPORT);
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
                SetEnemyType(EnemyTypes.BOSS);
                MissileCount = 100;
                MoveData.Acceleration = 10f;
                MoveData.RotateSpeed = 6f;
                MoveData.MaxSpeed = 60f;
                Health = 10;
                break;


            default:
                Debug.Log("Invalid Enemy Name : " + transform.name);
                break;
        }
    }
    #endregion 
}