using UnityEngine;

public class FlightMovement : MonoBehaviour
{

    #region Properties
    public bool boostActive;
    public MovementProperties MoveData;


    private Transform MyTransform;
    private ParticleSystem particles;
    private x360Controller controller;
    private AudioManager _audioInstance;
    private Rigidbody MyRigidbody;
    #endregion


    void Start()
    {
        boostActive = false;
        MyTransform = transform;
        MoveData.Set(0f, 1f, 250f, 80f, 40f);

        _audioInstance = AudioManager.instance;
        MyRigidbody = GetComponent<Rigidbody>();
        particles = GetComponent<ParticleSystem>();
        controller = GamePadManager.Instance.GetController(0);
    }

    void FixedUpdate()
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
        if (MoveData.Speed <= 0f)
            return;

        Vector3 movedir = MyTransform.forward;
        movedir *= MoveData.Speed * Time.fixedDeltaTime;
        MyRigidbody.MovePosition(MyTransform.position + MyTransform.forward * Time.fixedDeltaTime * MoveData.Speed);
    }
    #endregion    
}