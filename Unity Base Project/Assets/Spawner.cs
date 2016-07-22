using UnityEngine;

public class Spawner : MonoBehaviour {

    private float count;
    private GameObject droids;


    // Use this for initialization
    void Start() {
        count = 10;
        InvokeRepeating("SpawnDroids", 5f, 3f);
        droids = Resources.Load<GameObject>("Droid");
    }

    private void SpawnDroids()
    {
        GameObject go = null;
        go = Instantiate(droids, new Vector3(transform.position.x, transform.position.y, transform.position.z - 15f), Quaternion.identity) as GameObject;
        go.transform.position = transform.position;
        go.transform.name = "Droid";
        go.transform.parent = transform.parent;

        count--;
        if(count <= 0f)
        {
            CancelInvoke("SpawnDroids");
            Destroy(gameObject);
        }           
    }
}