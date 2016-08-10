using UnityEngine;
using GoingDark.Core.Enums;

public class IEnemy : MonoBehaviour
{

    #region Properties
    [SerializeField]
    private bool hasShield;


    public EnemyTypes Type = EnemyTypes.None;
    public Impairments Debuff = Impairments.None;

    [SerializeField]
    private GameObject stunned;

    private EnemyManager manager;    
    private HealthProperties HealthData;
    private ShieldProperties ShieldData;
    #endregion


    public virtual void Initialize()
    {
        if (hasShield)      
            ShieldData = new ShieldProperties(transform.GetChild(0).gameObject, 100f);        

        stunned.SetActive(false);
        Invoke("AddToManager", 1f);
        manager = transform.root.GetComponent<EnemyManager>();
    }

    void AddToManager()
    {
        manager.AddEnemy(transform.GetComponent<EnemyStateManager>());
        LoadEnemyData();
    }

    #region Accessors        
    public HealthProperties GetHealthData()
    {
        return HealthData;
    }
    public ShieldProperties GetShieldData()
    {
        return ShieldData;
    }
    public EnemyManager GetManager()
    {
        return manager;
    }
    #endregion

    #region Debuffs
    void ResetDebuff()
    {
        Debuff = Impairments.None;
        stunned.SetActive(false);

        if (Type == EnemyTypes.Droid)
            Kill();
    }
    #endregion

    #region Msg Functions   
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
    public void LaserHit(LaserProjectile laser)
    {
        if (hasShield && ShieldData.GetShieldActive())
        {
            if (Type == EnemyTypes.FinalBoss)
            {
                laser.Kill();
                return;
            }

            switch (laser.Type)
            {
                case LaserType.Basic:
                    ShieldHit(2.5f);
                    break;
                case LaserType.Charged:
                    ShieldHit(12.5f);
                    break;
                case LaserType.Ball:
                    ShieldHit(50f);
                    break;
            }
        }
        else
        {
            switch (laser.Type)
            {
                case LaserType.Basic:
                    HealthData.Damage(.5f);
                    break;
                case LaserType.Charged:
                    HealthData.Damage(1.25f);
                    break;
                case LaserType.Ball:
                    HealthData.Damage(5f);
                    break;
            }
        }
    }
    
    public void Kill()
    {
        if (GetComponent<EnemyTrail>() != null)
        {
            manager.RemoveEnemy(GetComponent<EnemyStateManager>());
            GetComponent<EnemyTrail>().Kill();
        }
        else
        {
            manager.RemoveEnemy(GetComponent<EnemyStateManager>());
            Destroy(gameObject);
        }
    }
    #endregion

    #region Private Methods
    void LoadEnemyData()
    {
        float multiplier = 0f;
        switch (manager.Difficulty)
        {
            case GameDifficulty.Easy:
                multiplier = .5f;
                break;
            case GameDifficulty.Normal:
                multiplier = 1f;
                break;
            case GameDifficulty.Hard:
                multiplier = 2f;
                break;
            case GameDifficulty.Nightmare:
                multiplier = 3f;
                break;
        }
        GetComponent<EnemyMovement>().LoadEnemyData(manager.Difficulty);
        switch (Type)
        {
            case EnemyTypes.Droid:
                HealthData = new HealthProperties(5 * multiplier, transform); break;
            case EnemyTypes.JetFighter:
                HealthData = new HealthProperties(10 * multiplier, transform); break;
            case EnemyTypes.Trident:
                HealthData = new HealthProperties(10 * multiplier, transform); break;
            case EnemyTypes.Basic:
                HealthData = new HealthProperties(12 * multiplier, transform); break;
            case EnemyTypes.SquadLead:
                HealthData = new HealthProperties(20 * multiplier, transform); break;
            case EnemyTypes.Transport:
                HealthData = new HealthProperties(25 * multiplier, transform); break;
            case EnemyTypes.Tank:
                HealthData = new HealthProperties(50 * multiplier, transform); break;
            case EnemyTypes.FinalBoss:
                HealthData = new HealthProperties(100 * multiplier, transform); break;
        }
    }
    #endregion 
}