using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidGenerator : MonoBehaviour {

    public int maxAsteroids;
    public int numAsteroids;

    public GameObject spawnPts;
    public GameObject[] asteroidPrefabs;

	// Use this for initialization
	void Start () {
        numAsteroids = 0;

        if(maxAsteroids == 0)
            maxAsteroids = 35;
	}
	
	// Update is called once per frame
	void Update () {
        if (CheckAsteroidPrefabs() && CheckSpawnPts())
            if(numAsteroids < maxAsteroids)
                SpawnAsteroid();
	}

    private void SpawnAsteroid() {
        GameObject go = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)],
                            spawnPts.transform.position, Quaternion.identity) as GameObject;

        go.transform.parent = spawnPts.transform;
        numAsteroids++;
    }

    private bool CheckAsteroidPrefabs() {
        if (asteroidPrefabs.Length > 0)
            return true;

        return false;
    }

    private bool CheckSpawnPts() {
        if (spawnPts != null)
            return true;

        return false;
    }

    public void DeleteAsteroid()
    {
        numAsteroids--;
    }
}
