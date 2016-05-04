using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NebulaGenerator : MonoBehaviour {
    //**    Attach to Environment Gameobject    **//

    public int maxNebulaClouds;
    public int numNebulaClouds;

    public GameObject[] spawnPts;
    public GameObject[] nebulaPrefabs;

    public float boundsX, boundsY;

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
        if (CheckNebulaPrefabs() && CheckSpawnPts())
            if (numNebulaClouds < maxNebulaClouds)
                SpawnCloud();
    }

    private void SpawnCloud()
    {
        GameObject[] gos = AvailableSpawnPoints();

        for (int cnt = 0; cnt < gos.Length; cnt++)
        {
            float x = Random.Range(-boundsX / 2, boundsX / 2);
            float y = Random.Range(-boundsY / 2, boundsY / 2);
            float z = Random.Range(-boundsX / 2, boundsX / 2);
            Vector3 randomPos = new Vector3(x, y, z);

            GameObject go = Instantiate(nebulaPrefabs[Random.Range(0, nebulaPrefabs.Length)],
                        randomPos, Quaternion.identity) as GameObject;

            go.transform.parent = gos[cnt].transform;
            numNebulaClouds++;
        }
    }

    private bool CheckNebulaPrefabs()
    {
        if (nebulaPrefabs.Length > 0)
            return true;

        return false;
    }

    private bool CheckSpawnPts()
    {
        if (spawnPts.Length > 0)
            return true;
        else
            return false;
    }

    private GameObject[] AvailableSpawnPoints()
    {
        List<GameObject> gos = new List<GameObject>();

        for (int cnt = 0; cnt < spawnPts.Length; cnt++)
            if (spawnPts[cnt].transform.childCount <= 8)
                gos.Add(spawnPts[cnt]);

        return gos.ToArray();
    }
}