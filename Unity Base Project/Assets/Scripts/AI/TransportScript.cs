using UnityEngine;
using System.Collections;

public class TransportScript : MonoBehaviour {

    private bool cloaked = false;

	// Use this for initialization
	void Start () {        
       
	}
	
	// Update is called once per frame
	void Update ()  {

	}

    bool GetCloaked()
    {
        return cloaked;
    }

    void SetCloaked(bool val) {
        Debug.Log("Transport called cloak");
        Material mat = gameObject.GetComponent<Renderer>().material;

        if (val)
            mat.SetFloat("_Mode", 2);                   
        else
            mat.SetFloat("_Mode", 0);

        cloaked = val;
    }

    void OnTriggerEnter(Collider col) {
        if (col.tag == "Player")
            SetCloaked(true);
    }

    void OnTriggerExit(Collider col) {
        if (col.tag == "Player")
            SetCloaked(false);
    }


    void Kill() {
        Debug.Log("Destroyed Transport Ship");
    }
}
