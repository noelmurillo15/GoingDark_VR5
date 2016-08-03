namespace GoingDark.Core.Enums
{
    public enum GameDifficulty
    {
        Easy,
        Normal,
        Hard,
        Nightmare
    }
    public enum Impairments
    {
        None,
        Slowed,
        Stunned,
    }
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
        //BasicMissileUpgrade,
        //ShieldBreakMissileUpgrade,
        //ChromaticMissileUpgrade,
        //EMPMissileUpgrade,
        LaserPowerUpgrade,
        Laser2PowerUpgrade,
        HealthUpgrade,
        ShieldUpgrade,
        //HyperdriveUpgrade,
        //CloakUpgrade,
        //EMPUpgrade
    }

    #region Projectiles
    public enum MissileType
    {
        Basic,
        Emp,
        ShieldBreak,
        Chromatic
    }

    public enum LaserType
    {
        Basic,
        Charged
    }
    #endregion

    #region EnemyAI
    public enum EnemyTypes
    {
        None,
        Basic,
        Droid,
        SquadLead,
        JetFighter,
        Transport,
        Trident,
        Boss,   
        Any     
    };
    public enum EnemyDifficulty
    {
        NONE,
        EASY,
        MED,
        HARD,
        NIGHTMARE
    }
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
        Online
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