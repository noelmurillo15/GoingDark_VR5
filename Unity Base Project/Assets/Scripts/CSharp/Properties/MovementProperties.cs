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
            Speed += Time.deltaTime * Acceleration;
        else if (Speed > (MaxSpeed * Boost * triggerVal) + 1.0f)
            DecreaseSpeed();
    }
    public void IncreaseSpeed()
    {
        if (Speed < (MaxSpeed * Boost))
            Speed += Time.deltaTime * Acceleration;
        else if (Speed > (MaxSpeed * Boost) + .5f)
            DecreaseSpeed();
    }
    public void DecreaseSpeed()
    {
        if (Speed > 1f)
            Speed = Mathf.Lerp(Speed, 0f, Time.deltaTime * .1f);
        else
            Speed = 0f;
    }
}
