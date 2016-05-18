using UnityEngine;
using GD.Core.Enums;

public class IEnemy : MonoBehaviour
{

    #region Properties
    public EnemyTypes Type;    
    public int MissileCount;
    public Transform Target { get; protected set; }
    public Transform MyTransform { get; protected set; }

    private MovementStats MoveData;
    #endregion


    public virtual void Initialize()
    {
        Debug.Log("IEnemy Initializing...");
        MyTransform = transform;
        Type = EnemyTypes.NONE;       
        MissileCount = 0;
        Target = null;

        Init(transform.name);
        Debug.Log("IEnemy READY");
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
    public void SetEnemyTarget(Transform _target)
    {
        Debug.Log("Setting Enemy Target : " + _target.name);
        Target = _target;
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
    private void Init(string enemyName)
    {
        MoveData.Speed = 0f;
        MoveData.Boost = 1f;
        MoveData.MaxSpeed = 50f;
        MoveData.RotateSpeed = 2f;
        MoveData.Acceleration = 5f;
        switch (enemyName)
        {
            case "Droid":
                SetEnemyType(EnemyTypes.KAMIKAZE);
                MoveData.Acceleration = 6f;
                MoveData.RotateSpeed = 2f;
                MoveData.MaxSpeed = 75f;
                break;
            case "Trident":
                SetEnemyType(EnemyTypes.TRIDENT);
                MoveData.Acceleration = 4f;
                MoveData.RotateSpeed = 3f;
                MoveData.MaxSpeed = 50f;
                break;
            case "BasicEnemy":
                SetEnemyType(EnemyTypes.BASIC);
                MissileCount = 10;
                MoveData.Acceleration = 3.6f;
                MoveData.RotateSpeed = 4f;
                MoveData.MaxSpeed = 40f;
                break;
            case "Transport":
                SetEnemyType(EnemyTypes.TRANSPORT);
                MoveData.Acceleration = 4f;
                MoveData.RotateSpeed = 5f;
                MoveData.MaxSpeed = 100f;
                break;
            case "Boss":
                SetEnemyType(EnemyTypes.BOSS);
                MissileCount = 100;
                MoveData.Acceleration = 1.5f;
                MoveData.RotateSpeed = 10f;
                MoveData.MaxSpeed = 20f;
                break;


            default:
                Debug.Log("Invalid Enemy Name : " + enemyName);
                break;
        }
    }
    #endregion 
}