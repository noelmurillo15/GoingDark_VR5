using UnityEngine;

public class Turret : MonoBehaviour
{
    //  Enemy Data
    Transform MyTransform;
    private EnemyStateManager behavior;
    private bool lockedOn;
    private float randomShot;
    private ObjectPoolManager pool;
    // Use this for initialization
    void Start()
    {
        MyTransform = transform;
        randomShot = 20f;
        lockedOn = false;

        behavior = transform.parent.GetComponentInParent<EnemyStateManager>();
        pool = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();

        InvokeRepeating("DestroyPlayer", 5f, randomShot);
    }

    private void LockOn()
    {
        if (behavior.Target != null)
        {
            Vector3 playerDir = behavior.Target.position - MyTransform.position;
            Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.deltaTime * 5f, 0.0f);
            MyTransform.rotation = Quaternion.LookRotation(direction);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (behavior.Target != null)
        {
            if (lockedOn)
                LockOn();
        }
    }
    void DestroyPlayer()
    {
        if (behavior.Target != null)
            lockedOn = true;
        Shoot();
    }

    public void Shoot()
    {
        if (behavior.Target != null)
            if (lockedOn)
            {
                GameObject obj = pool.GetMiniBossLaser();
                if (obj != null)
                {
                    obj.transform.position = transform.position;
                    obj.transform.rotation = transform.rotation;
                    obj.SetActive(true);
                }
            }
    }

    public void SpawnExplosion(Vector3 pos)
    {
        GameObject obj = pool.GetBossLaserExplode();
        if (obj != null)
        {
            obj.transform.position = pos;
            obj.SetActive(true);
        }
    }
}