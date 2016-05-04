using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NebulaGenerator : MonoBehaviour {
    //**    Attach to Environment Gameobject    **//

    public int maxNebulaClouds;
    public int numNebulaClouds;

    public GameObject[] spawnPts;
    public GameObject[] nebulaPrefabs;

    // Use this for initialization
    void Start()
    {
        numNebulaClouds = 0;

        if (maxNebulaClouds == 0)
            maxNebulaClouds = 50;
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
            GameObject go = Instantiate(nebulaPrefabs[Random.Range(0, nebulaPrefabs.Length)],
                            gos[cnt].transform.position, Quaternion.identity) as GameObject;

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