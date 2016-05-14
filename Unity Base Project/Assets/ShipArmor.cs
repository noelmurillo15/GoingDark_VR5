using UnityEngine;
using System.Collections;

public class ShipArmor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ChangeArmor(int ArmorStage)
    {
        switch (ArmorStage)
        {
            case 2:
                gameObject.GetComponent<MeshRenderer>().material.color += new Color(0.5f, 0.0f, 0.0f);
                break;
            case 3:
                gameObject.GetComponent<MeshRenderer>().material.color += new Color(0.5f, 0.0f, 0.5f);
                break;
            case 4:
                gameObject.GetComponent<MeshRenderer>().material.color += new Color(0.5f, 0.5f, 0.5f);
                break;
            case 5:
                gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.0f, 0.0f, 0.0f);
                break;
            default:
                break;
        }

    }
}
