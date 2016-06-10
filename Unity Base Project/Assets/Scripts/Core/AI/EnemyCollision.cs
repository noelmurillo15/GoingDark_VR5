using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyCollision : MonoBehaviour
{

    #region Properties
    private float detectionTimer;
    private EnemyBehavior behavior;

    //  Player
    private GameObject messages;
    private CloakSystem pCloak;
    private SystemManager systemManager;
    #endregion

    void Start()
    {
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
            detectionTimer = 0f;
            messages.SendMessage("EnemyClose");
            pCloak = systemManager.GetSystem(SystemType.CLOAK).GetComponent<CloakSystem>();
        }

        if (col.CompareTag("Decoy"))
            detectionTimer = 0f;        
    }

    void OnTriggerStay(Collider col)
    {
        if (detectionTimer <= 0.0f && col.CompareTag("Player"))
        {
            detectionTimer = Random.Range(.5f, 5f);
            if (pCloak != null && pCloak.GetCloaked())
            {
                behavior.LastKnownPos = col.transform.position;
                behavior.ChangeState(EnemyStates.ALERT);
                detectionTimer += pCloak.GetCloakTimer();
            }
            else
                behavior.SetEnemyTarget(col.transform);
            
            return;
        }
        if (detectionTimer <= 0.0f && col.CompareTag("Decoy"))
        {
            behavior.SetEnemyTarget(col.transform);
            detectionTimer = Random.Range(.5f, 5f);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player") || col.CompareTag("Decoy"))
            behavior.ChangeState(EnemyStates.ALERT);        
    }
    #endregion
}