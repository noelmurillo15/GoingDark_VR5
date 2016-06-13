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
        messages = GameObject.Find("WarningMessages");
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
        if (col.CompareTag("Player"))
        {
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
        }

        if (col.CompareTag("Decoy"))
            detectionTimer = 0f;
    }

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player") && detectionTimer <= 0.0f)
        {
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
        if (col.CompareTag("Decoy") && detectionTimer <= 0.0f)
        {
            detectionTimer = 5f;
            behavior.SetEnemyTarget(col.transform);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if(behavior.State != EnemyStates.Patrol)
                behavior.SetLastKnown(col.transform.position);
        }
    }
    #endregion
}