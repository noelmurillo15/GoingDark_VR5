using UnityEngine;
using System.Collections;

public class SpinText : MonoBehaviour {

    // Use this for initialization
    Transform myTransform;
    [SerializeField]
    Transform centerPos;
    [SerializeField]
    float rotateSpeed;
    void Start () {
        myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
        myTransform.RotateAround(centerPos.position,Vector3.down, rotateSpeed* Time.deltaTime);
	}
}
