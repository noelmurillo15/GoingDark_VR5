using UnityEngine;
using System.Collections;

public class JoyStickMovement : MonoBehaviour {

    private int turnRateY;
    private int turnRateX;
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
        turnRateY = 0;
        turnRateX = 0;
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
       
        moveDir = Vector3.zero;
        ManualTurnXAxis();
        ManualTurnYAxis();
        ManualWalk();
        m_controller.Move(moveDir);
    }

    #region Movement
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
    private void ManualTurnYAxis() {
        transform.Rotate(0, turnRateY * rotateSpeed * Time.deltaTime, 0);
        radar.transform.Rotate(0, -turnRateY * rotateSpeed * Time.deltaTime, 0);
    }
    public void ManualTurnXAxis() {
        transform.Rotate(turnRateX * rotateSpeed * Time.deltaTime, 0, 0);
    }
    #endregion

    #region Modifiers
    public void TurnUp() {
        turnRateX = -1;
    }
    public void TurnDown() {
        turnRateX = 1;
    }
    public void TurnLeft() {
        turnRateY = -1;
    }
    public void TurnRight() {
        turnRateY = 1;
    }
    public void turnRateZero() {
        turnRateY = 0;
        turnRateX = 0;
    }
    public void StopMovement() {
        turnRateZero();      
        moveSpeed = 0.0f;
    }
    #endregion

    #region Accessors
    public float GetSpeed() {
        return moveSpeed;
    }
    public float GetMaxSpeed() {
        return maxSpeed;
    }
    #endregion
}