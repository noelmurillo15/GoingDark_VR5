using UnityEngine;
using GD.Core.Enums;
using System.Collections.Generic;

public class ShipSystems : MonoBehaviour
{

    #region Properties
    public Dictionary<SystemType, ShipDevice> MyDevices;

    // Constant Devices
    public GameObject MissionLog { get; private set; }
    public GameObject WeaponSelect { get; private set; }
    #endregion


    // Use this for initialization
    void Start()
    {
        MyDevices = new Dictionary<SystemType, ShipDevice>();

        // Initialize once you collect the device (?)
        InitializeDevice(SystemType.EMP);
        InitializeDevice(SystemType.CLOAK);
        InitializeDevice(SystemType.RADAR);
        InitializeDevice(SystemType.DECOY);
        InitializeDevice(SystemType.LASERS);
        InitializeDevice(SystemType.SHIELD);
        InitializeDevice(SystemType.MISSILES);
        InitializeDevice(SystemType.HYPERDRIVE);

        // Constant Devices
        MissionLog = GameObject.Find("ButtonObject");
        WeaponSelect = GameObject.Find("WeaponButtonObj");
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Accessors
    public SystemStatus GetDeviceStatus(SystemType key)
    {
        if (MyDevices.ContainsKey(key))
            return MyDevices[key].Status;
        else
            return SystemStatus.NOTAVAILABLE;
    }
    public Component AccessSystemData(SystemType key)
    {
        if (MyDevices.ContainsKey(key))
            return MyDevices[key].SystemData;

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
        ShipDevice dev = new ShipDevice();
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
            dev.Object = go;
            dev.Status = SystemStatus.OFFLINE;
            MyDevices.Add(key, dev);
            RetrieveSystemData(key);
        }
    }    

    public void ChangeDeviceStatus(SystemType key, SystemStatus stat)
    {
        if (!MyDevices.ContainsKey(key))
        {
            Debug.Log("Device is not initialized : " + key.ToString());
            return;
        }

        ShipDevice m_dev = MyDevices[key];
        m_dev.Status = stat;
        MyDevices[key] = m_dev;
    }

    public void ToggleDeviceStatus(SystemType key)
    {
        if (!MyDevices.ContainsKey(key))
        {
            Debug.Log("Device is not initialized : " + key.ToString());
            return;
        }

        ShipDevice m_dev = MyDevices[key];
        if (m_dev.Status == SystemStatus.OFFLINE)
            m_dev.Status = SystemStatus.ONLINE;
        else
            m_dev.Status = SystemStatus.OFFLINE;
        MyDevices[key] = m_dev;
    }    

    private void RetrieveSystemData(SystemType key)
    {
        if (MyDevices.ContainsKey(key))
        {
            ShipDevice m_dev = MyDevices[key];
            switch (key)
            {
                case SystemType.EMP:
                    m_dev.SystemData = m_dev.Object.GetComponent<EmpSystem>();
                    break;
                case SystemType.CLOAK:
                    m_dev.SystemData = m_dev.Object.GetComponent<CloakSystem>();
                    break;
                case SystemType.DECOY:
                    m_dev.SystemData = m_dev.Object.GetComponent<DecoySystem>();
                    break;
                case SystemType.HYPERDRIVE:
                    m_dev.SystemData = m_dev.Object.GetComponent<HyperdriveSystem>();
                    break;
                case SystemType.MISSILES:
                    m_dev.SystemData = m_dev.Object.GetComponent<MissileSystem>();
                    break;
            }
            m_dev.Status = SystemStatus.ONLINE;
            MyDevices[key] = m_dev;
        }
    }
    #endregion
}