namespace GoingDark.Core.Enums
{
    public enum GameDifficulty
    {
        Easy,
        Normal,
        Hard,
        Nightmare
    }

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
        Charged,
        Ball,
        Continuous
    }
    public enum Impairments
    {
        None,
        Slowed,
        Stunned,
    }

    #region EnemyAI
    public enum EnemyTypes
    {
        None,
        Basic,
        Droid,
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

    public enum MissionType
    {
        Scavenge,
        Combat,
        Stealth
    }

    public struct Mission
    {
        public string missionName;
        public string missionInfo;

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
}