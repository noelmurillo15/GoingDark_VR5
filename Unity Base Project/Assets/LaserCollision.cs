﻿using UnityEngine;

public class LaserCollision : MonoBehaviour {

    #region Properties
    private LaserProjectile laser;
    #endregion

    // Use this for initialization
    void Start () {
        laser = GetComponent<LaserProjectile>();
	}

    #region Collisions
    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Enemy") || col.transform.CompareTag("Shield"))
        {
            col.transform.SendMessage("LaserHit", laser);
            laser.Kill();
        }

        else if (col.transform.CompareTag("Orb"))
        {
            col.transform.SendMessage("DMG");
            laser.Kill();
        }
        else if (col.transform.CompareTag("Asteroid"))
        {
            col.transform.SendMessage("Kill");
            laser.Kill();
        }
        else if (col.transform.CompareTag("Player"))
        {
            Debug.Log("Player hit with Laser");
        }
    }
    #endregion
}