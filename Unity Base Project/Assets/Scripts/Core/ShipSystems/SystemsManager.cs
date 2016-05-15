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
            case SystemType.EMP:
                AvailableDevices.Add(key, Systems.GetSystem(key).GetComponent<EmpSystem>() as ShipDevice);
                break;
            case SystemType.HYPERDRIVE:
                AvailableDevices.Add(key, Systems.GetSystem(key).GetComponent<HyperdriveSystem>() as ShipDevice);
                break;
            case SystemType.MISSILES:
                AvailableDevices.Add(key, Systems.GetSystem(key).GetComponent<MissileSystem>() as ShipDevice);
                break;
            case SystemType.CLOAK:
                AvailableDevices.Add(key, Systems.GetSystem(key).GetComponent<CloakSystem>() as ShipDevice);
                break;
        }        
    }

    public void ActivateSystem(SystemType type)
    {
        AvailableDevices[type].Activate();
    }

    public float GetSystemCooldown(SystemType type)
    {
        if(AvailableDevices.ContainsKey(type))
            return AvailableDevices[type].Cooldown;

        return 0f;
    }
}