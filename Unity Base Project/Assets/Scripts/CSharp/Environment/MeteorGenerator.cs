using UnityEngine;

public class MeteorGenerator : MonoBehaviour
{

    // Use this for initialization
    private BoxCollider box;
    private Vector3 spawnPoint;
    private Vector3 bound;
    public GameObject meteorPref;
    public GameObject pt;
    public float spawnTimer;
    void Start()
    {
        box = GetComponent<BoxCollider>();
        spawnTimer = Random.Range(15f, 45f);
        spawnTimer = 5f;
        bound.x = box.size.x * 0.5f;
        bound.y = box.size.y * 0.5f;
        bound.z = box.size.z * 0.5f - box.center.z * 0.5f;
        spawnPoint.y = bound.y;
        Debug.Log(spawnPoint.y);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
            SpawnMeteor();

    }

    private void SpawnMeteor()
    {
        RandomizeSpawnPoint();
        GameObject go = Instantiate(meteorPref, Vector3.zero, Quaternion.identity) as GameObject;

        go.transform.parent = pt.transform;
        go.transform.localPosition = spawnPoint;
        spawnTimer = Random.Range(15f, 45f);
        spawnTimer = 5f;

    }

    private void RandomizeSpawnPoint()
    {
        spawnPoint.x = Random.Range(-bound.x, bound.x);
        spawnPoint.z = Random.Range(-bound.z, bound.z);
    }

}
