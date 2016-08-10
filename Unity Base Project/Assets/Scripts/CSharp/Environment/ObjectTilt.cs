using UnityEngine;
using System.Collections;

public class ObjectTilt : MonoBehaviour {
    private Vector3 velocity;
    private Vector3 rotation;
    private Transform myTransform;
    // Use this for initialization
    void Start () {
        velocity.x = Random.Range(-0.5f, 0.5f);
        velocity.y = Random.Range(-0.5f, 0.5f);
        velocity.z = Random.Range(-0.5f, 0.5f);

        rotation.x = Random.Range(-15.0f, 15.0f);
        rotation.y = Random.Range(-15.0f, 15.0f);
        rotation.z = Random.Range(-15.0f, 15.0f);

        myTransform = transform;
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        myTransform.Rotate(rotation * Time.fixedDeltaTime);
        myTransform.Translate(velocity * Time.fixedDeltaTime);
    }
}
