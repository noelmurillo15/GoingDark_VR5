using UnityEngine;

public class ObjectPoolManager : MonoBehaviour {

    #region Properties
    private Transform MyTransform;

    //  Missiles
    private ObjectPooling emp = new ObjectPooling();
    private ObjectPooling basic = new ObjectPooling();
    private ObjectPooling chromatic = new ObjectPooling();
    private ObjectPooling shieldbreak = new ObjectPooling();
    private ObjectPooling enemyMissile = new ObjectPooling();

    //  Enemy 
    private ObjectPooling ammopool = new ObjectPooling();
    private ObjectPooling TrackEnemy = new ObjectPooling();
    private ObjectPooling explosionpool = new ObjectPooling();



    //  Lasers
    private ObjectPooling BasicLasers = new ObjectPooling();
    private ObjectPooling ChargedLasers = new ObjectPooling();
    private ObjectPooling ChargedBall = new ObjectPooling();
    private ObjectPooling BossLaser = new ObjectPooling();
    private ObjectPooling MiniBossLaser = new ObjectPooling();

    private ObjectPooling BasicExplosion = new ObjectPooling();
    private ObjectPooling ChargedLaserExplosion = new ObjectPooling();
    private ObjectPooling BossLaserExplode = new ObjectPooling();
    #endregion

    // Use this for initialization
    void Awake () {
        MyTransform = transform;

        //  Missiles
        emp.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/EmpMissile"), 6, MyTransform);
        basic.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/BasicMissile"), 6, MyTransform);
        chromatic.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/ChromaticMissile"),6, MyTransform);
        shieldbreak.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/ShieldBreakMissile"), 6, MyTransform);
        enemyMissile.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/EnemyMissile"), 15, MyTransform);

        //  Enemy
        ammopool.Initialize(Resources.Load<GameObject>("AmmoDrop"), 10, MyTransform);
        TrackEnemy.Initialize(Resources.Load<GameObject>("Tracer"), 10, MyTransform);
        explosionpool.Initialize(Resources.Load<GameObject>("Projectiles/Explosions/EnemyExplosion"), 12, MyTransform);


        //  Lasers
        BasicLasers.Initialize(Resources.Load<GameObject>("Projectiles/Lasers/LaserBeam"), 28, MyTransform);
        ChargedLasers.Initialize(Resources.Load<GameObject>("Projectiles/Lasers/ChargedShot"), 14, MyTransform);
        ChargedBall.Initialize(Resources.Load<GameObject>("Projectiles/Lasers/ChargedBall"), 8, MyTransform);

        BossLaser.Initialize(Resources.Load<GameObject>("Projectiles/Lasers/BossLaser"), 2, MyTransform);
        MiniBossLaser.Initialize(Resources.Load<GameObject>("Projectiles/Lasers/MiniBossLaser"), 8, MyTransform);
        

        //  Laser Explosion
        BasicExplosion.Initialize(Resources.Load<GameObject>("Projectiles/Explosions/LaserExplosion"), 24, MyTransform);
        ChargedLaserExplosion.Initialize(Resources.Load<GameObject>("Projectiles/Explosions/ChargeLaserExplosion"), 28, MyTransform);
        BossLaserExplode.Initialize(Resources.Load<GameObject>("Projectiles/Explosions/BossLaserExplode"), 8, MyTransform);
    }

    #region Accessors
    public GameObject GetAmmoDrop()
    {
        return ammopool.GetPooledObject();
    }
    public GameObject GetEnemyExplosion()
    {
        return explosionpool.GetPooledObject();
    }

    public GameObject GetEnemyMissile()
    {
        return enemyMissile.GetPooledObject();
    }
    public GameObject GetBossLaser()
    {
        return BossLaser.GetPooledObject();
    }
    public GameObject GetMiniBossLaser()
    {
        return MiniBossLaser.GetPooledObject();
    }
    public GameObject GetBossLaserExplode()
    {
        return BossLaserExplode.GetPooledObject();
    }
    public GameObject GetBaseMissile()
    {
        return basic.GetPooledObject();
    }
    public GameObject GetEmpMissile()
    {
        return emp.GetPooledObject();
    }
    public GameObject GetSBMissile()
    {
        return shieldbreak.GetPooledObject();
    }
    public GameObject GetChromeMissile()
    {
        return chromatic.GetPooledObject();
    }

    public GameObject GetBaseLaser()
    {
        return BasicLasers.GetPooledObject();
    }
    public GameObject GetBaseLaserExplosion()
    {
        return BasicExplosion.GetPooledObject();
    }
    public GameObject GetChargedLaser()
    {
        return ChargedLasers.GetPooledObject();
    }
    public GameObject GetChargeLaserExplosion()
    {
        return ChargedLaserExplosion.GetPooledObject();
    }
    public GameObject GetChargedBall()
    {
        return ChargedBall.GetPooledObject();
    }
    public GameObject GetTrackedEnemy()
    {
        return TrackEnemy.GetPooledObject();
    }
    #endregion
}
