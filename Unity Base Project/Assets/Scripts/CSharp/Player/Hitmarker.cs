﻿using UnityEngine;

public class Hitmarker : MonoBehaviour
{
    #region Properties
    private bool rayhit;

    private int range;
    private int layermask;

    private RaycastHit hit;
    private Transform MyTransform;

    private Transform parent;
    private GameObject TargetImg;
    private GameObject LockOnMarker;
    #endregion


    // Use this for initialization
    void Start()
    {
        range = 1600;
        layermask = 1 << 11;    //  enemies layer
        rayhit = true;
        MyTransform = transform;
        TargetImg = Resources.Load<GameObject>("LockOn");
        parent = GameObject.FindGameObjectWithTag("GameManager").transform;
        LockOnMarker = Instantiate(TargetImg, Vector3.zero, Quaternion.identity) as GameObject;
        LockOnMarker.transform.parent = parent;
        LockOnMarker.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        rayhit = false;

        if (Physics.Raycast(MyTransform.position, MyTransform.forward, out hit, range, layermask))
            if (hit.collider.CompareTag("Enemy") && hit.collider.GetType() == typeof(BoxCollider))
                rayhit = true;        

        if(LockOnMarker != null)
            LockOnMarker.SetActive(rayhit);

        if (rayhit)
            LockOnUpdate();
    }

    public bool GetLockedOn()
    {
        return rayhit;
    }
    public Transform GetRaycastHit()
    {
        return hit.transform;
    }
    void LockOnUpdate()
    {
        if (LockOnMarker == null)
            LockOnMarker = Instantiate(TargetImg, Vector3.zero, Quaternion.identity) as GameObject;

        LockOnMarker.transform.parent = GetRaycastHit();
        LockOnMarker.transform.position = GetRaycastHit().position;
        LockOnMarker.transform.LookAt(MyTransform);
    }
}