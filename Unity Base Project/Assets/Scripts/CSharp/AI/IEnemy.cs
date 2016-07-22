using UnityEngine;
using GoingDark.Core.Enums;

public class IEnemy : MonoBehaviour {

    #region Properties
    [SerializeField]
    public bool hasShield;

    private ShieldProperties ShieldData;
    private HealthProperties HealthData;

    public EnemyTypes Type = EnemyTypes.None;
    public Impairments Debuff = Impairments.None;
        
    private GameObject stunned;
    private EnemyManager manager;
    
    #endregion


    public virtual void Initialize()
    {
        if(hasShield)
            ShieldData = new ShieldProperties(transform.GetChild(0).gameObject, 100f);        

        manager = transform.root.GetComponent<EnemyManager>();

        stunned = transform.GetChild(1).gameObject;
        stunned.SetActive(false);                

        Invoke("AddToManager", 1f);
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
        if (Type == EnemyTypes.Droid)
            Invoke("Kill", 3f);
        else
            Invoke("ResetDebuff", 5f);
    }
    void ShieldHit(float _val)
    {
        if (hasShield)
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
                    EMPHit();
                    ShieldHit(20f);
                    missile.Kill();
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
                case MissileType.Emp:
                    HealthData.Damage(1);
                    EMPHit();
                    break;
                case MissileType.ShieldBreak:
                    HealthData.Damage(1);
                    break;
                case MissileType.Chromatic:
                    HealthData.Damage(10);
                    break;
            }
            SendMessage("CheckHealth");
            missile.Kill();
        }
    }
    public void LaserHit(LaserProjectile laser)
    {
        if (hasShield && ShieldData.GetShieldActive())
            ShieldHit(5f);        
        else
        {
            HealthData.Damage(.5f);
            SendMessage("CheckHealth");
        }
        laser.Kill();
    }     
    public void Kill()
    {                     
        manager.RemoveEnemy(GetComponent<EnemyStateManager>());
        Destroy(gameObject);
    }
    #endregion

    #region Public Methods
    public void ResetDebuff()
    {
        Debuff = Impairments.None;
        stunned.SetActive(false);
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