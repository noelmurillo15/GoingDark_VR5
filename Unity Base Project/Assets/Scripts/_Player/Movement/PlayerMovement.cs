using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {
    //**    Attach to Player    **//

    #region Properties
    private Vector3 moveDir;
    public MovementProperties MoveData;
    private Transform MyTransform;
    private CharacterController m_controller;

    //  Auto-Movement
    private bool autoPilot;
    private bool resetRotation;
    private float orientationTimer;
    private Vector3 autoPilotDestination;
    #endregion


    // Use this for initialization
    void Start() {
        moveDir = Vector3.zero;
        MyTransform = transform;
        m_controller = GetComponent<CharacterController>();

        MoveData.Speed = 0f;
        MoveData.Boost = 1f;
        MoveData.MaxSpeed = 100f;
        MoveData.RotateSpeed = 40f;
        MoveData.Acceleration = 25f;

        autoPilot = false;
        resetRotation = false;
        orientationTimer = 0.0f;
        OutOfBounds();
    }

    // Update is called once per frame
    void FixedUpdate(){
        if (!autoPilot && !resetRotation)
        {
            moveDir = Vector3.zero;

            //  Speed
            if (Input.GetAxis("LTrigger") > 0f)
            {
                IncreaseSpeed();
               // AudioManager.instance.PlayThrusterSound();
            }
            else
                DecreaseSpeed();

            //  Rotation
            if (Input.GetAxis("Horizontal") > 0f)
                TurnLeft();
            else if (Input.GetAxis("Horizontal") < 0f)
                TurnRight();

            if (Input.GetAxis("Vertical") > 0f)
                GoUp();
            else if (Input.GetAxis("Vertical") < 0f)
                GoDown();

            if (Input.GetAxis("RVertical") > 0f)
                RollRight();
            else if (Input.GetAxis("RVertical") < 0f)
                RollLeft();

            Flight();
        }
        else if (autoPilot)
            Autopilot();
        else if (resetRotation)
            Reorient();        
    }

    #region Movement
    public MovementProperties GetMoveData()
    {
        return MoveData;
    }
    public void StopMovement()
    {
        MoveData.Speed = 0f;
    }
    public void IncreaseSpeed()
    {
        if (MoveData.Speed < (MoveData.MaxSpeed * MoveData.Boost))
            MoveData.Speed += Time.deltaTime * MoveData.Acceleration;
        else if (MoveData.Speed > (MoveData.MaxSpeed * MoveData.Boost) + .5f)
            DecreaseSpeed();
    }
    public void DecreaseSpeed()
    {
        MoveData.Speed = Mathf.Lerp(MoveData.Speed, 0f, Time.deltaTime * .5f);
    }
    public void TurnLeft()
    {
        MyTransform.Rotate(Vector3.up * Time.deltaTime * -MoveData.RotateSpeed);
    }
    public void TurnRight()
    {
        MyTransform.Rotate(Vector3.up * Time.deltaTime * MoveData.RotateSpeed);     
    }
    public void GoUp()
    {
        MyTransform.Rotate(Vector3.right * Time.deltaTime * MoveData.RotateSpeed);
    }
    public void GoDown()
    {
        MyTransform.Rotate(Vector3.right * Time.deltaTime * -MoveData.RotateSpeed);
    }  
    public void RollLeft()
    {
        MyTransform.Rotate(Vector3.back * Time.deltaTime * -MoveData.RotateSpeed);
    }
    public void RollRight()
    {
        MyTransform.Rotate(Vector3.back * Time.deltaTime * MoveData.RotateSpeed);
    }
    void Flight()
    {
        if (MoveData.Speed <= 0f)
            return;

        moveDir = MyTransform.TransformDirection(Vector3.forward);
        moveDir *= MoveData.Speed * Time.deltaTime;
        m_controller.Move(moveDir);
    }
    #endregion

    #region Auto-Pilot
    void OutOfBounds()
    {
        autoPilot = true;
        autoPilotDestination = Vector3.zero;
    }
    void OutOfBounds(Vector3 targetPos)
    {
        autoPilot = true;
        autoPilotDestination = targetPos;
    }
    void InBounds()
    {
        autoPilot = false;
    }
    void ResetOrientation()
    {
        orientationTimer = 5.0f;
        resetRotation = true;
    }
    void Reorient()
    {
        if (MyTransform.rotation != Quaternion.identity && orientationTimer > 0.0f)
        {
            orientationTimer -= Time.deltaTime;
            MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.identity, Time.deltaTime * 0.5f);
        }
        else
            resetRotation = false;
    }
    void Autopilot() {
        //  Look At
        Vector3 playerDir = autoPilotDestination - MyTransform.position;
        Vector3 destination = Vector3.RotateTowards(MyTransform.forward, playerDir, (MoveData.RotateSpeed * 0.05f) * Time.deltaTime, 0.0f);
        MyTransform.rotation = Quaternion.LookRotation(destination);

        //  Move Towards
        IncreaseSpeed();
        moveDir = Vector3.zero;
        moveDir = MyTransform.TransformDirection(Vector3.forward);
        moveDir *= (MoveData.Speed * Time.deltaTime);
        m_controller.Move(moveDir);        
    }                  
    #endregion
}