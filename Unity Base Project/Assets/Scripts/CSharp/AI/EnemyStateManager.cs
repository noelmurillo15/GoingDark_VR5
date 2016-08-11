using UnityEngine;
using GoingDark.Core.Enums;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(EnemyCollision))]
[RequireComponent(typeof(EnemyMovement))]
public class EnemyStateManager : MonoBehaviour
{
    #region Properties        
    public EnemyStates State;

    private IEnemy stats;
    public Transform Target { get; private set; }
    public Vector3 LastKnownPos { get; private set; }

    private bool lostSight;
    public float losingsightTimer;
    #endregion

    void Start()
    {
        Target = null;
        lostSight = false;
        losingsightTimer = 0f;
        State = EnemyStates.Idle;
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

    #region Public Methods
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

    public void BroadcastAlert(object[] storage)
    {
        if (Target == null)
        {
            if (LastKnownPos == Vector3.zero)
            {
                if (Vector3.Distance((Vector3)storage[1], transform.position) <= 2000f)
                {
                    SetLastKnown((Vector3)storage[0]);
                }
            }
        }
    }

    public void SetLastKnown(Vector3 pos)
    {
        Target = null;
        LastKnownPos = pos;
        ChangeState(EnemyStates.Alert);
    }

    public void ChangeState(EnemyStates newState)
    {
        State = newState;
        ChangeBehavior();
    }

    void ChangeBehavior() {
        switch (State)
        {
            case EnemyStates.Idle:
                stats.GetEnemyMovement().SetSpeedBoost(0f);
                lostSight = false;
                losingsightTimer = 0f;
                break;
            case EnemyStates.Patrol:
                stats.GetEnemyMovement().SetSpeedBoost(.5f);
                LastKnownPos = Vector3.zero;
                lostSight = false;
                losingsightTimer = 0f;
                break;
            case EnemyStates.Alert:
                stats.GetEnemyMovement().autopilot = false;
                stats.GetEnemyMovement().SetSpeedBoost(.8f);                
                lostSight = true;
                if(losingsightTimer <= 0f)
                    losingsightTimer = 10f;
                break;
            case EnemyStates.Attack:
                stats.GetEnemyMovement().autopilot = false;
                stats.GetEnemyMovement().SetSpeedBoost(1f);
                LastKnownPos = Vector3.zero;
                losingsightTimer = 0f;
                lostSight = false;
                break;
            case EnemyStates.Flee:
                stats.GetEnemyMovement().SetSpeedBoost(1.25f);
                losingsightTimer = 0f;
                lostSight = false;
                break;
            case EnemyStates.Follow:
                stats.GetEnemyMovement().SetSpeedBoost(.5f);
                break;
        }
    }             
    #endregion
}