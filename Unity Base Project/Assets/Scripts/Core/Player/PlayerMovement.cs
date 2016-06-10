using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    //**    Attach to Player    **//

    #region Properties
    public MovementProperties MoveData;
    private Transform MyTransform;
    private x360Controller m_GamePad;
    private Rigidbody MyRigidbody;

    //  Auto-Movement
    private bool stunned;
    private bool autoPilot;
    private bool resetRotation;
    private float orientationTimer;
    private Vector3 autoPilotDestination;
    #endregion


    // Use this for initialization
    void Start() {
        MyTransform = transform;

        MoveData.Boost = 1f;
        MoveData.MaxSpeed = 5f;
        MoveData.RotateSpeed = 40f;
        MoveData.Acceleration = 1f;

        stunned = false;
        autoPilot = false;
        resetRotation = false;
        orientationTimer = 0.0f;

        MyRigidbody = GetComponent<Rigidbody>();
        m_GamePad = GamePadManager.Instance.GetController(0);
        MoveData.Speed = MoveData.MaxSpeed;
        //OutOfBounds();
    }

    // Update is called once per frame
    void Update(){
        if (!autoPilot && !resetRotation && !stunned)
        {
            //  Speed
            if (m_GamePad.GetLeftTrigger() > 0f)
                MoveData.ChangeSpeed(m_GamePad.GetLeftTrigger());
            else
                MoveData.DecreaseSpeed();

            //  Rotation
            if (m_GamePad.GetLeftStick().X > 0f)
                TurnRight();
            else if (m_GamePad.GetLeftStick().X < 0f)
                TurnLeft();

            if (m_GamePad.GetLeftStick().Y > 0f)
                GoUp();
            else if (m_GamePad.GetLeftStick().Y < 0f)
                GoDown();

            // right jstick
            if (m_GamePad.GetRightStick().X > 0f)
                RollRight();
            else if (m_GamePad.GetRightStick().X < 0f)
                RollLeft();

            Flight();
        }
        else if (autoPilot)
            Autopilot();
        else if (resetRotation)
            Reorient();
        else if (stunned)
            MoveData.DecreaseSpeed();     
    }

     public void PlayerStunned()
    {
        stunned = true;
        Invoke("Healed", 5f);
    }

    void Healed()
    {
        stunned = false;
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

        MyRigidbody.MovePosition(MyTransform.position + (MyTransform.forward * MoveData.Speed));
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
        MoveData.IncreaseSpeed();
        MyRigidbody.MovePosition(MyTransform.position + (MyTransform.forward * MoveData.Speed));
    }
    #endregion
}