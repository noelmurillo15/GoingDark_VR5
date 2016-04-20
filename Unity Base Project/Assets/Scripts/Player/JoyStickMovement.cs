using UnityEngine;
using System.Collections;

public class JoyStickMovement : MonoBehaviour {

    public int turnRate;
    public float maxSpeed;
    public float moveSpeed;
    public float rotateSpeed;
    public float runMultiplier;

    private Vector3 moveDir;
    public CharacterController m_controller;

    // Use this for initialization
    void Start()
    {
        turnRate = 0;
        maxSpeed = 20.0f;
        moveSpeed = 5.0f;
        rotateSpeed = 30.0f;
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
            moveDir *= (moveSpeed * Time.deltaTime) * runMultiplier;
        else
            moveDir *= moveSpeed * Time.deltaTime;
    }

    public void SetMoveSpeed(float speed) {
        moveSpeed = speed;
    }

    public void goDown() {
        transform.Translate((Vector3.up * 2.0f) * Time.deltaTime);
    }

    public void goUp() {
        transform.Translate((Vector3.up * -2.0f) * Time.deltaTime);
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
}