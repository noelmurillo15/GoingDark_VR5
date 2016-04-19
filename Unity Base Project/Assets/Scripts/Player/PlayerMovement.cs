using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour {
    //**    Attach Script to Player Gameobject   **//

    public bool ManualDriveMode;
    public bool HyperDriveMode;
    
    private int turnRate;
    private float limiter;
    private float maxSpeed;
    private float moveSpeed;
    private float rotateSpeed;
    private float runMultiplier;

    private Vector3 moveDir;

    private GameObject m_leapMount;
    private PlayerData m_playerData;
    private LeapMovement m_leapMove;
    private CharacterController m_controller;

    public float speedBarX;
    public GameObject speedBarColor;


    // Use this for initialization
    void Start() {
        HyperDriveMode = false;
        ManualDriveMode = false;

        turnRate = 0;
        limiter = 0.0f;
        maxSpeed = 5.0f;
        moveSpeed = 0.0f;
        rotateSpeed = 5.0f;
        runMultiplier = 2.0f;

        speedBarX = 0.0f;

        moveDir = Vector3.zero;

        if (m_playerData == null)
            m_playerData = this.GetComponent<PlayerData>();

        if (m_leapMount == null)
            m_leapMount = GameObject.FindGameObjectWithTag("LeapMount");

        if (m_controller == null)
            m_controller = this.GetComponent<CharacterController>();

        if (m_leapMove == null)
            m_leapMove = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<LeapMovement>();

        if (speedBarColor == null)
            speedBarColor = GameObject.Find("SpeedBarColor");
    }

    // Update is called once per frame
    void Update() {       

        if (!m_playerData.GetGamePaused()) {
            if (ManualDriveMode) {
                moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Verticle"));

                m_leapMove.UpdateManualMovement();

                ManualTurn();
                ManualWalk();
                // no speed
                // y pos = -1.1
                // y scale = 0
                // speed
                // y pos = .095
                // y scale =  .25
                speedBarColor.SetActive(true);
                speedBarX = (moveSpeed / maxSpeed) * 0.25f;

                Vector3 newScale;
                newScale.x = speedBarColor.transform.localScale.x;
                newScale.y = speedBarX;
                newScale.z = speedBarColor.transform.localScale.z;
                speedBarColor.transform.localScale = newScale;

                Vector3 newPos;
                newPos.x = speedBarColor.transform.localPosition.x;
                newPos.y = -1.1f + ((moveSpeed / maxSpeed) * 1.2f);
                newPos.z = speedBarColor.transform.localPosition.z;
                speedBarColor.transform.localPosition = newPos;

                m_controller.Move(moveDir);
            }
            else {
                speedBarColor.SetActive(false);
                moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Verticle"));

                AutoTurn();
                AutoWalk();

                m_controller.Move(moveDir);
            }
        }
        else
        {
            Debug.Log("GAME PAUSED!");            
        }
    }

    private void ManualTurn() {
        transform.Rotate(0, (limiter * turnRate) * Time.deltaTime * rotateSpeed, 0);
    }

    private void ManualWalk() {
        moveDir = transform.TransformDirection(Vector3.forward);
        if (moveSpeed >= maxSpeed)
            moveDir *= (moveSpeed * Time.deltaTime) * runMultiplier;       
        else
            moveDir *= moveSpeed * Time.deltaTime;        
    }

    private void AutoTurn() {
        //transform.Rotate(0, Input.GetAxis("Rotation") * Time.deltaTime * rotateSpeed, 0);
    }

    private void AutoWalk() {
        moveDir = transform.TransformDirection(Vector3.forward);
        moveDir *= Time.deltaTime * runMultiplier;        
    }

    public void SetMoveSpeed(float speed) {
        moveSpeed = speed;
    }

    public void SetTurnRate(int tr) {
        if (limiter > 0.5f)
            limiter -= 1.5f * Time.deltaTime;
        else
            limiter = 0.0f;
    }

    public void turnLeft() {
        if (turnRate == 1)
            limiter = 0.0f;

        turnRate = -1;
        if (limiter < 2.0f)
            limiter += Time.deltaTime;
    }

    public void turnRight() {
        if (turnRate == -1)
            limiter = 0.0f;

        turnRate = 1;
        if (limiter < 2.0f)
            limiter += Time.deltaTime;
    }

    public void IncreaseSpeed() {
        if (moveSpeed < maxSpeed)
            moveSpeed += 2 * Time.deltaTime;
        else {
            moveSpeed = maxSpeed;
            if (m_leapMount.transform.localPosition.z > -4.5f) {
                rotateSpeed = 10;                

                m_leapMount.transform.Translate(0.0f, 0.0f, -1.8f * Time.deltaTime);
            }
            if (m_leapMount.transform.localPosition.y < 2.0f)           
                m_leapMount.transform.Translate(0.0f, Time.deltaTime, 0.0f);            
        }
    }

    public void DescreaseSpeed() {
        if (moveSpeed > 0) {
            if (m_leapMount.transform.localPosition.z < 0.36f) 
                m_leapMount.transform.Translate(0.0f, 0.0f, 1.8f * Time.deltaTime);
                
            if (m_leapMount.transform.localPosition.y > 2.4f)
                m_leapMount.transform.Translate(0.0f, -Time.deltaTime, 0.0f);

            moveSpeed -= 4 * Time.deltaTime;
        }
        else {
            if (m_leapMount.transform.localPosition.z < 0.36f) 
                m_leapMount.transform.Translate(0.0f, 0.0f, 1.8f * Time.deltaTime);
            
            if (m_leapMount.transform.localPosition.y > 2.4f)
                m_leapMount.transform.Translate(0.0f, -Time.deltaTime, 0.0f);

            moveSpeed = 0.0f;
        }
        rotateSpeed = 5;
    }

    public bool GetDriveMode(){
        return ManualDriveMode;
    }

    public void SetManualDrive(bool boolean) {
        ManualDriveMode = boolean;
    }

    public void StopAllMovement() {
        turnRate = 0;
        limiter = 0.0f;
        moveSpeed = 0.0f;
    }
}