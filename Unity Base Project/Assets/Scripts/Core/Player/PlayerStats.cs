using UnityEngine;
using GoingDark.Core.Enums;

public class PlayerStats : MonoBehaviour
{

    #region Properties
    public Impairments Debuff { get; private set; }

    //  Player Data
    public ShieldProperties ShieldData;
    public PlayerSaveData SaveData;
    public SystemManager SystemData;
    public HealthBar HealthData;
    public Shieldbar ShieldBar;
    private DebuffManager debuffManager;
    // Respawn
    private Vector3 station;
    private DeathTransition deathTransition;
    #endregion


    // Use this for initialization
    void Start()
    {
        station = GameObject.Find("Station").transform.position;
        SystemData = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();
        HealthData = GameObject.Find("PlayerHealth").GetComponent<HealthBar>();
        ShieldBar = GameObject.Find("PlayerShields").GetComponent<Shieldbar>();
        deathTransition = GameObject.FindGameObjectWithTag("LeapMount").GetComponent<DeathTransition>();
        debuffManager = GameObject.Find("Debuffs").GetComponent<DebuffManager>();

        ShieldData.ShieldHealth = 100;
        ShieldData.ShieldActive = true;
        ShieldData.Shield = GameObject.FindGameObjectWithTag("Shield");
    }

    #region Accessors
    public SystemManager GetSystemData()
    {
        return SystemData;
    }
    public ShieldProperties GetShieldData()
    {
        return ShieldData;
    }
    public PlayerSaveData GetSaveData()
    {
        return SaveData;
    }
    #endregion

    #region Message Calls
    void Hit()
    {
        Debug.Log("Player Stats : Hit");
        if (ShieldData.ShieldActive)
        {
            ShieldHit();
            return;
        }
        HealthData.Hit();
        AudioManager.instance.PlayHit();
        SystemData.SystemDamaged();
    }
    void EMPHit()
    {
        Debug.Log("Player Stats : EmpHit");
        Debuff = Impairments.STUNNED;
        debuffManager.Stunned(5f); 
        if (!IsInvoking("RemoveDebuff"))
            Invoke("RemoveDebuff", 5f);
    }

    void RemoveDebuff()
    {
        Debuff = Impairments.NONE;
    }

    void ShieldHit()
    {
        if (ShieldData.ShieldActive)
        {
            Debug.Log("Player Stats : ShieldHit");
            AudioManager.instance.PlayShieldHit();
            ShieldData.ShieldHealth -= 25;
            ShieldBar.DecreaseShield(25.0f); // 4 hits to kill

            if (ShieldData.ShieldHealth <= 0f)
            {
                ShieldData.ShieldHealth = 0f;
                ShieldData.ShieldActive = false;
                ShieldData.Shield.SetActive(false);
            }
        }
    }
    public void EnvironmentalDMG()
    {
        if (ShieldData.ShieldActive)
        {
            ShieldHit();
            return;
        }
        SystemData.SystemDamaged();
    }
    void Kill()
    {
        Debug.Log("Player Stats : Player Death");
        deathTransition.SendMessage("Death");
        Invoke("Respawn", 2f);
    }
    #endregion

    void Respawn()
    {
        HealthData.HitCount = 0;
        HealthData.SendMessage("Reset");
        ShieldBar.Reset();
        ShieldData.ShieldActive = true;
        ShieldData.ShieldHealth = 100;
        ShieldData.Shield.SetActive(true);
        SystemData.FullSystemRepair();
        deathTransition.SendMessage("Respawn");

        transform.position = new Vector3(station.x, station.y + 30, station.z);
    }    
}