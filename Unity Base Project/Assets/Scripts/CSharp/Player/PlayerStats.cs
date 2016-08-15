using UnityEngine;
using UnityEngine.UI;
using GoingDark.Core.Enums;
using GoingDark.Core.Utility;

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
    [SerializeField]
    private PauseManager save;

    private string diff;
    private int startCredits;
    private float dmgMultiplier;
    private const string display = "Credits : {0}";

    [SerializeField]
    private Text creditsDisplay;

    private CloakSystem cloak;
    private HyperdriveSystem hype;
    private SystemManager systemManager;

    private MessageScript msgs;
    private x360Controller controller;
    private DeathTransition deathTransition;
    #endregion


    private void Awake()
    {        
        SaveData = new PlayerSaveData();
        DebuffData = new DebuffProperties();
        HealthData = new HealthProperties(100f, transform, true);
        ShieldData = new ShieldProperties(GameObject.FindGameObjectWithTag("Shield"), 100f, true);

        startCredits = PlayerPrefs.GetInt("Credits");
        creditsDisplay.text = string.Format(display, startCredits);
        string diff = PlayerPrefs.GetString("Difficulty");         
        switch (diff)
        {
            case "Easy":
                dmgMultiplier = 1f;
                break;
            case "Medium":
                dmgMultiplier = 1.5f;
                break;
            case "Hard":
                dmgMultiplier = 2f;
                break;
            case "Nightmare":
                dmgMultiplier = 3f;
                break;
            default:
                Debug.LogError("Player Could not get Game difficulty");
                break;
        }

        Debug.Log("Player Dmg Mult : " + dmgMultiplier);

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
    public int GetCredits()
    {
        return startCredits;
    }
    public void UpdateCredits(int add)
    {
        startCredits += add;
        creditsDisplay.text = string.Format(display, startCredits);
    }
    #endregion

    #region Shield
    public void HealShield()
    {
        ShieldData.Heal(5f);
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
        msgs.Slow(10f);
        DebuffData.Slow(10f);
    }
    public void PlayerStunned()
    {      
        msgs.Stun(5f);
        DebuffData.Stun(5f);
    }
    #endregion

    #region Message Calls   
    public void CrashHit(float _speed)
    {
        Debug.Log("Player Crashed");
        controller.AddRumble(1f, new Vector2(1f, 1f));
        HealthData.Damage(_speed * 20f);
        UnCloak();
    }    
    private void EMPHit()
    {
        Debug.Log("Player Emp Dmg");
        controller.AddRumble(5f, new Vector2(1f, 1f));
        PlayerStunned();
        UnCloak();      
    }
    void SplashDmg()
    {
        if (!ShieldData.GetShieldActive())
        {
            Debug.Log("Player Splash Dmg");
            controller.AddRumble(.5f, new Vector2(.5f, .5f));
            HealthData.Damage(4 * dmgMultiplier);
            UnCloak();
        }
    }
    private void ShieldHit(EnemyLaserProjectile laser)
    {
        Debug.Log("Player Shield Hit By Laser");
        controller.AddRumble(.5f, new Vector2(1f, 1f));
        ShieldData.Damage(laser.GetBaseDamage() * dmgMultiplier);
        laser.Kill();

        if (IsInvoking("HealShield"))
            CancelInvoke("HealShield");

        if (!ShieldData.Active)
            Invoke("RechargeShield", 30f);
        else
            InvokeRepeating("HealShield", 10f, 2f);
    }
    private void ShieldHit(EnemyMissileProjectile missile)
    {
        Debug.Log("Player Shield Hit By Missile");
        controller.AddRumble(.5f, new Vector2(1f, 1f));
        ShieldData.Damage(missile.GetBaseDamage());
        missile.Kill();

        if (IsInvoking("HealShield"))
            CancelInvoke("HealShield");

        if (!ShieldData.Active)
            Invoke("RechargeShield", 30f);
        else
            InvokeRepeating("HealShield", 10f, 2f);
    }

    private void LaserDmg(EnemyLaserProjectile laser)
    {
        if (ShieldData.GetShieldActive())
        {
            ShieldHit(laser);
            return;
        }
        controller.AddRumble(1f, new Vector2(1f, 1f));
        HealthData.Damage(laser.GetBaseDamage());
        laser.Kill();                        
        UnCloak();

        CancelInvoke("RechargeShield");
        Invoke("RechargeShield", 30f);  //  reset timer
    }
    private void MissileDmg(EnemyMissileProjectile missile)
    {
        if (ShieldData.GetShieldActive())
        {
            ShieldHit(missile);
            return;
        }
        controller.AddRumble(1f, new Vector2(1f, 1f));
        HealthData.Damage(missile.GetBaseDamage() * dmgMultiplier);
        missile.Kill();
        UnCloak();

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
        if ((startCredits - 50) > 0)
        {
            UpdateCredits(-50);
            HealthData.FullRestore();
            ShieldData.FullRestore();
            systemManager.FullSystemRepair();
        }
    }
    public void Respawn()
    {
        switch (diff)
        {
            case "Easy":
                UpdateCredits(-200); break;
            case "Medium":
                UpdateCredits(-200 * 2); break;
            case "Hard":
                UpdateCredits(-200 * 3); break;
            case "Nightmare":
                UpdateCredits(-200 * 5); break;
            default:
                Debug.LogError("Player Could not get Game difficulty");
                break;
        }
        Repair();  
        GoToStation();
        if(hype != null)      
            hype.StopHyperdrive();
    }
    public void Kill()
    {
        save.SaveGame();
        deathTransition.Death();
        Invoke("GameOver", 2f);
    }
    void GameOver()
    {
        transform.root.GetComponent<GameOver>().InitializeGameOverScene();
    }
    #endregion
}