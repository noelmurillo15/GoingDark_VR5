using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour {
    //**    Attach to Player    **//

    //  Health
    public int hitCount;
    //  Weapons
    public int numMissiles;
    //  Credits
    public int numCredits;
    //  Movement
    public float moveSpeed;
    public float maxSpeed;
    public float rotateSpeed;
    public float acceleration;

    private PlayerShipData shipData;

    //  Shields
    private bool shieldOn;
    private float shieldTimer;
    private int shieldHealth;
    private GameObject shield;

    // Use this for initialization
    void Start () {      
        hitCount = 0;
        moveSpeed = 0f;
        maxSpeed = 50f;
        rotateSpeed = 20f;
        acceleration = 5f;
        numCredits = PlayerPrefs.GetInt("Credits", 100);
        numMissiles = PlayerPrefs.GetInt("MissleCount", 10);
        shipData = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<PlayerShipData>();

        // shield defaults
        shieldOn = true;
        shieldTimer = 0.0f;
        shieldHealth = 3;
        shield = GameObject.FindGameObjectWithTag("Shield");
    }
	
	// Update is called once per frame
	void Update () {
        // shield cooldown and reactivate
        if (shieldTimer > 0.0f)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0.0f)
            {
                shieldOn = true;
                shield.SetActive(shieldOn);
            }
        }
    }

    #region Accessors
    public int GetHitCount()
    {
        return hitCount;
    }
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

    public bool GetShield()
    {
        return shieldOn;
    }

    public PlayerShipData GetShipData()
    {
        return shipData;
    }
    #endregion

    #region Modifiers
    public void DecreaseMissileCount()
    {
        numMissiles--;
    }
    public void IncreaseHitCount()
    {
        hitCount++;
    }
    public void DecreaseHitCount()
    {
        hitCount--;
    }    
    public void IncreaseSpeed(float percentage)
    {
        if (moveSpeed < (maxSpeed * percentage))
            moveSpeed += Time.deltaTime * acceleration;
        else if (moveSpeed > (maxSpeed * percentage) + .5f)
            DecreaseSpeed();
    }
    public void DecreaseSpeed()
    {
        if (moveSpeed > 0.0f)
            moveSpeed -= Time.deltaTime * acceleration * 2.5f;
        else
            moveSpeed = 0.0f;
    }
    public void StopMovement()
    {
        moveSpeed = 0f;
    }
    #endregion

    #region Msg Functions
    public void EMPHit()
    {
        Debug.Log("EMP has affected Player's Systems");
        shipData.SetIsStunned(true);
    }

    public void Hit()
    {
        if (shieldOn)
        {
            shieldHealth--;
            if (shieldHealth == 0)
            {
                shieldHealth = 100;
                shieldOn = false;
                shield.SetActive(shieldOn);
                shieldTimer = 10.0f;
            }
        }
        else
        {
            IncreaseHitCount();
            PlayerHealth m_Health = GameObject.Find("Health").GetComponent<PlayerHealth>();
            m_Health.UpdatePlayerHealth();
        }
        AudioManager.instance.PlayHit();

        if (hitCount > 2)
            Kill();
    }

    public void EnvironmentalDMG()
    {
        if (shieldOn)
        {
            shieldHealth--;
            if (shieldHealth == 0)
            {
                shieldHealth = 100;
                shieldOn = false;
                shield.SetActive(shieldOn);
                shieldTimer = 10.0f;
            }
        }
        else
        {
            IncreaseHitCount();
            PlayerHealth m_Health = GameObject.Find("Health").GetComponent<PlayerHealth>();
            m_Health.UpdatePlayerHealth();
        }

        if (hitCount > 2)
            Kill();
    }

    public void Kill()
    {
        Debug.Log("Destroyed Player Ship");
        SceneManager.LoadScene("GameOver");
    }
    #endregion    
}