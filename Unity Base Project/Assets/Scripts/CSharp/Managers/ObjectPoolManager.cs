using UnityEngine;

public class ObjectPoolManager : MonoBehaviour {


    #region Player
    //  Missiles
    private ObjectPooling empMissile = new ObjectPooling();
    private ObjectPooling baseMissile = new ObjectPooling();
    private ObjectPooling chromeMissile = new ObjectPooling();
    private ObjectPooling shieldbreakMissile = new ObjectPooling();

    //  Lasers
    private ObjectPooling baseLaser = new ObjectPooling();    
    private ObjectPooling ballLaser = new ObjectPooling();
    private ObjectPooling chargedLaser = new ObjectPooling();

    //  Explosions
    private ObjectPooling baseLaserEx = new ObjectPooling();
    private ObjectPooling chargedLaserEx = new ObjectPooling();
    #endregion

    #region Enemy
    private ObjectPooling ammoDrops = new ObjectPooling();
    private ObjectPooling trackingIcon = new ObjectPooling();
    private ObjectPooling explosionpool = new ObjectPooling();

    //  Missiles
    private ObjectPooling enemyMissile = new ObjectPooling();

    //  Lasers
    private ObjectPooling bossCannon = new ObjectPooling();
    private ObjectPooling miniCannon = new ObjectPooling();
    private ObjectPooling baseEnemyLaser = new ObjectPooling();

    //  Explosions
    private ObjectPooling baseEnemyLaserEx = new ObjectPooling();
    #endregion
    private Transform MyTransform;

    // Use this for initialization
    void Awake () {
        MyTransform = transform;

        //  Missiles
        empMissile.Initialize(Resources.Load<GameObject>("Missiles/EmpMissile"), 6, MyTransform);
        baseMissile.Initialize(Resources.Load<GameObject>("Missiles/BasicMissile"), 6, MyTransform);
        chromeMissile.Initialize(Resources.Load<GameObject>("Missiles/ChromaticMissile"),6, MyTransform);
        shieldbreakMissile.Initialize(Resources.Load<GameObject>("Missiles/ShieldBreakMissile"), 6, MyTransform);
        enemyMissile.Initialize(Resources.Load<GameObject>("Missiles/EnemyMissile"), 20, MyTransform);

        //  Lasers
        baseLaser.Initialize(Resources.Load<GameObject>("Lasers/LaserBeam"), 18, MyTransform);
        chargedLaser.Initialize(Resources.Load<GameObject>("Lasers/ChargedShot"), 14, MyTransform);
        ballLaser.Initialize(Resources.Load<GameObject>("Lasers/ChargedBall"), 8, MyTransform);

        baseEnemyLaser.Initialize(Resources.Load<GameObject>("Lasers/EnemyLaserBeam"), 32, MyTransform);
        bossCannon.Initialize(Resources.Load<GameObject>("Lasers/BossLaser"), 2, MyTransform);
        miniCannon.Initialize(Resources.Load<GameObject>("Lasers/MiniBossLaser"), 8, MyTransform);        

        //  Explosions
        baseLaserEx.Initialize(Resources.Load<GameObject>("Explosions/LaserExplosion"), 24, MyTransform);
        chargedLaserEx.Initialize(Resources.Load<GameObject>("Explosions/ChargeLaserExplosion"), 28, MyTransform);
        baseEnemyLaserEx.Initialize(Resources.Load<GameObject>("Explosions/BossLaserExplode"), 8, MyTransform);
        explosionpool.Initialize(Resources.Load<GameObject>("Explosions/EnemyExplosion"), 12, MyTransform);

        //  Misc
        ammoDrops.Initialize(Resources.Load<GameObject>("AmmoDrop"), 10, MyTransform);
        trackingIcon.Initialize(Resources.Load<GameObject>("Tracer"), 10, MyTransform);
    }

    #region Missiles
    //  Players
    public GameObject GetEmpMissile()
    {
        return empMissile.GetPooledObject();
    }
    public GameObject GetBaseMissile()
    {
        return baseMissile.GetPooledObject();
    }
    public GameObject GetSBMissile()
    {
        return shieldbreakMissile.GetPooledObject();
    }
    public GameObject GetChromeMissile()
    {
        return chromeMissile.GetPooledObject();
    }

    //  Enemy
    public GameObject GetEnemyMissile()
    {
        return enemyMissile.GetPooledObject();
    }
    #endregion

    #region Lasers
    //  Player
    public GameObject GetBaseLaser()
    {
        return baseLaser.GetPooledObject();
    }
    public GameObject GetChargedLaser()
    {
        return chargedLaser.GetPooledObject();
    }
    public GameObject GetChargedBall()
    {
        return ballLaser.GetPooledObject();
    }

    //  Enemy
    public GameObject GetBaseEnemyLaser()
    {
        return baseEnemyLaser.GetPooledObject();
    }
    public GameObject GetMiniBossLaser()
    {
        return miniCannon.GetPooledObject();
    }
    public GameObject GetBossLaser()
    {
        return bossCannon.GetPooledObject();
    }
    #endregion

    #region Explosions
    //  Player
    public GameObject GetBaseLaserExplosion()
    {
        return baseLaserEx.GetPooledObject();
    }
    public GameObject GetChargeLaserExplosion()
    {
        return chargedLaserEx.GetPooledObject();
    }

    //  Enemy
    public GameObject GetEnemyExplosion()
    {
        return explosionpool.GetPooledObject();
    }
    public GameObject GetBossLaserExplode()
    {
        return baseEnemyLaserEx.GetPooledObject();
    }
    #endregion

    #region Misc
    public GameObject GetAmmoDrop()
    {
        return ammoDrops.GetPooledObject();
    }

    public GameObject GetTrackedEnemy()
    {
        return trackingIcon.GetPooledObject();
    }
    #endregion
}
