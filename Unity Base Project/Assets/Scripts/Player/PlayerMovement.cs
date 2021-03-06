﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    #region Properties
    public int invert;
    public float speedAmt;
    public bool boostActive;
    public MovementProperties MoveData;

    private x360Controller controller;

    private Transform MyTransform;
    private ParticleSystem particles;
    private AudioManager _audioInstance;
    private Rigidbody MyRigidbody;
    #endregion


    void Start()
    {
        invert = PersistentGameManager.Instance.GetOptionInvert();
        MoveData.Set(0f, 1f, 100f, 60f, 20f);
        speedAmt = 0f;
        boostActive = false;
        MyTransform = transform;

        _audioInstance = AudioManager.instance;
        MyRigidbody = GetComponent<Rigidbody>();
        particles = GetComponent<ParticleSystem>();
        controller = GamePadManager.Instance.GetController(0);
    }

    void FixedUpdate()
    {
        if (boostActive)
        {
            MoveData.Boost = 5f;
            MoveData.Acceleration = 50f;
        }

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
            MyTransform.Rotate(Vector3.right * Time.fixedDeltaTime * (MoveData.RotateSpeed * invert * controller.GetLeftStick().Y));        
    }
    void Flight()
    {
        if (MoveData.MaxSpeed > 0f)
            speedAmt = MoveData.Speed / MoveData.MaxSpeed;
        else
            speedAmt = 0f;

        _audioInstance.ThrusterVolume(speedAmt);
        particles.startSpeed = -(speedAmt * .1f);

        MyRigidbody.AddForce(MyTransform.forward * MoveData.Speed, ForceMode.VelocityChange);
        if (MyRigidbody.velocity.magnitude > MoveData.Speed)
            MyRigidbody.velocity = MyRigidbody.velocity.normalized * MoveData.Speed;        
    }
    #endregion    
}