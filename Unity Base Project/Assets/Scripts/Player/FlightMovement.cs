using UnityEngine;

public class FlightMovement : MonoBehaviour
{

    #region Properties
    public bool boostActive;
    public MovementProperties MoveData;


    private Transform MyTransform;
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
        controller = GamePadManager.Instance.GetController(0);
    }

    void Update()
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
            MyTransform.Rotate(Vector3.up * Time.deltaTime * (MoveData.RotateSpeed * controller.GetLeftStick().X));
    }
    public void Roll()
    {
        if (controller.GetRightStick().X != 0f)
            MyTransform.Rotate(Vector3.back * Time.deltaTime * (MoveData.RotateSpeed * controller.GetRightStick().X));
    }
    public void Pitch()
    {
        if (controller.GetLeftStick().Y != 0f)
            MyTransform.Rotate(Vector3.right * Time.deltaTime * (MoveData.RotateSpeed * controller.GetLeftStick().Y));
    }
    void Flight()
    {
        if (MoveData.Speed <= 0f)
            return;

        _audioInstance.PlayThruster();

        MyRigidbody.AddForce(MyTransform.forward * MoveData.Speed, ForceMode.VelocityChange);
        if (MyRigidbody.velocity.magnitude > MoveData.Speed)
            MyRigidbody.velocity = MyRigidbody.velocity.normalized * MoveData.Speed;        
    }
    #endregion    
}