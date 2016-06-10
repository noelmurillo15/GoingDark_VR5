﻿using System;
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
    private GameObject projs;
    #endregion

    public void Initialize(GameObject _poolobj, int _poolamount)
    {
        if(projs == null)
            projs = GameObject.Find("Projectiles");

        pooledObj = _poolobj;
        pooledAmnt = _poolamount;
        poolList = new List<GameObject>();
        for (int i = 0; i < pooledAmnt; i++)
        {
            GameObject obj = Transform.Instantiate(pooledObj, Vector3.zero, Quaternion.identity) as GameObject;
            obj.SendMessage("Initialize");
            obj.transform.parent = projs.transform;
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