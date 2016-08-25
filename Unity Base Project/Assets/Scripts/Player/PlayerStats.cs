using UnityEngine;
using UnityEngine.UI;
using GoingDark.Core.Enums;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
    private int deathCount = 0;
    private float dmgMultiplier;
    private Vector2 rumbleIntesity;
    private const string display = "Credits : {0}";

    private CloakSystem cloak;
    private HyperdriveSystem hype;
    private SystemManager systemManager;

    private MessageScript msgs;
    private x360Controller controller;
    private DeathTransition deathTransition;
    #endregion

    public int DeathCount
    {
        get { return deathCount; }
        set { deathCount = value; }
    }


    private void Awake()
    {
        rumbleIntesity = new Vector2(1f, 1f);
        HealthData = new HealthProperties(100f, transform, true);        

        startCredits = PlayerPrefs.GetInt("Credits");
        creditsDisplay.text = string.Format(display, startCredits);

        diff = PlayerPrefs.GetString("Difficulty");         
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
                diff = "Easy";
                dmgMultiplier = 1f;
                break;
        }

        controller = GamePadManager.Instance.GetController(0);       
        deathTransition = GameObject.FindGameObjectWithTag("LeapMount").GetComponent<DeathTransition>();
        msgs = transform.GetComponentInChildren<MessageScript>();

        systemManager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();
        Invoke("FindSystems", 1.5f);
    }

    void FindSystems()
    {
        cloak = systemManager.GetSystemScript(SystemType.Cloak) as CloakSystem;
        hype = systemManager.GetSystemScript(SystemType.Hyperdrive) as HyperdriveSystem;
        ShieldData = new ShieldProperties(GameObject.FindGameObjectWithTag("Shield"), 100f, true);
    }

    #region Accessors
    public int GetCredits()
    {
        return startCredits;
    }
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

    #region Modifiers
    public void UnCloak()
    {
        if (cloak.GetCloaked())
            cloak.UnCloakShip();
    }
    public void UpdateCredits(int add)
    {
        startCredits += add;
        creditsDisplay.text = string.Format(display, startCredits);
    }
    public void HealShield()
    {
        if (ShieldData.GetShieldActive())
            ShieldData.Heal(2.5f);
        else
            CancelInvoke("HealShield");

        if (ShieldData.Health >= 100)
            CancelInvoke("HealShield");
    }
    public void ShieldRecharge()
    {
        if (IsInvoking("HealShield"))
            CancelInvoke("HealShield");

        if(ShieldData.GetShieldActive())
            InvokeRepeating("HealShield", 10f, .5f);
    }
    public void PlayerSlowed()
    {
        msgs.Slow(8f);
        DebuffData.Slow(8f);
    }
    public void PlayerStunned()
    {              
        msgs.Stun(3f);
        DebuffData.Stun(3f);
    }    
    #endregion

    #region Damage Calls   
    public void CrashHit(float _speed)
    {
        controller.AddRumble(1f, rumbleIntesity);
        HealthData.Damage(_speed * 20f);
        UnCloak();
    }    
    private void EMPHit()
    {
        controller.AddRumble(5f, rumbleIntesity);
        PlayerStunned();
        UnCloak();      
    }
    private void SlowHit()
    {
        PlayerSlowed();
        UnCloak();
    }
    private void SysruptHit()
    {
        systemManager.SystemDamaged();
        UnCloak();
    }
    void SplashDmg()
    {
        controller.AddRumble(.25f, rumbleIntesity);
        if (ShieldData.GetShieldActive())
            ShieldData.Damage(5 * dmgMultiplier);
        else
            HealthData.Damage(5 * dmgMultiplier);                
    }
    private void ShieldHit(EnemyLaserProjectile laser)
    {
        controller.AddRumble(.5f, rumbleIntesity);
        ShieldData.Damage(laser.GetBaseDamage() * dmgMultiplier);        
        laser.Kill();
    }
    private void ShieldHit(EnemyMissileProjectile missile)
    {
        controller.AddRumble(.5f, rumbleIntesity);
        ShieldData.Damage(missile.GetBaseDamage() * dmgMultiplier);        
        missile.Kill();
    }
    private void LaserDmg(EnemyLaserProjectile laser)
    {
        UnCloak();
        if (ShieldData.GetShieldActive())
        {
            ShieldHit(laser);
            ShieldRecharge();
            return;
        }
        controller.AddRumble(1f, rumbleIntesity);
        HealthData.Damage(laser.GetBaseDamage() * dmgMultiplier);
        laser.Kill();                        
    }
    private void MissileDebuff(EnemyMissileProjectile missile)
    {        
        switch (missile.Type)
        {
            case EnemyMissileType.Slow:
                SlowHit();
                break;
            case EnemyMissileType.Emp:
                EMPHit();
                break;
            case EnemyMissileType.Sysrupt:
                SysruptHit();
                break;
            case EnemyMissileType.ShieldBreak:
                ShieldHit(missile);
                break;
        }
        missile.Kill();
    }
    private void MissileDmg(EnemyMissileProjectile missile)
    {
        UnCloak();
        if (ShieldData.GetShieldActive())
        {
            ShieldHit(missile);
            ShieldRecharge();
            return;
        }
        controller.AddRumble(1f, rumbleIntesity);
        HealthData.Damage(missile.GetBaseDamage() * dmgMultiplier);
        missile.Kill();
    }
    #endregion

    #region Death
    public void GoToStation()
    {
        Timing.RunCoroutine(Fade());
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
        DeathCount += 1;
        deathTransition.Death();
        Invoke("GameOver", .1f);
    }
    public void FadeOut()
    {
        Timing.RunCoroutine(FadeToSceneChange());
    }
    #endregion

    #region Coroutines
    IEnumerator<float> Fade()
    {
        deathTransition.SpawnPlayer();
        deathTransition.NotDead();        
        yield return Timing.WaitForSeconds(1.0f);

        transform.SendMessage("StopMovement");
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(station.position.x, station.position.y, station.position.z - 150f);
        yield return Timing.WaitForSeconds(1.0f);

        deathTransition.notSpawned();
    }
    IEnumerator<float> FadeToSceneChange()
    {
        deathTransition.SpawnPlayer();
        deathTransition.NotDead();
        yield return Timing.WaitForSeconds(1.0f);

        transform.SendMessage("StopMovement");
        yield return Timing.WaitForSeconds(1.0f);
        deathTransition.notSpawned();

        if (SceneManager.GetActiveScene().name == "Level4")
            SceneManager.LoadScene("Credits");
        else
            SceneManager.LoadScene("LevelSelect");

    }
    #endregion
}