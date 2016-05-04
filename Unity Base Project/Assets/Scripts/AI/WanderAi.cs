using UnityEngine;
using System.Collections;

public class WanderAi : MonoBehaviour {
    //**        Attach to Enemy     **//
    public int range = 80;
    public float speed = 25f;
    public float rotationSpeed = 40f;
    public float maxHeadingChange = 30f;
    public float directionChangeInterval = 5f;
    public bool isThereAnyThing = false;

    private float heading;
    private RaycastHit hit;
    private Vector3 targetRotation;
    private CharacterController controller;

    void Awake() {
        controller = GetComponent<CharacterController>();        

        // Set random initial rotation
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 0);

        StartCoroutine(NewHeading());
    }

    void Update() {
        if (!isThereAnyThing)
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime / 5f);
        }

        controller.Move(transform.forward * Time.deltaTime * speed);

        CheckRayCasts();

        if (isThereAnyThing)
        {
            //StopCoroutine(NewHeading());
            if (Physics.Raycast(transform.position - (transform.forward * 4), transform.right, out hit, (range / 2.0f)) ||
            Physics.Raycast(transform.position - (transform.forward * 4), -transform.right, out hit, (range / 2.0f)))
            {
                if (hit.collider.gameObject.CompareTag("Asteroid"))
                {
                    isThereAnyThing = false;
                    //StartCoroutine(NewHeading());
                }
            }
        }

        // Use to debug the Physics.RayCast.
        Debug.DrawRay(transform.position + (transform.right * 12), transform.forward * range, Color.red);
        Debug.DrawRay(transform.position - (transform.right * 12), transform.forward * range, Color.red);
        Debug.DrawRay(transform.position - (transform.forward * 4), -transform.right * (range / 2.0f), Color.yellow);
        Debug.DrawRay(transform.position - (transform.forward * 4), transform.right * (range / 2.0f), Color.yellow);
    }

    IEnumerator NewHeading() {
        while (true) {
            NewHeadingRoutine();
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    void NewHeadingRoutine() {
        var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
        var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
        heading = Random.Range(floor, ceil);        
        targetRotation = new Vector3(0, heading, 0);
    }

    void CheckRayCasts()
    {
        if (Physics.Raycast(transform.position + (transform.right * 12), transform.forward, out hit, range))
        {
            if (hit.collider.gameObject.CompareTag("Asteroid"))
            {
                Debug.Log("Right Raycast Hit");
                isThereAnyThing = true;
                transform.Rotate(Vector3.down * Time.deltaTime * rotationSpeed);
            }
        }
        else if (Physics.Raycast(transform.position - (transform.right * 12), transform.forward, out hit, range))
        {
            if (hit.collider.gameObject.CompareTag("Asteroid"))
            {
                Debug.Log("Left Raycast Hit");
                isThereAnyThing = true;
                transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
            }
        }
    }
}