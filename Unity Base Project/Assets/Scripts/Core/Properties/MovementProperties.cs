using System;

[Serializable]
public class MovementProperties
{
    public float Speed;
    public float Boost;
    public float MaxSpeed;
    public float RotateSpeed;
    public float Acceleration;

    public void Reset()
    {
        Speed = 0f;
        Boost = 0f;
        MaxSpeed = 0f;
        RotateSpeed = 0f;
        Acceleration = 0f;
    }
}
