using UnityEngine;

public class EmpCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Enemy") || col.CompareTag("TransportShip"))
        {
            col.SendMessage("EMPHit");
        }
    }
}
