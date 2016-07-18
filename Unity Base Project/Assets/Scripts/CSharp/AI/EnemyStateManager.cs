using UnityEngine;
using GoingDark.Core.Enums;

[RequireComponent(typeof(EnemyCollision))]
public class EnemyStateManager : IEnemy
{
    #region Properties        
    public EnemyStates State;    

    public bool lostSight;
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
        State = EnemyStates.Idle;
        losingsightTimer = 0f;
        lostSight = false;
        Target = null;

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
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionSystem>().PlayerSeen();
            ChangeState(EnemyStates.Attack);        
        }
        else
            ChangeState(EnemyStates.Patrol);        
    }    

    public void BroadcastAlert(object[] storage)
    {
        if (Target == null)
        {
            if (LastKnownPos == Vector3.zero)
            {
                if (Vector3.Distance((Vector3)storage[1], MyTransform.position) <= 2000f)
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

    public void BroadcastWin()
    {
        Target = null;
        ChangeState(EnemyStates.Patrol);
    }

    public void SetUniqueAi(MonoBehaviour _script)
    {
        //if(uniqueAi == null)
        //    uniqueAi = _script;
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
                //SetSpeedBoost(0f);
                lostSight = false;
                losingsightTimer = 0f;
                break;
            case EnemyStates.Patrol:
                //SetSpeedBoost(.5f);
                LastKnownPos = Vector3.zero;
                lostSight = false;
                losingsightTimer = 0f;
                break;
            case EnemyStates.Alert:                
                //SetSpeedBoost(.8f);                
                lostSight = true;
                if(losingsightTimer <= 0f)
                    losingsightTimer = 10f;
                break;
            case EnemyStates.Attack:
                //SetSpeedBoost(1f);
                LastKnownPos = Vector3.zero;
                losingsightTimer = 0f;
                lostSight = false;
                break;
            case EnemyStates.Flee:
                break;
            case EnemyStates.Follow:
                break;
        }
    }             
    #endregion
}