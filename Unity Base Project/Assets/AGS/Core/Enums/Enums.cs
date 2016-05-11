namespace AGS.Core.Enums
{
    public enum InputButtonType
    {
        Button,
        ButtonDown,
        ButtonUp,
        DoubleTap,
        DoubleTapAndHold
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight,
        None
    }
    public enum VectorDirection
    {
        Up,
        Down,
        Forward,
        Back        
    }
    public enum MovingEnvironmentMovementType
    {
        Normal,
        Lerp,
        Slerp        
    }
    public enum EnvironmentPathDirection
    {
        Forward,
        Backward
    }

    public enum EnvironmentPathType
    {
        PingPong,
        Circular
    }

    public enum DamageableResourceType
    {
        Health,
        Stamina,
        Air
    }
    public enum ResourceEffectType
    {
        Heal,
        Damage
    }
    public enum StatusEffectStrengthType
    {
        FixedValue,
        Percentage
    }

    public enum SuperNaturalEffectType
    {
        Invulnerability,
        DamageResistance
    }

    public enum MovementEffectType
    {
        SpeedChange,
        Stun
    }
    public enum HitObstacle
    {
        None,
        Ground,
        MovingGround,
        LadderTop,
        LadderStand,
        Movable,
        Stairs,
        RampTop,
        Character,
        Ragdoll
    }
    public enum InteractionTargetHeight
    {
        Low,
        Mid,
        High
    }
    public enum MovableObjectWeight
    {
        Tiny,
        Light,
        Medium,
        Heavy,
        Massive
    }
    public enum MovableObjectState
    {
        Idle,
        Grabbed,
        PickedUp,
        Carried,
        Thrown
    }
    public enum CombatSkillFireType
    {
        OneShot,
        Sustained
    }
    public enum CombatSkillAttackType
    {
        Primary,
        Secondary
    }
    public enum CombatMoveSetType
    {
        Unarmed,
        Pistol,
        Bazooka
    }
    public enum CombatMoveType
    {
        SingleHit,
        ComboStarter,
        ComboChainer,
        ComboFinisher
    }
    public enum CombatMovePosition
    {
        Standing,
        Airborne,
        Crouching,
        Sneaking,
        Swimming
    }
    public enum ThrowableWeaponType
    {
        Grenade,
        StickyBomb
    }
    public enum ThrowableWeaponThrowingType
    {
        Forward,
        Arc
    }
    public enum ThrowingSkillHand
    {
        Left,
        Right
    }

    public enum HitVolumeIndex
    {
        First,
        Second,
        Third,
        Fourth,
        None
    }

    public enum ProjectileType
    {
        Bullet,
        Grenade,
        Missile
    }
    public enum ProjectileSubType
    {
        Normal,
        Laser
    }

    public enum GameLevelLimitSide
    {
        Left,
        Right,
        Top,
        Bottom
    }
    public enum GameLevelBoundType
    {
        Block,
        Kill
    }
    public enum ForceType
    {
        Normal,
        Impulse
    }
}
