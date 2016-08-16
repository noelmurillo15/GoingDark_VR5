using UnityEngine;
using UnityEngine.UI;
using GoingDark.Core.Enums;
using GoingDark.Core.Utility;

public class PlayerStats : MonoBehaviour
{
    #region Properties
    public ShieldProperties ShieldData;
    public HealthProperties HealthData;
    public DebuffProperties DebuffData;

    [SerializeField]
    private Transform station;
    [SerializeField]
    private PauseManager save;
    [SerializeField]
    private Text creditsDisplay;

    private string diff;
    private int startCredits;
    private float dmgMultiplier;
    private const string display = "Credits : {0}";

    private CloakSystem cloak;
    private HyperdriveSystem hype;
    private SystemManager systemManager;

    private MessageScript msgs;
    private x360Controller controller;
    private DeathTransition deathTransition;
    #endregion


    private void Awake()
    {        
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
    public CloakSystem GetCloak()
    {
        return cloak;
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

    #region Credits
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

    #region Damage Calls   
    public void UnCloak()
    {
        if (cloak.GetCloaked())
            cloak.UnCloakShip();
    }
    public void CrashHit(float _speed)
    {
        controller.AddRumble(1f, new Vector2(1f, 1f));
        HealthData.Damage(_speed * 20f);
        UnCloak();
    }    
    private void EMPHit()
    {
        controller.AddRumble(5f, new Vector2(1f, 1f));
        PlayerStunned();
        UnCloak();      
    }
    void SplashDmg()
    {
        if (!ShieldData.GetShieldActive())
        {
            controller.AddRumble(.5f, new Vector2(1f, 1f));
            HealthData.Damage(4 * dmgMultiplier);
        }
        else
        {
            controller.AddRumble(.5f, new Vector2(1f, 1f));
            ShieldData.Damage(4 * dmgMultiplier);
        }
    }
    private void ShieldHit(EnemyLaserProjectile laser)
    {
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
    #endregion

    #region Death
    public void ClearScreen()
    {
        deathTransition.SpawnPlayer();
        deathTransition.NotDead();
        deathTransition.notSpawned();
    }    
    public void GoToStation()
    {
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(station.position.x + 30, station.position.y, station.position.z - 500f);             
        Invoke("ClearScreen", 1f);
    }
    public void Repair(int cost)
    {
        if ((startCredits - cost) > 0)
        {
            UpdateCredits(-cost);
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
        }
        Repair(0);  
        GoToStation();
        if(hype != null)      
            hype.StopHyperdrive();
    }
    void GameOver()
    {
        transform.root.GetComponent<EnemyManager>().AllEnemiesPatrol();
        transform.root.GetComponent<GameOver>().InitializeGameOverScene();
    }
    public void Kill()
    {
        save.SaveGame();
        deathTransition.Death();
        Invoke("GameOver", 2f);
    }
    #endregion
}