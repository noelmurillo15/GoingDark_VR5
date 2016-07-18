using UnityEngine;

public class ObjectPoolManager : MonoBehaviour {

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

    // Use this for initialization
    void Start () {
        ammopool.Initialize(Resources.Load<GameObject>("AmmoDrop"), 10, transform);
        laserpool.Initialize(Resources.Load<GameObject>("Projectiles/Lasers/EnemyLaser"), 30, transform);
        missilepool.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/EnemyMissile"), 30, transform);
        explosionpool.Initialize(Resources.Load<GameObject>("Projectiles/Explosions/EnemyExplosion"), 40, transform);

        emp.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/EmpMissile"), 4, transform);
        basic.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/BasicMissile"), 4, transform);
        chromatic.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/ChromaticMissile"), 4, transform);
        shieldbreak.Initialize(Resources.Load<GameObject>("Projectiles/Missiles/ShieldBreakMissile"), 4, transform);

        BasicLasers.Initialize(Resources.Load<GameObject>("Projectiles/Lasers/LaserBeam"), 12, transform);
        ChargedLasers.Initialize(Resources.Load<GameObject>("Projectiles/Lasers/ChargedShot"), 12, transform);
        BasicExplosion.Initialize(Resources.Load<GameObject>("Projectiles/Explosions/BasicExplosion"), 6, transform);
        ChargedExplosion.Initialize(Resources.Load<GameObject>("Projectiles/Explosions/ChargeLaserExplosion"), 6, transform);
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
    #endregion
}
