using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class ObjectPooling : MonoBehaviour
{

    public ObjectPooling current;
    public GameObject pooledObj;
    public int pooledAmnt = 20;
    public bool Growth = true;
    public List<GameObject> poolList;

    public void Initialize(GameObject _poolobj, int _poolamount)
    {
        pooledObj = _poolobj;
        pooledAmnt = _poolamount;
        poolList = new List<GameObject>();
        Debug.Log("Creating Pool of : " + _poolamount);
        for (int i = 0; i < pooledAmnt; i++)
        {
            GameObject obj = Instantiate(pooledObj, Vector3.zero, Quaternion.identity) as GameObject;
            obj.SetActive(false);
            poolList.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < poolList.Count; i++)
            if (!poolList[i].activeInHierarchy)
                return poolList[i];

        Debug.Log("Pool ran out of missiles");
        return null;
    }
}
