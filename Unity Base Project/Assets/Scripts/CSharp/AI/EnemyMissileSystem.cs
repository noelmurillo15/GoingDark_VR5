using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyMissileSystem : MonoBehaviour {

    #region Properties
    [SerializeField]
    private EnemyMissileType Type;

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
            case EnemyMissileType.Basic:
                maxFireRate = 5f;
                break;
        }
        MyTransform = transform;
        enemyStats = GetComponent<IEnemy>();
        stateManager = transform.GetComponentInParent<EnemyStateManager>();
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

    private void Shoot()
    {
        fireRate = maxFireRate;
        if (enemyStats.GetDebuffData() != Impairments.Stunned)
        {
            GameObject obj = null;
            switch (Type)
            {
                case EnemyMissileType.Basic:
                    obj = poolManager.GetEnemyMissile();
                    break;
            }
            if (obj != null)
            {
                obj.transform.position = MyTransform.position;
                obj.transform.rotation = MyTransform.rotation;
                obj.SetActive(true);
            }
            else
                Debug.LogError("Enemy Ran Out of Missiles : " + Type.ToString());
        }
    }
}