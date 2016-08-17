using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyCollision : MonoBehaviour {

    #region Properties   
    [SerializeField]
    private Transform LockOnReticle;  
     
    private bool inRange;
    private float triggerTimer;
    private float collisionTimer;

    private IEnemy stats;
    private EnemyMovement movement;
    private EnemyManager enemyManager;
    private EnemyStateManager stateManager;

    //  Player
    private CloakSystem playerCloak;
    private MessageScript playerMsgs;
    private Transform playerTransform;
    #endregion

    void Awake()
    {
        inRange = false;
        triggerTimer = 5f;
        collisionTimer = 5f;
        stats = GetComponent<IEnemy>();
        stateManager = GetComponent<EnemyStateManager>();
    }

    public void Initialize()
    {
        if (LockOnReticle != null)
            LockOnReticle.gameObject.SetActive(false);
        else
            Debug.LogError("Enemy does not have lock on reticle : " + transform.name);

        enemyManager = stats.GetManager();
        Invoke("FindPlayer", 2f);
    }

    void FindPlayer()
    {
        movement = stats.GetEnemyMovement();
        playerTransform = enemyManager.GetPlayerTransform();
        playerMsgs = GameObject.Find("PlayerCanvas").GetComponent<MessageScript>();
        playerCloak = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().GetCloak();
    }

    void FixedUpdate()
    {
        if (triggerTimer > 0.0f)
            triggerTimer -= Time.fixedDeltaTime;

        if (collisionTimer > 0.0f)
            collisionTimer -= Time.fixedDeltaTime;

        if (inRange)
            LockOnReticle.LookAt(playerTransform);
    }    

    #region Collision
    void OnTriggerEnter(Collider col)
    {
        if (stateManager.Target == null)
        {
            if (col.CompareTag("Decoy"))
            {
                enemyManager.SendAlert(col.transform.position);
            }
            if (col.CompareTag("Player"))
            {
                if (stats.GetEnemyType() == EnemyTypes.FinalBoss)
                    AudioManager.instance.PlayBossTheme();

                inRange = true;                
                if (LockOnReticle != null)
                    LockOnReticle.gameObject.SetActive(true);

                if (!playerCloak.GetCloaked())
                    enemyManager.SendAlert(transform.position);                

                playerMsgs.EnemyClose();
            }
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (triggerTimer <= 0f)
        {
            if (col.CompareTag("Decoy"))
            {
                triggerTimer = 5f;
                if (stateManager.Target == null)
                    stateManager.SetEnemyTarget(col.transform);
            }
            if (col.CompareTag("Player"))
            {
                triggerTimer = 5f;
                if (!playerCloak.GetCloaked())
                {
                    if (stateManager.Target == null)
                        stateManager.SetEnemyTarget(col.transform);
                }
                else
                {
                    if (stateManager.GetState() != EnemyStates.Patrol)
                        stateManager.SetLastKnown(col.transform.position);
                }
            }
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            inRange = false;

            if (stateManager.GetState() != EnemyStates.Patrol)
                stateManager.SetLastKnown(col.transform.position);

            if (LockOnReticle != null)
                LockOnReticle.gameObject.SetActive(false);            
        }
        if (col.CompareTag("Decoy"))
        {
            if (stateManager.GetState() != EnemyStates.Patrol)
                stateManager.SetLastKnown(col.transform.position);
        }
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.transform.CompareTag("Player") && collisionTimer <= 0f)
        {
            if (stats.GetEnemyType() == EnemyTypes.Droid)
            {
                hit.transform.SendMessage("EMPHit");
                stats.Kill();
            }
            else
            {
                stats.CrashHit(movement.GetMoveData().Speed / movement.GetMoveData().MaxSpeed);
                movement.StopMovement();
            }
            collisionTimer = 5f;
        }

        if (hit.transform.CompareTag("Asteroid") && collisionTimer <= 0f)
        {
            movement.StopMovement();
            collisionTimer = 5f;
        }
    }
    #endregion
}