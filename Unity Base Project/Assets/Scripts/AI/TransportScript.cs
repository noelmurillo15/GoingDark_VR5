using UnityEngine;
using System.Collections;

public class TransportScript : MonoBehaviour {

    private bool cloaked;
    private float cloakTimer;
    private float cloakCooldown;


    // Use this for initialization
    void Start () {
        cloaked = false;
        cloakTimer = 5.0f;
    }
	
	// Update is called once per frame
	void Update ()  {

        if (cloakTimer > 0.0f)
            cloakTimer -= Time.deltaTime;
        else
        {
            cloakTimer = 0.0f;
            SetCloaked(false);
        }               
	}

    bool GetCloaked()
    {
        return cloaked;
    }

    void SetCloaked(bool val) {          
        if (val && cloakCooldown < 0.0f) {
            cloakTimer = 30.0f;
        }
        else {
            cloakCooldown = 60.0f;
        }            
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
