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
            Vector3 randomPos = new Vector3(Random.Range(-boundsX + transform.position.x, boundsX + transform.position.x),
                Random.Range(-boundsY + transform.position.y, boundsY + transform.position.y),
                Random.Range(-boundsZ + transform.position.z, boundsZ + transform.position.z));

            GameObject junk = Instantiate(junkPrefabs[Random.Range(0, junkPrefabs.Length)],
                               randomPos,new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)))
                               as GameObject;

            junk.transform.parent = points[i].transform;
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
