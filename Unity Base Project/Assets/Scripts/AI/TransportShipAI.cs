using UnityEngine;
using System.Collections;

public class TransportShipAI : MonoBehaviour {

    public Material transMat;
    public Material opaqueMat;

    private GameObject mesh1;
    private GameObject mesh2;

    // Use this for initialization
    void Start () {
        mesh1 = transform.GetChild(0).gameObject;
        mesh2 = transform.GetChild(1).gameObject;

        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            mesh1.GetComponent<Renderer>().material = transMat;
            mesh2.GetComponent<Renderer>().material = transMat;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            mesh1.GetComponent<Renderer>().material = opaqueMat;
            mesh2.GetComponent<Renderer>().material = opaqueMat;
        }
    }
}
