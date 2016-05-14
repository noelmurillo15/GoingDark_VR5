﻿namespace GD.Core.Enums
{
    using UnityEngine;
    public enum MissileType
    {
        EMP,
        BASIC,
        CHROMATIC,
        SHIELDBREAKER
    }
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

    public enum SystemStatus
    {
        NOTAVAILABLE,
        OFFLINE,
        ONLINE
    }

    public enum SystemType
    {
        EMP,
        CLOAK,
        RADAR,
        DECOY,
        LASERS,
        MISSILES,
        HYPERDRIVE
    }   

    public struct ShipDevice
    {        
        public SystemStatus Status { get; set; }       
        public GameObject Object { get; set; }
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