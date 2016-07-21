using UnityEngine;

public class ObjectPoolManager : MonoBehaviour {

    #region Properties
    private Transform MyTransform;

    private ObjectPooling emp = new ObjectPooling();
    private ObjectPooling basic = new ObjectPooling();
    private ObjectPooling chromatic = new ObjectPooling();
    private ObjectPooling shieldbreak = new ObjectPooling();

    private ObjectPooling ammopool = new ObjectPooling();
    private ObjectPooling laserpool = new ObjectPooling();
    private ObjectPooling missilepool = new ObjectPooling();
    private ObjectPooling explosionpool = new ObjectPooling();

    private ObjectPooling BasicLasers = new ObjectPooling();
    private ObjectPooling ChargedLasers = new ObjectPooling();
    private ObjectPooling BasicExplosion = new ObjectPooling();
    private ObjectPooling ChargedExplosion = new ObjectPooling();
    private ObjectPooling TrackEnemy = new ObjectPooling();
    #endregion

    // Use this for initialization
    void Start () {
        MyTransform = transform;

        //  Missiles
        emp.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/EmpMissile"), 4, MyTransform);
        basic.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/BasicMissile"), 4, MyTransform);
        chromatic.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/ChromaticMissile"), 4, MyTransform);
        shieldbreak.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/ShieldBreakMissile"), 4, MyTransform);

        //  Enemy
        ammopool.Initialize(Resources.Load<GameObject>("AmmoDrop"), 10, MyTransform);
        //laserpool.Initialize(Resources.Load<GameObject>("Projectiles/Lasers/EnemyLaser"), 30, MyTransform);
        missilepool.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/EnemyMissile"), 25, MyTransform);
        explosionpool.Initialize(Resources.Load<GameObject>("Projectiles/Explosions/EnemyExplosion"), 10, MyTransform);        

        //  Lasers
        BasicLasers.Initialize(Resources.Load<GameObject>("Projectiles/Lasers/LaserBeam"), 15, MyTransform);
        ChargedLasers.Initialize(Resources.Load<GameObject>("Projectiles/Lasers/ChargedShot"), 15, MyTransform);
        BasicExplosion.Initialize(Resources.Load<GameObject>("Projectiles/Explosions/BasicExplosion"), 8, MyTransform);
        ChargedExplosion.Initialize(Resources.Load<GameObject>("Projectiles/Explosions/ChargeLaserExplosion"), 8, MyTransform);

        //track enemy
        TrackEnemy.Initialize(Resources.Load<GameObject>("Tracer"), 10, MyTransform);
    }

    #region Accessors
    //  Enemy
    public GameObject GetAmmoDrop()
    {
        return ammopool.GetPooledObject();
    }
    public GameObject GetEnemyLaser()
    {
        return laserpool.GetPooledObject();
    }
    public GameObject GetEnemyMissile()
    {
        return missilepool.GetPooledObject();
    }
    public GameObject GetEnemyExplosion()
    {
        return explosionpool.GetPooledObject();
    }

    //  Missiles
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

    //  Lasers
    public GameObject GetBaseLaser()
    {
        return BasicLasers.GetPooledObject();
    }
    public GameObject GetBaseLaserExplosion()
    {
        return BasicExplosion.GetPooledObject();
    }
    public GameObject GetTrackedEnemy()
    {
        return TrackEnemy.GetPooledObject();
    }

    #endregion
}
