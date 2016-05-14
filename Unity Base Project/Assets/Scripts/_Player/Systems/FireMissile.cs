using UnityEngine;
using GD.Core.Enums;

public class FireMissile : MonoBehaviour {

    private SystemsManager manager;


    // Use this for initialization
    void Start () {
        manager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemsManager>();
    }
	
	// Update is called once per frame
	void Update () {

    }
   
    void OnTriggerEnter(Collider col)
    {
        if (col.name == "rightPalm")
            manager.ActivateSystem(SystemType.MISSILES);
    }
}
