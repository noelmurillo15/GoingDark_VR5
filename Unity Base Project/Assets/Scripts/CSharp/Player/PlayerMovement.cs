using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    #region Properties
    public float speedAmt;
    public bool boostActive;
    public MovementProperties MoveData;


    private PlayerStats stats;
    private Transform MyTransform;
    private ParticleSystem particles;
    private x360Controller controller;
    private AudioManager _audioInstance;
    private CharacterController charControl;
    #endregion


    void Start()
    {
        speedAmt = 0f;
        boostActive = false;
        MyTransform = transform;
        MoveData.Set(80f, 1f, 120f, 50f, 20f);

        stats = GetComponent<PlayerStats>();
        _audioInstance = AudioManager.instance;
        particles = GetComponent<ParticleSystem>();
        charControl = GetComponent<CharacterController>();
        controller = GamePadManager.Instance.GetController(0);
    }

    void FixedUpdate()
    {
        if (!stats.GetDebuffData().isStunned)
        {
            if (controller.GetLeftTrigger() > 0f)
                MoveData.ChangeSpeed(controller.GetLeftTrigger());
            else
                MoveData.DecreaseSpeed();
            
            Yaw();
            Roll();
            Pitch();
            Flight();
        }
        else if (stats.GetDebuffData().isStunned)
            MoveData.DecreaseSpeed();

        speedAmt = MoveData.Speed / MoveData.MaxSpeed;
    }

    #region Accessors
    public MovementProperties GetMoveData()
    {
        return MoveData;
    }
    #endregion

    #region Movement    
    public void StopMovement()
    {
        MoveData.Speed = 0f;
    }
    public void Yaw()
    {
        MyTransform.Rotate(Vector3.up * Time.fixedDeltaTime * (MoveData.RotateSpeed * controller.GetLeftStick().X));
    }
    public void Roll()
    {
        MyTransform.Rotate(Vector3.back * Time.fixedDeltaTime * (MoveData.RotateSpeed * controller.GetRightStick().X));
    }
    public void Pitch()
    {
        MyTransform.Rotate(Vector3.right * Time.fixedDeltaTime * (MoveData.RotateSpeed * controller.GetLeftStick().Y));
    }
    void Flight()
    {
        if (MoveData.Speed <= 0f)
            return;

        _audioInstance.ThrusterVolume(speedAmt);
        particles.startSpeed = -(speedAmt * .1f);

        Vector3 movedir = MyTransform.forward;
        movedir *= MoveData.Speed * Time.fixedDeltaTime;
        charControl.Move(movedir);
    }
    #endregion    
}