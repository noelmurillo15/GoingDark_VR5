using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(CharacterController))]
public class PatrolAi : MonoBehaviour {
    //**        Attach to Enemy     **//

    //  Raycast Data
    public int range;
    public float interval;
    public float maxHeadingChange;
    public float speedBoost;
    public bool pathBlocked;
    private RaycastHit hit;

    //  Movement
    public bool autoMove;
    public float heading;
    private Vector3 targetRotation;

    //  Enemy Data
    private EnemyStats stats;
    private CharacterController controller;


    void Awake() {
        range = 100;
        interval = 5f;
        maxHeadingChange = 45f;
        pathBlocked = false;
        speedBoost = 0.5f;


        stats = GetComponent<EnemyStats>();
        controller = GetComponent<CharacterController>();        

        // Set random initial rotation
        heading = Random.Range(0, 360);
        transform.localEulerAngles = new Vector3(0, heading, 0);

        StartCoroutine(NewHeading());
    }

    void Update() {
        if (!autoMove)
        {
            if (!pathBlocked)
                transform.localEulerAngles = Vector3.Slerp(transform.localEulerAngles, targetRotation, Time.deltaTime / interval);

            stats.IncreaseSpeed(speedBoost);
            controller.Move(transform.forward * Time.deltaTime * stats.GetMoveSpeed());

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
            Vector3 playerDir = Vector3.zero - transform.position;
            Vector3 direction = Vector3.RotateTowards(transform.forward, playerDir, (stats.GetRotateSpeed() * 0.1f) * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(direction);

            Vector3 moveDir = Vector3.zero;
            moveDir = transform.TransformDirection(Vector3.forward);
            moveDir *= (stats.GetMaxSpeed() * Time.deltaTime * 5f);
            controller.Move(moveDir);

            heading = transform.localEulerAngles.y;
        }

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
                transform.Rotate(Vector3.down * Time.deltaTime * stats.GetRotateSpeed());
            }
        }
        else if (Physics.Raycast(transform.position - (transform.right * 12), transform.forward, out hit, range)) {
            if (hit.collider.gameObject.CompareTag("Asteroid")) {
                pathBlocked = true;
                transform.Rotate(Vector3.up * Time.deltaTime * stats.GetRotateSpeed());
            }
        }
    }
    #endregion

    public void SetSpeedBoost(float boost)
    {
        speedBoost = boost;
    }

    public void OutOfBounds(Vector3 targetPos)
    {
        autoMove = true;
    }

    public void InBounds()
    {
        autoMove = false;
    }

    #region Coroutine
    private IEnumerator NewHeading() {
        while (true) {
            NewHeadingRoutine();
            yield return new WaitForSeconds(interval);
        }
    }

    private void NewHeadingRoutine() {
        var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
        var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
        heading = Random.Range(floor, ceil);        
        targetRotation = new Vector3(0f, heading, 0f);
    }
    #endregion
}