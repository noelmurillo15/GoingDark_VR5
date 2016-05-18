using UnityEngine;
using GD.Core.Enums;

public class EnemyStats : IEnemy
{

    #region Properties    
    public int MissileCount;
    private MovementStats MoveData;    
    #endregion

    void Awake()
    {
        Debug.Log("EStats Awake Called");
        Init();
    }

    public override void Init()
    {
        base.Init();
        MissileCount = 0;
        MoveData.Speed = 0f;
        MoveData.Boost = .5f;
        Initialize(transform.name);

        Debug.Log("EStats Initialized");
    }

    void Update()
    {
        IncreaseSpeed();
    }

    #region Accessors
    public MovementStats GetMoveData()
    {
        return MoveData;
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
    private void Initialize(string enemyName)
    {
        switch (enemyName)
        {
            case "Droid":
                SetEnemyType(EnemyTypes.KAMIKAZE);
                //SetUniqueBehavior(GetComponent<KamikazeAI>());
                MoveData.Acceleration = 6f;
                MoveData.RotateSpeed = 2f;
                MoveData.MaxSpeed = 75f;
                break;
            case "Trident":
                SetEnemyType(EnemyTypes.TRIDENT);
                //SetUniqueBehavior(GetComponent<EnemyTetherAI>());
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
                //SetUniqueBehavior(GetComponent<TransportShipAI>());
                MoveData.Acceleration = 4f;
                MoveData.RotateSpeed = 5f;
                MoveData.MaxSpeed = 100f;
                break;
            case "Boss":
                SetEnemyType(EnemyTypes.BOSS);
                //SetUniqueBehavior(GetComponent<BossAI>());
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