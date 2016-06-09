using UnityEngine;
using GoingDark.Core.Enums;
using System.Collections;

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
        behavior.ChangeState(EnemyStates.PATROL);
        behavior.AutoPilot = false;

        // Set random rotation
        MyTransform = transform;
        MyTransform.eulerAngles = new Vector3(headingX, headingY, 0);

        //  Start Coroutine
        StartCoroutine(NewHeading());
    }

    void FixedUpdate() {
        if (!behavior.AutoPilot)  
        {
            if (!blocked)
            {
                if (behavior.Target == null)
                {
                    MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime / behavior.GetMoveData().RotateSpeed);
                }
                else
                {
                    Vector3 playerDir = behavior.Target.position - MyTransform.position;
                    Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.deltaTime / behavior.GetMoveData().RotateSpeed, 0.0f);
                    MyTransform.rotation = Quaternion.LookRotation(direction);
                    headingX = MyTransform.eulerAngles.x;
                    headingY = MyTransform.eulerAngles.y;
                }
            }

            behavior.IncreaseSpeed();

            //CheckRayCasts();

            //if (blocked)
            //{
            //    if (Physics.Raycast(MyTransform.position - (MyTransform.forward * 4), MyTransform.right, out hit, (range / 2.0f)) ||
            //    Physics.Raycast(MyTransform.position - (MyTransform.forward * 4), -MyTransform.right, out hit, (range / 2.0f)))
            //    {
            //        if (hit.collider.gameObject.CompareTag("Asteroid"))
            //            blocked = false;
            //    }
            //}            
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

        MyRigidbody.MovePosition(MyTransform.position + MyTransform.forward * Time.deltaTime * behavior.GetMoveData().Speed);

        // Use to debug the Physics.RayCast.
        //Debug.DrawRay(MyTransform.position + (MyTransform.right * 12), MyTransform.forward * range, Color.red);
        //Debug.DrawRay(MyTransform.position - (MyTransform.right * 12), MyTransform.forward * range, Color.red);
        //Debug.DrawRay(MyTransform.position - (MyTransform.forward * 4), -MyTransform.right * (range / 2.0f), Color.yellow);
        //Debug.DrawRay(MyTransform.position - (MyTransform.forward * 4), MyTransform.right * (range / 2.0f), Color.yellow);
    }
    #region Asteroid Avoidance
    private void CheckRayCasts() {
        if (Physics.Raycast(behavior.MyTransform.position + (behavior.MyTransform.right * 12), behavior.MyTransform.forward, out hit, range)) {
            if (hit.collider.gameObject.CompareTag("Asteroid")) {
                blocked = true;
                behavior.MyTransform.Rotate(Vector3.down * Time.deltaTime * behavior.GetMoveData().RotateSpeed);
            }
        }
        else if (Physics.Raycast(behavior.MyTransform.position - (behavior.MyTransform.right * 12), behavior.MyTransform.forward, out hit, range)) {
            if (hit.collider.gameObject.CompareTag("Asteroid")) {
                blocked = true;
                behavior.MyTransform.Rotate(Vector3.up * Time.deltaTime * behavior.GetMoveData().RotateSpeed);
            }
        }
    }
    #endregion

    #region Coroutine
    private IEnumerator NewHeading() {
        while (true) {
            NewHeadingRoutine();
            interval = Random.Range(2f, 10f);
            yield return new WaitForSeconds(interval);
        }
    }
    private void NewHeadingRoutine() {
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
        behavior.AutoPilot = true;
    }

    void InBounds()
    {
        behavior.AutoPilot = false;
    }
    #endregion
}