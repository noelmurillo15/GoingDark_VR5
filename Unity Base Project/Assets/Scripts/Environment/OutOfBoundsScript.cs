using UnityEngine;

public class OutOfBoundsScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player")) {
            Debug.Log("Player in playable area");
            col.SendMessage("InBounds");
        }

        if (col.CompareTag("Enemy"))
        {
            Debug.Log("Enemy In Bounds");
            col.SendMessage("InBounds");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("Player Out Of Bounds");
            col.SendMessage("OutOfBounds", this.transform.position);
        }
        if (col.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Out Of Bounds");
            col.SendMessage("OutOfBounds", this.transform.position);
        }
    }
}