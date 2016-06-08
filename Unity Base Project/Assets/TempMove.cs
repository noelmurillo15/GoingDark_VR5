using UnityEngine;
using System.Collections;

public class TempMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
        if (Input.GetKey(KeyCode.Space))
        {
            gameObject.transform.position += new Vector3(0f, 50 * Time.deltaTime, 0f);
        }
	}
}
