using UnityEngine;
using GD.Core.Enums;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{

    #region Properties
    public Impairments Debuff { get; private set; }
    private SystemManager Systems;
    
    
    public ShieldProperties ShieldData;
    public PlayerSaveData playerSaveData;


    private PlayerHealth m_Health;
    #endregion


    // Use this for initialization
    void Start()
    {
        ShieldData.ShieldActive = true;
        ShieldData.ShieldHealth = 50;
        

        playerSaveData.Credits = PlayerPrefs.GetInt("Credits");
        ShieldData.Shield = GameObject.FindGameObjectWithTag("Shield");
        m_Health = GameObject.Find("Health").GetComponent<PlayerHealth>();
        Systems = GameObject.Find("Devices").GetComponent<SystemManager>();
    }

    #region Accessors
    public SystemManager GetSystems()
    {
        return Systems;
    }
    
    public void AddCredits(int _credits)
    {
        playerSaveData.Credits += _credits;
    }
    #endregion

    #region Modifiers
    
    #endregion

    #region Private Methods
    void UpdateSector(string _name)
    {
        playerSaveData.SectorName = _name;
    }    
    void RemoveDebuff()
    {
        Debuff = Impairments.NONE;
        //MoveData.Boost = 1f;
    }
    void Hit()
    {
        if (ShieldData.ShieldActive)
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
        //MoveData.Boost = 0f;
        if(!IsInvoking("RemoveDebuff"))
            Invoke("RemoveDebuff", 10f);
    }
    void ShieldHit()
    {
        ShieldData.ShieldHealth -= 25;
        if (ShieldData.ShieldHealth <= 0)
        {
            ShieldData.ShieldHealth = 0;
            ShieldData.ShieldActive = false;
            ShieldData.Shield.SetActive(false);
        }
        AudioManager.instance.PlayShieldHit();
    }
    public void EnvironmentalDMG()
    {
        if (ShieldData.ShieldActive)
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