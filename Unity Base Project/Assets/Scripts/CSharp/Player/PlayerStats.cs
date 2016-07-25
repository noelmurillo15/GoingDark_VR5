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

    [SerializeField]
    public Transform station;
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
    public void HealShield()
    {
        Debug.Log("Player Shield Healed + 10hp : Current " + ShieldData.Health);
        ShieldData.Heal(10f);

        if (ShieldData.Health >= 100)
            CancelInvoke("HealShield");
    }
    public void RechargeShield()
    {
        Debug.Log("Player Shield Recharged");
        ShieldData.FullRestore();
        ShieldData.Shield.SetActive(true);
    }
    #endregion

    #region Message Calls
    private void RemoveDebuff()
    {
        Debug.Log("Debuffs removed from Player");
        Debuff = Impairments.None;
    }
    private void ShieldHit()
    {
        ShieldData.Damage(20f);

        if (IsInvoking("HealShield"))
            CancelInvoke("HealShield");
        if (IsInvoking("RechargeShield"))
            CancelInvoke("RechargeShield");

        if (!ShieldData.Active)
            Invoke("RechargeShield", 30f);        
        else
            InvokeRepeating("HealShield", 10f, 2f);           
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
    void SplashDmg()
    {
        if (!ShieldData.GetShieldActive())
        {
            Debug.Log("Player affected by splash dmg");
            HealthData.Damage(2);

            CancelInvoke("RechargeShield");
            Invoke("RechargeShield", 30f);  //  reset timer
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
    public void GoToStation()
    {
        Respawn();
    }
    private void Respawn()
    {
        HealthData.FullRestore();
        ShieldData.FullRestore();
        systemManager.FullSystemRepair();
        deathTransition.SendMessage("Respawn");
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(station.position.x + 30, station.position.y, station.position.z - 500f);
    }       
    private void Kill()
    {
        deathTransition.SendMessage("Death");
        Invoke("Respawn", 2.5f);
    }
    #endregion
}