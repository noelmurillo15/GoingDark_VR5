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
        string diff = PlayerPrefs.GetString("Difficulty");
        GetComponent<EnemyMovement>().LoadEnemyData(diff);

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
    }
    #endregion 
}