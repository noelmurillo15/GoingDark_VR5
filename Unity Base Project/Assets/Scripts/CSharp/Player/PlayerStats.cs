using UnityEngine;
using GoingDark.Core.Enums;

public class PlayerStats : MonoBehaviour
{
    #region Properties
    public PlayerSaveData SaveData;
    public ShieldProperties ShieldData;
    public HealthProperties HealthData;
    public DebuffProperties DebuffData;

    [SerializeField]
    private Transform station;
    [SerializeField]
    private GameObject stunned;

    private CloakSystem cloak;
    private HyperdriveSystem hype;
    private SystemManager systemManager;

    private MessageScript msgs;
    private x360Controller controller;
    private DeathTransition deathTransition;
    #endregion
    float buffer = 0f;

    private void Awake()
    {        
        SaveData = new PlayerSaveData();
        DebuffData = new DebuffProperties();
        HealthData = new HealthProperties(100f, transform, true);
        ShieldData = new ShieldProperties(GameObject.FindGameObjectWithTag("Shield"), 100f, true);
        
        controller = GamePadManager.Instance.GetController(0);       
        deathTransition = GameObject.FindGameObjectWithTag("LeapMount").GetComponent<DeathTransition>();
        msgs = transform.GetComponentInChildren<MessageScript>();

        systemManager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();
        Invoke("FindSystems", 1f);
    }

    void FindSystems()
    {
        cloak = systemManager.GetSystemScript(SystemType.Cloak) as CloakSystem;
        hype = systemManager.GetSystemScript(SystemType.Hyperdrive) as HyperdriveSystem;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.K))
            Kill();

        if (buffer > 0f)
            buffer -= Time.deltaTime;
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
    public CloakSystem GetCloak()
    {
        return cloak;
    }
    #endregion

    #region Shield
    public void HealShield()
    {
        ShieldData.Heal(10f);

        if (ShieldData.Health >= 100)
            CancelInvoke("HealShield");
    }
    public void RechargeShield()
    {
        ShieldData.FullRestore();
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
            DebuffData.isStunned = true;
            stunned.SetActive(true);
            Invoke("RemoveStun", 5f);
            msgs.Stun();
        }      
    }

    void RemoveSlow()
    {
        DebuffData.isSlowed = false;
    }
    void RemoveStun()
    {
        msgs.NotStunned();
        stunned.SetActive(false);
        DebuffData.isStunned = false;
    }
    #endregion

    #region Message Calls   
    public void CrashHit(float _speed)
    {
        Debug.Log("Player Crashed");
        controller.AddRumble(1f, new Vector2(1f, 1f));
        HealthData.Damage(_speed * 25f);
        UnCloak();
    }    
    private void EMPHit()
    {
        Debug.Log("Player Emp Dmg");
        controller.AddRumble(5f, new Vector2(1f, 1f));
        PlayerStunned();
        UnCloak();

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
        Debug.Log("Player Splash Dmg");
        controller.AddRumble(.5f, new Vector2(.5f, .5f));
        if (!ShieldData.GetShieldActive())
        {
            UnCloak();
            HealthData.Damage(2);
            CancelInvoke("RechargeShield");
            Invoke("RechargeShield", 30f);  //  reset timer
        }
    }
    private void ShieldHit()
    {
        Debug.Log("Player Shield Dmg");
        controller.AddRumble(.5f, new Vector2(1f, 1f));
        ShieldData.Damage(20f);

        if (IsInvoking("HealShield"))
            CancelInvoke("HealShield");

        if (!ShieldData.Active)
            Invoke("RechargeShield", 30f);
        else
            InvokeRepeating("HealShield", 10f, 2f);
    }
    private void Hit()
    {
        if (ShieldData.GetShieldActive())
        {
            ShieldHit();
            return;
        }

        Debug.Log("Player Ship Dmg");
        controller.AddRumble(1f, new Vector2(1f, 1f));
        UnCloak();
        
        HealthData.Damage(10);                         
        CancelInvoke("RechargeShield");
        Invoke("RechargeShield", 30f);  //  reset timer
    }
    public void GoToStation()
    {
        deathTransition.SpawnPlayer();        
        Invoke("ClearScreen", 1f);
    }

    public void ClearScreen()
    {
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(station.position.x + 30, station.position.y, station.position.z - 500f);
        deathTransition.NotDead();
        deathTransition.notSpawned();
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
    public void Respawn()
    {
        Repair();  
        GoToStation();
        if(hype != null)      
            hype.StopHyperdrive();
    }
    private void Kill()
    {
        if (buffer <= 0f)
        {
            deathTransition.Death();
            buffer = 5f;
            Invoke("GameOver", 3f);
            
        }
    }
    void GameOver()
    {
        transform.root.GetComponent<GameOver>().InitializeGameOverScene();
    }
    #endregion
}