using UnityEngine;
using GoingDark.Core.Enums;

public class IEnemy : MonoBehaviour
{

    #region Properties
    [SerializeField]
    public EnemyTypes Type;
    [SerializeField]
    private bool hasShield;
    [SerializeField]
    private GameObject stunned;

    private Impairments Debuff;
    private HealthProperties HealthData;
    private ShieldProperties ShieldData;

    private EnemyManager manager;
    private EnemyMovement movement;
    private EnemyCollision collision;
    private EnemyStateManager statemanager;
    #endregion


    void Awake()
    {        
        stunned.SetActive(false);
        Debuff = Impairments.None;

        movement = GetComponent<EnemyMovement>();
        collision = GetComponent<EnemyCollision>();
        statemanager = GetComponent<EnemyStateManager>();

        movement.enabled = false;
        collision.enabled = false;

        Invoke("LoadEnemyData", .5f);
    }
    #region Accessors        
    public EnemyManager GetManager()
    {
        return manager;
    }
    public HealthProperties GetHealthData()
    {
        return HealthData;
    }
    public ShieldProperties GetShieldData()
    {
        return ShieldData;
    }
    public EnemyStateManager GetStateManager()
    {
        return statemanager;
    }
    public EnemyMovement GetEnemyMovement()
    {
        return movement;
    }
    public EnemyCollision GetEnemyCollision()
    {
        return collision;
    }
    public Impairments GetDebuffData()
    {
        return Debuff;
    }
    #endregion

    #region Debuffs
    void ResetDebuff()
    {
        Debuff = Impairments.None;
        stunned.SetActive(false);
    }
    #endregion

    #region Damage Calls 
    public void CrashHit(float _speed)
    {
        if (hasShield && ShieldData.GetShieldActive())
        {
            ShieldHit(_speed * 25f);
        }
        else
        {
            if(HealthData != null)
                HealthData.Damage(_speed * HealthData.MaxHealth * .5f);
        }
    }
    void ShieldHit(float dmg)
    {
        ShieldData.Damage(dmg);
    }
    void EMPHit()
    {
        stunned.SetActive(true);
        Debuff = Impairments.Stunned;
        Invoke("ResetDebuff", 5f);
    }
    void SplashDmg()
    {
        if (hasShield && ShieldData.GetShieldActive())
            ShieldHit(10f);
        else
            HealthData.Damage(2);
    }
    public void MissileHit(Missile missile)
    {
        if (hasShield && ShieldData.GetShieldActive())
        {
            if (Type == EnemyTypes.FinalBoss)
            {
                missile.Kill();
                return;
            }

            switch (missile.Type)
            {
                case MissileType.Basic:
                    missile.Deflect();
                    break;
                case MissileType.Emp:
                    EMPHit();
                    missile.Kill();
                    break;
                case MissileType.ShieldBreak:
                    ShieldHit(100f);
                    missile.Kill();
                    break;
                case MissileType.Chromatic:
                    missile.Deflect();
                    break;
            }
        }
        else
        {
            switch (missile.Type)
            {
                case MissileType.Basic:
                    HealthData.Damage(5);
                    break;
                case MissileType.Chromatic:
                    HealthData.Damage(25);
                    break;
            }
            missile.Kill();
        }
    }
    public void LaserDmg(LaserProjectile laser)
    {
        if (hasShield && ShieldData.GetShieldActive())
        {
            if (Type == EnemyTypes.FinalBoss)
            {
                laser.Kill();
                return;
            }

            if (laser.Type == LaserType.Basic)
                ShieldHit(2.5f);
            else
                ShieldHit(12.5f);
        }
        else
        {
            if (laser.Type == LaserType.Basic)
                HealthData.Damage(1f);
            else
                HealthData.Damage(5f);
        }
        laser.Kill();
    }

    public void Kill()
    {
        if (GetComponent<EnemyTrail>() != null)
        {
            manager.RemoveEnemy(this);
            GetComponent<EnemyTrail>().Kill();
        }
        else
        {
            manager.RemoveEnemy(this);
            Destroy(gameObject);
        }
    }
    #endregion

    #region Private Methods
    void LoadEnemyData()
    {
        float multiplier = 0f;
        string diff = PlayerPrefs.GetString("Difficulty");

        switch (diff)
        {
            case "Easy":
                multiplier = .5f;
                break;
            case "Medium":
                multiplier = 1f;
                break;
            case "Hard":
                multiplier = 2f;
                break;
            case "Nightmare":
                multiplier = 3f;
                break;
        }

        if (hasShield)
            ShieldData = new ShieldProperties(transform.GetChild(0).gameObject, 100f * multiplier);

        switch (Type)
        {
            case EnemyTypes.Droid:
                HealthData = new HealthProperties(5 * multiplier, transform); break;
            case EnemyTypes.JetFighter:
                HealthData = new HealthProperties(8 * multiplier, transform); break;
            case EnemyTypes.Trident:
                HealthData = new HealthProperties(10 * multiplier, transform); break;
            case EnemyTypes.Basic:
                HealthData = new HealthProperties(12 * multiplier, transform); break;
            case EnemyTypes.SquadLead:
                HealthData = new HealthProperties(20 * multiplier, transform); break;
            case EnemyTypes.Transport:
                HealthData = new HealthProperties(25 * multiplier, transform); break;
            case EnemyTypes.Tank:
                HealthData = new HealthProperties(60 * multiplier, transform); break;
            case EnemyTypes.FinalBoss:
                HealthData = new HealthProperties(100 * multiplier, transform); break;
        }
        manager = transform.root.GetComponent<EnemyManager>();

        movement.enabled = true;
        collision.enabled = true;

        movement.Initialize();
        collision.Initialize();
        movement.LoadEnemyData(diff);

        statemanager.ChangeState(EnemyStates.Patrol);
        manager.AddEnemy(this);
    }
    #endregion 
}