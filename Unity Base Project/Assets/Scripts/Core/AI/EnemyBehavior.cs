using UnityEngine;
using GD.Core.Enums;

public class EnemyBehavior : IEnemy
{

    #region Properties    
    public bool AutoPilot;
    public EnemyStates State;
    #endregion

    void Awake()
    {
        Debug.Log("EnemyBehavior Initializing...");
        base.Initialize();
        State = EnemyStates.IDLE;        
        Debug.Log("EnemyBehavior READY");
    }

    #region Public Methods
    public void ChangeState(EnemyStates newState)
    {
        Debug.Log("Changing Enemy State");
        State = newState;
        ChangeBehavior();
    }

    public void ChangeBehavior() {
        Debug.Log("Changing Enemy Behavior");
        switch (State)
        {
            case EnemyStates.IDLE:
                break;
            case EnemyStates.PATROL:
                break;
            case EnemyStates.RUNNING:
                break;
            case EnemyStates.ATTACKING:
                break;
            case EnemyStates.SEARCHING:
                break;


            default:
                Debug.Log("Invalid Enemy State!");
                break;
        }
    }             
    #endregion
}