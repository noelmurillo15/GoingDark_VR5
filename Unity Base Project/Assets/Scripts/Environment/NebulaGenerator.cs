using UnityEngine;

public class NebulaGenerator : MonoBehaviour
{
    //**    Attach to Environment Gameobject    **//

    public int maxNebulaClouds;
    public int numNebulaClouds;

    public GameObject spawnPts;
    public GameObject[] nebulaPrefabs;

    private float boundsX, boundsY;

    // Use this for initialization
    void Start()
    {
        numNebulaClouds = 0;

        if (maxNebulaClouds == 0)
            maxNebulaClouds = 50;

        BoxCollider col = GetComponent<BoxCollider>();

        boundsX = col.size.x;
        boundsY = col.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (numNebulaClouds < maxNebulaClouds)
            SpawnCloud();
    }

    private void SpawnCloud()
    {
        float x = Random.Range((-boundsX / 2), (boundsX / 2));
        float y = Random.Range(-boundsY / 2, boundsY / 2);
        float z = Random.Range((-boundsX / 2), (boundsX / 2));
        Vector3 randomPos = new Vector3(x, y, z);

        GameObject go = Instantiate(nebulaPrefabs[Random.Range(0, nebulaPrefabs.Length)],
                    Vector3.zero, Quaternion.identity) as GameObject;

        go.transform.parent = spawnPts.transform;
        go.transform.localPosition = randomPos;
        numNebulaClouds++;
    }
}