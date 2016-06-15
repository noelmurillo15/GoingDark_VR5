using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyCollision : MonoBehaviour
{

    #region Properties
    public float detectionTimer;
    private EnemyBehavior behavior;

    //  Player
    private GameObject messages;
    private CloakSystem pCloak;
    private SystemManager systemManager;
    private EnemyManager manager;
    #endregion

    void Start()
    {
        manager = transform.parent.GetComponent<EnemyManager>();
        manager.AddEnemy(gameObject);
        detectionTimer = 0f;
        behavior = GetComponent<EnemyBehavior>();
        messages = GameObject.Find("PlayerCanvas");
        systemManager = GameObject.Find("Devices").GetComponent<SystemManager>();
    }

    void Update()
    {
        if (detectionTimer > 0.0f)
            detectionTimer -= Time.deltaTime;
    }

    #region Collision
    void OnTriggerEnter(Collider col)
    {
        if (behavior.Target == null)
        {
            if (col.CompareTag("Decoy"))
            {
                Debug.Log("Picked Up Decoy");
                detectionTimer = 0f;
                return;
            }

            if (col.CompareTag("Player"))
            {
                Debug.Log("Picked Up Player");
                if (pCloak == null)
                    pCloak = systemManager.GetSystem(SystemType.Cloak).GetComponent<CloakSystem>();

                if (pCloak.GetCloaked())
                    detectionTimer = pCloak.GetCloakTimer();
                else
                {
                    if (col.CompareTag("Player"))
                        manager.SendAlert(transform.position);

                    detectionTimer = 0f;
                }
                messages.SendMessage("EnemyClose");
                return;
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Decoy") && detectionTimer <= 0.0f)
        {
            Debug.Log("Decoy Locked as Target");
            detectionTimer = 5f;
            behavior.SetEnemyTarget(col.transform);
            return;
        }

        if (col.CompareTag("Player") && detectionTimer <= 0.0f)
        {
            Debug.Log("Player Locked as Target");
            if (pCloak.GetCloaked())
            {
                detectionTimer = pCloak.GetCloakTimer();
                behavior.SetLastKnown(col.transform.position);
                return;
            }

            detectionTimer = 5f;
            behavior.SetEnemyTarget(col.transform);
            return;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("Lost Lock on Target");
            if (behavior.State != EnemyStates.Patrol)
                behavior.SetLastKnown(col.transform.position);
        }
        if (col.CompareTag("Decoy"))
        {
            Debug.Log("Lost Lock on Target");
            if (behavior.State != EnemyStates.Patrol)
                behavior.SetLastKnown(col.transform.position);
        }
    }
    #endregion
}