using UnityEngine;
using GD.Core.Enums;

public class EnemyCollision : MonoBehaviour
{

    #region Properties
    private float detectionTimer;
    private EnemyBehavior behavior;
    private SystemManager systemManager;
    #endregion

    void Start()
    {
        detectionTimer = 0f;
        behavior = GetComponent<EnemyBehavior>();
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
            detectionTimer = 0f;                       
    }

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player") && detectionTimer <= 0.0f)
        {           
            if (systemManager.GetSystemCooldown(SystemType.CLOAK) > 0f)
                behavior.ChangeState(EnemyStates.SEARCHING);            
            else
                behavior.SetEnemyTarget(col.transform);
            
            detectionTimer = Random.Range(.5f, 5f);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            behavior.ChangeState(EnemyStates.SEARCHING);
        }
    }
    #endregion
}