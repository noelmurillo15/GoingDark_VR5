using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyCollision : MonoBehaviour
{

    #region Properties
    public bool inRange;
    private float detectionTimer;
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
        detectionTimer = 0f;
        if (LockOnReticle != null)
            LockOnReticle.gameObject.SetActive(false);
        else
            Debug.LogError("Enemy does not have lock on reticle : " + transform.name);
        behavior = GetComponent<EnemyStateManager>();       
        
        Invoke("FindPlayer", 5.2f);
    }

    void FindPlayer()
    {
        playerTrans = behavior.GetManager().GetPlayerTransform();
        messages = GameObject.Find("PlayerCanvas").GetComponent<MessageScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().GetCloak();
    }

    void LateUpdate()
    {
        if (detectionTimer > 0.0f)
            detectionTimer -= Time.deltaTime;

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
                detectionTimer = 0f;
                behavior.GetManager().SendAlert(col.transform.position);
            }

            if (col.CompareTag("Player"))
            {
                inRange = true;
                detectionTimer = 0f;                
                
                if (LockOnReticle != null)
                    LockOnReticle.gameObject.SetActive(true);

                messages.EnemyClose();
            }
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Decoy") && detectionTimer <= 0.0f)
        {
            detectionTimer = 5f;
            behavior.SetEnemyTarget(col.transform);
        }

        if (col.CompareTag("Player") && detectionTimer <= 0.0f)
        {
            detectionTimer = 5f;
            if (player.GetCloaked())
            {
                if (behavior.State != EnemyStates.Patrol)
                    behavior.GetManager().SendAlert(transform.position);
                return;
            }

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
    #endregion
}