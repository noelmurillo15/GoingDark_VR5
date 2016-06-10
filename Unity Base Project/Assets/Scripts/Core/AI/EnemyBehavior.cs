using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyBehavior : IEnemy
{

    #region Properties    
    public Transform Target { get; protected set; }
    public Vector3 LastKnownPos { get; set; }

    public EnemyStates State;
    public MonoBehaviour alertAi;
    public MonoBehaviour patrolAi;
    public MonoBehaviour uniqueAi;

    public bool lostSight;
    public bool AutoPilot;
    public float losingsightTimer;

    
    #endregion

    void Awake()
    {
        Initialize();        
    }

    public override void Initialize()
    {
        base.Initialize();        
        LastKnownPos = Vector3.zero;
        State = EnemyStates.IDLE;
        losingsightTimer = 0f;
        lostSight = false;
        uniqueAi = null;
        Target = null;

        patrolAi = GetComponent<PatrolAi>();
        alertAi = GetComponent<AlertAi>();
    }

    private void Update()
    {
        if (losingsightTimer > 0.0f)
            losingsightTimer -= Time.deltaTime;        

        if (lostSight && losingsightTimer <= 0)
        {
            Debug.Log("Enemy Lost Player's Position");
            lostSight = false;
            SetEnemyTarget(null);
        }

        if (Debuff == Impairments.STUNNED)
            SetSpeedBoost(0f);
    }

    #region Public Methods
    public void SetEnemyTarget(Transform _target)
    {
        Target = _target;
        if (Target != null)     
            ChangeState(EnemyStates.ATTACKING);        
        else
            ChangeState(EnemyStates.PATROL);        
    }    

    public void BroadcastAlert(object[] storage)
    {
        if (Target == null)
        {
            if (LastKnownPos == Vector3.zero)
            {
                if (Vector3.Distance((Vector3)storage[1], MyTransform.position) <= 5000f)
                {
                    Debug.Log("Enemy responding to broadcast");
                    SetLastKnown((Vector3)storage[0]);
                }
            }
        }
    }

    public void SetLastKnown(Vector3 pos)
    {
        Target = null;
        LastKnownPos = pos;
        ChangeState(EnemyStates.ALERT);
    }

    public void BroadcastWin()
    {
        Target = null;
        ChangeState(EnemyStates.PATROL);
    }


    public void SetUniqueAi(MonoBehaviour _script)
    {
        if(uniqueAi == null)
            uniqueAi = _script;
    }

    public void ChangeState(EnemyStates newState)
    {
        Debug.Log("Changing enemy State : " + newState);
        State = newState;
        ChangeBehavior();
    }

    public void ChangeBehavior() {
        switch (State)
        {
            case EnemyStates.IDLE:
                alertAi.enabled = false;
                patrolAi.enabled = false;
                SetSpeedBoost(0f);
                lostSight = false;
                losingsightTimer = 0f;
                break;
            case EnemyStates.PATROL:
                alertAi.enabled = false;
                patrolAi.enabled = true;
                uniqueAi.enabled = false;
                LastKnownPos = Vector3.zero;
                lostSight = false;
                losingsightTimer = 0f;
                SetSpeedBoost(.5f);
                break;
            case EnemyStates.ALERT:                
                alertAi.enabled = true;
                uniqueAi.enabled = false;
                patrolAi.enabled = false;
                SetSpeedBoost(1f);                
                lostSight = true;
                if(losingsightTimer <= 0f)
                    losingsightTimer = 20f;
                break;
            case EnemyStates.ATTACKING:
                LastKnownPos = Vector3.zero;
                alertAi.enabled = false;
                uniqueAi.enabled = true;
                patrolAi.enabled = true;
                losingsightTimer = 0f;
                lostSight = false;
                SetSpeedBoost(1f);
                break;
            case EnemyStates.RUNNING:
                break;
            case EnemyStates.FOLLOW:
                break;


            default:
                Debug.Log("Invalid Enemy State!");
                break;
        }
    }             
    #endregion
}