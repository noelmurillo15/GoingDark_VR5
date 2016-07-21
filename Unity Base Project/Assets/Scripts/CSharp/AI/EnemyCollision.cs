using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyCollision : MonoBehaviour
{

    #region Properties
    public float detectionTimer;
    private EnemyStateManager behavior;
    private EnemyManager manager;
    private GameObject TargetImg;
    private ObjectPoolManager poolmanager;

    public GameObject container;
    private Hitmarker LockOn;
    private bool targetActive;
    //  Player
    private GameObject messages;
    private GameObject LockImg;
    private Collider Col;
    #endregion

    public void Initialize()
    {
        targetActive = false;
        detectionTimer = 0f;
        behavior = GetComponent<EnemyStateManager>();
        messages = GameObject.Find("PlayerCanvas");
        manager = GetComponentInParent<EnemyManager>();
        container = GameObject.FindGameObjectWithTag("GameManager");
        TargetImg = Resources.Load<GameObject>("LockObject");

        poolmanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();
        //pool.Initialize(TargetImg, 3, container.transform);
    }

    void Update()
    {
        if (detectionTimer > 0.0f)
            detectionTimer -= Time.deltaTime;

        if (LockImg != null)
        {
            LockImg.transform.LookAt(Col.transform);
        }


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
                Col = col;
                LockImg = poolmanager.GetTrackedEnemy();
                LockImg.SetActive(true);

                if (LockImg != null)
                {
                    LockImg.transform.parent = transform;
                    LockImg.transform.position = transform.position;
                }

                if (manager.GetPlayerCloak().GetCloaked())
                    detectionTimer = 5f;
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
        }

        if (col.CompareTag("Player") && detectionTimer <= 0.0f)
        {
            if (manager.GetPlayerCloak().GetCloaked())
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

            LockImg.SetActive(false);
        }
        if (col.CompareTag("Decoy"))
        {
            if (behavior.State != EnemyStates.Patrol)
                behavior.SetLastKnown(col.transform.position);
        }
    }
    #endregion
}