using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{

    private Transform MyTransform;

    #region Player
    //  Missiles
    private ObjectPooling empMissile = new ObjectPooling();
    private ObjectPooling baseMissile = new ObjectPooling();
    private ObjectPooling chromeMissile = new ObjectPooling();
    private ObjectPooling shieldbreakMissile = new ObjectPooling();

    //  Missile Explosions
    private ObjectPooling empMissileExplode = new ObjectPooling();
    private ObjectPooling baseMissileExplode = new ObjectPooling();
    private ObjectPooling chromeMissileExplode = new ObjectPooling();
    private ObjectPooling shieldbreakMissileExplode = new ObjectPooling();

    //  Lasers
    private ObjectPooling baseLaser = new ObjectPooling();
    private ObjectPooling chargedLaser = new ObjectPooling();

    //  Laser Explosions
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
    private ObjectPooling baseEnemyLaser = new ObjectPooling();
    private ObjectPooling chargedEnemyLaser = new ObjectPooling();
    private ObjectPooling miniCannon = new ObjectPooling();
    private ObjectPooling bossCannon = new ObjectPooling();

    //  Explosions
    private ObjectPooling enemyBaseLaserEx = new ObjectPooling();
    private ObjectPooling enemyChargedLaserEx = new ObjectPooling();
    private ObjectPooling bossLaserEx = new ObjectPooling();
    #endregion

    // Use this for initialization
    void Awake()
    {
        MyTransform = transform;

        //  Player Missiles
        empMissile.Initialize(Resources.Load<GameObject>("Missiles/EmpMissile"), 6, MyTransform);
        baseMissile.Initialize(Resources.Load<GameObject>("Missiles/BasicMissile"), 6, MyTransform);
        chromeMissile.Initialize(Resources.Load<GameObject>("Missiles/ChromaticMissile"), 6, MyTransform);
        shieldbreakMissile.Initialize(Resources.Load<GameObject>("Missiles/ShieldBreakMissile"), 6, MyTransform);

        //  Player Missile Explosions
        baseMissileExplode.Initialize(Resources.Load<GameObject>("Explosions/BasicExplosion"), 12, MyTransform);
        empMissileExplode.Initialize(Resources.Load<GameObject>("Explosions/EmpExplosion"), 4, MyTransform);
        chromeMissileExplode.Initialize(Resources.Load<GameObject>("Explosions/ChromaticExplosion"), 4, MyTransform);
        shieldbreakMissileExplode.Initialize(Resources.Load<GameObject>("Explosions/ShieldBreakExplosion"), 4, MyTransform);

        //  Player Lasers
        baseLaser.Initialize(Resources.Load<GameObject>("Lasers/LaserBeam"), 36, MyTransform);
        chargedLaser.Initialize(Resources.Load<GameObject>("Lasers/ChargedShot"), 20, MyTransform);

        //  Player Laser Explosion
        baseLaserEx.Initialize(Resources.Load<GameObject>("Explosions/LaserExplosion"), 36, MyTransform);
        chargedLaserEx.Initialize(Resources.Load<GameObject>("Explosions/ChargeLaserExplosion"), 20, MyTransform);



        //  Enemy Missiles
        enemyMissile.Initialize(Resources.Load<GameObject>("Missiles/EnemyMissile"), 20, MyTransform);

        //  Enemy Lasers
        baseEnemyLaser.Initialize(Resources.Load<GameObject>("Lasers/EnemyLaserBeam"), 64, MyTransform);
        chargedEnemyLaser.Initialize(Resources.Load<GameObject>("Lasers/EnemyChargedShot"), 32, MyTransform);
        miniCannon.Initialize(Resources.Load<GameObject>("Lasers/MiniBossLaser"), 4, MyTransform);
        bossCannon.Initialize(Resources.Load<GameObject>("Lasers/BossLaser"), 2, MyTransform);

        //  Enemy Laser Explosion
        enemyBaseLaserEx.Initialize(Resources.Load<GameObject>("Explosions/EnemyLaserExplosion"), 48, MyTransform);
        enemyChargedLaserEx.Initialize(Resources.Load<GameObject>("Explosions/EnemyChargedLaserExplosion"), 48, MyTransform);
        bossLaserEx.Initialize(Resources.Load<GameObject>("Explosions/BossLaserExplode"), 2, MyTransform);

        //  Misc
        explosionpool.Initialize(Resources.Load<GameObject>("Explosions/EnemyExplosion"), 12, MyTransform);
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

    //  Player Explosions
    public GameObject GetBaseMissExplode()
    {
        return baseMissileExplode.GetPooledObject();
    }
    public GameObject GetEmpMissExplode()
    {
        return empMissileExplode.GetPooledObject();
    }
    public GameObject GetSBMissExplode()
    {
        return shieldbreakMissileExplode.GetPooledObject();
    }
    public GameObject GetChromeMissExplode()
    {        
        return chromeMissileExplode.GetPooledObject();
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

    //  Enemy
    public GameObject GetBaseEnemyLaser()
    {
        return baseEnemyLaser.GetPooledObject();
    }
    public GameObject GetChargedEnemyLaser()
    {
        return chargedEnemyLaser.GetPooledObject();
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
    public GameObject GetEnemyBaseLaserExplode()
    {
        return enemyBaseLaserEx.GetPooledObject();
    }
    public GameObject GetEnemyChargedLaserExplode()
    {
        return enemyChargedLaserEx.GetPooledObject();
    }
    public GameObject GetBossLaserExplode()
    {
        return bossLaserEx.GetPooledObject();
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