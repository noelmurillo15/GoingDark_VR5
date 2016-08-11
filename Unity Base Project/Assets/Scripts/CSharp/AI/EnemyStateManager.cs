using UnityEngine;
using GoingDark.Core.Enums;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(EnemyCollision))]
[RequireComponent(typeof(EnemyMovement))]
public class EnemyStateManager : IEnemy
{
    #region Properties        
    public EnemyStates State;

    public Transform Target { get; private set; }
    public Vector3 LastKnownPos { get; private set; }

    private bool lostSight;
    public float losingsightTimer;
    public EnemyMovement movement;
    #endregion

    void Awake()
    {
        Initialize();        
    }

    public override void Initialize()
    {
        Target = null;
        lostSight = false;
        losingsightTimer = 0f;
        State = EnemyStates.Idle;
        LastKnownPos = Vector3.zero;
        movement = GetComponent<EnemyMovement>();

        base.Initialize();        
        GetComponent<EnemyCollision>().Initialize();
    }

    private void Update()
    {
        if (losingsightTimer > 0.0f)
            losingsightTimer -= Time.deltaTime;        

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
                GetManager().PlayerSeen();

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

    public void ChangeBehavior() {
        switch (State)
        {
            case EnemyStates.Idle:
                movement.SetSpeedBoost(0f);
                lostSight = false;
                losingsightTimer = 0f;
                break;
            case EnemyStates.Patrol:
                movement.SetSpeedBoost(.5f);
                LastKnownPos = Vector3.zero;
                lostSight = false;
                losingsightTimer = 0f;
                break;
            case EnemyStates.Alert:
                movement.autopilot = false;
                movement.SetSpeedBoost(.8f);                
                lostSight = true;
                if(losingsightTimer <= 0f)
                    losingsightTimer = 10f;
                break;
            case EnemyStates.Attack:
                movement.autopilot = false;
                movement.SetSpeedBoost(1f);
                LastKnownPos = Vector3.zero;
                losingsightTimer = 0f;
                lostSight = false;
                break;
            case EnemyStates.Flee:
                movement.SetSpeedBoost(1.25f);
                losingsightTimer = 0f;
                lostSight = false;
                break;
            case EnemyStates.Follow:
                movement.SetSpeedBoost(.5f);
                break;
        }
    }             
    #endregion
}