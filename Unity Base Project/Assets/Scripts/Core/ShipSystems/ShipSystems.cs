using UnityEngine;
using GD.Core.Enums;
using System.Collections.Generic;

public class ShipSystems : MonoBehaviour
{

    #region Properties
    public Dictionary<SystemType, GameObject> MyDevices;

    // Constant Devices
    public GameObject MissionLog { get; private set; }
    public GameObject WeaponSelect { get; private set; }

    private SystemsManager manager;
    #endregion


    // Use this for initialization
    void Awake()
    {
        MyDevices = new Dictionary<SystemType, GameObject>();
        manager = GetComponent<SystemsManager>();

        // Main Systems
        InitializeDevice(SystemType.EMP);
        InitializeDevice(SystemType.CLOAK);
        InitializeDevice(SystemType.DECOY);
        InitializeDevice(SystemType.MISSILES);
        InitializeDevice(SystemType.HYPERDRIVE);

        // Secondary System
        //InitializeDevice(SystemType.LASERS);
        //InitializeDevice(SystemType.SHIELD);
        //InitializeDevice(SystemType.RADAR);

        // Constant Systems
        MissionLog = GameObject.Find("ButtonObject");
        WeaponSelect = GameObject.Find("WeaponButtonObj");
    }

    #region Accessors
    public SystemStatus GetSystemStatus(SystemType key)
    {
        if (MyDevices.ContainsKey(key))
            return SystemStatus.AVAILABLE;
        else
            return SystemStatus.NOTAVAILABLE;
    }

    public GameObject GetSystem(SystemType key)
    {
        if (GetSystemStatus(key) == SystemStatus.AVAILABLE)
            return MyDevices[key];

        return null;
    }
    #endregion

    #region Modifiers
    private void InitializeDevice(SystemType key)
    {
        if (MyDevices.ContainsKey(key))
        {
            Debug.Log("Device is already initialized : " + key.ToString());
            return;
        }

        GameObject obj = null;
        switch (key)
        {
            case SystemType.EMP:
                obj = Resources.Load<GameObject>("Devices/Emp");
                break;
            case SystemType.CLOAK:
                obj = Resources.Load<GameObject>("Devices/Cloak");
                break;
            case SystemType.RADAR:
                obj = Resources.Load<GameObject>("Devices/Radar");
                break;
            case SystemType.DECOY:
                obj = Resources.Load<GameObject>("Devices/Decoy");
                break;
            case SystemType.LASERS:
                obj = Resources.Load<GameObject>("Devices/Lasers");
                break;
            case SystemType.SHIELD:
                obj = Resources.Load<GameObject>("Devices/Shield");
                break;
            case SystemType.MISSILES:
                obj = Resources.Load<GameObject>("Devices/Missiles");
                break;
            case SystemType.HYPERDRIVE:
                obj = Resources.Load<GameObject>("Devices/HyperDrive");
                break;
        }

        if (obj != null)
        {
            GameObject go = Instantiate(obj, (transform.position + obj.transform.localPosition), obj.transform.rotation) as GameObject;
            go.transform.parent = transform;
            MyDevices.Add(key, go);
            manager.SendMessage("AddDevice", key);
        }
    }    
    #endregion
}