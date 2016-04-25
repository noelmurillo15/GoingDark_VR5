using UnityEngine;
using System.Collections;

public class TransportScript : MonoBehaviour {

    private bool cloaked = false;
    Material transMat;


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
        transMat = this.GetComponent<Renderer>().material;
        
        if (val) {
            transMat.SetInt("_SrcBlend", 5);
            transMat.SetInt("_DstBlend", 10);
            transMat.SetInt("_ZWrite", 0);
        }    
        else {
            transMat.SetInt("_SrcBlend", 10);
            transMat.SetInt("_DstBlend", 10);
            transMat.SetInt("_ZWrite", 0);
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
