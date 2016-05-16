using UnityEngine;
using GD.Core.Enums;

[RequireComponent(typeof(PatrolAi))]
public class EnemyBehavior : MonoBehaviour {

    #region Properties
    public EnemyStates State { get; private set; }

    //  Detection
    public float losingSight { get; set; }
    public Transform Target { get; private set; }

    //  Behaviors
    private EnemyStats stats;
    private PatrolAi patrol;
    private KamikazeAI kamiAI;
    private EnemyAttack attackAI;
    private TransportShipAI transportAI;
    #endregion

    void Start() {
        Target = null;
        losingSight = 0f;
        stats = GetComponent<EnemyStats>();

        Initialize();
        ChangeState(EnemyStates.PATROL);
    }

    void Update() {      
        if (losingSight > 0f)
            losingSight -= Time.deltaTime;
        else
        {
            if(losingSight < 0f)
                SetEnemyTarget(null);

            losingSight = 0f;
        }
    }

    #region Private Methods
    void Initialize()
    {
        kamiAI = null;
        attackAI = null;
        transportAI = null;
        patrol = GetComponent<PatrolAi>();
        switch (stats.Type)
        {
            case EnemyTypes.BASIC:
                attackAI = GetComponent<EnemyAttack>();
                break;

            case EnemyTypes.KAMIKAZE:
                kamiAI = GetComponent<KamikazeAI>();
                break;

            case EnemyTypes.TRANSPORT:
                transportAI = GetComponent<TransportShipAI>();
                break;

            case EnemyTypes.TRIDENT:
                break;

            case EnemyTypes.BOSS:
                break;


            default:                
                break;
        }
    }
    
    public void ChangeBehavior() {
        switch (State)
        {
            case EnemyStates.IDLE:
                stats.SetSpeedBoost(0f);
                break;

            case EnemyStates.PATROL:
                if (stats.Type == EnemyTypes.TRANSPORT)
                    stats.SetSpeedBoost(.25f);
                else
                    stats.SetSpeedBoost(.5f);

                if (attackAI != null)
                    attackAI.enabled = false;
                break;

            case EnemyStates.RUNNING:
                break;

            case EnemyStates.ATTACKING:
                stats.SetSpeedBoost(1f);

                if (attackAI != null)
                    attackAI.enabled = true;
                break;

            case EnemyStates.SEARCHING:
                stats.SetSpeedBoost(1.25f);
                break;


            default:
                Debug.Log("Invalid State : " + State.ToString());
                break;
        }
            
    }

    public void SetEnemyTarget(Transform newTarget)
    {
        if (newTarget == null)
            ChangeState(EnemyStates.PATROL);        
        else
            ChangeState(EnemyStates.ATTACKING);        

        Target = newTarget;
    }

    public void ChangeState(EnemyStates newState)
    {
        //Debug.Log(transform.name + " has changed state to : " + newState.ToString());
        State = newState;
        ChangeBehavior();
    }        
    #endregion
}