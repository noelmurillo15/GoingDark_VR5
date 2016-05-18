using UnityEngine;
using GD.Core.Enums;

public class EnemyCollision : MonoBehaviour
{

    #region Properties
    public float detectionTimer;
    private SystemManager Systems;
    #endregion

    void Start()
    {
        detectionTimer = 0f;        
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
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player") && detectionTimer <= 0.0f)
        {           

            if(Systems == null)
                Systems = GameObject.Find("Devices").GetComponent<SystemManager>();


            if (Systems.GetSystemCooldown(SystemType.CLOAK) > 0f)
            {
                Debug.Log("Player is Cloaked");
                //ChangeState(EnemyStates.SEARCHING);
            }
            else
            {
                //SetEnemyTarget(col.transform);
            }
            detectionTimer = Random.Range(.5f, 4.5f);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            //ChangeState(EnemyStates.SEARCHING);
        }
    }
    #endregion
}