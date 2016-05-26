using UnityEngine;
using GD.Core.Enums;

public class EnemyBehavior : IEnemy
{

    #region Properties    
    public Transform Target { get; protected set; }

    public bool AutoPilot;
    public EnemyStates State;
    public MonoBehaviour uniqueAi;
    public MonoBehaviour patrolAi;

    public bool lostSight;
    public float losingsightTimer;

    private EnemyManager manager;
    #endregion

    void Awake()
    {
        Initialize();        
    }

    public override void Initialize()
    {
        base.Initialize();
        manager = transform.parent.GetComponent<EnemyManager>();
        State = EnemyStates.IDLE;
        losingsightTimer = 0f;
        lostSight = false;
        uniqueAi = null;
        Target = null;

        patrolAi = GetComponent<PatrolAi>();
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
        {
            manager.FoundTarget(_target, MyTransform.position);
            ChangeState(EnemyStates.ATTACKING);
        }
        else
            ChangeState(EnemyStates.PATROL);        
    }
    public void BroadcastWin()
    {
        Target = null;
        ChangeState(EnemyStates.PATROL);
    }

    public void BroadcastAlert(object[] storage)
    {
        if (Target == null)
        {
            if (Vector3.Distance((Vector3)storage[1], MyTransform.position) <= 2000f)
            {
                Target = (Transform)storage[0];
                if (Target != null && Target.CompareTag("Player"))
                    ChangeState(EnemyStates.ALERT);
            }
        }
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
                SetSpeedBoost(0f);
                break;
            case EnemyStates.PATROL:
                uniqueAi.enabled = false;
                patrolAi.enabled = true;
                SetSpeedBoost(.5f);
                break;
            case EnemyStates.RUNNING:
                break;
            case EnemyStates.ATTACKING:
                uniqueAi.enabled = true;
                SetSpeedBoost(1f);
                lostSight = false;
                losingsightTimer = 0f;
                //patrolAi.enabled = false; // un-comment if you have your own movement ai for attacking player
                break;
            case EnemyStates.ALERT:
                uniqueAi.enabled = false;
                SetSpeedBoost(.8f);                
                lostSight = true;
                if(losingsightTimer <= 0f)
                    losingsightTimer = 2.5f;
                break;
            case EnemyStates.FOLLOW:
                uniqueAi.enabled = false;
                SetSpeedBoost(.6f);
                break;


            default:
                Debug.Log("Invalid Enemy State!");
                break;
        }
    }             
    #endregion
}