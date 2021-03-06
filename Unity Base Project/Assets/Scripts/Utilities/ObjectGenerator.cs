﻿using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ObjectGenerator : MonoBehaviour {

    #region Properties
    public int maxObjects;
    private int numObjects;

    public GameObject spawnPt;
    public GameObject[] objPrefab;

    private Vector3 bounds;
    private BoxCollider boxcol;
    #endregion


    void Start () {
        numObjects = 0;

        boxcol = GetComponent<BoxCollider>();
        bounds = new Vector3(boxcol.size.x * .5f, boxcol.size.y * .5f, boxcol.size.z * .5f);

        if (objPrefab.Length == 0)
            Debug.LogError("Object Generator's prefab list is empty");
    }

    // Update is called once per frame
    void FixedUpdate() {
        while (maxObjects > numObjects)
            SpawnObject();
    }

    void SpawnObject()
    {
        float x = Random.Range(-bounds.x + boxcol.center.x, bounds.x + boxcol.center.x);
        float y = Random.Range(-bounds.y + boxcol.center.y, bounds.y + boxcol.center.y);
        float z = Random.Range(-bounds.z + boxcol.center.z, bounds.z + boxcol.center.z);

        Vector3 randomPos = new Vector3(x, y, z);

        GameObject go = Instantiate(objPrefab[Random.Range(0, objPrefab.Length)],
                        Vector3.zero, Quaternion.identity) as GameObject;

        go.transform.parent = spawnPt.transform;
        go.transform.localPosition = randomPos;
        numObjects++;
    }

    public void DestroyObject()
    {
        numObjects--;
    }
}