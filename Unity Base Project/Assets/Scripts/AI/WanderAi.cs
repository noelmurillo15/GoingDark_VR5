using UnityEngine;
using System.Collections;

public class WanderAi : MonoBehaviour {
    //**        Attach to Enemy     **//
    public float speed = 0.25f;
    public float maxHeadingChange = 30.0f;
    public float directionChangeInterval = 1.0f;

    float heading;
    Vector3 targetRotation;
    CharacterController controller;    

    void Awake() {
        controller = GetComponent<CharacterController>();        

        // Set random initial rotation
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 0);

        StartCoroutine(NewHeading());
    }

    void Update() {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
        var forward = transform.TransformDirection(Vector3.forward);
        controller.Move(transform.forward * speed);
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
}