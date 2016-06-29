﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hitmarker : MonoBehaviour
{
    [SerializeField]
    private Sprite StaticMarker;
    [SerializeField]
    private Sprite hitMarker;

    private Image reticle;
    private float HitDisplayDuration;
    private float HitTime;
    private bool ShowHitMarker;

    private int range;
    private bool rayhit;
    private RaycastHit hit;
    private Transform MyTransform;

    private GameObject TargetImg;
    private GameObject LockOnMarker;
    // Use this for initialization
    void Start()
    {
        range = 1600;
        rayhit = true;
        ShowHitMarker = false;
        reticle = GetComponent<Image>();
        MyTransform = transform;
        HitDisplayDuration = 0.8f;

        TargetImg = Resources.Load<GameObject>("LockObject");
        LockOnMarker = Instantiate(TargetImg, Vector3.zero, Quaternion.identity) as GameObject;
        LockOnMarker.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        rayhit = false;
        if (GetComponent<Image>().sprite != StaticMarker && Time.time - HitTime > HitDisplayDuration)
            GetComponent<Image>().sprite = StaticMarker;

        if (Physics.Raycast(MyTransform.position, MyTransform.forward, out hit, range))
        {
            if (hit.collider.CompareTag("Asteroid"))
            {
                rayhit = true;
            }
            else if (hit.collider.CompareTag("Enemy") && hit.collider.GetType() == typeof(BoxCollider))
            {
                rayhit = true;
                reticle.color = Color.red;
                objUpdate();
            }
        }

        if (!rayhit)
        {
            reticle.color = Color.white;
            if(LockOnMarker != null)
                LockOnMarker.SetActive(false);
        }            
    }

    public void HitMarkerShow(float TimeWhenShot)
    {
        HitTime = TimeWhenShot;
        GetComponent<Image>().sprite = hitMarker;
    }
    void objUpdate()
    {
        if (LockOnMarker != null)
        {
            LockOnMarker.SetActive(true);
            LockOnMarker.transform.parent = hit.transform;
            LockOnMarker.transform.position = hit.transform.position;
            LockOnMarker.transform.LookAt(MyTransform);
        }
        else
        {
            LockOnMarker = Instantiate(TargetImg, Vector3.zero, Quaternion.identity) as GameObject;
            LockOnMarker.transform.parent = hit.transform;
            LockOnMarker.transform.position = hit.transform.position;
            LockOnMarker.transform.LookAt(MyTransform);
        }
    }
}