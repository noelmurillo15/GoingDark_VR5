﻿namespace GoingDark.Core.Enums
{
    public enum GameDifficulty
    {
        Easy,
        Normal,
        Hard,
        Nightmare
    }

    #region Debuffs
    public enum Impairments
    {
        None,
        Slowed,
        Stunned,
    }
    #endregion

    #region Store
    public enum ItemType
    {
        Consumable,
        Upgrade
    }

    public enum ShopType
    {
        ConsumableList,
        WeaponList,
        DeviceList
    }

    public enum Items
    {
        BasicMissile,
        ShieldBreakMissile,
        ChromaticMissile,
        EMPMissile,
        LaserPowerUpgrade,
        Laser2PowerUpgrade,
        HealthUpgrade,
        ShieldUpgrade,
    }

    public struct Item
    {
        public string ItemName;
        public int ItemPrice;
        public int ItemLevel;
        public ItemType ItemType;
        public Items Type;
    }
    #endregion

    #region Projectiles
    public enum MissileType
    {
        Basic,
        Emp,
        ShieldBreak,
        Chromatic,
        NumberOfType        
    }
    public enum LaserType
    {
        Basic,
        Charged,
        NumberOfType
    }
    #endregion

    #region Enemies
    public enum EnemyMissileType
    {
        Basic,
    }
    public enum EnemyLaserType
    {
        Basic,
        Charged,
        MiniCannon,
        Cannon
    }
    public enum EnemyTypes
    {
        None,
        Basic,
        Droid,
        SquadLead,
        JetFighter,
        Transport,
        Trident,
        Tank,
        Any,
        FinalBoss
    };
    public enum EnemyStates
    {
        Idle,
        Patrol,
        Alert,
        Follow,
        Flee,
        Puzzled,
        Attack,
    }    
    #endregion

    #region Ship Systems
    public enum SystemType
    {
        None,
        Emp,
        Cloak,
        Decoy,
        Laser,
        Shield,
        Missile,
        Hyperdrive
    }   
    public enum SystemStatus
    {
        Offline,
        Online,
        Damaged
    }    
    #endregion

    #region Missions
    public enum MissionType
    {
        Scavenge,
        Combat,
        Stealth,
        Elimination,
        ControlPoint
    }
    public struct Mission
    {
        public string missionName;
        public string missionInfo;
        public string blueprint;

        public bool completed;
        public bool isOptional;
        public bool spotted;
        public bool isActive;

        public float missionTimer;

        public int credits;
        public int objectives;

        public MissionType type;
        public EnemyTypes enemy;
    }
    #endregion
}