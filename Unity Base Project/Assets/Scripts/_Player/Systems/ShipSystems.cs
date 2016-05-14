using UnityEngine;
using GD.Core.Enums;
using System.Collections.Generic;

public class ShipSystems : MonoBehaviour
{

    #region Properties
    public Dictionary<SystemType, ShipDevice> MyDevices;

    // Pick-up Devices
    public EMP Emp { get; private set; }
    public Cloak Cloak { get; private set; }
    public Decoy Decoy { get; private set; }
    public MissileSystem Missiles { get; private set; }
    public HyperDrive HyperDrive { get; private set; }

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

    void InitializeDevice(SystemType key)
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

            RetrieveData(key);

            dev.Object = go;
            dev.Status = SystemStatus.ONLINE;
            MyDevices.Add(key, dev);
            Debug.Log(key.ToString() + " : " + dev.Status.ToString());
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

        if (stat == SystemStatus.OFFLINE)
            m_dev.Object.SetActive(false);
        else
            m_dev.Object.SetActive(true);

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
        {
            m_dev.Status = SystemStatus.ONLINE;
            m_dev.Object.SetActive(true);
        }
        else
        {
            m_dev.Status = SystemStatus.OFFLINE;
            m_dev.Object.SetActive(false);
        }
        MyDevices[key] = m_dev;
    }

    public SystemStatus GetDeviceStatus(SystemType key)
    {
        if (MyDevices.ContainsKey(key))
            return MyDevices[key].Status;
        else
            return SystemStatus.NOTAVAILABLE;
    }

    void RetrieveData(SystemType key)
    {
        switch (key)
        {
            case SystemType.EMP:
                Emp = GetComponentInChildren<EMP>();
                break;
            case SystemType.CLOAK:
                Cloak = GetComponentInChildren<Cloak>();
                break;
            case SystemType.DECOY:
                Decoy = GetComponentInChildren<Decoy>();
                break;
            case SystemType.HYPERDRIVE:
                HyperDrive = GetComponentInChildren<HyperDrive>();
                break;
            case SystemType.MISSILES:
                Missiles = GetComponentInChildren<MissileSystem>();
                break;
        }
    }
}