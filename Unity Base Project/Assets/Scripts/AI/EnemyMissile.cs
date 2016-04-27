﻿using UnityEngine;
using System.Collections;

public class EnemyMissile : MonoBehaviour {
    //**        Attach to Enemy Missile Prefab      **//

    //  Missile Data
    public int LookSpeed;
    public bool tracking;
    public float velocity;
    public float destroyTimer;
    public bool destroyMissile;
    public GameObject Explosion;

    //  Target Data
    private Vector3 targetLocation;
    public Quaternion targetRotation;

    // Messages
    private GameObject messages;

    void Start() {
        messages = GameObject.Find("Screen");
        tracking = false; 
        destroyMissile = false;
        velocity = 80.0f;
        destroyTimer = 5.0f;
        LookSpeed = 20;
        messages.SendMessage("MissileIncoming");
    }

    void Update() {

        if (destroyTimer > 0.0f)
            destroyTimer -= Time.deltaTime;
        else
            Destroy(this.gameObject);


        if (!destroyMissile) {
            transform.position += transform.forward * velocity * Time.deltaTime;
            if (tracking)
            {
                targetRotation = Quaternion.LookRotation(targetLocation - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * LookSpeed);
            }
        }

        if (destroyMissile && Explosion != null) {
            Instantiate(Explosion, transform.position, transform.rotation);
            Explosion = null;            
        }        
    }    

    void OnTriggerEnter(Collider col) {
        if (!tracking) {
            if (col.transform.tag == "PlayerShip" || col.transform.tag == "Asteroid") {
                Debug.Log("Enemy Missile Tracking " + col.transform.tag);
                this.GetComponent<MeshRenderer>().enabled = false;
                targetLocation = col.transform.position;
                tracking = true;
            }
        }
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "PlayerShip" && !destroyMissile)
        {
            Debug.Log("Enemy Missile Collided with " + col.transform.tag);
            destroyMissile = true;
            col.gameObject.SendMessage("Hit");
            messages.SendMessage("MissileDestroyed");
        }
        if (col.transform.tag == "Asteroid" && !destroyMissile)
        {
            Debug.Log("Enemy Missile Collided with " + col.transform.tag);
            destroyMissile = true;
            col.gameObject.SendMessage("DestroyAsteroid");
            messages.SendMessage("MissileDestroyed");
        }
    }
}
