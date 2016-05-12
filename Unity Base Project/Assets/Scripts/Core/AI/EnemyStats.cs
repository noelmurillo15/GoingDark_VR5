using UnityEngine;
using GD.Core.Enums;

public class EnemyStats : MonoBehaviour {

    #region Properties    
    public EnemyTypes Type { get; private set; }

    public MovementStats MoveData;

    //  Weapons
    public int MissileCount { get; private set; }

    //  Impairments
    public bool Stunned { get; private set; }
    private float StunTimer { get; set; }     
    #endregion

    void Start()
    {
        MoveData.Speed = 0f;
        MoveData.Boost = 1f;
        MissileCount = 0;
        Initialize(transform.name);
    }

    void Update()
    {
        if (StunTimer > 0f)
        {
            StunTimer -= Time.deltaTime;
            DecreaseSpeed();
        }
        else
        {
            Stunned = false;
        }        
    }

    public MovementStats GetMoveData()
    {
        return MoveData;
    }

    #region Private Methods
    void Initialize(string enemyName)
    {
        switch (enemyName)
        {
            case "Droid":
                Type = EnemyTypes.KAMIKAZE;
                MoveData.Acceleration = 6f;
                MoveData.RotateSpeed = 2f;
                MoveData.MaxSpeed = 75f;
                break;
            case "Trident":
                Type = EnemyTypes.TRIDENT;
                MoveData.Acceleration = 4f;
                MoveData.RotateSpeed = 3f;
                MoveData.MaxSpeed = 50f;
                break;
            case "BasicEnemy":
                Type = EnemyTypes.BASIC;
                MissileCount = 5;
                MoveData.Acceleration = 3.6f;
                MoveData.RotateSpeed = 4f;
                MoveData.MaxSpeed = 40f;
                break;
            case "Transport":
                Type = EnemyTypes.TRANSPORT;
                MoveData.Acceleration = 4f;
                MoveData.RotateSpeed = 5f;
                MoveData.MaxSpeed = 100f;
                break;
            case "Boss":
                Type = EnemyTypes.BOSS;
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

    public void SetSpeedBoost(float newBoost)
    {
        MoveData.Boost = newBoost;
    }
    public void StopMovement()
    {
        MoveData.Speed = 0f;
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
    public void DecreaseMissileCount()
    {
        MissileCount--;
    }        
          
    private bool RandomChance()
    {        
        if (Random.Range(1, 3) == 1)
            return true;

        return false;
    }
    #endregion

    #region Msg Functions
    public void EMPHit()
    {
        Debug.Log("EMP has affected " + transform.name + "'s Systems");
        Stunned = true;
        StunTimer = 10f;
    }
    public void Hit()
    {
        Debug.Log(transform.name + " Has Hit Asteroid");
        StopMovement();
    }    
    public void Kill() {
        if (RandomChance()) {
            GameObject ammo = Resources.Load<GameObject>("AmmoDrop");
            Instantiate(ammo, transform.position, transform.rotation);
        }
        Destroy(this.gameObject);
    }
    #endregion
}