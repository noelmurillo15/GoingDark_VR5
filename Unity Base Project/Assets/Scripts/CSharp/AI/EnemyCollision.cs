﻿using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyCollision : MonoBehaviour
{

    #region Properties
    public bool isLockedOn;
    [SerializeField]
    public Transform LockOnReticle;

    private float detectionTimer;
    private EnemyStateManager behavior;

    //  Player
    private GameObject messages;
    private CloakSystem player;
    #endregion

    public void Initialize()
    {
        detectionTimer = 0f;

        behavior = GetComponent<EnemyStateManager>();
        messages = GameObject.Find("PlayerCanvas");
        isLockedOn = false;

        if (LockOnReticle != null)
            LockOnReticle.gameObject.SetActive(false);

        Invoke("FindPlayer", 5f);
    }

    void Update()
    {
        if (detectionTimer > 0.0f)
            detectionTimer -= Time.deltaTime;

        if (isLockedOn && behavior.Target != null)
            if (LockOnReticle != null)
                LockOnReticle.transform.LookAt(behavior.Target.position);
    }

    void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Systems").GetComponentInChildren<CloakSystem>();

        if(player == null)
            Debug.LogError("Did Not Find Player Cloak");
    }


    void OnTriggerEnter(Collider col)
    {
        if (behavior.Target == null)
        {
            if (col.CompareTag("Decoy"))
            {
                detectionTimer = 0f;
            }

            if (col.CompareTag("Player"))
            {
                if (player == null)
                {
                    Debug.LogError("Enemy Did Not Reference Player Cloak");
                    player = GameObject.FindGameObjectWithTag("Systems").GetComponentInChildren<CloakSystem>();
                }


                isLockedOn = true;
                if (player.GetCloaked())
                    detectionTimer = 5f;
                else
                {
                    if (col.CompareTag("Player"))
                        behavior.GetManager().SendAlert(transform.position);

                    detectionTimer = 0f;
                }
                if (LockOnReticle != null)
                    LockOnReticle.gameObject.SetActive(true);
                messages.SendMessage("EnemyClose");
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
            if (player == null)
            {
                Debug.LogError("Enemy Did Not Reference Player Cloak");
                player = GameObject.FindGameObjectWithTag("Systems").GetComponentInChildren<CloakSystem>();
            }

            if (player.GetCloaked())
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

            if (LockOnReticle != null)
                LockOnReticle.gameObject.SetActive(false);
            isLockedOn = false;
        }
        if (col.CompareTag("Decoy"))
        {
            if (behavior.State != EnemyStates.Patrol)
                behavior.SetLastKnown(col.transform.position);
        }
    }
}