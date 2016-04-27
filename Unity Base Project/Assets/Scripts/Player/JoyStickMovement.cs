using UnityEngine;
using System.Collections;

public class JoyStickMovement : MonoBehaviour {

    //  Movement Vars
    private int turnRateY;
    private int turnRateX;
    private float maxSpeed;
    private float moveSpeed;
    public float rotateSpeed;
    private float runMultiplier;

    private Vector3 moveDir;
    private CharacterController m_controller;

    //  Auto Pilot Vars
    private bool autoMove;
    private bool autoRotate;
    private bool resetRotation;
    private float orientationTimer;
    private Vector3 targetPosition;
    private GameObject autoPilotSign;
    private GameObject reorientSign;

    //  Radar Vars
    private GameObject radar;
    private GameObject playerBlip;


    // Use this for initialization
    void Start() {
        autoMove = false;
        autoRotate = false;
        resetRotation = false;
        orientationTimer = 0.0f;
        turnRateY = 0;
        turnRateX = 0;
        maxSpeed = 20.0f;
        moveSpeed = 0.0f;
        rotateSpeed = 10.0f;
        runMultiplier = 1.5f;

        

        radar = GameObject.Find("Radar");
        playerBlip = GameObject.Find("Blip_Triangle_Player");
        autoPilotSign = GameObject.Find("AutoPilot");
        reorientSign = GameObject.Find("Reorient");

        reorientSign.SetActive(resetRotation);
        autoPilotSign.SetActive(autoRotate);

        moveDir = Vector3.zero;
        m_controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update(){
        //Vector3 newRot = transform.localEulerAngles;
        //newRot.x -= 180.0f;
        //playerBlip.transform.localEulerAngles = newRot;

        if (!autoRotate && !autoMove && !resetRotation) {
            moveDir = Vector3.zero;
            ManualTurnXAxis();
            ManualTurnYAxis();
            ManualWalk();
            m_controller.Move(moveDir);
        }
        else if(autoRotate || autoMove)
            Autopilot();
        else if (resetRotation)
            Reorient();        
    }

    #region Movement
    private void Autopilot() {
        float angle = 0.0f;
        if (autoRotate)
        {
            Debug.Log("Auto Rotate");
            Vector3 playerDir = targetPosition - transform.position;
            Vector3 newEnemyDir = Vector3.RotateTowards(transform.forward, playerDir, (rotateSpeed * 0.25f) * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newEnemyDir);
            angle = Vector3.Angle(newEnemyDir, playerDir);
        }

        if (autoMove)
        {
            Debug.Log("Auto Move");
            moveDir = Vector3.zero;
            moveDir = transform.TransformDirection(Vector3.forward);
            moveDir *= (50.0f * Time.deltaTime) * runMultiplier;
            m_controller.Move(moveDir);
        }

        if (angle <= 2.5f && autoRotate)
        {
            autoRotate = false;
            autoMove = true;
        }
    }

    public void OutOfBounds(Vector3 targetPos)
    {
        autoRotate = true;
        autoPilotSign.SetActive(autoRotate);
        targetPosition = targetPos;
    }

    public void InBounds()
    {
        autoRotate = false;
        autoMove = false;
        autoPilotSign.SetActive(autoRotate);
    }

    public void ResetOrientation()
    {
        orientationTimer = 5.0f;
        resetRotation = true;
    }

    public void Reorient()
    {
        if (transform.rotation != Quaternion.identity && orientationTimer > 0.0f) {
            orientationTimer -= Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * 0.75f);
        }
        else
            resetRotation = false;

        reorientSign.SetActive(resetRotation);
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