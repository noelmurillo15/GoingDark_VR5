using UnityEngine;
using GD.Core.Enums;
using System.Collections;

[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(CharacterController))]
public class PatrolAi : MonoBehaviour {
    //**        Attach to Enemy     **//

    //  Raycast Data
    private int range;    
    private bool pathBlocked;
    private RaycastHit hit;

    //  Movement
    public bool AutoPilot;
    private float interval;
    public float headingX, headingY;
    private float maxHeadingChange;
    private Vector3 targetRotation;
    public Transform SecondaryTarget { get; set; }

    //  Enemy Data        
    private EnemyStats stats;
    private EnemyBehavior behavior;
    private CharacterController controller;
    private Transform MyTransform;


    void Awake() {
        AutoPilot = false;
        pathBlocked = false;
        SecondaryTarget = null;

        range = 100;
        interval = 5f;
        maxHeadingChange = 45f;

        
        targetRotation = Vector3.zero;
        stats = GetComponent<EnemyStats>();
        behavior = GetComponent<EnemyBehavior>();
        controller = GetComponent<CharacterController>();

        MyTransform = transform;

        // Set random initial rotation
        headingX = 0f;
        headingY = Random.Range(0, 360);
        MyTransform.eulerAngles = new Vector3(headingX, headingY, 0);
        StartCoroutine(NewHeading());
    }

    void Update() {
        if (!AutoPilot)  
        {
            if (!pathBlocked)
            {
                if (behavior.Target == null)
                    MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime / stats.GetMoveData().RotateSpeed);
                else
                {
                    Vector3 playerDir = behavior.Target.position - MyTransform.position;
                    Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.deltaTime / stats.GetMoveData().RotateSpeed, 0.0f);
                    MyTransform.rotation = Quaternion.LookRotation(direction);
                    headingX = MyTransform.eulerAngles.x;
                    headingY = MyTransform.eulerAngles.y;
                }
            }

            if (stats.Debuff == Impairments.NONE)
                stats.IncreaseSpeed();
            else if (stats.Debuff == Impairments.STUNNED)
                stats.DecreaseSpeed();

            CheckRayCasts();

            if (pathBlocked)
            {
                if (Physics.Raycast(MyTransform.position - (MyTransform.forward * 4), MyTransform.right, out hit, (range / 2.0f)) ||
                Physics.Raycast(MyTransform.position - (MyTransform.forward * 4), -MyTransform.right, out hit, (range / 2.0f)))
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

            if (SecondaryTarget != null)
                direction = SecondaryTarget.position - MyTransform.position;
            else
                direction = direction - MyTransform.position;

            Vector3 rotation = Vector3.RotateTowards(MyTransform.forward, direction, Time.deltaTime, 0.0f);
            MyTransform.rotation = Quaternion.LookRotation(rotation);
            headingX = MyTransform.eulerAngles.x;
            headingY = MyTransform.eulerAngles.y;
        }

        controller.Move(MyTransform.forward * Time.deltaTime * stats.GetMoveData().Speed);

        // Use to debug the Physics.RayCast.
        //Debug.DrawRay(MyTransform.position + (MyTransform.right * 12), MyTransform.forward * range, Color.red);
        //Debug.DrawRay(MyTransform.position - (MyTransform.right * 12), MyTransform.forward * range, Color.red);
        //Debug.DrawRay(MyTransform.position - (MyTransform.forward * 4), -MyTransform.right * (range / 2.0f), Color.yellow);
        //Debug.DrawRay(MyTransform.position - (MyTransform.forward * 4), MyTransform.right * (range / 2.0f), Color.yellow);
    }
    #region Asteroid Avoidance
    private void CheckRayCasts() {
        if (Physics.Raycast(MyTransform.position + (MyTransform.right * 12), MyTransform.forward, out hit, range)) {
            if (hit.collider.gameObject.CompareTag("Asteroid")) {
                pathBlocked = true;
                MyTransform.Rotate(Vector3.down * Time.deltaTime * stats.GetMoveData().RotateSpeed);
            }
        }
        else if (Physics.Raycast(MyTransform.position - (MyTransform.right * 12), MyTransform.forward, out hit, range)) {
            if (hit.collider.gameObject.CompareTag("Asteroid")) {
                pathBlocked = true;
                MyTransform.Rotate(Vector3.up * Time.deltaTime * stats.GetMoveData().RotateSpeed);
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
        var floor = Mathf.Clamp(headingX - maxHeadingChange, 0, 360);
        var ceil = Mathf.Clamp(headingX + maxHeadingChange, 0, 360);
        headingX = Random.Range(floor, ceil);

        floor = Mathf.Clamp(headingY - maxHeadingChange, 0, 360);
        ceil = Mathf.Clamp(headingY + maxHeadingChange, 0, 360);
        headingY = Random.Range(floor, ceil);

        targetRotation = new Vector3(headingX, headingY, 0f);
    }
    #endregion

    #region Msgs
    void OutOfBounds()
    {
        AutoPilot = true;
    }

    void InBounds()
    {
        AutoPilot = false;
    }
    #endregion
}