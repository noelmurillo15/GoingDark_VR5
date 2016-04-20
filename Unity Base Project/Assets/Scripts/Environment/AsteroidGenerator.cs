using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidGenerator : MonoBehaviour {
    //**        Attach To Environment GameObject        **//

    public int maxAsteroids;
    public int numAsteroids;    

    public GameObject[] spawnPts;
    public GameObject[] asteroidPrefabs;

	// Use this for initialization
	void Start () {
        numAsteroids = 0;
        maxAsteroids = 35;
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
            GameObject go = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)],
                            gos[cnt].transform.position, Quaternion.identity) as GameObject;

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
            if (spawnPts[cnt].transform.childCount <= 5)
                gos.Add(spawnPts[cnt]);

        return gos.ToArray();
    }

    public void DeleteAsteroid()
    {
        numAsteroids--;
    }
}
