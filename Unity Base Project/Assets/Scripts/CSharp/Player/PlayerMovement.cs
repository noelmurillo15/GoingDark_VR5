using UnityEngine;
using GoingDark.Core.Enums;

public class PlayerMovement : MonoBehaviour
{

    #region Properties
    public float speedAmt;
    public bool boostActive;
    public MovementProperties MoveData;

    private x360Controller controller;

    private PlayerStats stats;
    private Transform MyTransform;
    private ParticleSystem particles;
    private AudioManager _audioInstance;
    private Rigidbody MyRigidbody;
    #endregion


    void Start()
    {
        MoveData.Set(80f, 1f, 120f, 50f, 20f);
        speedAmt = 0f;
        boostActive = false;
        MyTransform = transform;

        stats = GetComponent<PlayerStats>();
        _audioInstance = AudioManager.instance;
        MyRigidbody = GetComponent<Rigidbody>();
        particles = GetComponent<ParticleSystem>();
        controller = GamePadManager.Instance.GetController(0);
    }

    void FixedUpdate()
    {
        if (stats.GetDebuffData().debuff != Impairments.Stunned)
        {                       
            Yaw();
            Roll();
            Pitch();
            Flight();
        }
        else
            MoveData.DecreaseSpeed();        
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
        if (controller.GetLeftStick().X != 0f)
            MyTransform.Rotate(Vector3.up * Time.fixedDeltaTime * (MoveData.RotateSpeed * controller.GetLeftStick().X));        
    }
    public void Roll()
    {
        if (controller.GetRightStick().X != 0f)
            MyTransform.Rotate(Vector3.back * Time.fixedDeltaTime * (MoveData.RotateSpeed * controller.GetRightStick().X));
    }
    public void Pitch()
    {
        if (controller.GetLeftStick().Y != 0f)
            MyTransform.Rotate(Vector3.right * Time.fixedDeltaTime * (MoveData.RotateSpeed * controller.GetLeftStick().Y));        
    }
    void Flight()
    {
        speedAmt = MoveData.Speed / MoveData.MaxSpeed;
        _audioInstance.ThrusterVolume(speedAmt);
        particles.startSpeed = -(speedAmt * .1f);

        MyRigidbody.AddForce(MyTransform.forward * MoveData.Speed, ForceMode.VelocityChange);
        if (MyRigidbody.velocity.magnitude > MoveData.Speed)
            MyRigidbody.velocity = MyRigidbody.velocity.normalized * MoveData.Speed;        
    }
    #endregion    
}