using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour {
    //**    Attach Script to Player Gameobject   **//

    public bool ManualDriveMode;
    public bool HyperDriveMode;
    
    private int turnRate;
    private float maxSpeed;
    private float moveSpeed;
    private float rotateSpeed;
    private float runMultiplier;

    private Vector3 moveDir;

    private GameObject m_leapMount;
    private PlayerData m_playerData;
    private LeapDriveHUD m_driveHUD;
    public LeapMovement m_leapMovement;
    private CharacterController m_controller;

    public float speedBarX;
    public GameObject speedBarColor;


    // Use this for initialization
    void Start() {
        HyperDriveMode = false;
        ManualDriveMode = false;

        turnRate = 0;
        maxSpeed = 20.0f;
        moveSpeed = 0.0f;
        rotateSpeed = 20.0f;
        runMultiplier = 2.0f;

        speedBarX = 0.0f;

        moveDir = Vector3.zero;

        if (m_playerData == null)
            m_playerData = this.GetComponent<PlayerData>();

        if (m_leapMount == null)
            m_leapMount = GameObject.FindGameObjectWithTag("LeapMount");

        if (m_controller == null)
            m_controller = this.GetComponent<CharacterController>();

        if (m_driveHUD == null)
            m_driveHUD = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<LeapDriveHUD>();

        if (speedBarColor == null)
            speedBarColor = GameObject.Find("SpeedBarColor");

        if (m_leapMovement == null)
            m_leapMovement = GameObject.Find("DrivingBox").GetComponent<LeapMovement>();
    }

    // Update is called once per frame
    void Update() {       

        if (!m_playerData.GetGamePaused()) {
            if (ManualDriveMode) {
                moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Verticle"));

                m_driveHUD.UpdateManualMovement();

                ManualTurn();
                ManualWalk();

                #region SpeedHUD
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
                #endregion

                m_controller.Move(moveDir);
            }
            else {
                speedBarColor.SetActive(false);
            }
        }
        else
        {
            Debug.Log("GAME PAUSED!");            
        }
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

    public void goUp()
    {
        transform.Translate(Vector3.up * 0.5f);
    }

    public void goDown()
    {
        transform.Translate(Vector3.up * -0.5f);
    }
    public void turnRateZero() {
        turnRate = 0;
    }

    public void turnLeft() {
        turnRate = -1;
    }

    public void turnRight() {
        turnRate = 1;
    }

    public void IncreaseSpeed() {
        if (moveSpeed < maxSpeed)
            moveSpeed += 5 * Time.deltaTime;
        else {
            moveSpeed = maxSpeed;
            if (m_leapMount.transform.localPosition.z > -4.5f) {
                rotateSpeed = 30;                

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

            moveSpeed -= 10 * Time.deltaTime;
        }
        else {
            if (m_leapMount.transform.localPosition.z < 0.36f) 
                m_leapMount.transform.Translate(0.0f, 0.0f, 1.8f * Time.deltaTime);
            
            if (m_leapMount.transform.localPosition.y > 2.4f)
                m_leapMount.transform.Translate(0.0f, -Time.deltaTime, 0.0f);

            moveSpeed = 0.0f;
        }
        rotateSpeed = 20;
    }

    public bool GetDriveMode(){
        return ManualDriveMode;
    }

    public void SetManualDrive(bool boolean) {
        ManualDriveMode = boolean;
    }

    public void StopAllMovement() {
        turnRate = 0;
        moveSpeed = 0.0f;
    }
}