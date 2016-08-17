using UnityEngine;
using GoingDark.Core.Enums;

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
    private ObjectPooling enemyMiniCannonEx = new ObjectPooling();
    private ObjectPooling bossCannonEx = new ObjectPooling();
    #endregion

    // Use this for initialization
    void Awake()
    {
        MyTransform = transform;

        //  Player Missiles
        baseMissile.Initialize(Resources.Load<GameObject>("Missiles/BasicMissile"), 6, MyTransform);
        baseMissileExplode.Initialize(Resources.Load<GameObject>("Explosions/BasicMissileExplosion"), 12, MyTransform);

        empMissile.Initialize(Resources.Load<GameObject>("Missiles/EmpMissile"), 6, MyTransform);
        empMissileExplode.Initialize(Resources.Load<GameObject>("Explosions/EmpExplosion"), 6, MyTransform);

        shieldbreakMissile.Initialize(Resources.Load<GameObject>("Missiles/ShieldBreakMissile"), 6, MyTransform);
        shieldbreakMissileExplode.Initialize(Resources.Load<GameObject>("Explosions/ShieldBreakExplosion"), 6, MyTransform);

        chromeMissile.Initialize(Resources.Load<GameObject>("Missiles/ChromaticMissile"), 6, MyTransform);
        chromeMissileExplode.Initialize(Resources.Load<GameObject>("Explosions/ChromaticExplosion"), 6, MyTransform);


        //  Player Lasers
        baseLaser.Initialize(Resources.Load<GameObject>("Lasers/LaserBeam"), 36, MyTransform);
        baseLaserEx.Initialize(Resources.Load<GameObject>("Explosions/LaserExplosion"), 36, MyTransform);

        chargedLaser.Initialize(Resources.Load<GameObject>("Lasers/ChargedShot"), 20, MyTransform);
        chargedLaserEx.Initialize(Resources.Load<GameObject>("Explosions/ChargedLaserExplosion"), 20, MyTransform);


        //  Enemy Missiles
        enemyMissile.Initialize(Resources.Load<GameObject>("Missiles/EnemyMissile"), 20, MyTransform);

        //  Enemy Lasers
        baseEnemyLaser.Initialize(Resources.Load<GameObject>("Lasers/EnemyLaserBeam"), 96, MyTransform);
        chargedEnemyLaser.Initialize(Resources.Load<GameObject>("Lasers/EnemyChargedShot"), 32, MyTransform);
        miniCannon.Initialize(Resources.Load<GameObject>("Lasers/MiniBossLaser"), 4, MyTransform);
        bossCannon.Initialize(Resources.Load<GameObject>("Lasers/BossLaser"), 3, MyTransform);

        //  Enemy Laser Explosion
        enemyBaseLaserEx.Initialize(Resources.Load<GameObject>("Explosions/EnemyLaserExplosion"), 96, MyTransform);
        enemyChargedLaserEx.Initialize(Resources.Load<GameObject>("Explosions/EnemyChargedLaserExplosion"), 48, MyTransform);
        enemyMiniCannonEx.Initialize(Resources.Load<GameObject>("Explosions/MiniCannonExplosion"), 12, MyTransform);
        bossCannonEx.Initialize(Resources.Load<GameObject>("Explosions/BossLaserExplosion"), 3, MyTransform);

        //  Misc
        explosionpool.Initialize(Resources.Load<GameObject>("Explosions/EnemyExplosion"), 24, MyTransform);
        ammoDrops.Initialize(Resources.Load<GameObject>("AmmoDrop"), 10, MyTransform);
        trackingIcon.Initialize(Resources.Load<GameObject>("Tracer"), 10, MyTransform);
    }

    #region Missiles
    public GameObject GetMissile(MissileType _type)
    {
        switch (_type)
        {
            case MissileType.Basic:
                return baseMissile.GetPooledObject();
            case MissileType.Emp:
                return empMissile.GetPooledObject();
            case MissileType.ShieldBreak:
                return shieldbreakMissile.GetPooledObject();
            case MissileType.Chromatic:
                return chromeMissile.GetPooledObject();
        }
        Debug.LogError("Pool Manager Ran Out Of Player : " + _type + " missiles");
        return null;
    }    
    public GameObject GetMissileExplosion(MissileType _type)
    {
        switch (_type)
        {
            case MissileType.Basic:
                return baseMissileExplode.GetPooledObject();
            case MissileType.Emp:
                return empMissileExplode.GetPooledObject();
            case MissileType.ShieldBreak:
                return shieldbreakMissileExplode.GetPooledObject();
            case MissileType.Chromatic:
                return chromeMissileExplode.GetPooledObject();
        }
        Debug.LogError("Pool Manager Ran Out Of Player : " + _type + " Explosions");
        return null;
    }


    public GameObject GetMissile(EnemyMissileType _type)
    {
        switch (_type)
        {
            case EnemyMissileType.Basic:
                return enemyMissile.GetPooledObject();
        }

        Debug.LogError("Pool Manager Ran Out Of Enemy : " + _type + " Missile");
        return null;
    }
    #endregion

    #region Lasers
    public GameObject GetLaser(LaserType _type)
    {
        switch (_type)
        {
            case LaserType.Basic:
                return baseLaser.GetPooledObject();
            case LaserType.Charged:
                return chargedLaser.GetPooledObject();
        }

        Debug.LogError("Pool Manager Ran Out Of Player : " + _type + " Laser");
        return null;
    }

    public GameObject GetLaserExplosion(LaserType _type)
    {
        switch (_type)
        {
            case LaserType.Basic:
                return baseLaserEx.GetPooledObject();
            case LaserType.Charged:
                return chargedLaserEx.GetPooledObject();
        }

        Debug.LogError("Pool Manager Ran Out Of Player : " + _type + " Laser Explosions");
        return null;
    }


    public GameObject GetLaser(EnemyLaserType _type)
    {
        switch (_type)
        {
            case EnemyLaserType.Basic:
                return baseEnemyLaser.GetPooledObject();
            case EnemyLaserType.Charged:
                return chargedEnemyLaser.GetPooledObject();
            case EnemyLaserType.MiniCannon:
                return miniCannon.GetPooledObject();
            case EnemyLaserType.Cannon:
                return bossCannon.GetPooledObject();
        }

        Debug.LogError("Pool Manager Ran Out Of Enemy : " + _type + " Laser");
        return null;
    }
    public GameObject GetLaserExplosion(EnemyLaserType _type)
    {
        switch (_type)
        {
            case EnemyLaserType.Basic:
                return enemyBaseLaserEx.GetPooledObject();
            case EnemyLaserType.Charged:
                return enemyChargedLaserEx.GetPooledObject();
            case EnemyLaserType.MiniCannon:
                return enemyMiniCannonEx.GetPooledObject();
            case EnemyLaserType.Cannon:
                return bossCannonEx.GetPooledObject();
        }

        Debug.LogError("Pool Manager Ran Out Of Enemy : " + _type + " Laser Explosions");
        return null;
    }
    #endregion

    #region Misc
    public GameObject GetEnemyExplosion()
    {
        return explosionpool.GetPooledObject();
    }
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