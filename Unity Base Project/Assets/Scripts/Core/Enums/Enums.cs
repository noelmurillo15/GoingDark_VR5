﻿namespace GD.Core.Enums
{
    public enum MissileType
    {
        NONE,
        EMP,
        BASIC,
        CHROMATIC,
        SHIELDBREAKER
    }
    public enum Impairments
    {
        NONE,
        SLOWED,
        STUNNED,
    }

    #region EnemyAI
    public enum EnemyTypes
    {
        NONE,
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
    #endregion

    #region Ship Systems
    public enum SystemType
    {
        NONE,
        EMP,
        CLOAK,
        RADAR,
        DECOY,
        LASERS,
        SHIELD,
        MISSILES,
        HYPERDRIVE
    }   
    public enum SystemStatus
    {
        OFFLINE,
        ONLINE
    }
    #endregion

    public struct MovementStats
    {
        public float Speed { get; set; }
        public float Boost { get; set; }
        public float MaxSpeed { get; set; }
        public float RotateSpeed { get; set; }
        public float Acceleration { get; set; }        
    }
}