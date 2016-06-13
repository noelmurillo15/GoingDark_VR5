using UnityEngine;
using GoingDark.Core.Enums;
using System.Collections.Generic;

public class SystemManager : MonoBehaviour {

    #region Properties
    public Dictionary<SystemType, ShipDevice> MainDevices;
    public Dictionary<SystemType, GameObject> SecondaryDevices;

    private GameObject MissionLog;
    private MessageScript messages;
    #endregion


    void Awake()
    {
        MainDevices = new Dictionary<SystemType, ShipDevice>();
        SecondaryDevices = new Dictionary<SystemType, GameObject>();

        // Main Systems        
        InitializeDevice(SystemType.Emp);
        InitializeDevice(SystemType.Cloak);
        InitializeDevice(SystemType.Decoy);
        InitializeDevice(SystemType.Laser);
        InitializeDevice(SystemType.Missile);
        InitializeDevice(SystemType.Hyperdrive);

        // Secondary Systems        
        InitializeDevice(SystemType.Shield);

        // References
        MissionLog = GameObject.Find("MissionLog");
        messages = GameObject.Find("WarningMessages").GetComponent<MessageScript>();
    }

    #region Private Methods
    private void InitializeDevice(SystemType key)
    {
        if (MainDevices.ContainsKey(key) || SecondaryDevices.ContainsKey(key))
        {
            Debug.Log("Device is already initialized : " + key.ToString());
            return;
        }

        GameObject system = null;
        switch (key)
        {
            case SystemType.Emp:
                system = Resources.Load<GameObject>("Devices/Emp");
                break;
            case SystemType.Cloak:
                system = Resources.Load<GameObject>("Devices/Cloak");
                break;
            case SystemType.Decoy:
                system = Resources.Load<GameObject>("Devices/Decoy");
                break;
            case SystemType.Laser:
                system = Resources.Load<GameObject>("Devices/Lasers");
                break;
            case SystemType.Shield:
                system = Resources.Load<GameObject>("Devices/Shield");
                break;
            case SystemType.Missile:
                system = Resources.Load<GameObject>("Devices/Missiles");
                break;
            case SystemType.Hyperdrive:
                system = Resources.Load<GameObject>("Devices/HyperDrive");
                break;
        }

        if (system != null)
        {
            GameObject go = Instantiate(system, Vector3.zero, Quaternion.identity) as GameObject;
            go.transform.parent = transform;
            go.transform.localPosition = system.transform.localPosition;
            go.transform.localRotation = system.transform.localRotation;
            AddDevice(key, go);
            return;
        }
        Debug.Log(key.ToString() + " Device was not Initialized");
    }
    private void AddDevice(SystemType key, GameObject dev)
    {
        switch (key)
        {
            #region Main Devices
            case SystemType.Decoy:
                
                MainDevices.Add(key, dev.GetComponent<DecoySystem>() as ShipDevice);
                MainDevices[key].SetStatus(SystemStatus.Online); break;

            case SystemType.Emp:
                MainDevices.Add(key, dev.GetComponent<EmpSystem>() as ShipDevice);
                MainDevices[key].SetStatus(SystemStatus.Online); break;

            case SystemType.Hyperdrive:
                MainDevices.Add(key, dev.GetComponent<HyperdriveSystem>() as ShipDevice);
                MainDevices[key].SetStatus(SystemStatus.Online); break;

            case SystemType.Missile:
                MainDevices.Add(key, dev.GetComponent<MissileSystem>() as ShipDevice);
                MainDevices[key].SetStatus(SystemStatus.Online); break;

            case SystemType.Cloak:
                MainDevices.Add(key, dev.GetComponent<CloakSystem>() as ShipDevice);
                MainDevices[key].SetStatus(SystemStatus.Online); break;

            case SystemType.Laser:
                MainDevices.Add(key, dev.GetComponent<LaserSystem>() as ShipDevice);
                MainDevices[key].SetStatus(SystemStatus.Online); break;
            #endregion

            #region Secondary Devices
            case SystemType.Shield:
                SecondaryDevices.Add(key, dev);
                break;            
                #endregion
        }        
    }
    #endregion

    #region Public Methods
    public void ActivateSystem(SystemType key)
    {
        if (MainDevices.ContainsKey(key))
            MainDevices[key].Activate();
    }

    public bool GetActive(SystemType key)
    {
        if (MainDevices.ContainsKey(key))
            return MainDevices[key].Activated;

        return false;
    }

    public GameObject GetSystem(SystemType key)
    {
        if (MainDevices.ContainsKey(key))
            return MainDevices[key].gameObject;

        return null;
    }

    public void SystemDamaged()
    {
        List<SystemType> keylist = new List<SystemType>(MainDevices.Keys);
        int rand = Random.Range(0, keylist.Count);
        SystemType type = keylist[rand];
        if (MainDevices.ContainsKey(type))
        {
            MainDevices[type].SetStatus(SystemStatus.Offline);
            messages.SendMessage("SystemReport", type.ToString());
        }
    }
    public void FullSystemRepair()
    {
        List<SystemType> keylist = new List<SystemType>(MainDevices.Keys);
        for (int i = 0; i < keylist.Count; i++)
        {
            MainDevices[keylist[i]].Repair();
        }                    
    }

    public void ToggleSystem(SystemType key)
    {
        if (SecondaryDevices.ContainsKey(key))
            SecondaryDevices[key].SetActive(!SecondaryDevices[key].activeSelf);
    }

    public bool GetSystemStatus(SystemType key)
    {
        if (MainDevices.ContainsKey(key))
            if (MainDevices[key].Status == SystemStatus.Online)
                return true;        

        return false;
    }

    public int GetSystemCooldown(SystemType key)
    {
        if (MainDevices.ContainsKey(key))
            return (int)MainDevices[key].GetCooldown();

        return 0;
    }
    public void ToggleMissionLog()
    {
        MissionLog.SendMessage("TogglePanel");
    }
    #endregion
}