namespace GoingDark.Core.Enums
{

    public enum MissileType
    {
        BASIC,
        EMP,
        SHIELDBREAKER,
        CHROMATIC
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
        BOSS,
        ANY
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
        IDLE,
        PATROL,
        RUNNING,
        ATTACKING,
        FOLLOW,
        ALERT,
        LOST
    }
    #endregion

    #region Ship Systems
    public enum SystemType
    {
        NONE,
        EMP,
        CLOAK,
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

    public enum MissionType
    {
        SCAVENGE,
        COMBAT,
        STEALTH
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