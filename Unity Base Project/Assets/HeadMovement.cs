using UnityEngine;

public class HeadMovement : MonoBehaviour {

    private Transform head;

	// Use this for initialization
	void Start () {
        head = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.rotation = Quaternion.Lerp(transform.rotation, head.rotation, Time.fixedDeltaTime);
	}
}
