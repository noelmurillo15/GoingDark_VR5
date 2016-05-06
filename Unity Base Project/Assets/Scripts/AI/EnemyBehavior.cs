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
            transportAI.enabled = true;
            wanderAI.enabled = true;
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
        if (stats.GetEnemyType() == EnemyStats.ENEMY_TYPE.TRANSPORT) {
            if (transportAI.GetCloakTimer() != 0.0f)
                wanderAI.SetSpeedBoost(2.0f);
            else
                wanderAI.SetSpeedBoost(0.5f);
        }
        else
            wanderAI.enabled = wandering;

        if (attackAI != null)
            attackAI.enabled = playerDetected;

        if (kamiAI != null)
            kamiAI.enabled = playerDetected;        
    }    

    #region Collision
    void OnTriggerEnter(Collider col) {
        if (col.tag == "Player")
        {
            playerDetected = true;
            messages.SendMessage("EnemyClose");
        }
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
                }
                else
                {
                    wandering = true;
                    playerDetected = false;
                    transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
                }
            }
            ChangeState();
            detectionTimer = Random.Range(.5f, 5f);
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