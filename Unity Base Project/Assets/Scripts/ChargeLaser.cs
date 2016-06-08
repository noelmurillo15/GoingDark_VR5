using UnityEngine;

public class ChargeLaser : MonoBehaviour
{
    public float delay = 1.25f;
    private GameObject laser;
    private Transform leapcam;
    private GameObject environment;
    private ObjectPooling pool;
    private ObjectPooling explosionpool;
    private GameObject explosion;


    // Use this for initialization
    void Start()
    {
        leapcam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        explosion = Resources.Load<GameObject>("ChargeLaserExplosion");
        laser = Resources.Load<GameObject>("ChargedShot");
        environment = GameObject.Find("Environment");

        pool = new ObjectPooling();
        pool.Initialize(laser, 3);

        explosionpool = new ObjectPooling();
        explosionpool.Initialize(explosion, 1);

        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        delay = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        delay -= Time.deltaTime;
        if (delay <= 0.0f)
        {
            GameObject obj = pool.GetPooledObject();
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.SetActive(true);
            obj.SendMessage("SelfDestruct", this);
            gameObject.SetActive(false);
        }
    }

    public void SpawnExplosion(Vector3 pos)
    {
        GameObject obj = explosionpool.GetPooledObject();
        if (obj != null)
        {
            obj.transform.position = pos;
            obj.SetActive(true);
        }
    }
}
