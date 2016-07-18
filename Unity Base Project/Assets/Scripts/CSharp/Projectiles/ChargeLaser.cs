using UnityEngine;

public class ChargeLaser : MonoBehaviour
{
    public float delay = .25f;
    private Transform leapcam;
    private Transform MyTransform;

    private ObjectPoolManager poolmanager;



    // Use this for initialization
    void Start()
    {
        MyTransform = transform;
        leapcam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        poolmanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();

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
            GameObject obj = poolmanager.GetBaseLaser();
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
        GameObject obj = poolmanager.GetBaseLaserExplosion();
        if (obj != null)
        {
            obj.transform.position = pos;
            obj.SetActive(true);
        }
    }
}
