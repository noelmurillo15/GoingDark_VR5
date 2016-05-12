using UnityEngine;
using GD.Core.Enums;

public class EnemyStats : MonoBehaviour {

    #region Properties    
    public EnemyTypes Type { get; private set; }    

    // Movement
    public float Speed { get; private set; }
    public float Boost { get; private set; }
    public float MaxSpeed { get; private set; }
    public float RotateSpeed { get; private set; }
    public float Acceleration { get; private set; }

    //  Weapons
    public int MissileCount { get; private set; }

    //  Impairments
    public bool Stunned { get; private set; }
    private float StunTimer { get; set; }     
    #endregion

    void Start()
    {
        Speed = 0f;
        Boost = 1f;
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

    #region Private Methods
    void Initialize(string enemyName)
    {
        switch (enemyName)
        {
            case "Droid":
                Type = EnemyTypes.KAMIKAZE;
                Acceleration = 6f;
                RotateSpeed = 2f;
                MaxSpeed = 75f;
                break;
            case "Trident":
                Type = EnemyTypes.TRIDENT;
                Acceleration = 4f;
                RotateSpeed = 3f;
                MaxSpeed = 50f;
                break;
            case "BasicEnemy":
                Type = EnemyTypes.BASIC;
                MissileCount = 5;
                Acceleration = 3.6f;
                RotateSpeed = 4f;
                MaxSpeed = 40f;
                break;
            case "Transport":
                Type = EnemyTypes.TRANSPORT;
                Acceleration = 4f;
                RotateSpeed = 5f;
                MaxSpeed = 100f;
                break;
            case "Boss":
                Type = EnemyTypes.BOSS;
                MissileCount = 100;
                Acceleration = 1.5f;
                RotateSpeed = 10f;
                MaxSpeed = 20f;
                break;


            default:
                Debug.Log("Invalid Enemy Name : " + enemyName);
                break;
        }        
    }    

    public void StopMovement()
    {
        Speed = 0f;
    }
    public void SetSpeedBoost(float newBoost)
    {
        Boost = newBoost;
    }
    public void DecreaseMissileCount()
    {
        MissileCount--;
    }        
    public void IncreaseSpeed() {
        if (Speed < (MaxSpeed * Boost))
            Speed += Time.deltaTime * Acceleration;
        else if (Speed > (MaxSpeed * Boost) + .5f)
            Speed -= Time.deltaTime * Acceleration * 4f;
    }
    public void DecreaseSpeed() {
        if (Speed > 0.0f)
            Speed -= Time.deltaTime * Acceleration * 2.5f;
        else
            Speed = 0.0f;
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