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
        Color col = GetComponent<Renderer>().material.color;

        Debug.Log("Transport called cloak");

        if (val)
            col.a = 0.5f;        
        else
            col.a = 1.0f;
        
        cloaked = val;
    }

    void OnTriggerEnter(Collider col) {
        if (col.tag == "Player" && !col.GetComponent<PlayerData>().GetPlayerCloak().GetCloaked())
            SetCloaked(true);
    }

    void OnTriggerExit(Collider col) {
        if (col.tag == "Player" && !col.GetComponent<PlayerData>().GetPlayerCloak().GetCloaked())
            SetCloaked(false);
    }


    void Kill() {
        Debug.Log("Destroyed Transport Ship");
    }
}
