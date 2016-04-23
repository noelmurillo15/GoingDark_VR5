using UnityEngine;
using System.Collections;

public class JoyStickMovement : MonoBehaviour {

    private int turnRate;
    private float maxSpeed;
    private float moveSpeed;
    private float rotateSpeed;
    private float runMultiplier;

    private Vector3 moveDir;
    private CharacterController m_controller;

    // Use this for initialization
    void Start() {
        turnRate = 0;
        maxSpeed = 20.0f;
        moveSpeed = 0.0f;
        rotateSpeed = 15.0f;
        runMultiplier = 2.0f;

        moveDir = Vector3.zero;
        m_controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update(){
        moveDir = Vector3.zero;
        ManualTurn();
        ManualWalk();
        m_controller.Move(moveDir);
    }

    private void ManualTurn() {
        transform.Rotate(0, turnRate * rotateSpeed * Time.deltaTime, 0);
    }
    private void ManualWalk() {
        moveDir = transform.TransformDirection(Vector3.forward);
        if (moveSpeed >= maxSpeed)
        {
            rotateSpeed = 30.0f;
            moveDir *= (moveSpeed * Time.deltaTime) * runMultiplier;
        }
        else
        {
            rotateSpeed = 15.0f;
            moveDir *= moveSpeed * Time.deltaTime;
        }
    }
    public void SetMoveSpeed(float speed) {
        moveSpeed = speed;
    }
    public void IncreaseSpeed(float percentage)
    {
        if (moveSpeed < (maxSpeed * percentage))
            moveSpeed += 5.0f * Time.deltaTime;
    }
    public void DecreaseSpeed()
    {
        if (moveSpeed > 0.0f)
            moveSpeed -= 10.0f * Time.deltaTime;
        else
            moveSpeed = 0.0f;
    }

    public void goDown() {
        Vector3 newR = transform.rotation.eulerAngles;
        newR.x += 7.5f * Time.deltaTime;
        if (newR.x < 45.0f || newR.x > 315.0f)
            transform.rotation = Quaternion.Euler(newR);
    }
    public void goUp() {
        Vector3 newR = transform.rotation.eulerAngles;
        newR.x -= 7.5f * Time.deltaTime;
        if(newR.x > 315.0f || newR.x < 45.0f)
            transform.rotation = Quaternion.Euler(newR);        
    }
    public void turnRateZero() {
        turnRate = 0;
    }
    public void TurnLeft() {
        turnRate = -1;
    }
    public void TurnRight() {
        turnRate = 1;
    }
    public void StopMovement() {
        turnRate = 0;        
        moveSpeed = 0.0f;
    }
    public float GetSpeed()
    {
        return moveSpeed;
    }
    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
}