using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class ObjectPooling
{
    #region Properties
    public ObjectPooling current;
    public GameObject pooledObj;
    public int pooledAmnt = 20;
    public bool Growth = true;
    public List<GameObject> poolList;
    #endregion


    public void Initialize(GameObject _poolobj, int _poolamount, GameObject myparent)
    {
        if (_poolobj == null)
        {
            Debug.LogError("Poolobj is set to null : " + _poolamount.ToString());
            return;
        }

        pooledObj = _poolobj;
        pooledAmnt = _poolamount;
        poolList = new List<GameObject>();
        for (int i = 0; i < pooledAmnt; i++)
        {
            GameObject obj = Transform.Instantiate(pooledObj, Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.parent = myparent.transform;
            poolList.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < poolList.Count; i++)
            if (!poolList[i].activeInHierarchy)
                return poolList[i];

        Debug.Log("Pool ran out of : " + poolList[0].name);
        return null;
    }
}
