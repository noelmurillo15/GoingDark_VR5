using UnityEngine;
using GD.Core.Enums;

public class FireMissile : MonoBehaviour {

    private ShipDevices devices;
	// Use this for initialization
	void Start () {
	        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void Fire()
    {
        if (devices.GetDeviceStatus(Devices.MISSILES) == DeviceStatus.ONLINE)
        {
            devices.Missiles.FireMissile();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "rightPalm")
            Fire();
    }
}
