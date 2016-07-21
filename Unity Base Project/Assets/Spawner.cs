using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
    private GameObject droids;
    private int droidcount;
    private int Maxcount;
    // Use this for initialization
    void Start () {
        droids = Resources.Load<GameObject>("Droid");
        droidcount = 1;
        InvokeRepeating("SpawnDroids", 1f, 8f);
        Maxcount = 20;

    }

    private void SpawnDroids()
    {
        GameObject[] go = new GameObject[droidcount];
        if ( droidcount < Maxcount)
            for (int i = 0; i < droidcount; i++)
            {
                go[i] = Instantiate(droids, transform.position, Quaternion.identity) as GameObject;
               // go[i].SendMessage("LoadEnemyData");
                go[i].transform.position = transform.position;
                go[i].transform.name = "Droid";
                go[i].transform.parent = transform.parent;
            }
    }
    public int GetDroidSpawnCount()
    {
        return droidcount;
    }
}
