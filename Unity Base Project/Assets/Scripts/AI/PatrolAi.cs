using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(CharacterController))]
public class PatrolAi : MonoBehaviour {
    //**        Attach to Enemy     **//

    //  Raycast Data
    private int range;
    private float interval;
    private float maxHeadingChange;
    private float speedBoost;
    private bool pathBlocked;
    private RaycastHit hit;

    //  Movement
    public bool autoMove;
    public float heading;
    private Vector3 targetRotation;

    //  Enemy Data    
    public Transform target;
    private EnemyStats stats;
    private CharacterController controller;


    void Awake() {
        pathBlocked = false;

        range = 100;
        interval = 5f;
        speedBoost = 0.5f;
        maxHeadingChange = 45f;

        target = null;
        targetRotation = Vector3.zero;
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
            {
                if (target == null)
                {
                    transform.localEulerAngles = Vector3.Slerp(transform.localEulerAngles, targetRotation, Time.deltaTime / stats.GetRotateSpeed());
                }
                else
                {
                    Vector3 playerDir = target.position - transform.position;
                    Vector3 direction = Vector3.RotateTowards(transform.forward, playerDir, Time.deltaTime / stats.GetRotateSpeed(), 0.0f);
                    transform.rotation = Quaternion.LookRotation(direction);
                    heading = transform.localEulerAngles.y;
                }
            }

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
            // Auto-pilot back into playable area
            Vector3 playerDir = Vector3.zero - transform.position;
            Vector3 direction = Vector3.RotateTowards(transform.forward, playerDir, (stats.GetRotateSpeed() * 0.075f) * Time.deltaTime, 0.0f);
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

    #region Accessors
    public Transform GetTarget()
    {
        return target;
    }
    #endregion

    #region Modifiers
    public void SetEnemyTarget(Transform _target)
    {
        if (_target == null)
        {
            if (stats.GetEnemyType() == EnemyStats.ENEMY_TYPE.TRANSPORT)
                SetSpeedBoost(0.25f);
            else
                SetSpeedBoost(.5f);
        }
        else
            SetSpeedBoost(1f);

        target = _target;
    }
    public void SetSpeedBoost(float boost)
    {
        speedBoost = boost;
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
        var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
        var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
        heading = Random.Range(floor, ceil);        
        targetRotation = new Vector3(0f, heading, 0f);
    }
    #endregion

    #region Msgs
    public void OutOfBounds()
    {
        autoMove = true;
    }

    public void InBounds()
    {
        autoMove = false;
    }
    #endregion
}