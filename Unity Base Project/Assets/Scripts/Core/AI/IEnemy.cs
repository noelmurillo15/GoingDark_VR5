using UnityEngine;
using GD.Core.Enums;

public class IEnemy : MonoBehaviour
{

    #region Properties
    public EnemyTypes Type = EnemyTypes.NONE;
    public EnemyDifficulty Level = EnemyDifficulty.NONE;

    public int MissileCount;
    private MovementStats MoveData;
    public Transform MyTransform { get; private set; }    
    #endregion


    public virtual void Initialize()
    {
        MyTransform = transform;
        MissileCount = 0;
        LoadEnemyData();
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
        Debug.Log("Setting Enemy Type : " + _type);
        Type = _type;
    }    
    #endregion

    #region Msg Functions
    public void EMPHit()
    {
        Debug.Log("Enemy has been hit by Emp");
    }

    public void Hit()
    {
        Debug.Log("Enemy has creashed with an Asteroid");
    }

    public void Kill()
    {
        Debug.Log("Enemy has been Destroyed");
        Destroy(gameObject);
    }
    #endregion

    #region Public Methods
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
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 100f;
                        break;
                    case EnemyDifficulty.MED:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 120f;
                        break;
                    case EnemyDifficulty.HARD:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 160f;
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
                        break;
                    case EnemyDifficulty.MED:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 120f;
                        break;
                    case EnemyDifficulty.HARD:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 160f;
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
                        break;
                    case EnemyDifficulty.MED:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 120f;
                        break;
                    case EnemyDifficulty.HARD:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 180f;
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
                        break;
                    case EnemyDifficulty.MED:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 140f;
                        break;
                    case EnemyDifficulty.HARD:
                        MoveData.Acceleration = 25f;
                        MoveData.RotateSpeed = 2f;
                        MoveData.MaxSpeed = 200f;
                        break;
                    case EnemyDifficulty.NIGHTMARE:
                        break;
                }
                break;
            case "Boss":
                SetEnemyType(EnemyTypes.BOSS);
                MissileCount = 100;
                MoveData.Acceleration = 2f;
                MoveData.RotateSpeed = 10f;
                MoveData.MaxSpeed = 50f;
                break;


            default:
                Debug.Log("Invalid Enemy Name : " + transform.name);
                break;
        }
    }
    #endregion 
}