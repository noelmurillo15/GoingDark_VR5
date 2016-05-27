namespace GD.Core.Enums
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
        ALERT
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
}