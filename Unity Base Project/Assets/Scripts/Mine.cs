﻿using UnityEngine;

public class Mine : MonoBehaviour
{
    private Transform MyTransform;
    public bool MineArmed;
    private EnemyBehavior behavior;
    private GameObject Explosion;
    private Transform player;

    // Use this for initialization
    void Start()
    {
        MyTransform = transform;
        MineArmed = true;
        behavior = GetComponent<EnemyBehavior>();
        player = GameObject.Find("Player").transform;
        Explosion = transform.GetChild(1).gameObject;
        Explosion.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && MineArmed)
        {
            col.transform.SendMessage("Hit");
            Trigger();
        }
    }

    void Trigger()
    {
        Explosion.SetActive(true);
        Invoke("Kill", 1f);
        MineArmed = false;
    }

    void Kill()
    {
        Debug.Log("Mine Destroyed");
        Destroy(gameObject);
    }
}