namespace GD.Core.Enums
{
    using UnityEngine;

    public enum EnemyTypes
    {
        BASIC,
        KAMIKAZE,
        TRANSPORT,
        TRIDENT,
        BOSS
    };

    public enum EnemyStates
    {
        IDLE,
        PATROL,
        RUNNING,
        ATTACKING,
        SEARCHING
    }
    public enum Impairments
    {
        NONE,
        SLOWED,
        STUNNED,
    }


    public struct MovementStats
    {
        public float Speed { get; set; }
        public float Boost { get; set; }
        public float MaxSpeed { get; set; }
        public float RotateSpeed { get; set; }
        public float Acceleration { get; set; }        
    }
}