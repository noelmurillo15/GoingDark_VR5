using UnityEngine;
using GoingDark.Core.Enums;

public class PlayerStats : MonoBehaviour
{
    #region Properties
    public Impairments Debuff { get; private set; }

    private SystemManager systemManager;
    private DebuffManager debuffManager;

    public PlayerSaveData SaveData;
    public ShieldProperties ShieldData;
    private HealthProperties HealthData;       

    private Transform station;
    private x360Controller controller;
    private DeathTransition deathTransition;
    #endregion


    private void Start()
    {
        Debuff = Impairments.None;

        debuffManager = GameObject.Find("Debuffs").GetComponent<DebuffManager>();
        systemManager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();

        SaveData = new PlayerSaveData();
        HealthData = new HealthProperties(100f, transform, true);
        ShieldData = new ShieldProperties(GameObject.FindGameObjectWithTag("Shield"), 100f, true);

        station = GameObject.Find("Station").transform;
        controller = GamePadManager.Instance.GetController(0);       
        deathTransition = GameObject.FindGameObjectWithTag("LeapMount").GetComponent<DeathTransition>();
    }

    #region Accessors
    public PlayerSaveData GetSaveData()
    {
        return SaveData;
    }
    public SystemManager GetSystemData()
    {
        return systemManager;
    }
    public ShieldProperties GetShieldData()
    {
        return ShieldData;
    }
    #endregion

    #region Modifiers
    public void RechargeShield()
    {
        Debug.Log("Player Shield Recharged");
        CancelInvoke();
        ShieldData.FullRestore();
        ShieldData.Shield.SetActive(true);
    }
    #endregion

    #region Message Calls
    private void RemoveDebuff()
    {
        Debuff = Impairments.None;
    }
    private void ShieldHit()
    {
        ShieldData.Damage(20f);
        if (!ShieldData.Active)
        {
            Debug.Log("Recharging Player shield in 30");
            Invoke("RechargeShield", 30f);
        }            
    }
    private void EMPHit()
    {
        controller.AddRumble(5f, new Vector2(.5f, .5f), 4.5f);

        Debuff = Impairments.Stunned;
        debuffManager.Stunned(5f);
        if (!IsInvoking("RemoveDebuff"))
            Invoke("RemoveDebuff", 5f);

        if (ShieldData.GetShieldActive())
            ShieldData.Damage(50f);        
        else
        {
            systemManager.SystemDamaged();
            CancelInvoke("RechargeShield");
            Invoke("RechargeShield", 30f); //  reset timer
        }        
    }
    private void Hit()
    {        
        controller.AddRumble(.5f, new Vector2(1f, 1f), .5f);
        if (ShieldData.GetShieldActive())
        {
            ShieldHit();
            return;
        }

        HealthData.Damage(10);                         
        CancelInvoke("RechargeShield");
        Invoke("RechargeShield", 30f);  //  reset timer
    }     
    private void Respawn()
    {
        HealthData.FullRestore();
        ShieldData.FullRestore();
        systemManager.FullSystemRepair();
        deathTransition.SendMessage("Respawn");
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(station.position.x, station.position.y, station.position.z - 100f);
    } 
    public void GoToStation()
    {
        deathTransition.SendMessage("Respawn");
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(station.position.x, station.position.y, station.position.z - 100f);
    }   
    private void Kill()
    {
        deathTransition.SendMessage("Death");
        Invoke("Respawn", 2.5f);
    }
    #endregion
}