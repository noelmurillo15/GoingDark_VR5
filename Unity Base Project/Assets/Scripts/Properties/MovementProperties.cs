using System;
using UnityEngine;

[Serializable]
public class MovementProperties
{
    public float Speed;
    public float Boost;
    public float MaxSpeed;
    public float RotateSpeed;
    public float Acceleration;

    public MovementProperties()
    {

    }

    public void Set(float _speed, float _boost, float _maxspeed, float _rotatespeed, float _accel)
    {
        Speed = _speed;
        Boost = _boost;
        MaxSpeed = _maxspeed;
        Acceleration = _accel;
        RotateSpeed = _rotatespeed;
    }

    public void SetSpeed(float _val)
    {
        Speed = _val;
    }
    public void SetBoost(float _val)
    {
        Boost = _val;
    }
    public void SetMaxSpeed(float _val)
    {
        MaxSpeed = _val;
    }
    public void SetAccel(float _val)
    {
        Acceleration = _val;
    }
    public void SetRotateSpeed(float _val)
    {
        RotateSpeed = _val;
    }

    public void Reset()
    {
        Speed = 0f;
        Boost = 0f;
        MaxSpeed = 0f;
        RotateSpeed = 0f;
        Acceleration = 0f;
    }

    public void ChangeSpeed(float triggerVal)
    {
        if (Speed < (MaxSpeed * Boost * triggerVal))
            Speed += Time.fixedDeltaTime * Acceleration;
        else
            DecreaseSpeed();
    }
    public void IncreaseSpeed()
    {
        if (Speed < (MaxSpeed * Boost))
            Speed += Time.fixedDeltaTime * Acceleration;
        else
            DecreaseSpeed();
    }
    public void DecreaseSpeed()
    {
        if (Speed > 1f)
            Speed = Mathf.Lerp(Speed, 0f, Time.fixedDeltaTime * .5f);
        else
            Speed = 0f;
    }
}
