using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyLaserSystem : MonoBehaviour
{

    #region Properties
    [SerializeField]
    private EnemyLaserType Type;
    [SerializeField]
    private float fireRate;

    private Transform MyTransform;
    private ObjectPoolManager pool;
    private EnemyStateManager behavior;
    #endregion


    void Start()
    {
        MyTransform = transform;
        behavior = MyTransform.GetComponentInParent<EnemyStateManager>();
        pool = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();
    }

    void FixedUpdate()
    {
        if (behavior.Target != null)
        {
            if (!IsInvoking("Shoot"))
                InvokeRepeating("Shoot", 2f, fireRate);

            LockOn();
        }
        else
        {
            if(IsInvoking("Shoot"))
                CancelInvoke();
        }
    }

    private void LockOn()
    {
        Vector3 playerDir = behavior.Target.position - MyTransform.position;
        Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.fixedDeltaTime * 5f, 0.0f);
        MyTransform.rotation = Quaternion.LookRotation(direction);
    }

    public void Shoot()
    {
        GameObject obj = null;
        switch (Type)
        {
            case EnemyLaserType.Basic:
                obj = pool.GetBaseEnemyLaser();
                break;
            case EnemyLaserType.Charged:
                obj = pool.GetChargedEnemyLaser();
                break;
            case EnemyLaserType.MiniCannon:
                obj = pool.GetMiniBossLaser();
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