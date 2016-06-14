using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    #region Properties
    private Vector3 movedir;
    private Transform MyTransform;
    private x360Controller m_GamePad;
    public MovementProperties MoveData;
    private CharacterController controller;

    //  Auto-Movement
    private bool cruise;
    private bool stunned;
    private bool autoPilot;
    private bool resetRotation;
    private float speedAmt;
    private float orientationTimer;
    private Vector3 autoPilotDestination;

    ParticleSystem particles;
    #endregion


    void Start() {
        MyTransform = transform;

        MoveData.Boost = 1f;
        MoveData.MaxSpeed = 80f;
        MoveData.RotateSpeed = 50f;
        MoveData.Acceleration = 25f;
        MoveData.Speed = MoveData.MaxSpeed;
        speedAmt = 0f;

        cruise = false;
        stunned = false;
        autoPilot = false;
        resetRotation = false;
        movedir = Vector3.zero;
        orientationTimer = 0.0f;

        controller = GetComponent<CharacterController>();
        m_GamePad = GamePadManager.Instance.GetController(0);
        //OutOfBounds();
        particles = GetComponent<ParticleSystem>();
    }

    void Update(){
        if (!autoPilot && !resetRotation && !stunned)
        {
            if (m_GamePad.GetLeftTrigger() > 0f)
            {
                cruise = false;                
                MoveData.ChangeSpeed(m_GamePad.GetLeftTrigger());                
            }
            else if (!cruise)
                MoveData.DecreaseSpeed();

            if (m_GamePad.GetButtonDown("LeftThumbstick"))
                cruise = !cruise;            

            Yaw();
            Roll();
            Pitch();
            Flight();
        }
        else if (autoPilot)
            Autopilot();
        else if (resetRotation)
            Reorient();
        else if (stunned)
            MoveData.DecreaseSpeed();

        speedAmt = MoveData.Speed / MoveData.MaxSpeed;

        if (MoveData.Speed > 0.0f)
            particles.startSpeed = -speedAmt;
        else
            particles.startSpeed = 0;   
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
    public void Yaw()
    {
        if(m_GamePad.GetLeftStick().X != 0)
            MyTransform.Rotate(Vector3.up * Time.deltaTime * (MoveData.RotateSpeed * m_GamePad.GetLeftStick().X));
    }
    public void Roll()
    {
        if (m_GamePad.GetRightStick().X != 0)
            MyTransform.Rotate(Vector3.back * Time.deltaTime * (MoveData.RotateSpeed * m_GamePad.GetRightStick().X));
    }
    public void Pitch()
    {
        if (m_GamePad.GetLeftStick().Y != 0)
            MyTransform.Rotate(Vector3.right * Time.deltaTime * (MoveData.RotateSpeed * m_GamePad.GetLeftStick().Y));        
    }     
    void Flight()
    {
        if (MoveData.Speed <= 0f)
            return;

        AudioManager.instance.PlayThruster();
        AudioManager.instance.ThrusterVolume(speedAmt);

        movedir = Vector3.zero;
        movedir = MyTransform.forward;
        movedir *= MoveData.Speed * Time.deltaTime;
        controller.Move(movedir);
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
        movedir = Vector3.zero;
        movedir = MyTransform.forward;
        movedir *= MoveData.Speed * Time.deltaTime;
        controller.Move(movedir);
    }
    #endregion
}