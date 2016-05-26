using UnityEngine;
using GD.Core.Enums;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{

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
    private PlayerHealth m_Health;
    #endregion


    // Use this for initialization
    void Start()
    {
        shieldOn = true;
        shieldHealth = 50;
        MoveData.Speed = 0f;
        MoveData.Boost = 1f;
        MoveData.MaxSpeed = 100f;
        MoveData.RotateSpeed = 40f;
        MoveData.Acceleration = 25f;

        numCredits = PlayerPrefs.GetInt("Credits");
        shield = GameObject.FindGameObjectWithTag("Shield");
        m_Health = GameObject.Find("Health").GetComponent<PlayerHealth>();
        Systems = GameObject.Find("Devices").GetComponent<SystemManager>();
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
            MoveData.Speed -= Time.deltaTime * MoveData.Acceleration * 2.5f;
        else
            MoveData.Speed = 0.0f;
    }
    #endregion

    #region Private Methods
    void UpdateSector(string _name)
    {
        sectorName = _name;
    }    
    void RemoveDebuff()
    {
        Debuff = Impairments.NONE;
        MoveData.Boost = 1f;
    }
    void Hit()
    {
        if (shieldOn)
        {
            ShieldHit();
            return;
        }

        m_Health.Hit();
        Systems.SystemDamaged();        
        AudioManager.instance.PlayHit();
        Debug.Log("Player Has Been Hit");
    }
    void EMPHit()
    {
        Debug.Log("EMP has affected Player's Systems");
        Debuff = Impairments.STUNNED;
        MoveData.Boost = 0f;
        if(!IsInvoking("RemoveDebuff"))
            Invoke("RemoveDebuff", 10f);
    }
    void ShieldHit()
    {
        shieldHealth -= 25;
        if (shieldHealth <= 0)
        {
            shieldHealth = 0;
            shieldOn = false;
            shield.SetActive(shieldOn);
        }
        AudioManager.instance.PlayShieldHit();
    }
    public void EnvironmentalDMG()
    {
        if (shieldOn)
        {
            ShieldHit();
            return;
        }
        m_Health.Hit();
        Systems.SystemDamaged();
    }
    void Kill()
    {
        Debug.Log("Destroyed Player Ship");
        SceneManager.LoadScene("GameOver");
    }
    #endregion    
}