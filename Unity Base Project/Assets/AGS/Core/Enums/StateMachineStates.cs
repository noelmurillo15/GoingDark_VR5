namespace AGS.Core.Enums
{
    public enum GameLevelState
    {
        Start,
        Running,
        Completed,
        Failed,
        PlayerDead
    }
    public enum MovingEnvironmentState
    {
        Idle,
        Moving
    }
    public enum DamageableState
    {
        Normal,
        Invulnerable,
        Destroyed,
        Weakened,
        Strengthened
    }
    public enum MovableState
    {
        Normal,
        Slowed,
        Hasted,
        Stunned
    }
    public enum SkillState
    {
        Enabled,
        Disabled
    }
    public enum HorizontalMovementIntention
    {
        Idle,
        Move,
        Sprint,
        Sneak,
        Crouch,
        StandUp,
        Strafe
    }
    public enum HorizontalMovementState
    {
        Idle,
        Moving,
        Crouching,
        Sprinting,
        Sneaking,
        Strafing
    }
    public enum VerticalMovementIntention
    {
        None,
        Idle,
        Jump,
        WallJump,
        Fall,
        Land
    }
    public enum VerticalMovementState
    {
        Idle,
        Landing,
        Jumping,
        Falling,
        WallJumping
    }
    public enum SlidingIntention
    {
        None,
        ManualSlide,
        NaturalSlide,
        HelplessSlide,
        PreventSlide,
    }
    public enum SlidingState
    {
        Idle,
        ManualSliding,
        NaturalSliding,
        HelplessSliding,
        PreventSliding
    }
    public enum SwimmingIntention
    {
        None,
        SurfaceJump,
        DoStroke
    }
    public enum SwimmingState
    {
        OutOfWater,
        InWater,
        DoingStroke,
        SurfaceJumping
    }
    public enum HazardState
    {
        Active,
        Recharging,
        Inactive
    }
    public enum LadderClimbingState
    {
        Idle,
        Approaching,
        Climbing,
        Releasing,
        ExitingTop,
        Jumping        
    }
    public enum LedgeClimbingIntention
    {
        None,
        Climb,
        Release,
        Jump
    }
    public enum LedgeClimbingState
    {
        Idle,
        Approaching,
        Climbing,
        Releasing,
        Grabbing,
        Jumping        
    }
    public enum ObjectMovementIntention
    {
        None,
        Grab,
        Release,
        Carry,
        Throw
    }
    public enum ObjectMovementState
    {
        Idle,
        Approaching,
        Grabbing,
        Releasing,
        Carrying,
        Throwing
    }
    public enum SwingingState
    {
        Idle,
        Approaching,
        Swinging,
        Climbing,
        StopSwinging,
        Releasing,
        Jumping
    }
    public enum SwingingStateIntention
    {
        None,
        Climb,
        Release,
        Jump,
        Swing,
        StopSwing
    }
    public enum SwitchInteractionState
    {
        Idle,
        Approaching,
        Switching
    }
    public enum SwingUnitState
    {
        Idle,
        SwingingNatural,
        ReducingSpeed
    }
    public enum CombatSkillState
    {
        Idle,
        Charging,
        Firing,
        SustainedFiring,
        Recharing
    }
    public enum CombatSkillStateIntention
    {
        None,
        Charge,
        Fire,
        SustainedFire
    }
    public enum AIStateMachineState
    {
        Idle,
        Patrolling,
        Chasing,
        Attacking,
        Puzzled
    }
    public enum AIStateIntention
    {
        None,
        Patrol,
        Chase,
        Attack
    }
    public enum AimingStateMachineState
    {
        Idle,
        Aiming,
        LockedOnTarget
    }
    public enum AimingStateIntention
    {
        None,
        Aim,
        LockOnTarget
    }
    public enum MissionState
    {
        Active,
        Completed,
        Failed,
        Inactive
    }
}
