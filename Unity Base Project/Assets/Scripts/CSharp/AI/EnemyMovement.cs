using UnityEngine;
using MovementEffects;
using GoingDark.Core.Enums;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    #region Properties
    public MovementProperties MoveData;
    private EnemyStateManager behavior;

    //  Patrol    
    public bool autopilot;
    private float headingChange;
    public float headingX, headingY;
    private Vector3 targetRotation;
    private Vector3 autopilotlocation;

    //  Alert
    private float Distance;

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

        //  Alert
        Distance = 0f;

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
            Vector3 rotation = Vector3.RotateTowards(MyTransform.forward, dir, Time.fixedDeltaTime, 0.0f);
            MyTransform.rotation = Quaternion.LookRotation(rotation);
            headingX = MyTransform.eulerAngles.x;
            headingY = MyTransform.eulerAngles.y;
            MyRigidbody.MovePosition(MyTransform.position + MyTransform.forward * Time.fixedDeltaTime * MoveData.Speed);

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
        MyRigidbody.MovePosition(MyTransform.position + MyTransform.forward * Time.fixedDeltaTime * MoveData.Speed);
    }

    #region States
    void Patrol()
    {
        MoveData.IncreaseSpeed();
        MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.Euler(targetRotation), Time.fixedDeltaTime / MoveData.RotateSpeed);
    }
    void Alert()
    {
        MoveData.IncreaseSpeed();
        Vector3 lastplayerdir = behavior.LastKnownPos - MyTransform.position;
        if (Vector3.Distance(MyTransform.position, behavior.LastKnownPos) < 50f)
        {
            Debug.Log("Enemy has not found player, going back on patrol");
            Distance = 0;
            behavior.losingsightTimer = 0f;

            headingX = MyTransform.eulerAngles.x;
            headingY = MyTransform.eulerAngles.y;
            return;
        }

        Vector3 dir = Vector3.RotateTowards(MyTransform.forward, lastplayerdir, Time.fixedDeltaTime / MoveData.RotateSpeed, 0.0f);
        MyTransform.rotation = Quaternion.LookRotation(dir);        
    }
    void Attack()
    {
        MoveData.IncreaseSpeed();
        Vector3 playerDir = behavior.Target.position - MyTransform.position;
        Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.fixedDeltaTime / MoveData.RotateSpeed, 0.0f);

        switch (behavior.Type)
        {
            case EnemyTypes.Basic:
                if (Vector3.Distance(behavior.Target.position, MyTransform.position) > 500f)
                    MyTransform.rotation = Quaternion.LookRotation(direction, MyTransform.up);
                else
                {
                    direction.x += 75f;
                    MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.Euler(direction), Time.fixedDeltaTime / MoveData.RotateSpeed);
                }
                break;
            case EnemyTypes.Droid:
                MyTransform.rotation = Quaternion.LookRotation(direction);
                break;
            case EnemyTypes.SquadLead:
                if (Vector3.Distance(behavior.Target.position, MyTransform.position) > 500f)
                    MyTransform.rotation = Quaternion.LookRotation(direction, MyTransform.up);
                else
                {
                    direction.x += 75f;
                    MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.Euler(direction), Time.fixedDeltaTime / MoveData.RotateSpeed);
                }
                break;
            case EnemyTypes.JetFighter:
                break;
            case EnemyTypes.Transport:
                break;
            case EnemyTypes.Trident:
                break;
            case EnemyTypes.Boss:
                break;
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
        Vector3 playerDir =  MyTransform.position - behavior.Target.position;
        Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.fixedDeltaTime / MoveData.RotateSpeed, 0.0f);

        MyTransform.rotation = Quaternion.LookRotation(direction);

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
                    case EnemyTypes.Droid:
                        MoveData.Set(0f, .5f, 100f, 1f, 20f);
                        break;
                    case EnemyTypes.JetFighter:
                        MoveData.Set(0f, .5f, 120f, 1.25f, 30f);
                        break;
                    case EnemyTypes.Trident:
                        MoveData.Set(0f, .5f, 80f, 1.5f, 24f);
                        break;
                    case EnemyTypes.Basic:
                        MoveData.Set(0f, .5f, 60f, 2f, 18f);
                        break;
                    case EnemyTypes.SquadLead:
                        MoveData.Set(0f, .5f, 80f, 2.25f, 15f);
                        break;                    
                    case EnemyTypes.Transport:
                        MoveData.Set(0f, .5f, 120f, 3f, 12f);
                        break;                    
                    case EnemyTypes.Boss:
                        MoveData.Set(0f, .5f, 50f, 5f, 10f);
                        break;
                }
                break;
            #endregion

            #region Normal
            case GameDifficulty.Normal:
                switch (behavior.Type)
                {
                    case EnemyTypes.Droid:
                        MoveData.Set(0f, .5f, 100f, 1f, 25f);
                        break;
                    case EnemyTypes.JetFighter:
                        MoveData.Set(0f, .5f, 120f, 1.25f, 35f);
                        break;
                    case EnemyTypes.Trident:
                        MoveData.Set(0f, .5f, 80f, 1.5f, 28f);
                        break;
                    case EnemyTypes.Basic:
                        MoveData.Set(0f, .5f, 60f, 2f, 22f);
                        break;
                    case EnemyTypes.SquadLead:
                        MoveData.Set(0f, .5f, 80f, 2.25f, 20f);
                        break;
                    case EnemyTypes.Transport:
                        MoveData.Set(0f, .5f, 120f, 3f, 18f);
                        break;
                    case EnemyTypes.Boss:
                        MoveData.Set(0f, .5f, 50f, 5f, 12f);
                        break;
                }
                break;
            #endregion

            #region Hard
            case GameDifficulty.Hard:
                switch (behavior.Type)
                {
                    case EnemyTypes.Droid:
                        MoveData.Set(0f, .5f, 100f, 1f, 32f);
                        break;
                    case EnemyTypes.JetFighter:
                        MoveData.Set(0f, .5f, 120f, 1.25f, 45f);
                        break;
                    case EnemyTypes.Trident:
                        MoveData.Set(0f, .5f, 80f, 1.5f, 35f);
                        break;
                    case EnemyTypes.Basic:
                        MoveData.Set(0f, .5f, 60f, 2f, 28f);
                        break;
                    case EnemyTypes.SquadLead:
                        MoveData.Set(0f, .5f, 80f, 2.25f, 26f);
                        break;
                    case EnemyTypes.Transport:
                        MoveData.Set(0f, .5f, 120f, 3f, 22f);
                        break;
                    case EnemyTypes.Boss:
                        MoveData.Set(0f, .5f, 50f, 5f, 15f);
                        break;
                }
                break;
            #endregion

            #region Nightmare
            case GameDifficulty.Nightmare:
                switch (behavior.Type)
                {
                    case EnemyTypes.Droid:
                        MoveData.Set(0f, .5f, 100f, 1f, 45f);
                        break;
                    case EnemyTypes.JetFighter:
                        MoveData.Set(0f, .5f, 120f, 1.25f, 60f);
                        break;
                    case EnemyTypes.Trident:
                        MoveData.Set(0f, .5f, 80f, 1.5f, 40f);
                        break;
                    case EnemyTypes.Basic:
                        MoveData.Set(0f, .5f, 60f, 2f, 38f);
                        break;
                    case EnemyTypes.SquadLead:
                        MoveData.Set(0f, .5f, 80f, 2.25f, 35f);
                        break;
                    case EnemyTypes.Transport:
                        MoveData.Set(0f, .5f, 120f, 3f, 30f);
                        break;
                    case EnemyTypes.Boss:
                        MoveData.Set(0f, .5f, 50f, 5f, 20f);
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