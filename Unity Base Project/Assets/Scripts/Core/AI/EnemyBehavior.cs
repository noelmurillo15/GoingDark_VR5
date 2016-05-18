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
        ChangeState(EnemyStates.IDLE);
        AutoPilot = false;
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
    }             
    #endregion
}