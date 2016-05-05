﻿using UnityEngine;

public class EnemyStats : MonoBehaviour {
    //**    Attach to an Enemy  **//

    //  Movement
    public float moveSpeed;
    private float rotateSpeed;
    private float acceleration;
    public float maxSpeed = 40.0f;

    //  Weapons
    private int numMissiles;


    void Start()
    {
        moveSpeed = 0f;
        rotateSpeed = 20f;
        acceleration = 2.5f;
        numMissiles = 5;
    }

    void Update()
    {
        
    }

    #region Accessors
    public int GetNumMissiles()
    {
        return numMissiles;
    }
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
    public float GetRotateSpeed()
    {
        return rotateSpeed;
    }
    #endregion

    #region Modifiers
    public void DecreaseMissileCount()
    {
        numMissiles--;
    }
    
    public void IncreaseSpeed(float alterMaxSpeed) {
        // alter max speed = 0.5f : cuts max speed in half
        // alter max speed = 1.0f : does not alter max speed
        if (moveSpeed < (maxSpeed * alterMaxSpeed))
            moveSpeed += Time.deltaTime * acceleration;
    }
    public void DecreaseSpeed() {
        if (moveSpeed > 0.0f)
            moveSpeed -= Time.deltaTime * acceleration * 5.0f;
        else
            moveSpeed = 0.0f;
    }
    #endregion

    #region Msg Functions
    public void EMPHit()
    {
        Debug.Log("EMP has affected Enemy's Systems");
    }

    public void AsteroidHit()
    {
        Debug.Log("Enemy Has Hit Asteroid");
    }

    public void Kill()
    {
        Debug.Log("Destroyed Enemy Ship");
        Destroy(this.gameObject);
    }
    #endregion
}