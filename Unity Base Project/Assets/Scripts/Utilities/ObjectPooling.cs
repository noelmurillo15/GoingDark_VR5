using UnityEngine;
using System.Collections.Generic;


public class ObjectPooling {

    #region Properties
    private List<GameObject> poolList = new List<GameObject>();
    #endregion


    public void Initialize(GameObject _poolobj, int _poolamount, Transform _parent)
    {
        if (_poolobj == null)
        {
            Debug.LogError("Poolobj is set to null");
            return;
        }

        for (int i = 0; i < _poolamount; i++)
        {
            GameObject obj = Transform.Instantiate(_poolobj, Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.parent = _parent;
            obj.SetActive(false);
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
