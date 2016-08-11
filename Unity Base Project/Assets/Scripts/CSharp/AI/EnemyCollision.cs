using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyCollision : MonoBehaviour
{

    #region Properties
    public bool inRange;
    private float triggerTimer;
    private float collisionTimer;
    private EnemyManager manager;
    private EnemyStateManager behavior;

    [SerializeField]
    public Transform LockOnReticle;   

    //  Player
    private CloakSystem player;
    private Transform playerTrans;
    private MessageScript messages;
    #endregion

    public void Initialize()
    {
        inRange = false;
        triggerTimer = 0f;
        collisionTimer = 0f;
        if (LockOnReticle != null)
            LockOnReticle.gameObject.SetActive(false);
        else
            Debug.LogError("Enemy does not have lock on reticle : " + transform.name);
        behavior = GetComponent<EnemyStateManager>();
        manager = transform.root.GetComponent<EnemyManager>();
        Invoke("FindPlayer", 4f);
    }

    void FindPlayer()
    {
        playerTrans = manager.GetPlayerTransform();
        messages = GameObject.Find("PlayerCanvas").GetComponent<MessageScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().GetCloak();
    }

    void FixedUpdate()
    {
        if (triggerTimer > 0.0f)
            triggerTimer -= Time.fixedDeltaTime;

        if (collisionTimer > 0.0f)
            collisionTimer -= Time.fixedDeltaTime;

        if (inRange)
            LockOnReticle.LookAt(playerTrans);
    }    

    #region Collision
    void OnTriggerEnter(Collider col)
    {
        if (behavior.Target == null)
        {
            if (col.CompareTag("Decoy"))
            {
                triggerTimer = 0f;
                manager.SendAlert(col.transform.position);
            }

            if (col.CompareTag("Player"))
            {
                inRange = true;
                triggerTimer = 0f;                
                
                if (LockOnReticle != null)
                    LockOnReticle.gameObject.SetActive(true);

                if (player.GetCloaked())
                {
                    if (behavior.State != EnemyStates.Patrol)
                        manager.SendAlert(transform.position);
                }

                messages.EnemyClose();
            }
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Decoy") && triggerTimer <= 0.0f)
        {
            triggerTimer = 5f;
            behavior.SetEnemyTarget(col.transform);
        }

        if (col.CompareTag("Player") && triggerTimer <= 0.0f)
        {
            triggerTimer = 5f;
            if (!player.GetCloaked())
                behavior.SetEnemyTarget(col.transform);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            inRange = false;

            if (behavior.State != EnemyStates.Patrol)
                behavior.SetLastKnown(col.transform.position);

            if (LockOnReticle != null)
                LockOnReticle.gameObject.SetActive(false);            
        }
        if (col.CompareTag("Decoy"))
        {
            if (behavior.State != EnemyStates.Patrol)
                behavior.SetLastKnown(col.transform.position);
        }
    }


    void OnCollisionEnter(Collision hit)
    {
        if (hit.transform.CompareTag("Player") && collisionTimer <= 0f)
        {
            if (behavior.Type == EnemyTypes.Droid)
            {
                if (Random.Range(0, 2) == 1)
                    hit.transform.SendMessage("EMPHit");
                else
                    hit.transform.SendMessage("Hit");
                behavior.Kill();
            }
            else
            {
                behavior.CrashHit(behavior.movement.MoveData.Speed / behavior.movement.MoveData.MaxSpeed);
            }
            collisionTimer = 2f;
        }
    }
    #endregion
}