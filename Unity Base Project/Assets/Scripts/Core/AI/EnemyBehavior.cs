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
        manager.AddEnemy(gameObject);
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
        {
            GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>().PlayerSeen();
            manager.FoundTarget(_target.position, MyTransform.position);
            ChangeState(EnemyStates.ATTACKING);
        }
        else
            ChangeState(EnemyStates.PATROL);        
    }    

    public void BroadcastAlert(object[] storage)
    {
        if (LastKnownPos == Vector3.zero && Target == null)
        {
            if (Vector3.Distance((Vector3)storage[1], MyTransform.position) <= 5000f)
            {
                LastKnownPos = (Vector3)storage[0];
                ChangeState(EnemyStates.ALERT);
            }
        }
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
            case EnemyStates.RUNNING:
                alertAi.enabled = false;
                break;
            case EnemyStates.ATTACKING:
                alertAi.enabled = false;
                uniqueAi.enabled = true;
                SetSpeedBoost(1f);
                lostSight = false;
                losingsightTimer = 0f;
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
            case EnemyStates.FOLLOW:
                patrolAi.enabled = false;
                alertAi.enabled = false;
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