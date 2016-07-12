﻿using UnityEngine;

public class SpinScript : MonoBehaviour
{
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public float speed = 0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(x, y, z), speed * Time.deltaTime);
    }

    void OnBecameVisible()
    {
        enabled = true;
    }

    void OnBecameInvisible()
    {
        enabled = false;
    }
}