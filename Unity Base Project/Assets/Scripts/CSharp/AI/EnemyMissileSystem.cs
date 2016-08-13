using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyMissileSystem : MonoBehaviour
{
    #region Properties
    [SerializeField]
    private EnemyMissileType Type;

    private float fireRate;

    private IEnemy stats;
    private Transform MyTransform;
    private EnemyStateManager behavior;
    private ObjectPoolManager poolManager;
    #endregion


    void Start()
    {
        switch (Type)
        {
            case EnemyMissileType.Basic:
                fireRate = 5f;
                break;
        }
        MyTransform = transform;
        stats = transform.GetComponentInParent<IEnemy>();
        behavior = transform.GetComponentInParent<EnemyStateManager>();
        poolManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();
    }

    void FixedUpdate()
    {
        if (behavior.Target != null)
        {
            if (!IsInvoking("Fire"))
                InvokeRepeating("Fire", 3f, fireRate);

            LockOn();
        }
        else
        {
            if (IsInvoking("Fire"))
                CancelInvoke();
        }
    }

    private void LockOn()
    {
        Vector3 playerDir = behavior.Target.position - MyTransform.position;
        Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.fixedDeltaTime * 5f, 0.0f);
        MyTransform.rotation = Quaternion.LookRotation(direction);
    }

    private void Fire()
    {
        GameObject miss = null;
        switch (Type)
        {
            case EnemyMissileType.Basic:
                miss = poolManager.GetEnemyMissile();
                break;
        }
        if (miss != null)
        {
            miss.transform.position = MyTransform.position;
            miss.transform.rotation = MyTransform.rotation;
            miss.SetActive(true);
        }
        else
            Debug.LogError("Enemy Ran Out of Missiles : " + Type.ToString());
    }
}