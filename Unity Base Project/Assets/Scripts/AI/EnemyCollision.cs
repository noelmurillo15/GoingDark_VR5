using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyCollision : MonoBehaviour
{

    #region Properties        
    private float triggerTimer;
    private float collisionTimer;

    private IEnemy stats;
    private EnemyMovement movement;
    private EnemyManager enemyManager;
    private EnemyStateManager stateManager;

    //  Player
    private CloakSystem playerCloak;
    private MessageScript playerMsgs;
    #endregion

    void Awake()
    {
        triggerTimer = 5f;
        collisionTimer = 5f;
        stats = GetComponent<IEnemy>();
        stateManager = GetComponent<EnemyStateManager>();
    }

    public void Initialize()
    {
        enemyManager = stats.GetManager();
        Invoke("FindPlayer", 2f);
    }

    void FindPlayer()
    {
        movement = stats.GetEnemyMovement();
        playerMsgs = GameObject.Find("PlayerCanvas").GetComponent<MessageScript>();
        playerCloak = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().GetCloak();
    }

    void FixedUpdate()
    {
        if (triggerTimer > 0.0f)
            triggerTimer -= Time.fixedDeltaTime;

        if (collisionTimer > 0.0f)
            collisionTimer -= Time.fixedDeltaTime;
    }

    #region Collision
    void OnTriggerEnter(Collider col)
    {
        if (stateManager.Target == null)
        {
            if (col.CompareTag("Decoy"))
            {
                triggerTimer = 0f;
                enemyManager.SendAlert(col.transform.position);
            }
            if (col.CompareTag("Player"))
            {
                triggerTimer = 0f;

                if (stats.GetEnemyType() == EnemyTypes.FinalBoss)
                    AudioManager.instance.PlayBossTheme();

                if (playerCloak != null && !playerCloak.GetCloaked())
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
            if (stateManager.GetState() != EnemyStates.Patrol)
                stateManager.SetLastKnown(col.transform.position);
        }
        if (col.CompareTag("Decoy"))
        {
            if (stateManager.GetState() != EnemyStates.Patrol)
                stateManager.SetLastKnown(col.transform.position);
        }
    }

    void OnCollisionEnter(Collision hit)
    {
        if (collisionTimer <= 0f)
        {
            if (hit.transform.CompareTag("Player"))
            {
                if (stats.GetEnemyType() == EnemyTypes.Droid)
                {
                    hit.transform.SendMessage("EMPHit");                
                    stats.Kill();
                }
                else
                {
                    stats.CrashHit(movement.GetMoveData().Speed / movement.GetMoveData().MaxSpeed);
                }
                collisionTimer = 2f;
            }

            if (hit.transform.CompareTag("Meteor"))
            {
                collisionTimer = 5f;
                stats.Kill();
            }
        }
    }
    #endregion
}