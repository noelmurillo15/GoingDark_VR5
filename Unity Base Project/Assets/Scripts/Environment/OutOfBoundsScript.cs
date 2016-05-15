using UnityEngine;

public class OutOfBoundsScript : MonoBehaviour {

    private GameObject messages;

    // Use this for initialization
    void Start () {
        messages = GameObject.Find("WarningMessages");
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player")) {
            //Debug.Log("Player in playable area");
            messages.SendMessage("ManualPilot");
            col.SendMessage("InBounds");
        }

        if (col.CompareTag("Enemy") || col.CompareTag("TransportShip"))
        {
            if (col.GetType() == typeof(CharacterController))
            {
                //Debug.Log("Enemy In Bounds");
                col.SendMessage("InBounds");
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            //Debug.Log("Player Out Of Bounds");
            messages.SendMessage("AutoPilot");
            col.SendMessage("OutOfBounds", Vector3.zero);
        }
        if (col.CompareTag("Enemy") || col.CompareTag("TransportShip"))
        {
            if (col.GetType() == typeof(CharacterController))
            {
                //Debug.Log("Enemy Out Of Bounds");
                col.SendMessage("OutOfBounds");
            }
        }
    }
}