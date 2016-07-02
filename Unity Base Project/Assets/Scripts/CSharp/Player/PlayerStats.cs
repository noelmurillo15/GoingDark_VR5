using UnityEngine;
using GoingDark.Core.Enums;

public class PlayerStats : MonoBehaviour
{
    #region Properties
    public Impairments Debuff { get; private set; }

    public PlayerSaveData SaveData;
    public ShieldProperties ShieldData;

    private Shieldbar ShieldBar;
    private HealthBar HealthData;
    private SystemManager SystemData;
    private DebuffManager DebuffData;
    private DeathTransition deathTransition;

    private Vector3 station;
    private x360Controller controller;
    #endregion


    // Use this for initialization
    void Start()
    {
        station = GameObject.Find("Station").transform.position;
        SystemData = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();
        HealthData = GameObject.Find("PlayerHealth").GetComponent<HealthBar>();
        ShieldBar = GameObject.Find("PlayerShields").GetComponent<Shieldbar>();
        deathTransition = GameObject.FindGameObjectWithTag("LeapMount").GetComponent<DeathTransition>();
        DebuffData = GameObject.Find("Debuffs").GetComponent<DebuffManager>();

        ShieldData.ShieldHealth = 100;
        ShieldData.ShieldActive = true;
        ShieldData.Shield = GameObject.FindGameObjectWithTag("Shield");

        controller = GamePadManager.Instance.GetController(0);       
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
    void RemoveDebuff()
    {
        Debuff = Impairments.None;
    }
    void Hit()
    {        
        controller.AddRumble(.5f, new Vector2(1f, 1f), .4f);
        if (ShieldData.GetShieldActive())
        {
            ShieldHit();
            return;
        }

        CancelInvoke("RechargeShield");
        Invoke("RechargeShield", 20f);  //  reset timer

        HealthData.DecreaseHealth(20f);        
        AudioManager.instance.PlayHit();
        Debug.Log("Player HIT");
    }
    void EMPHit()
    {
        controller.AddRumble(5f, new Vector2(.5f, .5f), 4.5f);
        Debuff = Impairments.Stunned;
        DebuffData.Stunned(5f);
        SystemData.SystemDamaged();

        if (ShieldData.GetShieldActive())
        {
            ShieldData.TakeDamage(100);
            Debug.Log("Player took Emp hit, broke shield");
        }
        else
        {
            Debug.Log("Player took Emp hit, lost health");
            HealthData.DecreaseHealth(10);
            CancelInvoke("RechargeShield");
            Invoke("RechargeShield", 20f); //  reset timer
        }
                
        if (!IsInvoking("RemoveDebuff"))
            Invoke("RemoveDebuff", 5f);
    }

    void ShieldHit()
    {
        ShieldData.TakeDamage(20f);
        ShieldBar.DecreaseShield(20f);
        Debug.Log("Player took Shield hit");
        if (ShieldData.ShieldHealth <= 0f)
        {
            Debug.Log("Player took Shield hit, shield broke");
            ShieldData.ShieldHealth = 0f;
            ShieldData.ShieldActive = false;
            ShieldData.Shield.SetActive(false);
            Invoke("RechargeShield", 20f); 
        }
        AudioManager.instance.PlayShieldHit();
    }
    public void RechargeShield()
    {
        Debug.Log("Player Shield Recharged");
        CancelInvoke();
        ShieldData.ShieldRecharge(100f);
        ShieldBar.Reset();
        ShieldData.ShieldActive = true;
        ShieldData.Shield.SetActive(true);
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
    void Respawn()
    {
        Debug.Log("Player Respawned");
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
    void Kill()
    {
        Debug.Log("PLAYER DEAD!");
        deathTransition.SendMessage("Death");
        Invoke("Respawn", 2f);
    }
    #endregion
}