using UnityEngine;

public class HeadMovement : MonoBehaviour {

    private Transform head;
    private Transform MyTransform;

	// Use this for initialization
	void Start () {
        MyTransform = transform;
        head = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        MyTransform.rotation = Quaternion.Lerp(transform.rotation, head.rotation, Time.fixedDeltaTime);
	}
}
