using UnityEngine;

public class EnemyStats : MonoBehaviour {
    //**    Attach to an Enemy  **//

    //  Movement
    public float moveSpeed;
    private float rotateSpeed;
    private float acceleration;
    public float maxSpeed = 50.0f;

    //  Weapons
    public int numMissiles = 6;


    void Start()
    {
        moveSpeed = 0f;
        rotateSpeed = 20f;
        acceleration = 2.5f;
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
}