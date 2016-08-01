﻿using UnityEngine;
using GoingDark.Core.Enums;

public class PlayerStats : MonoBehaviour
{
    #region Properties
    private CloakSystem cloak;
    private SystemManager systemManager;

    public PlayerSaveData SaveData;
    public ShieldProperties ShieldData;
    public HealthProperties HealthData;
    public DebuffProperties DebuffData;

    [SerializeField]
    public Transform station;

    private MessageScript msgs;
    private x360Controller controller;
    private DeathTransition deathTransition;
    #endregion


    private void Start()
    {        
        systemManager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();

        SaveData = new PlayerSaveData();
        DebuffData = new DebuffProperties();
        HealthData = new HealthProperties(100f, transform, true);
        ShieldData = new ShieldProperties(GameObject.FindGameObjectWithTag("Shield"), 100f, true);

        controller = GamePadManager.Instance.GetController(0);       
        deathTransition = GameObject.FindGameObjectWithTag("LeapMount").GetComponent<DeathTransition>();
        msgs = transform.GetComponentInChildren<MessageScript>();

        Invoke("FindCloakSystem", 5f);
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
    public DebuffProperties GetDebuffData()
    {
        return DebuffData;
    }
    #endregion

    #region Shield
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

    #region Debuffs
    public void PlayerSlowed()
    {
        DebuffData.isSlowed = true;
        Invoke("RemoveSlow", 10f);
    }
    public void PlayerStunned()
    {      
        if (!DebuffData.isStunned)
        {
            Debug.Log("Stunning the Player");
            DebuffData.isStunned = true;
            Invoke("RemoveStun", 5f);
            msgs.Stun();
        }
        else
            Debug.Log("Player hit by EMP but already stunned");        
    }

    void RemoveSlow()
    {
        DebuffData.isSlowed = false;
    }
    void RemoveStun()
    {
        msgs.NotStunned();
        DebuffData.isStunned = false;
    }
    #endregion

    #region Message Calls
    void FindCloakSystem()
    {
        cloak = systemManager.GetSystemScript(SystemType.Cloak) as CloakSystem;
        if (cloak == null)
            Debug.LogError("Player's CloakSystem was not found");
    }
    private void ShieldHit()
    {
        ShieldData.Damage(20f);

        if (IsInvoking("HealShield"))
            CancelInvoke("HealShield");

        if (!ShieldData.Active)
            Invoke("RechargeShield", 30f);        
        else
            InvokeRepeating("HealShield", 10f, 2f);           
    }
    private void EMPHit()
    {
        UnCloak();
        controller.AddRumble(5f, new Vector2(.5f, .5f), 4.5f); PlayerStunned();

        if (ShieldData.GetShieldActive())
            ShieldData.Damage(50f);        
        else
        {
            PlayerStunned();
            systemManager.SystemDamaged();
            CancelInvoke("RechargeShield");
            Invoke("RechargeShield", 30f); //  reset timer
        }        
    }
    void SplashDmg()
    {
        if (!ShieldData.GetShieldActive())
        {
            UnCloak();
            Debug.Log("Player affected by splash dmg");
            HealthData.Damage(2);

            CancelInvoke("RechargeShield");
            Invoke("RechargeShield", 30f);  //  reset timer
        }
    }
    private void Hit()
    {
        UnCloak();
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
        deathTransition.SpawnPlayer();
        Invoke("Respawn", 1.5f);
    }

    public void UnCloak()
    {
        if(cloak.GetCloaked())
            cloak.UnCloakShip();
    }
    public void Repair()
    {
        HealthData.FullRestore();
        ShieldData.FullRestore();
        systemManager.FullSystemRepair();
    }
    private void Respawn()
    {
        Repair();
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(station.position.x + 30, station.position.y, station.position.z - 500f);
        deathTransition.NotDead();
        deathTransition.notSpawned();
    }
    private void Kill()
    {
        deathTransition.Death();
        Invoke("Respawn", 1.5f);
    }
    #endregion
}