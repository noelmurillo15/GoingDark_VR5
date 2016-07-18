using UnityEngine;

public class Turret : MonoBehaviour
{
    //  Enemy Data
    Transform MyTransform;
    private EnemyStateManager behavior;
    private bool lockedOn;
    private float randomShot;

    private ObjectPooling pool = new ObjectPooling();
    private ObjectPooling explosionPool = new ObjectPooling();


    public float x, y, z;
    // Use this for initialization
    void Start()
    {
        x = y = z = 0;
        MyTransform = transform;
        randomShot = .5f;
        lockedOn = false;

        behavior = transform.parent.GetComponentInParent<EnemyStateManager>();

        explosionPool.Initialize(Resources.Load<GameObject>("Projectiles/Explosions/ChargeLaserExplosion"), 3);
        pool.Initialize(Resources.Load<GameObject>("Projectiles/Lasers/EnemyLaser"), 5);

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
        randomShot = Random.Range(1.0f, 1.5f);
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
        {
            GameObject obj = pool.GetPooledObject();
            if (obj != null)
            {
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);
                obj.SendMessage("SelfDestruct", this);
            }
            else
                Debug.LogError("Obj Pool empty : " + obj.name);
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public void SpawnExplosion(Vector3 pos)
    {
        GameObject obj = explosionPool.GetPooledObject();
        if (obj != null)
        {
            obj.transform.position = pos;
            obj.SetActive(true);
        }
    }
}