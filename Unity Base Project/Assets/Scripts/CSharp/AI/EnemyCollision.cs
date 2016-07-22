using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyCollision : MonoBehaviour
{

    #region Properties
    public float detectionTimer;
    private EnemyStateManager behavior;


    //  Player
    private GameObject messages;

    [SerializeField]
    public Transform NewTarg;
    private Collider Col;
    private bool isActive;
    #endregion

    public void Initialize()
    {
        detectionTimer = 0f;
        behavior = GetComponent<EnemyStateManager>();
        messages = GameObject.Find("PlayerCanvas");
        isActive = false;

        if (NewTarg != null)
            NewTarg.gameObject.SetActive(false);
    }

    void Update()
    {
        if (detectionTimer > 0.0f)
            detectionTimer -= Time.deltaTime;

        if (isActive)
            NewTarg.transform.LookAt(Col.transform);
    }


    void OnTriggerEnter(Collider col)
    {
        if (behavior.Target == null)
        {
            Col = col;

            if (col.CompareTag("Decoy"))
            {
                detectionTimer = 0f;
                return;
            }

            if (col.CompareTag("Player"))
            {
                NewTarg.gameObject.SetActive(true);
                isActive = true;
                if (behavior.GetManager().GetPlayerCloak().GetCloaked())
                    detectionTimer = 5f;
                else
                {
                    if (col.CompareTag("Player"))
                        behavior.GetManager().SendAlert(transform.position);

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
        }

        if (col.CompareTag("Player") && detectionTimer <= 0.0f)
        {
            if (behavior.GetManager().GetPlayerCloak().GetCloaked())
            {
                detectionTimer = 5f;
                if (behavior.State != EnemyStates.Patrol)
                    behavior.SetLastKnown(col.transform.position);
                return;
            }

            detectionTimer = 5f;
            behavior.SetEnemyTarget(col.transform);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (behavior.State != EnemyStates.Patrol)
                behavior.SetLastKnown(col.transform.position);

            NewTarg.gameObject.SetActive(false);
            isActive = false;
        }
        if (col.CompareTag("Decoy"))
        {
            if (behavior.State != EnemyStates.Patrol)
                behavior.SetLastKnown(col.transform.position);
        }
    }
}