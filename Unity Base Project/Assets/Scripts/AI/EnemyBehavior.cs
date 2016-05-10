using UnityEngine;

[RequireComponent(typeof(PatrolAi))]
[RequireComponent(typeof(EnemyStats))]
public class EnemyBehavior : MonoBehaviour {
    //**    Attach to an Enemy  **//

    //  Detection
    public bool wandering;
    public bool playerDetected;
    private float detectionTimer;

    //  Base AI
    private EnemyStats stats;
    private PatrolAi wanderAI;

    //  Unique AI
    private KamikazeAI kamiAI;
    private EnemyAttack attackAI;
    private TransportShipAI transportAI;

    //  Player Data
    private Cloak playerCloak;


    void Start() {
        wandering = true;
        playerDetected = false;
        detectionTimer = 2.0f;

        stats = GetComponent<EnemyStats>();
        wanderAI = GetComponent<PatrolAi>();
        wanderAI.enabled = true;

        if (stats.GetEnemyType() == EnemyStats.ENEMY_TYPE.BASIC_ENEMY)
        {
            kamiAI = null;
            transportAI = null;
            attackAI = GetComponent<EnemyAttack>();
        }
        else if (stats.GetEnemyType() == EnemyStats.ENEMY_TYPE.KAMIKAZE)
        {
            attackAI = null;
            transportAI = null;
            kamiAI = GetComponent<KamikazeAI>();
        }
        else if (stats.GetEnemyType() == EnemyStats.ENEMY_TYPE.TRANSPORT)
        {
            kamiAI = null;
            attackAI = null;
            transportAI = GetComponent<TransportShipAI>();
            transportAI.enabled = true;
        }
        
        ChangeState();
        playerCloak = GameObject.Find("Cloak").GetComponent<Cloak>();
    }

    void Update() {
        if (detectionTimer > 0.0f)
            detectionTimer -= Time.deltaTime;
    }

    public void ChangeState() {
        if (stats.GetEnemyType() == EnemyStats.ENEMY_TYPE.TRANSPORT) {            
            if (transportAI.GetCloakTimer() > 0.0f)
                wanderAI.SetSpeedBoost(2.0f);
            else
                wanderAI.SetSpeedBoost(0.5f);
        }

        if (attackAI != null)
            attackAI.enabled = playerDetected;

        if (kamiAI != null)
            kamiAI.enabled = playerDetected;        
    }    

    #region Collision
    void OnTriggerEnter(Collider col) {       
        if (col.CompareTag("Player"))
        {
            wandering = false;
            playerDetected = true;
            detectionTimer = 0f;
            wanderAI.SetEnemyTarget(col.transform);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player") && detectionTimer <= 0.0f)
        {
            detectionTimer = Random.Range(.5f, 5f);
            if (!playerCloak.GetCloaked())
            {
                wandering = false;
                playerDetected = true;
            }
            else
            {
                wandering = true;
                playerDetected = false;
                transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            }
            ChangeState();
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.CompareTag("Player")) {
            wandering = true;
            playerDetected = false;
            wanderAI.SetEnemyTarget(null);
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);            
            ChangeState();
        }
    }
    #endregion
}