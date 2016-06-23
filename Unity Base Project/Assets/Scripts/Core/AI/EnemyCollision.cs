using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyCollision : MonoBehaviour
{

    #region Properties
    public float detectionTimer;
    private EnemyBehavior behavior;
    private EnemyManager manager;

    //  Player
    private GameObject messages;    
    #endregion

    public void Initialize()
    {
        detectionTimer = 0f;
        behavior = GetComponent<EnemyBehavior>();
        messages = GameObject.Find("PlayerCanvas");
        manager = GetComponentInParent<EnemyManager>();
    }

    void Update()
    {
        if (detectionTimer > 0.0f)
            detectionTimer -= Time.deltaTime;
    }    

    public void SetManagerRef(EnemyManager _manager)
    {
        manager = _manager;
    }

    #region Collision
    void OnTriggerEnter(Collider col)
    {
        if (behavior.Target == null)
        {
            if (col.CompareTag("Decoy"))
            {
                detectionTimer = 0f;
                return;
            }

            if (col.CompareTag("Player"))
            {

                if (manager.GetPlayerCloak().GetCloaked())
                    detectionTimer = manager.GetPlayerCloak().GetCloakTimer();
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
            detectionTimer = 5f;
            behavior.SetEnemyTarget(col.transform);
            return;
        }

        if (col.CompareTag("Player") && detectionTimer <= 0.0f)
        {
            if (manager.GetPlayerCloak().GetCloaked())
            {
                detectionTimer = manager.GetPlayerCloak().GetCloakTimer();
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
            if (behavior.State != EnemyStates.Patrol)
                behavior.SetLastKnown(col.transform.position);
        }
        if (col.CompareTag("Decoy"))
        {
            if (behavior.State != EnemyStates.Patrol)
                behavior.SetLastKnown(col.transform.position);
        }
    }
    #endregion
}