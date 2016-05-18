using UnityEngine;
using GD.Core.Enums;
using System.Collections;

public class PatrolAi : MonoBehaviour
{

    //  Raycast Data
    private int range;    
    private bool pathBlocked;
    private RaycastHit hit;

    //  Movement    
    private float interval;
    public float headingX, headingY;
    private float headingChange;
    private Vector3 targetRotation;

    //  Enemy Data   
    private EnemyBehavior behavior;     
    private CharacterController controller;


    void Start()
    {
        Debug.Log("Patrol Ai Initializing...");
        targetRotation = Vector3.zero;
        pathBlocked = false;
        headingChange = 45f;
        interval = 5f;
        range = 100;

        controller = GetComponent<CharacterController>();
        behavior = GetComponent<EnemyBehavior>();
        behavior.ChangeState(EnemyStates.PATROL);
        behavior.AutoPilot = false;

        // Set random initial rotation
        headingX = 0f;
        headingY = Random.Range(0, 360);
        behavior.MyTransform.eulerAngles = new Vector3(headingX, headingY, 0);
        StartCoroutine(NewHeading());
        Debug.Log("PatrolAI READY!");
    }

    void Update() {
        if (!behavior.AutoPilot)  
        {
            if (!pathBlocked)
            {
                if (behavior.Target == null)
                    behavior.MyTransform.rotation = Quaternion.Slerp(behavior.MyTransform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime / behavior.GetMoveData().RotateSpeed);
                else
                {
                    Vector3 playerDir = behavior.Target.position - behavior.MyTransform.position;
                    Vector3 direction = Vector3.RotateTowards(behavior.MyTransform.forward, playerDir, Time.deltaTime / behavior.GetMoveData().RotateSpeed, 0.0f);
                    behavior.MyTransform.rotation = Quaternion.LookRotation(direction);
                    headingX = behavior.MyTransform.eulerAngles.x;
                    headingY = behavior.MyTransform.eulerAngles.y;
                }
            }

            behavior.IncreaseSpeed();

            CheckRayCasts();

            if (pathBlocked)
            {
                if (Physics.Raycast(behavior.MyTransform.position - (behavior.MyTransform.forward * 4), behavior.MyTransform.right, out hit, (range / 2.0f)) ||
                Physics.Raycast(behavior.MyTransform.position - (behavior.MyTransform.forward * 4), -behavior.MyTransform.right, out hit, (range / 2.0f)))
                {
                    if (hit.collider.gameObject.CompareTag("Asteroid"))
                        pathBlocked = false;
                }
            }            
        }
        else
        {
            // Auto-pilot back into playable area
            Vector3 direction = Vector3.zero;

            //if (SecondaryTarget != null)
            //    direction = SecondaryTarget.position - MyTransform.position;
            //else
                direction = direction - behavior.MyTransform.position;

            Vector3 rotation = Vector3.RotateTowards(behavior.MyTransform.forward, direction, Time.deltaTime, 0.0f);
            behavior.MyTransform.rotation = Quaternion.LookRotation(rotation);
            headingX = behavior.MyTransform.eulerAngles.x;
            headingY = behavior.MyTransform.eulerAngles.y;
        }

        controller.Move(behavior. MyTransform.forward * Time.deltaTime * behavior.GetMoveData().Speed);

        // Use to debug the Physics.RayCast.
        Debug.DrawRay(behavior.MyTransform.position + (behavior.MyTransform.right * 12), behavior.MyTransform.forward * range, Color.red);
        Debug.DrawRay(behavior.MyTransform.position - (behavior.MyTransform.right * 12), behavior.MyTransform.forward * range, Color.red);
        Debug.DrawRay(behavior.MyTransform.position - (behavior.MyTransform.forward * 4), -behavior.MyTransform.right * (range / 2.0f), Color.yellow);
        Debug.DrawRay(behavior.MyTransform.position - (behavior.MyTransform.forward * 4), behavior.MyTransform.right * (range / 2.0f), Color.yellow);
    }
    #region Asteroid Avoidance
    private void CheckRayCasts() {
        if (Physics.Raycast(behavior.MyTransform.position + (behavior.MyTransform.right * 12), behavior.MyTransform.forward, out hit, range)) {
            if (hit.collider.gameObject.CompareTag("Asteroid")) {
                pathBlocked = true;
                behavior.MyTransform.Rotate(Vector3.down * Time.deltaTime * behavior.GetMoveData().RotateSpeed);
            }
        }
        else if (Physics.Raycast(behavior.MyTransform.position - (behavior.MyTransform.right * 12), behavior.MyTransform.forward, out hit, range)) {
            if (hit.collider.gameObject.CompareTag("Asteroid")) {
                pathBlocked = true;
                behavior.MyTransform.Rotate(Vector3.up * Time.deltaTime * behavior.GetMoveData().RotateSpeed);
            }
        }
    }
    #endregion

    #region Coroutine
    private IEnumerator NewHeading() {
        while (true) {
            NewHeadingRoutine();
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