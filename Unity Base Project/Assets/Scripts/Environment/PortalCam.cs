using UnityEngine;

public class PortalCam : MonoBehaviour {
   
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, 2 * Time.deltaTime);
	}

    void OnBecameVisible()
    {
        enabled = true;
    }
    void OnBecameInvisible()
    {
        enabled = false;
    }
}
