using UnityEngine;
using GD.Core.Enums;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour {
    //**    Attach to Player    **//

    #region Properties
    public Impairments Debuff { get; private set; }
    private MovementStats MoveData;
    private SystemManager Systems;
    //  Credits
    public int numCredits;

    //  Shields
    private bool shieldOn;
    private float shieldTimer;
    private int shieldHealth;
    private GameObject shield;
    #endregion


    // Use this for initialization
    void Start () {      
        MoveData.Speed = 0f;
        MoveData.Boost = 1f;
        MoveData.MaxSpeed = 50f;
        MoveData.RotateSpeed = 20f;
        MoveData.Acceleration = 5f;
        numCredits = PlayerPrefs.GetInt("Credits");
       // numMissiles = PlayerPrefs.GetInt("MissleCount");
        Systems = GameObject.Find("Devices").GetComponent<SystemManager>();

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
                AudioManager.instance.PlayShieldOn();
            }
        }
    }

    #region Accessors
    public bool GetShield()
    {
        return shieldOn;
    }       
    public SystemManager GetSystems()
    {
        return Systems;
    }
    public MovementStats GetMoveData()
    {
        return MoveData;
    }
    #endregion

    #region Modifiers
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
    #endregion

    #region Msg Functions
    public void EMPHit()
    {
        Debug.Log("EMP has affected Player's Systems");
    }

    public void Hit()
    {
        if (shieldOn)
        {
            AudioManager.instance.PlayShieldHit();
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
            PlayerHealth m_Health = GameObject.Find("Health").GetComponent<PlayerHealth>();
            m_Health.Hit();
            AudioManager.instance.PlayHit();
        }
       }

    public void EnvironmentalDMG()
    {
        if (shieldOn)
        {
            shieldHealth-=33;
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
            PlayerHealth m_Health = GameObject.Find("Health").GetComponent<PlayerHealth>();
            m_Health.Hit();
        }


    }

    public void Kill()
    {
        Debug.Log("Destroyed Player Ship");
        SceneManager.LoadScene("GameOver");
    }
    #endregion    
}