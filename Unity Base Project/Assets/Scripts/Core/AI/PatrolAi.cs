using UnityEngine;
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

        // Set random initial rotation
        headingX = 0f;
        headingY = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(headingX, headingY, 0);
        StartCoroutine(NewHeading());
    }

    void Update() {
        if (!AutoPilot)  
        {
            if (!pathBlocked)
            {
                if (behavior.Target == null) 
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime / stats.RotateSpeed);
                }
                else
                {
                    Vector3 playerDir = behavior.Target.position - transform.position;
                    Vector3 direction = Vector3.RotateTowards(transform.forward, playerDir, Time.deltaTime / stats.RotateSpeed, 0.0f);
                    transform.rotation = Quaternion.LookRotation(direction);
                    headingX = transform.eulerAngles.x;
                    headingY = transform.eulerAngles.y;
                }
            }

            stats.IncreaseSpeed();
            CheckRayCasts();

            if (pathBlocked)
            {
                if (Physics.Raycast(transform.position - (transform.forward * 4), transform.right, out hit, (range / 2.0f)) ||
                Physics.Raycast(transform.position - (transform.forward * 4), -transform.right, out hit, (range / 2.0f)))
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
                direction = SecondaryTarget.position - transform.position;
            else
                direction = direction - transform.position;

            Vector3 rotation = Vector3.RotateTowards(transform.forward, direction, Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(rotation);
            headingX = transform.eulerAngles.x;
            headingY = transform.eulerAngles.y;
        }

        controller.Move(transform.forward * Time.deltaTime * stats.Speed);

        // Use to debug the Physics.RayCast.
        Debug.DrawRay(transform.position + (transform.right * 12), transform.forward * range, Color.red);
        Debug.DrawRay(transform.position - (transform.right * 12), transform.forward * range, Color.red);
        Debug.DrawRay(transform.position - (transform.forward * 4), -transform.right * (range / 2.0f), Color.yellow);
        Debug.DrawRay(transform.position - (transform.forward * 4), transform.right * (range / 2.0f), Color.yellow);
    }
    #region Asteroid Avoidance
    private void CheckRayCasts() {
        if (Physics.Raycast(transform.position + (transform.right * 12), transform.forward, out hit, range)) {
            if (hit.collider.gameObject.CompareTag("Asteroid")) {
                pathBlocked = true;
                transform.Rotate(Vector3.down * Time.deltaTime * stats.RotateSpeed);
            }
        }
        else if (Physics.Raycast(transform.position - (transform.right * 12), transform.forward, out hit, range)) {
            if (hit.collider.gameObject.CompareTag("Asteroid")) {
                pathBlocked = true;
                transform.Rotate(Vector3.up * Time.deltaTime * stats.RotateSpeed);
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