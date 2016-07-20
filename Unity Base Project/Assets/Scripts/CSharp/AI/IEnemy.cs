using UnityEngine;
using GoingDark.Core.Enums;

public class IEnemy : MonoBehaviour {

    #region Properties
    public Transform Target { get; protected set; }
    public Transform MyTransform { get; private set; }
    public Vector3 LastKnownPos { get; protected set; }

    public EnemyTypes Type = EnemyTypes.None;
    public Impairments Debuff = Impairments.None;
    
    private ShieldProperties ShieldData;
    private HealthProperties HealthData;

    private GameObject stunned;
    private EnemyManager manager;
    private ObjectPoolManager poolmanager;
    #endregion


    public virtual void Initialize()
    {
        Target = null;
        MyTransform = transform;

        poolmanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();
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
    public ObjectPoolManager GetPoolManager()
    {
        return poolmanager;
    }
    #endregion

    #region Msg Functions
    void AddToManager()
    {
        manager = transform.root.GetComponent<EnemyManager>();
        manager.AddEnemy(MyTransform.GetComponent<EnemyStateManager>());
        GetComponent<EnemyCollision>().SetManagerRef(manager);
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
        ShieldData.Damage(_val);        
    }
    void SplashDmg()
    {
        if (!ShieldData.GetShieldActive())
        {
            Debug.Log("Enemy affected by splash dmg");
            HealthData.Damage(2);
            SendMessage("CheckHealth");
        }
    }
    public void MissileHit(Missile missile)
    {        
        if (ShieldData.GetShieldActive())
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
        if (ShieldData.GetShieldActive())
            ShieldHit(5f);        
        else
        {
            HealthData.Damage(.5f);
            SendMessage("CheckHealth");
        }
        laser.Kill();
    }     
    void Kill()
    {        
        GameObject explosive = poolmanager.GetEnemyExplosion();
        explosive.transform.position = transform.position;        
        explosive.SetActive(true);        

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
                        HealthData = new HealthProperties(10, MyTransform); break;
                    case EnemyTypes.Droid:
                        HealthData = new HealthProperties(5, MyTransform); break;
                    case EnemyTypes.Transport:
                        HealthData = new HealthProperties(15, MyTransform); break;
                    case EnemyTypes.Trident:
                        HealthData = new HealthProperties(10, MyTransform); break;
                    case EnemyTypes.Boss:
                        HealthData = new HealthProperties(50, MyTransform); break;
                }
                break;
            #endregion

            #region Normal
            case GameDifficulty.Normal:
                switch (Type)
                {
                    case EnemyTypes.Basic:
                        HealthData = new HealthProperties(25, MyTransform); break;
                    case EnemyTypes.Droid:
                        HealthData = new HealthProperties(15, MyTransform); break;
                    case EnemyTypes.Transport:
                        HealthData = new HealthProperties(30, MyTransform); break;
                    case EnemyTypes.Trident:
                        HealthData = new HealthProperties(20, MyTransform); break;
                    case EnemyTypes.Boss:
                        HealthData = new HealthProperties(100, MyTransform); break;
                }
                break;
            #endregion

            #region Hard
            case GameDifficulty.Hard:
                switch (Type)
                {
                    case EnemyTypes.Basic:
                        HealthData = new HealthProperties(40, MyTransform); break;
                    case EnemyTypes.Droid:
                        HealthData = new HealthProperties(25, MyTransform); break;
                    case EnemyTypes.Transport:
                        HealthData = new HealthProperties(50, MyTransform); break;
                    case EnemyTypes.Trident:
                        HealthData = new HealthProperties(35, MyTransform); break;
                    case EnemyTypes.Boss:
                        HealthData = new HealthProperties(200, MyTransform); break;
                }
                break;
            #endregion

            #region Nightmare
            case GameDifficulty.Nightmare:
                switch (Type)
                {
                    case EnemyTypes.Basic:
                        HealthData = new HealthProperties(80, MyTransform); break;
                    case EnemyTypes.Droid:
                        HealthData = new HealthProperties(50, MyTransform); break;
                    case EnemyTypes.Transport:
                        HealthData = new HealthProperties(100, MyTransform); break;
                    case EnemyTypes.Trident:
                        HealthData = new HealthProperties(70, MyTransform); break;
                    case EnemyTypes.Boss:
                        HealthData = new HealthProperties(300, MyTransform); break;
                }
                break;
            #endregion
        }  
    }
    #endregion 
}