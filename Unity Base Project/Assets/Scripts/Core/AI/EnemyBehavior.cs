using UnityEngine;
using GD.Core.Enums;

public class EnemyBehavior : MonoBehaviour
{

    #region Properties
    public EnemyStates State;    
    protected EnemyStats stats;
    #endregion

    public virtual void Init()
    {
        Debug.Log("EnemyBehavior Initialize Called");
        ChangeState(EnemyStates.IDLE);
        stats = null;
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