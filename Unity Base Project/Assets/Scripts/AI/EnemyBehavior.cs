using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(WanderAi))]
[RequireComponent(typeof(EnemyScript))]
public class EnemyBehavior : MonoBehaviour {
    //**    Attach to an Enemy  **//

    //  Behavior
    public bool isWandering;
    public bool foundPlayer;
    public float detectionTimer;

    //  Attached Scripts
    private WanderAi wanderAI;
    private EnemyScript attackAI;

    //  Player Data
    private Cloak playerCloak;

    //  Messages
    private GameObject messages;


    void Start() {
        isWandering = true;
        foundPlayer = false;
        detectionTimer = 2.0f;

        wanderAI = GetComponent<WanderAi>();
        attackAI = GetComponent<EnemyScript>();

        ChangeState();

        messages = GameObject.Find("Screen");
        playerCloak = GameObject.Find("Cloak").GetComponent<Cloak>();
    }

    void Update() {
        if (detectionTimer > 0.0f)
            detectionTimer -= Time.deltaTime;
    }

    private void ChangeState() {
        wanderAI.enabled = isWandering;
        attackAI.enabled = foundPlayer;
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
                    Debug.Log("Enemy Detected Player");
                    foundPlayer = true;
                    isWandering = false;
                    ChangeState();
                }
                detectionTimer = 2.0f;
            }            
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.tag == "Player") {
            Debug.Log("Enemy Lost Player");
            messages.SendMessage("EnemyAway");
            foundPlayer = false;
            isWandering = true;
            ChangeState();
        }
    }
    #endregion
}