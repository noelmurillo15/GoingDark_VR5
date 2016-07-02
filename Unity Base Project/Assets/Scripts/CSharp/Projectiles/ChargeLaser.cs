using UnityEngine;

public class ChargeLaser : MonoBehaviour
{
    public float delay = .25f;
    private Transform leapcam;
    private Transform MyTransform;

    private GameObject explosions;
    private GameObject projectiles;

    private ObjectPooling BasicLasers = new ObjectPooling();
    private ObjectPooling ChargedLasers = new ObjectPooling();
    private ObjectPooling BasicExplosion = new ObjectPooling();
    private ObjectPooling ChargedExplosion = new ObjectPooling();



    // Use this for initialization
    void Start()
    {
        MyTransform = transform;
        explosions = GameObject.Find("Explosions");
        projectiles = GameObject.Find("Projectiles");
        leapcam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        BasicLasers.Initialize(Resources.Load<GameObject>("Projectiles/Lasers/LaserBeam"), 12, projectiles);
        ChargedLasers.Initialize(Resources.Load<GameObject>("Projectiles/Lasers/ChargedShot"), 12, projectiles);

        BasicExplosion.Initialize(Resources.Load<GameObject>("Projectiles/Explosions/BasicExplosion"), 6, explosions);
        ChargedExplosion.Initialize(Resources.Load<GameObject>("Projectiles/Explosions/ChargeLaserExplosion"), 6, explosions);

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
            GameObject obj = BasicLasers.GetPooledObject();
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
        GameObject obj = BasicExplosion.GetPooledObject();
        if (obj != null)
        {
            obj.transform.position = pos;
            obj.SetActive(true);
        }
    }
}
