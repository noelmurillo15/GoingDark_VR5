using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    #region Properties
    public MovementProperties MoveData;

    private bool cruise;
    private bool stunned;
    private bool autoPilot;
    private bool resetRotation;
    public bool boostActive;

    private float speedAmt;
    private float orientationTimer;

    private Transform MyTransform;
    private x360Controller m_GamePad;
    private ParticleSystem particles;
    private Vector3 autoPilotDestination;
    private CharacterController controller;
    #endregion


    void Start() {
        MyTransform = transform;
        MoveData.Set(80f, 1f, 120f, 50f, 20f);

        speedAmt = 0f;
        orientationTimer = 0.0f;

        cruise = false;
        stunned = false;
        autoPilot = false;
        resetRotation = false;
        boostActive = false;

        particles = GetComponent<ParticleSystem>();
        controller = GetComponent<CharacterController>();
        m_GamePad = GamePadManager.Instance.GetController(0);
    }

    void Update(){
        if (!autoPilot && !resetRotation && !stunned)
        {
            if (!boostActive)
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
            }
            else
                MoveData.ChangeSpeed(1f);

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
            MyTransform.Rotate(Vector3.up * Time.deltaTime * (MoveData.RotateSpeed * m_GamePad.GetLeftStick().X));
    }
    public void Roll()
    {
            MyTransform.Rotate(Vector3.back * Time.deltaTime * (MoveData.RotateSpeed * m_GamePad.GetRightStick().X));
    }
    public void Pitch()
    {
            MyTransform.Rotate(Vector3.right * Time.deltaTime * (MoveData.RotateSpeed * m_GamePad.GetLeftStick().Y));        
    }     
    void Flight()
    {
        if (MoveData.Speed <= 0f)
            return;

        particles.startSpeed = -speedAmt;
        AudioManager.instance.PlayThruster();
        AudioManager.instance.ThrusterVolume(speedAmt);

        Vector3 movedir = MyTransform.forward;
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

        MoveData.IncreaseSpeed();
        Flight();
    }
    #endregion
}