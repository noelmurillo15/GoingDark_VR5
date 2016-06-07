using UnityEngine;
using GD.Core.Enums;

public class PlayerStats : MonoBehaviour
{

    #region Properties
    public Impairments Debuff { get; private set; }

    //  Player Data
    public ShieldProperties ShieldData;
    public HealthProperties HealthData;
    public PlayerSaveData SaveData;
    public SystemManager SystemData;

    // Respawn
    private Vector3 station;
    #endregion


    // Use this for initialization
    void Start()
    {
        station = GameObject.Find("Station").transform.position;
        HealthData = GameObject.Find("Health").GetComponent<HealthProperties>();
        SystemData = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();

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
    public HealthProperties GetHealthData()
    {
        return HealthData;
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

        AudioManager.instance.PlayHit();
        SystemData.SystemDamaged();
        HealthData.Hit();
    }
    void EMPHit()
    {
        Debug.Log("Player Stats : EmpHit");
        Debuff = Impairments.STUNNED;
        //if (!IsInvoking("RemoveDebuff"))
        //    Invoke("RemoveDebuff", 10f);
    }
    void ShieldHit()
    {
        if (ShieldData.ShieldActive)
        {
            Debug.Log("Player Stats : ShieldHit");
            AudioManager.instance.PlayShieldHit();
            ShieldData.ShieldHealth -= 25;

            if(ShieldData.ShieldHealth <= 0f)
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
        HealthData.Hit();
    }
    void Kill()
    {
        Debug.Log("Player Stats : Player Death");
        Invoke("Respawn", 2f);
    }
    #endregion

    void Respawn()
    {
        HealthData.hitCount = 0;
        HealthData.SendMessage("UpdatePlayerHealth");
        ShieldData.ShieldActive = true;
        ShieldData.ShieldHealth = 100;
        ShieldData.Shield.SetActive(true);
        SystemData.FullSystemRepair();
        
        transform.position = new Vector3(station.x, station.y + 30, station.z);
    }    
}