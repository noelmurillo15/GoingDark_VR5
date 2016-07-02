using UnityEngine;
using GoingDark.Core.Enums;

public class IEnemy : MonoBehaviour {

    #region Properties
    public Transform Target { get; protected set; }
    public Transform MyTransform { get; private set; }
    public Vector3 LastKnownPos { get; protected set; }

    public EnemyTypes Type = EnemyTypes.None;
    public Impairments Debuff = Impairments.None;

    private MovementProperties MoveData;
    private ShieldProperties ShieldData;
    private HealthProperties HealthData;

    private GameObject stunned;

    private EnemyManager manager;
    #endregion


    public virtual void Initialize()
    {
        MoveData = new MovementProperties();
        ShieldData = new ShieldProperties();
        HealthData = new HealthProperties();

        Target = null;
        MyTransform = transform;
        stunned = transform.GetChild(1).gameObject;
        stunned.SetActive(false);                

        Invoke("AddToManager", 1f);
    }

    #region Accessors
    public MovementProperties GetMoveData()
    {
        return MoveData;
    }
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
        manager = transform.parent.GetComponent<EnemyManager>();
        manager.AddEnemy(MyTransform.GetComponent<EnemyBehavior>());
        GetComponent<EnemyCollision>().SetManagerRef(manager);
        LoadEnemyData();
    }
    
    void EMPHit()
    {
        stunned.SetActive(true);
        Debuff = Impairments.Stunned;
        Invoke("ResetDebuff", 3f);
        if (Type == EnemyTypes.Droid)
            Invoke("Kill", 3f);
    }
    void ShieldHit(float _val)
    {
        ShieldData.TakeDamage(_val);        
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
            missile.Kill();
        }
    }
    public void LaserHit(LaserProjectile laser)
    {
        if (ShieldData.GetShieldActive())
        {
            ShieldHit(5f);
        }
        else
        {
            HealthData.Damage(1);
        }
        laser.Kill();
    }     
    void Kill()
    {        
        GameObject explosive = manager.GetEnemyExplosion();
        explosive.transform.position = transform.position;        
        explosive.SetActive(true);        

        manager.RemoveEnemy(GetComponent<EnemyBehavior>());
        Destroy(gameObject);
    }
    #endregion

    #region Public Methods
    public void ResetDebuff()
    {
        SetSpeedBoost(1f);
        Debuff = Impairments.None;
        stunned.SetActive(false);
    }
    public void SetSpeedBoost(float newBoost)
    {
        MoveData.Boost = newBoost;
    }
    #endregion

    #region Private Methods
    void LoadEnemyData()
    {
        switch (manager.Difficulty)
        {
            #region Easy
            case GameDifficulty.Easy:
                switch (Type)
                {
                    case EnemyTypes.Basic:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 60f, 2f, 10f);
                        HealthData.Set(10, MyTransform);
                        break;
                    case EnemyTypes.Droid:
                        MoveData.Set(0f, .5f, 110f, 1f, 10f);
                        HealthData.Set(5, MyTransform);
                        break;
                    case EnemyTypes.Transport:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 120f, 3f, 10f);
                        HealthData.Set(15, MyTransform);
                        break;
                    case EnemyTypes.Trident:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 80f, 1.8f, 10f);
                        HealthData.Set(10, MyTransform);
                        break;
                    case EnemyTypes.Boss:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 50f, 5f, 10f);
                        HealthData.Set(50, MyTransform);
                        break;
                }
                break;
            #endregion

            #region Normal
            case GameDifficulty.Normal:
                switch (Type)
                {
                    case EnemyTypes.Basic:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 90f, 1.8f, 15f);
                        HealthData.Set(25, MyTransform);
                        break;
                    case EnemyTypes.Droid:
                        MoveData.Set(0f, .5f, 120f, .8f, 20f);
                        HealthData.Set(15, MyTransform);
                        break;
                    case EnemyTypes.Transport:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 150f, 2.5f, 15f);
                        HealthData.Set(30, MyTransform);
                        break;
                    case EnemyTypes.Trident:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 95f, 1.5f, 18f);
                        HealthData.Set(20, MyTransform);
                        break;
                    case EnemyTypes.Boss:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 60f, 4f, 15f);
                        HealthData.Set(100, MyTransform);
                        break;
                }
                break;
            #endregion

            #region Hard
            case GameDifficulty.Hard:
                switch (Type)
                {
                    case EnemyTypes.Basic:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 110f, 1.6f, 25f);
                        HealthData.Set(40, MyTransform);
                        break;
                    case EnemyTypes.Droid:
                        MoveData.Set(0f, .5f, 160f, .7f, 40f);
                        HealthData.Set(25, MyTransform);
                        break;
                    case EnemyTypes.Transport:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 200f, 2f, 25f);
                        HealthData.Set(50, MyTransform);
                        break;
                    case EnemyTypes.Trident:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 120f, 1.2f, 30f);
                        HealthData.Set(35, MyTransform);
                        break;
                    case EnemyTypes.Boss:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 50f, 5f, 10f);
                        HealthData.Set(150, MyTransform);
                        break;
                }
                break;
            #endregion

            #region Nightmare
            case GameDifficulty.Nightmare:
                switch (Type)
                {
                    case EnemyTypes.Basic:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 150f, 1.4f, 40f);
                        HealthData.Set(80, MyTransform);
                        break;
                    case EnemyTypes.Droid:
                        MoveData.Set(0f, .5f, 200f, .6f, 50f);
                        HealthData.Set(50, MyTransform);
                        break;
                    case EnemyTypes.Transport:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 250f, 1.8f, 30f);
                        HealthData.Set(100, MyTransform);
                        break;
                    case EnemyTypes.Trident:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 160f, 1f, 45f);
                        HealthData.Set(70, MyTransform);
                        break;
                    case EnemyTypes.Boss:
                        ShieldData.Initialize(transform.GetChild(0).gameObject, 100f);
                        MoveData.Set(0f, .5f, 50f, 5f, 10f);
                        HealthData.Set(300, MyTransform);
                        break;
                }
                break;
            #endregion
        }  
    }
    #endregion 
}