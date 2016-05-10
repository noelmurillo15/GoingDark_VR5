using UnityEngine;
using System.Collections.Generic;

public class AsteroidGenerator : MonoBehaviour {

    public int maxAsteroids;
    public int numAsteroids;    

    public GameObject[] spawnPts;
    public GameObject[] asteroidPrefabs;

    private float boundsX, boundsY;

	// Use this for initialization
	void Start () {
        numAsteroids = 0;

        if (maxAsteroids == 0)
            maxAsteroids = 50;

        BoxCollider col = GetComponent<BoxCollider>();

        boundsX = col.size.x;
        boundsY = col.size.y;
	}
	
	// Update is called once per frame
	void Update () {
        if (CheckAsteroidPrefabs() && CheckSpawnPts())
            if(numAsteroids < maxAsteroids)
                SpawnAsteroid();
	}

    private void SpawnAsteroid() {
        GameObject[] gos = AvailableSpawnPoints();

        for (int cnt = 0; cnt < gos.Length; cnt++) {
            float x = Random.Range(-boundsX / 2, boundsX / 2);
            float y = Random.Range(-boundsY / 2, boundsY / 2);
            float z = Random.Range(-boundsX / 2, boundsX / 2);
            Vector3 randomPos = new Vector3(x, y, z);
            GameObject go = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)],
                            randomPos, Quaternion.identity) as GameObject;

            go.transform.parent = gos[cnt].transform;
            numAsteroids++;
        }        
    }

    private bool CheckAsteroidPrefabs() {
        if (asteroidPrefabs.Length > 0)
            return true;

        return false;
    }

    private bool CheckSpawnPts() {
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

    public void DeleteAsteroid()
    {
        numAsteroids--;
    }
}
