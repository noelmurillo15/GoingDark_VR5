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

    //  Current Sector Name
    public string sectorName;

    //  Shields
    private bool shieldOn;
    private float shieldHealth;
    private GameObject shield;
    #endregion


    // Use this for initialization
    void Start () {      
        MoveData.Speed = 0f;
        MoveData.Boost = 1f;
        MoveData.MaxSpeed = 120f;
        MoveData.RotateSpeed = 20f;
        MoveData.Acceleration = 10f;
        numCredits = PlayerPrefs.GetInt("Credits");
        Systems = GameObject.Find("Devices").GetComponent<SystemManager>();

        // shield defaults
        shieldOn = false;
        shieldHealth = 3;
        shield = GameObject.FindGameObjectWithTag("Shield");
    }
	
	// Update is called once per frame
	void Update () {
        // shield cooldown and reactivate
        if (shieldHealth != 0.0f)
        {
            shieldHealth += Time.deltaTime;
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
    public void UpdateSector(string _name)
    {
        sectorName = _name;
    }
    public void EMPHit()
    {
        Debug.Log("EMP has affected Player's Systems");
    }

    public void Hit()
    {
        Debug.Log("Player Has Been Hit");
        if (shieldOn)
        {
            AudioManager.instance.PlayShieldHit();
            shieldHealth -= 100;
            if (shieldHealth <= 0)
            {
                shieldHealth = 0;
                shieldOn = false;
                shield.SetActive(shieldOn);
            }
        }
        else
        {
            Systems.SystemDamaged();
            PlayerHealth m_Health = GameObject.Find("Health").GetComponent<PlayerHealth>();
            m_Health.Hit();
            AudioManager.instance.PlayHit();
        }
       }

    public void EnvironmentalDMG()
    {
        if (shieldOn)
        {
            AudioManager.instance.PlayShieldHit();
            shieldHealth -= 100;
            if (shieldHealth <= 0)
            {
                shieldHealth = 0;
                shieldOn = false;
                shield.SetActive(shieldOn);
            }
        }
        else
        {        
            Systems.SystemDamaged();
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