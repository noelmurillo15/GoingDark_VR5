using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(CharacterController))]
public class WanderAi : MonoBehaviour {
    //**        Attach to Enemy     **//

    //  Raycast Data
    public int range;
    public float interval;
    public float maxHeadingChange;
    public bool pathBlocked;
    private RaycastHit hit;

    //  Movement
    private float heading;
    private Vector3 targetRotation;

    //  Enemy Data
    private EnemyStats stats;
    private CharacterController controller;


    void Awake() {
        range = 100;
        interval = 5f;
        maxHeadingChange = 45f;
        pathBlocked = false;
         
        stats = GetComponent<EnemyStats>();
        controller = GetComponent<CharacterController>();        

        // Set random initial rotation
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 0);

        StartCoroutine(NewHeading());
    }

    void Update() {
        if (!pathBlocked)
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime / interval);

        stats.IncreaseSpeed(0.5f);
        controller.Move(transform.forward * Time.deltaTime * stats.GetMoveSpeed());

        CheckRayCasts();

        if (pathBlocked) {
            if (Physics.Raycast(transform.position - (transform.forward * 4), transform.right, out hit, (range / 2.0f)) ||
            Physics.Raycast(transform.position - (transform.forward * 4), -transform.right, out hit, (range / 2.0f))) {
                if (hit.collider.gameObject.CompareTag("Asteroid"))
                    pathBlocked = false;
            }
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
                Debug.Log("Right Raycast Hit");
                pathBlocked = true;
                transform.Rotate(Vector3.down * Time.deltaTime * stats.GetRotateSpeed());
            }
        }
        else if (Physics.Raycast(transform.position - (transform.right * 12), transform.forward, out hit, range)) {
            if (hit.collider.gameObject.CompareTag("Asteroid")) {
                Debug.Log("Left Raycast Hit");
                pathBlocked = true;
                transform.Rotate(Vector3.up * Time.deltaTime * stats.GetRotateSpeed());
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
        Debug.Log("NewHeadingRoutine");
        var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
        var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
        heading = Random.Range(floor, ceil);        
        targetRotation = new Vector3(0f, heading, 0f);
    }
    #endregion
}