using UnityEngine;
using GoingDark.Core.Enums;

public class FireMissile : MonoBehaviour {

   
    private SystemManager manager;


    // Use this for initialization
    void Start () {
        
        manager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();
    }
	
	// Update is called once per frame
	void Update () {

    }
   
    void OnTriggerEnter(Collider col)
    {
        if (col.name == "rightPalm")
            manager.ActivateSystem(SystemType.Missile);
    }
}
