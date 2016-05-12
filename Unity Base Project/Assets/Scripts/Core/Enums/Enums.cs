namespace GD.Core.Enums
{
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
}