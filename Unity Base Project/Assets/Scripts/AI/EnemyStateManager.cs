using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyStateManager : MonoBehaviour {

    #region Properties       
    [SerializeField] 
    private EnemyStates State;

    private IEnemy stats;
    public Transform Target { get; private set; }
    public Vector3 LastKnownPos { get; private set; }

    private bool lostSight;
    private float losingsightTimer;
    #endregion

    void Start()
    {
        Target = null;
        lostSight = false;
        losingsightTimer = 0f;
        LastKnownPos = Vector3.zero;
        stats = GetComponent<IEnemy>();
    }

    private void FixedUpdate()
    {
        if (losingsightTimer > 0.0f)
            losingsightTimer -= Time.fixedDeltaTime;        

        if (lostSight && losingsightTimer <= 0)
        {
            lostSight = false;
            SetEnemyTarget(null);
        }
    }

    #region Accessors
    public EnemyStates GetState()
    {
        return State;
    }
    #endregion

    #region Modifiers
    void BroadcastAlert(object[] storage)
    {
        if (Target != null)
            if (Vector3.Distance((Vector3)storage[1], transform.position) <= (1000f * stats.GetDifficultyMultiplier()))
                SetLastKnown((Vector3)storage[0]);
    }
    public void SetLastKnown(Vector3 pos)
    {
        LastKnownPos = pos;
        ChangeState(EnemyStates.Alert);
    }
    public void SetEnemyTarget(Transform _target)
    {
        Target = _target;
        if (Target != null)
        {
            if (Target.CompareTag("Player"))
                stats.GetManager().PlayerSeen();

            ChangeState(EnemyStates.Attack);
            return;
        }
        ChangeState(EnemyStates.Patrol);
    }
    void ChangeState(EnemyStates newState)
    {
        State = newState;
        ChangeBehavior();
    }
    void ChangeBehavior() {
        switch (State)
        {
            case EnemyStates.Patrol:
                stats.GetEnemyMovement().SetSpeedBoost(.5f);
                lostSight = false;
                losingsightTimer = 0f;
                break;
            case EnemyStates.Alert:
                Target = null;
                stats.GetEnemyMovement().SetSpeedBoost(.75f);                
                lostSight = true;
                if(losingsightTimer <= 0f)
                    losingsightTimer = 10f;
                break;
            case EnemyStates.Attack:
                stats.GetEnemyMovement().SetSpeedBoost(1f);
                losingsightTimer = 0f;
                lostSight = false;
                break;
            case EnemyStates.Flee:
                stats.GetEnemyMovement().SetSpeedBoost(1f);
                losingsightTimer = 0f;
                lostSight = false;
                break;
            case EnemyStates.Follow:
                stats.GetEnemyMovement().SetSpeedBoost(.5f);
                losingsightTimer = 0f;
                lostSight = false;
                break;
        }
    }             
    #endregion
}