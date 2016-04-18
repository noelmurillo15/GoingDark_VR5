using UnityEngine;
using System.Collections;

public class LeapOrientation : MonoBehaviour {
    //**    Attach Script to Leap Mount   **//

    private float padding;
    public GameObject leapCam;
    public Vector3 parentOffset;
    public Vector3 leapCamRotation;

	// Use this for initialization
	void Start () {
        if (leapCam == null)
            leapCam = GameObject.FindGameObjectWithTag("MainCamera");

        padding = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (padding > 0)
            padding -= Time.deltaTime;
        else padding = 0.0f;

        leapCamRotation = leapCam.transform.eulerAngles;
        if (Input.GetKey(KeyCode.LeftControl) && padding == 0.0f){            
            parentOffset.x = 360.0f - leapCamRotation.x;
            parentOffset.y = 360.0f - leapCamRotation.y;
            parentOffset.z = 360.0f - leapCamRotation.z;
            transform.Rotate(0.0f, parentOffset.y, 0.0f);
            padding = 0.2f;
        }
    }
}
