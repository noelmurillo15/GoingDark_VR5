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
                maxFireRate = .75f;
                break;
            case EnemyLaserType.Charged:
                maxFireRate = 1.5f;
                break;
            case EnemyLaserType.MiniCannon:
                maxFireRate = 8f;
                break;
        }
        MyTransform = transform;
        enemyStats = MyTransform.GetComponentInParent<IEnemy>();
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
        Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.fixedDeltaTime * 30f, 15.0f);
        MyTransform.rotation = Quaternion.LookRotation(direction);

        if (fireRate <= 0f)
            Shoot();
    }

    public void Shoot()
    {
        fireRate = maxFireRate;
        if (enemyStats.GetDebuffData() != Impairments.Stunned)
        {
            GameObject obj = poolManager.GetLaser(Type);

            if (obj != null)
            {
                obj.transform.position = MyTransform.position;
                obj.transform.rotation = MyTransform.rotation;
                obj.SetActive(true);
            }
        }
    }
}