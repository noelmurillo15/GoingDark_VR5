using UnityEngine;
using GD.Core.Enums;
using System.Collections.Generic;

public class SystemsManager : MonoBehaviour
{
    #region Properties
    public Dictionary<SystemType, ShipDevice> AvailableDevices;
    private ShipSystems Systems;
    #endregion


    // Use this for initialization
    void Awake()
    {
        AvailableDevices = new Dictionary<SystemType, ShipDevice>();
        Systems = GameObject.FindGameObjectWithTag("Systems").GetComponent<ShipSystems>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddDevice(SystemType key)
    {        
        switch (key)
        {
            case SystemType.DECOY:
                AvailableDevices.Add(key, Systems.GetSystem(key).GetComponent<DecoySystem>() as ShipDevice);
                break;
        }        
    }

    public void ActivateSystem(SystemType type)
    {
        AvailableDevices[type].Activate();
    }
}