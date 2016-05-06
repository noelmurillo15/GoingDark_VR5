using UnityEngine;

public class EnemyStats : MonoBehaviour {
    //**    Attach to an Enemy  **//

    //  Movement
    public float moveSpeed;
    public float maxSpeed;
    public float rotateSpeed;
    public float acceleration;
    //  Weapons
    public int numMissiles;

    //  Enemy Type
    public enum ENEMY_TYPE {
        BASIC_ENEMY, TRANSPORT, KAMIKAZE
    };
    private ENEMY_TYPE type;


    void Start()
    {       
        if (transform.name == "BetterEnemy")
        {
            moveSpeed = 0f;
            maxSpeed = 40f;
            numMissiles = 5;
            rotateSpeed = 20f;
            acceleration = 2.5f;
            type = ENEMY_TYPE.BASIC_ENEMY;
        }
        else if (transform.name == "Transport")
        {
            moveSpeed = 0f;
            maxSpeed = 50f;
            numMissiles = 0;
            rotateSpeed = 15f;
            acceleration = 3.0f;
            type = ENEMY_TYPE.TRANSPORT;
        }
        else if (transform.name == "Droid")
        {
            moveSpeed = 0f;
            maxSpeed = 60f;
            numMissiles = 0;
            rotateSpeed = 25f;
            acceleration = 4f;
            type = ENEMY_TYPE.KAMIKAZE;
        }
        else
            Debug.Log("Enemy's Name does not match!!");
    }

    void Update()
    {
        
    }

    #region Accessors
    public int GetNumMissiles()
    {
        return numMissiles;
    }
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
    public float GetRotateSpeed()
    {
        return rotateSpeed;
    }

    public ENEMY_TYPE GetEnemyType()
    {
        return type;
    }
    #endregion

    #region Modifiers
    public void DecreaseMissileCount()
    {
        numMissiles--;
    }    
    
    public void IncreaseSpeed(float alterMaxSpeed) {
        // alter max speed = 0.5f : cuts max speed in half
        // alter max speed = 1.0f : does not alter max speed
        if (moveSpeed < (maxSpeed * alterMaxSpeed))
            moveSpeed += Time.deltaTime * acceleration;
        else if (moveSpeed > (maxSpeed * alterMaxSpeed) + .5f)
            moveSpeed -= Time.deltaTime * acceleration * 4f;
    }
    public void DecreaseSpeed() {
        if (moveSpeed > 0.0f)
            moveSpeed -= Time.deltaTime * acceleration * 2.5f;
        else
            moveSpeed = 0.0f;
    }

    private bool RandomChance()
    {
        int chance = Random.Range(0, 10);
        if (chance >= 0)
            return true;

        return false;
    }
    #endregion

    #region Msg Functions
    public void EMPHit()
    {
        Debug.Log("EMP has affected Enemy's Systems");
    }

    public void AsteroidHit()
    {
        Debug.Log("Enemy Has Hit Asteroid");
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