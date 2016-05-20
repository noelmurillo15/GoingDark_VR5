﻿using UnityEngine;
using GD.Core.Enums;

public class EnemyBehavior : IEnemy
{

    #region Properties    
    public Transform Target { get; protected set; }

    public bool AutoPilot;
    public EnemyStates State;
    public MonoBehaviour uniqueAi;

    public bool lostSight;
    public float losingsightTimer;

    private EnemyManager manager;
    #endregion

    void Awake()
    {
        Initialize();        
    }

    public override void Initialize()
    {
        base.Initialize();
        manager = transform.parent.GetComponent<EnemyManager>();
        State = EnemyStates.IDLE;
        losingsightTimer = 0f;
        lostSight = false;
        uniqueAi = null;
        Target = null;
    }

    private void Update()
    {
        if (losingsightTimer > 0f)
            losingsightTimer -= Time.deltaTime;

        if (lostSight && losingsightTimer <= 0)
        {
            Debug.Log("Enemy Lost Player's Position");
            SetEnemyTarget(null);
            ChangeState(EnemyStates.PATROL);
        }
    }

    #region Public Methods
    public void SetEnemyTarget(Transform _target)
    {
        Target = _target;
        if (Target != null && Target.CompareTag("Player"))
            ChangeState(EnemyStates.ATTACKING);

        manager.FoundTarget(_target, MyTransform.position);
    }

    public void BroadcastAlert(object[] storage)
    {
        if (Target == null)
        {
            if (Vector3.Distance((Vector3)storage[1], MyTransform.position) <= 2000f)
            {
                Target = (Transform)storage[0];
                if (Target != null && Target.CompareTag("Player"))
                    ChangeState(EnemyStates.ALERT);
            }
        }
    }

    public void SetUniqueAi(MonoBehaviour _script)
    {
        if(uniqueAi == null)
            uniqueAi = _script;
    }

    public void ChangeState(EnemyStates newState)
    {
        Debug.Log("Changing enemy State : " + newState);
        State = newState;
        ChangeBehavior();
    }

    public void ChangeBehavior() {
        switch (State)
        {
            case EnemyStates.IDLE:
                SetSpeedBoost(0f);
                break;
            case EnemyStates.PATROL:
                SetSpeedBoost(.5f);
                lostSight = false;
                uniqueAi.enabled = false;
                break;
            case EnemyStates.RUNNING:
                break;
            case EnemyStates.ATTACKING:
                SetSpeedBoost(1f);
                lostSight = false;
                uniqueAi.enabled = true;
                if (Type == EnemyTypes.KAMIKAZE)
                    uniqueAi.SendMessage("SelfDestruct");
                break;
            case EnemyStates.SEARCHING:
                SetSpeedBoost(1f);
                lostSight = true;
                losingsightTimer = 10f;
                break;
            case EnemyStates.ALERT:
                SetSpeedBoost(1f);
                lostSight = true;
                losingsightTimer = 30f;
                break;
            case EnemyStates.FOLLOW:
                SetSpeedBoost(.6f);
                break;


            default:
                Debug.Log("Invalid Enemy State!");
                break;
        }
    }             
    #endregion
}