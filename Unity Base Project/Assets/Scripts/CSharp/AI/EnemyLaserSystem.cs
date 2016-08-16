using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyLaserSystem : MonoBehaviour {

    #region Properties
    [SerializeField]
    private EnemyLaserType Type;

    private float fireRate;
    private float maxFireRate;

    private IEnemy enemyStats;
    private Transform MyTransform;
    private ObjectPoolManager poolManager;
    private EnemyStateManager stateManager;
    #endregion


    void Start()
    {
        switch (Type)
        {
            case EnemyLaserType.Basic:
                maxFireRate = .5f;
                break;
            case EnemyLaserType.Charged:
                maxFireRate = 1f;
                break;
            case EnemyLaserType.MiniCannon:
                maxFireRate = 5f;
                break;
        }
        MyTransform = transform;
        enemyStats = GetComponent<IEnemy>();
        stateManager = MyTransform.GetComponentInParent<EnemyStateManager>();
        poolManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();
    }

    void FixedUpdate()
    {
        if (fireRate > 0f)
            fireRate -= Time.fixedDeltaTime;

        if (stateManager.Target != null)
            LockOn();        
    }

    private void LockOn()
    {
        Vector3 playerDir = stateManager.Target.position - MyTransform.position;
        Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.fixedDeltaTime * 5f, 0.0f);
        MyTransform.rotation = Quaternion.LookRotation(direction);

        if (fireRate <= 0f)
            Shoot();
    }

    public void Shoot()
    {
        fireRate = maxFireRate;
        if (enemyStats.GetDebuffData() != Impairments.Stunned)
        {
            GameObject obj = null;
            switch (Type)
            {
                case EnemyLaserType.Basic:
                    obj = poolManager.GetBaseEnemyLaser();
                    break;
                case EnemyLaserType.Charged:
                    obj = poolManager.GetChargedEnemyLaser();
                    break;
                case EnemyLaserType.MiniCannon:
                    obj = poolManager.GetMiniBossLaser();
                    break;
                case EnemyLaserType.Cannon:
                    Debug.LogError("Enemy Laser System Should not shoot cannons");
                    break;
            }

            if (obj != null)
            {
                obj.transform.position = MyTransform.position;
                obj.transform.rotation = MyTransform.rotation;
                obj.SetActive(true);
            }
            else
                Debug.LogError("Enemy Ran Out of Lasers : " + Type.ToString());
        }
    }
}