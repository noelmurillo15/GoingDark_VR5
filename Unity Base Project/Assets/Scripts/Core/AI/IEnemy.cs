using UnityEngine;
using GD.Core.Enums;

public class IEnemy : MonoBehaviour
{

    #region Properties
    public Transform MyTransform { get; private set; }    

    public EnemyTypes Type;
    public EnemyDifficulty Level = EnemyDifficulty.NONE;
    public int MissileCount;

    private MovementStats MoveData;
    #endregion


    public virtual void Initialize()
    {
        MyTransform = transform;
        Type = EnemyTypes.NONE;
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
                if (Level == EnemyDifficulty.EASY)
                {
                    MoveData.Acceleration = 6f;
                    MoveData.RotateSpeed = 3f;
                    MoveData.MaxSpeed = 80f;
                }
                else if(Level == EnemyDifficulty.MED)
                {
                    MoveData.Acceleration = 8f;
                    MoveData.RotateSpeed = 2.5f;
                    MoveData.MaxSpeed = 120f;
                }
                else if (Level == EnemyDifficulty.HARD)
                {
                    MoveData.Acceleration = 10f;
                    MoveData.RotateSpeed = 2f;
                    MoveData.MaxSpeed = 150f;
                }
                break;
            case "Trident":
                SetEnemyType(EnemyTypes.TRIDENT);                
                if (Level == EnemyDifficulty.EASY)
                {
                    MoveData.Acceleration = 5f;
                    MoveData.RotateSpeed = 4f;
                    MoveData.MaxSpeed = 70f;
                }
                else if (Level == EnemyDifficulty.MED)
                {
                    MoveData.Acceleration = 7f;
                    MoveData.RotateSpeed = 3.5f;
                    MoveData.MaxSpeed = 90f;
                }
                else if (Level == EnemyDifficulty.HARD)
                {
                    MoveData.Acceleration = 9f;
                    MoveData.RotateSpeed = 3f;
                    MoveData.MaxSpeed = 120f;
                }
                break;
            case "BasicEnemy":
                SetEnemyType(EnemyTypes.BASIC);
                MissileCount = 10;
                if (Level == EnemyDifficulty.EASY)
                {
                    MoveData.Acceleration = 4f;
                    MoveData.RotateSpeed = 5f;
                    MoveData.MaxSpeed = 60f;
                }
                else if (Level == EnemyDifficulty.MED)
                {
                    MoveData.Acceleration = 6f;
                    MoveData.RotateSpeed = 4.5f;
                    MoveData.MaxSpeed = 90f;
                }
                else if (Level == EnemyDifficulty.HARD)
                {
                    MoveData.Acceleration = 8f;
                    MoveData.RotateSpeed = 4f;
                    MoveData.MaxSpeed = 110f;
                }                               
                break;
            case "Transport":
                SetEnemyType(EnemyTypes.TRANSPORT);
                if (Level == EnemyDifficulty.EASY)
                {
                    MoveData.Acceleration = 5f;
                    MoveData.RotateSpeed = 6f;
                    MoveData.MaxSpeed = 100f;
                }
                else if (Level == EnemyDifficulty.MED)
                {
                    MoveData.Acceleration = 8f;
                    MoveData.RotateSpeed = 5f;
                    MoveData.MaxSpeed = 150f;
                }
                else if (Level == EnemyDifficulty.HARD)
                {
                    MoveData.Acceleration = 12f;
                    MoveData.RotateSpeed = 4.5f;
                    MoveData.MaxSpeed = 200f;
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