using UnityEngine;
using MovementEffects;
using GoingDark.Core.Enums;
using System.Collections.Generic;

public class PatrolAi : MonoBehaviour
{
    #region Properties
    //  Raycast Data
    private int range;
    private bool blocked;
    private RaycastHit hit;

    //  Movement    
    private float interval;
    private float headingChange;
    private float headingX, headingY;
    private Vector3 targetRotation;

    //  Enemy Data 
    private Transform MyTransform;
    private EnemyBehavior behavior;
    private Rigidbody MyRigidbody;
    #endregion


    void Start()
    {
        // Patrol Data
        targetRotation = Vector3.zero;
        blocked = false;
        range = 100;

        //  Movement
        headingY = Random.Range(0, 360);
        headingChange = 45f;
        headingX = 0f;
        interval = 5f;

        // Enemy Data
        MyRigidbody = GetComponent<Rigidbody>();
        behavior = GetComponent<EnemyBehavior>();
        behavior.ChangeState(EnemyStates.Patrol);
        behavior.AutoPilot = false;

        // Set random rotation
        MyTransform = transform;
        MyTransform.eulerAngles = new Vector3(headingX, headingY, 0);

        //  Start Coroutine
        Timing.RunCoroutine(NewHeading());
    }

    void FixedUpdate()
    {
        if (!behavior.AutoPilot)
        {
            if (behavior.Target == null)
            {
                MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime / behavior.GetMoveData().RotateSpeed);
            }
            else
            {
                if (behavior.Debuff != Impairments.Stunned)
                {
                    Vector3 playerDir = behavior.Target.position - MyTransform.position;
                    Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.deltaTime / behavior.GetMoveData().RotateSpeed, 0.0f);
                    if (behavior.Type == EnemyTypes.Droid)
                    {
                        MyTransform.rotation = Quaternion.LookRotation(direction);
                    }
                    else
                    {
                        if (Vector3.Distance(behavior.Target.position, MyTransform.position) > 250f)
                            MyTransform.rotation = Quaternion.LookRotation(direction);
                        else
                        {
                            direction.x += 87.03f;
                            MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.Euler(direction), Time.deltaTime / behavior.GetMoveData().RotateSpeed);
                        }
                    }
                    headingX = MyTransform.eulerAngles.x;
                    headingY = MyTransform.eulerAngles.y;
                }
            }
        }
        else
        {
            // Auto-pilot back into playable area
            Vector3 direction = Vector3.zero;
            direction = direction - MyTransform.position;
            Vector3 rotation = Vector3.RotateTowards(MyTransform.forward, direction, Time.deltaTime, 0.0f);
            MyTransform.rotation = Quaternion.LookRotation(rotation);
            headingX = MyTransform.eulerAngles.x;
            headingY = MyTransform.eulerAngles.y;
        }

        if (behavior.Debuff != Impairments.Stunned)
            behavior.GetMoveData().IncreaseSpeed();
        else
            behavior.GetMoveData().DecreaseSpeed();

        MyRigidbody.MovePosition(MyTransform.position + MyTransform.forward * Time.deltaTime * behavior.GetMoveData().Speed);
    }

    #region Coroutine
    private IEnumerator<float> NewHeading()
    {
        while (true)
        {
            NewHeadingRoutine();
            interval = Random.Range(2f, 10f);
            yield return Timing.WaitForSeconds(interval);
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
    void OutOfBounds()
    {
        if (behavior == null)
        {
            GetComponent<EnemyBehavior>();
            return;
        }

        if (behavior.State == EnemyStates.Patrol)
            behavior.AutoPilot = true;
    }

    void InBounds()
    {
        if (behavior == null)
        {
            GetComponent<EnemyBehavior>();
            return;
        }

        behavior.AutoPilot = false;
    }
    #endregion
}