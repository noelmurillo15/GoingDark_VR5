using UnityEngine;

[RequireComponent(typeof(WanderAi))]
[RequireComponent(typeof(EnemyAttack))]
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
    private WanderAi wanderAI;
    private EnemyAttack attackAI;


    void Start() {
        wandering = true;
        playerDetected = false;
        detectionTimer = 2.0f;

        wanderAI = GetComponent<WanderAi>();
        attackAI = GetComponent<EnemyAttack>();

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
        attackAI.enabled = playerDetected;
    }

    #region Msg Functions
    public void EMPHit()
    {
        Debug.Log("EMP has affected Enemy's Systems");
    }

    public void AsteroidHit()
    {
        Debug.Log("Enemy Has Hit Asteroid");
    }

    public void Kill()
    {
        Debug.Log("Destroyed Enemy Ship");
        Destroy(this.gameObject);
    }
    #endregion

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