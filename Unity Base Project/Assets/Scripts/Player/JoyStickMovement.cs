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

    private GameObject radar;
    private GameObject playerBlip;

    // Use this for initialization
    void Start() {
        turnRate = 0;
        maxSpeed = 20.0f;
        moveSpeed = 0.0f;
        rotateSpeed = 10.0f;
        runMultiplier = 1.5f;

        radar = GameObject.Find("Radar");
        playerBlip = GameObject.Find("Blip_Triangle_Player");

        moveDir = Vector3.zero;
        m_controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update(){
        Vector3 newRot = transform.localEulerAngles;
        newRot.x -= 180.0f;
        playerBlip.transform.localEulerAngles = newRot;
        
        //playerBlip.transform.localRotation.y = -transform.localRotation.y;
        moveDir = Vector3.zero;
        ManualTurn();
        ManualWalk();
        m_controller.Move(moveDir);
    }

    private void ManualTurn() {
        transform.Rotate(0, turnRate * rotateSpeed * Time.deltaTime, 0);
        radar.transform.Rotate(0, -turnRate * rotateSpeed * Time.deltaTime, 0);
    }

    private void ManualWalk() {
        moveDir = transform.TransformDirection(Vector3.forward);
        if (moveSpeed >= maxSpeed)
        {
            rotateSpeed = 20.0f;
            moveDir *= (moveSpeed * Time.deltaTime) * runMultiplier;
        }
        else
        {
            rotateSpeed = 10.0f;
            moveDir *= moveSpeed * Time.deltaTime;
        }
    }

    public void SetMoveSpeed(float speed) {
        moveSpeed = speed;
    }

    public void IncreaseSpeed(float percentage) {
        if (moveSpeed < (maxSpeed * percentage))
            moveSpeed += 5.0f * Time.deltaTime;
        else if (moveSpeed > (maxSpeed * percentage))
            moveSpeed -= 5.0f * Time.deltaTime;
        else
            moveSpeed = 0.0f;
    }

    public void DecreaseSpeed() {
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

    public float GetSpeed() {
        return moveSpeed;
    }

    public float GetMaxSpeed() {
        return maxSpeed;
    }
}