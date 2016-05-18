using UnityEngine;

public class AsteroidGenerator : MonoBehaviour {

    public int maxAsteroids;
    public int numAsteroids;    

    public GameObject spawnPts;
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
    void Update() {
        if (numAsteroids < maxAsteroids)
            SpawnAsteroid();
    }

    private void SpawnAsteroid() {
            float x = Random.Range(-boundsX * .5f, boundsX * .5f);
            float y = Random.Range(-boundsY * .5f, boundsY * .5f);
            float z = Random.Range(-boundsX * .5f, boundsX * .5f);
            Vector3 randomPos = new Vector3(x, y, z);
            GameObject go = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)],
                            Vector3.zero, Quaternion.identity) as GameObject;

            go.transform.parent = spawnPts.transform;
            go.transform.localPosition = randomPos;
            numAsteroids++;    
    }

    public void DeleteAsteroid()
    {
        numAsteroids--;
    }
}
