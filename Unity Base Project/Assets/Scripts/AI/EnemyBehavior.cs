using UnityEngine;

[RequireComponent(typeof(PatrolAi))]
[RequireComponent(typeof(EnemyStats))]
public class EnemyBehavior : MonoBehaviour {
    //**    Attach to an Enemy  **//

    //  Behavior
    public bool wandering;
    public bool playerDetected;
    private float detectionTimer;

    //  Player Data
    private Cloak playerCloak;
    private GameObject messages;

    //  Enemy Scripts
    private EnemyStats stats;
    private PatrolAi wanderAI;
    private KamikazeAI kamiAI;
    private EnemyAttack attackAI;
    private TransportShipAI transportAI;


    void Start() {
        wandering = true;
        playerDetected = false;
        detectionTimer = 2.0f;

        stats = GetComponent<EnemyStats>();
        wanderAI = GetComponent<PatrolAi>();

        if (stats.GetEnemyType() == EnemyStats.ENEMY_TYPE.BASIC_ENEMY)
        {
            Debug.Log("Getting Basic Enemy Script");
            kamiAI = null;
            transportAI = null;
            attackAI = GetComponent<EnemyAttack>();
        }
        else if (stats.GetEnemyType() == EnemyStats.ENEMY_TYPE.KAMIKAZE)
        {
            Debug.Log("Getting Kami Script");
            attackAI = null;
            transportAI = null;
            kamiAI = GetComponent<KamikazeAI>();
        }
        else if (stats.GetEnemyType() == EnemyStats.ENEMY_TYPE.TRANSPORT)
        {
            Debug.Log("Getting Transport Script");
            kamiAI = null;
            attackAI = null;
            transportAI = GetComponent<TransportShipAI>();
        }

        ChangeState();

        messages = GameObject.Find("Screen");
        playerCloak = GameObject.Find("Cloak").GetComponent<Cloak>();
    }

    void Update() {
        if (detectionTimer > 0.0f)
            detectionTimer -= Time.deltaTime;
    }

    private void ChangeState() {        
        wanderAI.enabled = wandering;

        if(attackAI != null)
            attackAI.enabled = playerDetected;

        if (transportAI != null)
            transportAI.enabled = playerDetected;

        if (kamiAI != null)
            kamiAI.enabled = playerDetected;        
    }    

    #region Collision
    void OnTriggerEnter(Collider col) {
        if (col.tag == "Player") 
            messages.SendMessage("EnemyClose");        
    }

    void OnTriggerStay(Collider col) {
        if (detectionTimer <= 0.0f)
        {
            if (col.tag == "Player")
            {
                if (!playerCloak.GetCloaked())
                {
                    wandering = false;
                    playerDetected = true;
                    ChangeState();
                }
                else
                {
                    wandering = true;
                    playerDetected = false;
                    transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
                    ChangeState();
                }
                detectionTimer = 2.0f;
            }            
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.tag == "Player") {
            wandering = true;
            playerDetected = false;
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            messages.SendMessage("EnemyAway");
            ChangeState();
        }
    }
    #endregion
}