using UnityEngine;
using MovementEffects;
using GoingDark.Core.Enums;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    #region Properties
    private IEnemy stats;
    private EnemyStateManager stateManager;
    
    private Transform MyTransform;
    private Rigidbody MyRigidbody;

    public bool autopilot;
    private float headingChange;
    private float headingX, headingY;
    private Vector3 targetRotation;
    private Vector3 autopilotlocation;
    public MovementProperties MoveData;
    #endregion

    void Awake()
    {
        MyTransform = transform;
        stats = GetComponent<IEnemy>();
        MyRigidbody = GetComponent<Rigidbody>();
        stateManager = GetComponent<EnemyStateManager>();
        MoveData = new MovementProperties();
    }

    public void Initialize()
    {
        //  Patrol
        autopilotlocation = Vector3.zero;
        targetRotation = Vector3.zero;
        headingChange = 45f;
        autopilot = false;

        headingY = Random.Range(1f, 359f);
        headingX = Random.Range(1f, 359f);

        // Set random rotation        
        MyTransform.eulerAngles = new Vector3(headingX, headingY, 0);

        //  Start Coroutine
        Timing.RunCoroutine(NewHeading());
    }

    void FixedUpdate()
    {
        if (stats.GetDebuffData() == Impairments.Stunned)
        {
            MoveData.DecreaseSpeed();
            return;
        }        

        switch (stateManager.State)
        {
            case EnemyStates.Patrol:
                Patrol();
                break;
            case EnemyStates.Alert:
                Alert();
                break;
            case EnemyStates.Follow:
                Follow();
                break;
            case EnemyStates.Flee:
                Flee();
                break;
            case EnemyStates.Attack:
                Attack();
                break;
        }
        MyRigidbody.MovePosition(MyTransform.position + MyTransform.forward * Time.fixedDeltaTime * MoveData.Speed);
    }

    #region States
    void Patrol()
    {
        if (autopilot)
        {
            MoveData.IncreaseSpeed();
            Vector3 dir = autopilotlocation - MyTransform.position;
            Vector3 rotation = Vector3.RotateTowards(MyTransform.forward, dir, Time.fixedDeltaTime, 0.0f);
            MyTransform.rotation = Quaternion.LookRotation(rotation);
            headingX = MyTransform.eulerAngles.x;
            headingY = MyTransform.eulerAngles.y;
        }
        else
        {
            MoveData.IncreaseSpeed();
            MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.Euler(targetRotation), Time.fixedDeltaTime / MoveData.RotateSpeed);
        }
    }
    void Alert()
    {
        MoveData.IncreaseSpeed();
        Vector3 lastplayerdir = stateManager.LastKnownPos - MyTransform.position;
        if (Vector3.Distance(MyTransform.position, stateManager.LastKnownPos) < 250f)
        {
            stateManager.losingsightTimer = 0f;
            return;
        }

        Vector3 dir = Vector3.RotateTowards(MyTransform.forward, lastplayerdir, Time.fixedDeltaTime / MoveData.RotateSpeed, 0.0f);
        MyTransform.rotation = Quaternion.LookRotation(dir);
    }
    void Attack()
    {
        MoveData.IncreaseSpeed();
        if (stateManager.Target != null)
        {
            Vector3 playerDir = stateManager.Target.position - MyTransform.position;
            Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.fixedDeltaTime / MoveData.RotateSpeed, 0.0f);
            if (stats.Type == EnemyTypes.Droid)
            {
                MyTransform.rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                if (Vector3.Distance(stateManager.Target.position, MyTransform.position) > 500f)
                    MyTransform.rotation = Quaternion.LookRotation(direction);
                else
                {
                    direction.x += 75f;
                    MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.Euler(direction), Time.fixedDeltaTime / MoveData.RotateSpeed);
                }
            }
        }
        headingX = MyTransform.eulerAngles.x;
        headingY = MyTransform.eulerAngles.y;
    }
    void Follow()
    {

    }
    void Flee()
    {
        MoveData.IncreaseSpeed();
        Vector3 playerDir = MyTransform.position - stateManager.Target.position;
        Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.fixedDeltaTime / MoveData.RotateSpeed, 0.0f);
        if (stats.Type == EnemyTypes.Droid)
        {
            MyTransform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            if (Vector3.Distance(stateManager.Target.position, MyTransform.position) > 200f)
                MyTransform.rotation = Quaternion.LookRotation(direction);
            else
            {
                direction.x += 75f;
                MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.Euler(direction), Time.fixedDeltaTime / MoveData.RotateSpeed);
            }
        }
        headingX = MyTransform.eulerAngles.x;
        headingY = MyTransform.eulerAngles.y;
    }
    #endregion

    #region Accessors
    public MovementProperties GetMoveData()
    {
        return MoveData;
    }
    public void StopMovement()
    {
        MoveData.Speed = 0f;
    }
    #endregion

    #region Modifiers
    public void SetSpeedBoost(float newBoost)
    {
        MoveData.Boost = newBoost;
    }

    public void LoadEnemyData(string diff)
    {
        float multiplier = 0f;
        switch (diff)
        {
            case "Easy":
                multiplier = 1f;
                break;
            case "Medium":
                multiplier = 1.25f;
                break;
            case "Hard":
                multiplier = 1.5f;
                break;
            case "Nightmare":
                multiplier = 2f;
                break;
        }
        switch (stats.Type)
        {
            case EnemyTypes.Droid:
                MoveData.Set(0f, .5f, 125f * multiplier, 1f, 18f * multiplier);
                break;
            case EnemyTypes.JetFighter:
                MoveData.Set(0f, .5f, 120f * multiplier, 1.25f, 20f * multiplier);
                break;
            case EnemyTypes.Trident:
                MoveData.Set(0f, .5f, 100f * multiplier, 1.5f, 16f * multiplier);
                break;
            case EnemyTypes.Basic:
                MoveData.Set(0f, .5f, 90f * multiplier, 2f, 15f * multiplier);
                break;
            case EnemyTypes.SquadLead:
                MoveData.Set(0f, .5f, 85f * multiplier, 2.5f, 15f * multiplier);
                break;
            case EnemyTypes.Transport:
                MoveData.Set(0f, .5f, 70f * multiplier, 3f, 12f * multiplier);
                break;
            case EnemyTypes.Tank:
                MoveData.Set(0f, .5f, 60f * multiplier, 5f, 10f * multiplier);
                break;
            case EnemyTypes.FinalBoss:
                MoveData.Set(0f, 0f, 0f, 6f, 0f);
                break;
        }
    }
    #endregion

    #region Coroutine
    private IEnumerator<float> NewHeading()
    {
        while (true)
        {
            NewHeadingRoutine();
            yield return Timing.WaitForSeconds(Random.Range(2f, 10f));
        }
    }
    private void NewHeadingRoutine()
    {
        var floor = Mathf.Clamp(headingX - headingChange, 0, 360);
        var ceil = Mathf.Clamp(headingX + headingChange, 0, 360);
        headingX = Random.Range(floor, ceil);

        floor = Mathf.Clamp(headingY - headingChange, 0, 360);
        ceil = Mathf.Clamp(headingY + headingChange, 0, 360);
        headingY = Random.Range(floor, ceil);

        targetRotation = new Vector3(headingX, headingY, 0f);
    }
    #endregion

    #region Msgs
    void OutOfBounds(Vector3 _loc)
    {
        autopilot = true;
        autopilotlocation = _loc;
    }

    void InBounds()
    {
        autopilot = false;
    }
    #endregion
}