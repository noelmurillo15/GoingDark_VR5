using UnityEngine;

public class SpaceSector : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            //col.SendMessage("UpdateSector", transform.parent.name.ToString());
        }
    }
}
