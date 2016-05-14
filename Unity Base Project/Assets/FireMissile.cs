using UnityEngine;
using GD.Core.Enums;

public class FireMissile : MonoBehaviour {

    private ShipSystems Systems;
	// Use this for initialization
	void Start () {
	        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void Fire()
    {
        if (Systems.GetDeviceStatus(SystemType.MISSILES) == SystemStatus.ONLINE)
        {
            Systems.Missiles.FireMissile();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "rightPalm")
            Fire();
    }
}
