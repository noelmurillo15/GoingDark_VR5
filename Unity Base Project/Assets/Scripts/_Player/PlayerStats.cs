using UnityEngine;
using GD.Core.Enums;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{

    #region Properties
    public Impairments Debuff { get; private set; }
    private SystemManager Systems;
    private MissileSystem missiles;
    private Transform station;
    public ShieldProperties ShieldData;
    public PlayerSaveData playerSaveData;

    public CameraShake camShake;
    private Texture2D screenBreak;
    private Texture2D shatter;
    float fadeTimer;

    private PlayerHealth m_Health;
    #endregion


    // Use this for initialization
    void Start()
    {
        ShieldData.ShieldHealth = 50;
        ShieldData.ShieldActive = true;

        station = GameObject.Find("Station").transform;
        playerSaveData.Credits = PlayerPrefs.GetInt("Credits");
        ShieldData.Shield = GameObject.FindGameObjectWithTag("Shield");
        m_Health = GameObject.Find("Health").GetComponent<PlayerHealth>();
        Systems = GameObject.Find("Devices").GetComponent<SystemManager>();
        camShake = GameObject.FindGameObjectWithTag("LeapMount").GetComponent<CameraShake>();
        screenBreak = null;
        shatter = Resources.Load<Texture2D>("broken-glass");
        fadeTimer = 1.0f;
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

    void OnGUI()
    {
        if (screenBreak)
        {
            fadeTimer -= Time.deltaTime * 0.15f;
            GUI.color = new Color(1, 1, 1, fadeTimer);
            if (GUI.color.a <= 0)
            {
                GUI.color = new Color(1, 1, 1, 1);
                screenBreak = null;
                fadeTimer = 1.0f;
            }
        }

    }
    void ScreenBreak()
    {
        screenBreak = shatter;
        GUI.color = new Color(1, 1, 1, 1);
        fadeTimer = 1.0f;
    }

    void Hit()
    {
        if (ShieldData.ShieldActive)
        {
            ShieldHit();
            return;
        }

        camShake.PlayShake();
        ScreenBreak();

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
        camShake.PlayShake();
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
        Debug.Log("Player Stats : Destroyed Player Ship");
        Invoke("Respawn", 2);
        //SceneManager.LoadScene("GameOver");
    }
    void Respawn()
    {
        m_Health.GetComponent<PlayerHealth>().hitCount = 0;
        m_Health.SendMessage("UpdatePlayerHealth");
        ShieldData.ShieldActive = true;
        ShieldData.ShieldHealth = 100;
        ShieldData.Shield.SetActive(true);
        Systems.FullSystemRepair();
        
        transform.position = new Vector3(station.position.x, station.position.y + 30, station.position.z);
    }
    #endregion    
}