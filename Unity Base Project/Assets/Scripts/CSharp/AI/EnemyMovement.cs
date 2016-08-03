using UnityEngine;
using MovementEffects;
using GoingDark.Core.Enums;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    #region Properties
    private EnemyStateManager behavior;
    public MovementProperties MoveData;

    //  Patrol    
    public bool autopilot;
    private float headingChange;
    public float headingX, headingY;
    private Vector3 targetRotation;
    private Vector3 autopilotlocation;

    //  Enemy Data 
    private Rigidbody MyRigidbody;
    private Transform MyTransform;
    #endregion

    void Start()
    {
        //  Patrol
        MoveData = new MovementProperties();
        autopilotlocation = Vector3.zero;
        headingY = Random.Range(0, 360);
        targetRotation = Vector3.zero;
        headingChange = 45f;
        autopilot = false;
        headingX = 0f;

        // Enemy Data
        MyRigidbody = GetComponent<Rigidbody>();
        behavior = GetComponent<EnemyStateManager>();
        behavior.ChangeState(EnemyStates.Patrol);

        // Set random rotation
        MyTransform = transform;
        MyTransform.eulerAngles = new Vector3(headingX, headingY, 0);

        //  Start Coroutine
        Timing.RunCoroutine(NewHeading());
    }

    void FixedUpdate()
    {
        if (behavior.Debuff == Impairments.Stunned)
        {
            MoveData.DecreaseSpeed();
            return;
        }

        if (autopilot)
        {
            MoveData.IncreaseSpeed();
            Vector3 dir = autopilotlocation - MyTransform.position;
            Vector3 rotation = Vector3.RotateTowards(MyTransform.forward, dir, Time.deltaTime, 0.0f);
            MyTransform.rotation = Quaternion.LookRotation(rotation);
            headingX = MyTransform.eulerAngles.x;
            headingY = MyTransform.eulerAngles.y;
            MyRigidbody.MovePosition(MyTransform.position + MyTransform.forward * Time.deltaTime * MoveData.Speed);

            if (Vector3.Distance(autopilotlocation, MyTransform.position) < 50f)
            {
                Debug.Log("Enemy made it back to zone");
                autopilot = false;
                autopilotlocation = Vector3.zero;
            }
            return;
        }

        switch (behavior.State)
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
        MyRigidbody.MovePosition(MyTransform.position + MyTransform.forward * Time.deltaTime * MoveData.Speed);
    }

    #region States
    void Patrol()
    {
        MoveData.IncreaseSpeed();
        MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime / MoveData.RotateSpeed);
    }
    void Alert()
    {
        MoveData.IncreaseSpeed();
        Vector3 lastplayerdir = behavior.LastKnownPos - MyTransform.position;
        if (Vector3.Distance(MyTransform.position, behavior.LastKnownPos) < 50f)
        {
            Debug.Log("Enemy has not found player, going back on patrol");
            behavior.losingsightTimer = 0f;
            return;
        }

        Vector3 dir = Vector3.RotateTowards(MyTransform.forward, lastplayerdir, Time.fixedDeltaTime / MoveData.RotateSpeed, 0.0f);
        MyTransform.rotation = Quaternion.LookRotation(dir);
    }
    void Attack()
    {
        MoveData.IncreaseSpeed();
        Vector3 playerDir = behavior.Target.position - MyTransform.position;
        Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.deltaTime / MoveData.RotateSpeed, 0.0f);
        if (behavior.Type == EnemyTypes.Droid)
        {
            MyTransform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            if (Vector3.Distance(behavior.Target.position, MyTransform.position) > 200f)
                MyTransform.rotation = Quaternion.LookRotation(direction);
            else
            {
                direction.x += 75f;
                MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.Euler(direction), Time.deltaTime / MoveData.RotateSpeed);
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
        Vector3 playerDir = MyTransform.position - behavior.Target.position;
        Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.deltaTime / MoveData.RotateSpeed, 0.0f);
        if (behavior.Type == EnemyTypes.Droid)
        {
            MyTransform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            if (Vector3.Distance(behavior.Target.position, MyTransform.position) > 200f)
                MyTransform.rotation = Quaternion.LookRotation(direction);
            else
            {
                direction.x += 75f;
                MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.Euler(direction), Time.deltaTime / MoveData.RotateSpeed);
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
    #endregion

    #region Modifiers
    public void SetSpeedBoost(float newBoost)
    {
        MoveData.Boost = newBoost;
    }

    public void LoadEnemyData()
    {
        switch (behavior.GetManager().Difficulty)
        {
            #region Easy
            case GameDifficulty.Easy:
                switch (behavior.Type)
                {
                    case EnemyTypes.Basic:
                        MoveData.Set(0f, .5f, 60f, 2f, 10f);
                        break;
                    case EnemyTypes.Droid:
                        MoveData.Set(0f, .5f, 110f, 1f, 10f);
                        break;
                    case EnemyTypes.JetFighter:
                        MoveData.Set(0f, .5f, 120f, 1.5f, 20f);
                        break;
                    case EnemyTypes.Transport:
                        MoveData.Set(0f, .5f, 120f, 3f, 10f);
                        break;
                    case EnemyTypes.Trident:
                        MoveData.Set(0f, .5f, 80f, 1.8f, 10f);
                        break;
                    case EnemyTypes.Tank:
                        MoveData.Set(0f, .5f, 50f, 5f, 10f);
                        break;
                    case EnemyTypes.FinalBoss:
                        MoveData.Set(0f, 0f, 0f, 0f, 0f);
                        break;
                }
                break;
            #endregion

            #region Normal
            case GameDifficulty.Normal:
                switch (behavior.Type)
                {
                    case EnemyTypes.Basic:
                        MoveData.Set(0f, .5f, 90f, 1.8f, 15f);
                        break;
                    case EnemyTypes.Droid:
                        MoveData.Set(0f, .5f, 120f, .8f, 20f);
                        break;
                    case EnemyTypes.Transport:
                        MoveData.Set(0f, .5f, 150f, 2.5f, 15f);
                        break;
                    case EnemyTypes.Trident:
                        MoveData.Set(0f, .5f, 95f, 1.5f, 18f);
                        break;
                    case EnemyTypes.Tank:
                        MoveData.Set(0f, .5f, 60f, 4f, 15f);
                        break;
                }
                break;
            #endregion

            #region Hard
            case GameDifficulty.Hard:
                switch (behavior.Type)
                {
                    case EnemyTypes.Basic:
                        MoveData.Set(0f, .5f, 110f, 1.6f, 25f);
                        break;
                    case EnemyTypes.Droid:
                        MoveData.Set(0f, .5f, 160f, .7f, 40f);
                        break;
                    case EnemyTypes.Transport:
                        MoveData.Set(0f, .5f, 200f, 2f, 25f);
                        break;
                    case EnemyTypes.Trident:
                        MoveData.Set(0f, .5f, 120f, 1.2f, 30f);
                        break;
                    case EnemyTypes.Tank:
                        MoveData.Set(0f, .5f, 50f, 5f, 10f);
                        break;
                }
                break;
            #endregion

            #region Nightmare
            case GameDifficulty.Nightmare:
                switch (behavior.Type)
                {
                    case EnemyTypes.Basic:
                        MoveData.Set(0f, .5f, 150f, 1.4f, 40f);
                        break;
                    case EnemyTypes.Droid:
                        MoveData.Set(0f, .5f, 200f, .6f, 50f);
                        break;
                    case EnemyTypes.Transport:
                        MoveData.Set(0f, .5f, 250f, 1.8f, 30f);
                        break;
                    case EnemyTypes.Trident:
                        MoveData.Set(0f, .5f, 160f, 1f, 45f);
                        break;
                    case EnemyTypes.Tank:
                        MoveData.Set(0f, .5f, 50f, 5f, 10f);
                        break;
                }
                break;
                #endregion
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
        if (behavior.State == EnemyStates.Patrol)
        {
            autopilotlocation = _loc;
            autopilot = true;
        }
    }
    #endregion
}