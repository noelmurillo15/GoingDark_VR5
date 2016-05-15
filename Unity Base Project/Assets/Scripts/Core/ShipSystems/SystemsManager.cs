using UnityEngine;
using GD.Core.Enums;
using System.Collections.Generic;

public class SystemsManager : MonoBehaviour {

    #region Properties
    public Dictionary<SystemType, ShipDevice> MainDevices;
    public Dictionary<SystemType, GameObject> SecondaryDevices;
    private ShipSystems Systems;
    #endregion


    void Awake()
    {
        MainDevices = new Dictionary<SystemType, ShipDevice>();
        SecondaryDevices = new Dictionary<SystemType, GameObject>();
        Systems = GameObject.FindGameObjectWithTag("Systems").GetComponent<ShipSystems>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Public Methods
    public void AddDevice(SystemType key)
    {        
        switch (key)
        {
            #region Main Devices
            case SystemType.DECOY:
                MainDevices.Add(key, Systems.GetSystem(key).GetComponent<DecoySystem>() as ShipDevice);
                MainDevices[key].SetStatus(DeviceStatus.ONLINE);
                break;
            case SystemType.EMP:
                MainDevices.Add(key, Systems.GetSystem(key).GetComponent<EmpSystem>() as ShipDevice);
                MainDevices[key].SetStatus(DeviceStatus.ONLINE);
                break;
            case SystemType.HYPERDRIVE:
                MainDevices.Add(key, Systems.GetSystem(key).GetComponent<HyperdriveSystem>() as ShipDevice);
                MainDevices[key].SetStatus(DeviceStatus.ONLINE);
                break;
            case SystemType.MISSILES:
                MainDevices.Add(key, Systems.GetSystem(key).GetComponent<MissileSystem>() as ShipDevice);
                MainDevices[key].SetStatus(DeviceStatus.ONLINE);
                break;
            case SystemType.CLOAK:
                MainDevices.Add(key, Systems.GetSystem(key).GetComponent<CloakSystem>() as ShipDevice);
                MainDevices[key].SetStatus(DeviceStatus.ONLINE);
                break;
            #endregion

            #region Secondary Devices
            case SystemType.RADAR:
                SecondaryDevices.Add(key, Systems.GetSystem(key));
                break;
            case SystemType.SHIELD:
                SecondaryDevices.Add(key, Systems.GetSystem(key));
                break;
            case SystemType.LASERS:
                SecondaryDevices.Add(key, Systems.GetSystem(key));
                break;
                #endregion
        }
    }

    public void ActivateSystem(SystemType key)
    {
        if (MainDevices.ContainsKey(key))
        {
            if (MainDevices[key].Status == DeviceStatus.ONLINE)
                MainDevices[key].Activate();
        }
    }

    public void ToggleSystem(SystemType key)
    {
        if (SecondaryDevices.ContainsKey(key))
            SecondaryDevices[key].SetActive(!SecondaryDevices[key].activeSelf);
    }

    public float GetSystemCooldown(SystemType key)
    {
        if (MainDevices.ContainsKey(key))
        {
            if (MainDevices[key].Status == DeviceStatus.ONLINE)
                return MainDevices[key].Cooldown;
            else
                return 100f;
        }
        return 0f;
    }
    #endregion
}