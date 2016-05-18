using UnityEngine;
using GD.Core.Enums;
using System.Collections;

public class PatrolAi : EnemyBehavior
{
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
    public Transform SecondaryTarget { get; protected set; }

    //  Enemy Data        
    private CharacterController controller;


    void Awake() {
        Debug.Log("PatrolAI Awake Called");
        Init();
    }

    public override void Init()
    {
        base.Init();

        ChangeState(EnemyStates.PATROL);

        AutoPilot = false;
        pathBlocked = false;
        SecondaryTarget = null;

        range = 100;
        interval = 5f;
        maxHeadingChange = 45f;

        targetRotation = Vector3.zero;
        stats = GetComponent<EnemyStats>();
        controller = GetComponent<CharacterController>();

        // Set random initial rotation
        headingX = 0f;
        headingY = Random.Range(0, 360);
        stats.MyTransform.eulerAngles = new Vector3(headingX, headingY, 0);
        StartCoroutine(NewHeading());

        Debug.Log("PatrolAI Initialized");
    }

    void Update() {
        if (!AutoPilot)  
        {
            if (!pathBlocked)
            {
                if (stats.Target == null)
                    stats.MyTransform.rotation = Quaternion.Slerp(stats.MyTransform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime / stats.GetMoveData().RotateSpeed);
                else
                {
                    Vector3 playerDir = stats.Target.position - stats.MyTransform.position;
                    Vector3 direction = Vector3.RotateTowards(stats.MyTransform.forward, playerDir, Time.deltaTime / stats.GetMoveData().RotateSpeed, 0.0f);
                    stats.MyTransform.rotation = Quaternion.LookRotation(direction);
                    headingX = stats.MyTransform.eulerAngles.x;
                    headingY = stats.MyTransform.eulerAngles.y;
                }
            }

            CheckRayCasts();

            if (pathBlocked)
            {
                if (Physics.Raycast(stats.MyTransform.position - (stats.MyTransform.forward * 4), stats.MyTransform.right, out hit, (range / 2.0f)) ||
                Physics.Raycast(stats.MyTransform.position - (stats.MyTransform.forward * 4), -stats.MyTransform.right, out hit, (range / 2.0f)))
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
                direction = SecondaryTarget.position - stats.MyTransform.position;
            else
                direction = direction - stats.MyTransform.position;

            Vector3 rotation = Vector3.RotateTowards(stats.MyTransform.forward, direction, Time.deltaTime, 0.0f);
            stats.MyTransform.rotation = Quaternion.LookRotation(rotation);
            headingX = stats.MyTransform.eulerAngles.x;
            headingY = stats.MyTransform.eulerAngles.y;
        }

        controller.Move(stats.MyTransform.forward * Time.deltaTime * stats.GetMoveData().Speed);

        // Use to debug the Physics.RayCast.
        Debug.DrawRay(stats.MyTransform.position + (stats.MyTransform.right * 12), stats.MyTransform.forward * range, Color.red);
        Debug.DrawRay(stats.MyTransform.position - (stats.MyTransform.right * 12), stats.MyTransform.forward * range, Color.red);
        Debug.DrawRay(stats.MyTransform.position - (stats.MyTransform.forward * 4), -stats.MyTransform.right * (range / 2.0f), Color.yellow);
        Debug.DrawRay(stats.MyTransform.position - (stats.MyTransform.forward * 4), stats.MyTransform.right * (range / 2.0f), Color.yellow);
    }
    #region Asteroid Avoidance
    private void CheckRayCasts() {
        if (Physics.Raycast(stats.MyTransform.position + (stats.MyTransform.right * 12), stats.MyTransform.forward, out hit, range)) {
            if (hit.collider.gameObject.CompareTag("Asteroid")) {
                pathBlocked = true;
                stats.MyTransform.Rotate(Vector3.down * Time.deltaTime * stats.GetMoveData().RotateSpeed);
            }
        }
        else if (Physics.Raycast(stats.MyTransform.position - (stats.MyTransform.right * 12), stats.MyTransform.forward, out hit, range)) {
            if (hit.collider.gameObject.CompareTag("Asteroid")) {
                pathBlocked = true;
                stats.MyTransform.Rotate(Vector3.up * Time.deltaTime * stats.GetMoveData().RotateSpeed);
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