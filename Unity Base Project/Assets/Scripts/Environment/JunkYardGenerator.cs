using UnityEngine;
using System.Collections.Generic;

public class JunkYardGenerator : MonoBehaviour {

    public int maxJunk;
    private int numJunk;

    public GameObject[] spawnPts;
    public GameObject[] junkPrefabs;

    private float boundsX, boundsY, boundsZ;
    // Use this for initialization
    void Start () {
        numJunk = 0;

        if (maxJunk == 0)
            maxJunk = 50;

        boundsX = GetComponent<BoxCollider>().size.x / 2;
        boundsY = GetComponent<BoxCollider>().size.y / 2;
        boundsZ = GetComponent<BoxCollider>().size.z / 2;
    }
	
	// Update is called once per frame
	void Update () {
        if (spawnPts.Length > 0 && junkPrefabs.Length > 0)
            if (maxJunk > numJunk)
                SpawnJunk();
	}

    private void SpawnJunk()
    {
        GameObject[] points = AvailableSpawnPoints();

        for (int i = 0; i < points.Length; i++)
        {
            float x = Random.Range((-boundsX), (boundsX));
            float y = Random.Range(-boundsY, boundsY);
            float z = Random.Range((-boundsX), (boundsX));
            Vector3 randomPos = new Vector3(x, y, z);

            GameObject junk = Instantiate(junkPrefabs[Random.Range(0, junkPrefabs.Length)],
                               Vector3.zero ,new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)))
                               as GameObject;

            junk.transform.parent = points[i].transform;
            junk.transform.localPosition = randomPos;
            numJunk++;
        }
        

    }

    private GameObject[] AvailableSpawnPoints()
    {
        List<GameObject> points = new List<GameObject>();

        for (int cnt = 0; cnt < spawnPts.Length; cnt++)
            if (spawnPts[cnt].transform.childCount <= 8)
                points.Add(spawnPts[cnt]);

        return points.ToArray();
    }
}
