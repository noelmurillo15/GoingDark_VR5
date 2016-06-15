using UnityEngine;

public class ChargeLaser : MonoBehaviour
{
    public float delay = .25f;
    private GameObject laser;
    private Transform leapcam;

    private ObjectPooling Basicpool;
    private ObjectPooling BasicExplosionPool;

    private ObjectPooling Chargedpool;
    private ObjectPooling chargeExplosionPool;

    private GameObject explosion;
    private Transform MyTransform;


    // Use this for initialization
    void Start()
    {
        MyTransform = transform;
        leapcam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        laser = Resources.Load<GameObject>("Projectiles/Lasers/LaserBeam");
        explosion = Resources.Load<GameObject>("Projectiles/Explosions/BasicExplosion");
        Basicpool = new ObjectPooling();
        Basicpool.Initialize(laser, 12);
        BasicExplosionPool = new ObjectPooling();
        BasicExplosionPool.Initialize(explosion, 6);

        laser = Resources.Load<GameObject>("Projectiles/Lasers/ChargedShot");
        explosion = Resources.Load<GameObject>("Projectiles/Explosions/ChargeLaserExplosion");
        Chargedpool = new ObjectPooling();
        Chargedpool.Initialize(laser, 12);
        chargeExplosionPool = new ObjectPooling();
        chargeExplosionPool.Initialize(explosion, 6);

        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        delay = .25f;
    }

    // Update is called once per frame
    void Update()
    {
        delay -= Time.deltaTime;
        if (delay <= 0.0f)
        {
            GameObject obj = Basicpool.GetPooledObject();
            obj.transform.position = MyTransform.position;
            obj.transform.rotation = MyTransform.rotation;
            obj.SetActive(true);
            obj.SendMessage("SelfDestruct", this);
            gameObject.SetActive(false);
        }

        MyTransform.rotation = leapcam.rotation;
    }

    public void SpawnExplosion(Vector3 pos)
    {
        GameObject obj = BasicExplosionPool.GetPooledObject();
        if (obj != null)
        {
            obj.transform.position = pos;
            obj.SetActive(true);
        }
    }
}
