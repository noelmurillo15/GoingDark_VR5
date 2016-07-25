using UnityEngine;
using System.Collections;

public class WayPointScript : MonoBehaviour {

    private GameObject WayPointManager;

	// Use this for initialization
	void Start () {
        WayPointManager = GameObject.FindGameObjectWithTag("WayPointManager");

    }

    // Update is called once per frame
    void Update () {
	
	}

    void OnTriggerEnter(Collider ColliderObject)//hey what hit ya?
    {
        if (ColliderObject.CompareTag("Player"))
            WayPointManager.SendMessage("SetNextActive");
    }

}
