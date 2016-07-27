using UnityEngine;
using GoingDark.Core.Enums;

public class IEnemy : MonoBehaviour {

    #region Properties
    [SerializeField]
    public bool hasShield;

    private EnemyManager manager;    
    private ShieldProperties ShieldData;
    private HealthProperties HealthData;

    public EnemyTypes Type = EnemyTypes.None;
    public Impairments Debuff = Impairments.None;
        
    [SerializeField]
    public GameObject stunned;
    #endregion


    public virtual void Initialize()
    {
        if(hasShield)
            ShieldData = new ShieldProperties(transform.GetChild(0).gameObject, 100f);        

        manager = transform.root.GetComponent<EnemyManager>();

        stunned.SetActive(false);                
        Invoke("AddToManager", 2f);
    }

    #region Accessors    
    public ShieldProperties GetShieldData()
    {        
        return ShieldData;
    }
    public HealthProperties GetHealthData()
    {
        return HealthData;
    }
    public EnemyManager GetManager()
    {
        return manager;
    }
    #endregion

    #region Msg Functions
    void AddToManager()
    {
        manager.AddEnemy(transform.GetComponent<EnemyStateManager>());
        LoadEnemyData();
    }
    
    void EMPHit()
    {
        stunned.SetActive(true);
        Debuff = Impairments.Stunned;
        Invoke("ResetDebuff", 5f);
    }
    void ShieldHit(float _val)
    {
        ShieldData.Damage(_val);        
    }
    void SplashDmg()
    {
        if (hasShield && !ShieldData.GetShieldActive())
        {
            HealthData.Damage(2);
            SendMessage("CheckHealth");
        }
    }
    public void MissileHit(Missile missile)
    {        
        if (hasShield && ShieldData.GetShieldActive())
        {
            switch (missile.Type)
            {
                case MissileType.Basic:
                    missile.Deflect();
                    break;
                case MissileType.Emp:
                    missile.Deflect();
                    break;
                case MissileType.Chromatic:
                    missile.Deflect();
                    break;
                case MissileType.ShieldBreak:
                    ShieldHit(100f);
                    missile.Kill();
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
                case MissileType.ShieldBreak:
                    HealthData.Damage(1);
                    break;
                case MissileType.Chromatic:
                    HealthData.Damage(25);
                    break;
            }
            SendMessage("CheckHealth");
            missile.Kill();
        }
    }
    public void LaserHit(LaserProjectile laser)
    {
        if (hasShield && ShieldData.GetShieldActive())
        {
            switch (laser.Type)
            {
                case LaserType.Basic:
                    ShieldHit(5f);
                    break;
                case LaserType.Charged:
                    ShieldHit(12.5f);
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
            }
            SendMessage("CheckHealth");
        }
    }
    public void ResetDebuff()
    {
        Debuff = Impairments.None;
        stunned.SetActive(false);

        if (Type == EnemyTypes.Droid)
            Kill();
    }
    public void Kill()
    {                     
        manager.RemoveEnemy(GetComponent<EnemyStateManager>());
        Destroy(gameObject);
    }
    #endregion

    #region Private Methods
    void LoadEnemyData()
    {
        GetComponent<EnemyMovement>().LoadEnemyData();
        switch (manager.Difficulty)
        {
            #region Easy
            case GameDifficulty.Easy:
                switch (Type)
                {
                    case EnemyTypes.Basic:
                        HealthData = new HealthProperties(10, transform); break;
                    case EnemyTypes.Droid:
                        HealthData = new HealthProperties(5, transform); break;
                    case EnemyTypes.SquadLead:
                        HealthData = new HealthProperties(20, transform); break;
                    case EnemyTypes.JetFighter:
                        HealthData = new HealthProperties(5, transform); break;
                    case EnemyTypes.Transport:
                        HealthData = new HealthProperties(15, transform); break;
                    case EnemyTypes.Trident:
                        HealthData = new HealthProperties(10, transform); break;
                    case EnemyTypes.Boss:
                        HealthData = new HealthProperties(50, transform); break;
                }
                break;
            #endregion

            #region Normal
            case GameDifficulty.Normal:
                switch (Type)
                {
                    case EnemyTypes.Basic:
                        HealthData = new HealthProperties(25, transform); break;
                    case EnemyTypes.Droid:
                        HealthData = new HealthProperties(15, transform); break;
                    case EnemyTypes.SquadLead:
                        HealthData = new HealthProperties(40, transform); break;
                    case EnemyTypes.JetFighter:
                        HealthData = new HealthProperties(15, transform); break;
                    case EnemyTypes.Transport:
                        HealthData = new HealthProperties(30, transform); break;
                    case EnemyTypes.Trident:
                        HealthData = new HealthProperties(20, transform); break;
                    case EnemyTypes.Boss:
                        HealthData = new HealthProperties(100, transform); break;
                }
                break;
            #endregion

            #region Hard
            case GameDifficulty.Hard:
                switch (Type)
                {
                    case EnemyTypes.Basic:
                        HealthData = new HealthProperties(40, transform); break;
                    case EnemyTypes.Droid:
                        HealthData = new HealthProperties(25, transform); break;
                    case EnemyTypes.SquadLead:
                        HealthData = new HealthProperties(64, transform); break;
                    case EnemyTypes.JetFighter:
                        HealthData = new HealthProperties(25, transform); break;
                    case EnemyTypes.Transport:
                        HealthData = new HealthProperties(50, transform); break;
                    case EnemyTypes.Trident:
                        HealthData = new HealthProperties(35, transform); break;
                    case EnemyTypes.Boss:
                        HealthData = new HealthProperties(200, transform); break;
                }
                break;
            #endregion

            #region Nightmare
            case GameDifficulty.Nightmare:
                switch (Type)
                {
                    case EnemyTypes.Basic:
                        HealthData = new HealthProperties(80, transform); break;
                    case EnemyTypes.Droid:
                        HealthData = new HealthProperties(50, transform); break;
                    case EnemyTypes.SquadLead:
                        HealthData = new HealthProperties(120, transform); break;
                    case EnemyTypes.JetFighter:
                        HealthData = new HealthProperties(50, transform); break;
                    case EnemyTypes.Transport:
                        HealthData = new HealthProperties(100, transform); break;
                    case EnemyTypes.Trident:
                        HealthData = new HealthProperties(70, transform); break;
                    case EnemyTypes.Boss:
                        HealthData = new HealthProperties(300, transform); break;
                }
                break;
            #endregion
        }  
    }
    #endregion 
}