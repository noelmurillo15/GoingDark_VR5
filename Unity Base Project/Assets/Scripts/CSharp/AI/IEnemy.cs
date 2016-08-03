using UnityEngine;
using GoingDark.Core.Enums;

public class IEnemy : MonoBehaviour
{

    #region Properties
    public EnemyTypes Type = EnemyTypes.None;
    public Impairments Debuff = Impairments.None;

    [SerializeField]
    private GameObject stunned;

    private EnemyManager manager;    
    private HealthProperties HealthData;
    #endregion


    public virtual void Initialize()
    {           
        stunned.SetActive(false);
        Invoke("AddToManager", 2f);
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
        missile.Kill();
    }
    public void LaserHit(LaserProjectile laser)
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
    }
    
    public void Kill()
    {
        GetComponent<EnemyTrail>().Kill();
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
                        HealthData = new HealthProperties(6, transform); break;
                    case EnemyTypes.Transport:
                        HealthData = new HealthProperties(15, transform); break;
                    case EnemyTypes.Trident:
                        HealthData = new HealthProperties(10, transform); break;
                    case EnemyTypes.Tank:
                        HealthData = new HealthProperties(50, transform); break;
                    case EnemyTypes.FinalBoss:
                        HealthData = new HealthProperties(100, transform); break;
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
                    case EnemyTypes.Tank:
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
                    case EnemyTypes.Tank:
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
                    case EnemyTypes.Tank:
                        HealthData = new HealthProperties(300, transform); break;
                }
                break;
                #endregion
        }
    }
    #endregion 
}