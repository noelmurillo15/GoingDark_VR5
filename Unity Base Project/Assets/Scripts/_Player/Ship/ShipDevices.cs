using UnityEngine;
using GD.Core.Enums;
using System.Collections.Generic;

public class ShipDevices : MonoBehaviour
{

    #region Properties
    public Dictionary<Devices, Device> MyDevices;

    // Pick-up Devices
    public EMP Emp { get; private set; }
    public Cloak Cloak { get; private set; }
    public Decoy Decoy { get; private set; }
    public ShootObject Missiles { get; private set; }
    public HyperDrive HyperDrive { get; private set; }

    // Constant Devices
    public GameObject MissionLog { get; private set; }
    public GameObject WeaponSelect { get; private set; }
    #endregion


    // Use this for initialization
    void Start()
    {
        MyDevices = new Dictionary<Devices, Device>();

        // Initialize once you collect the device (?)
        InitializeDevice(Devices.EMP);
        InitializeDevice(Devices.CLOAK);
        InitializeDevice(Devices.RADAR);
        InitializeDevice(Devices.DECOY);
        InitializeDevice(Devices.LASERS);
        InitializeDevice(Devices.MISSILES);
        InitializeDevice(Devices.HYPERDRIVE);

        // Constant Devices
        MissionLog = GameObject.Find("ButtonObject");
        WeaponSelect = GameObject.Find("WeaponButtonObj");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitializeDevice(Devices type)
    {
        if (MyDevices.ContainsKey(type))
        {
            Debug.Log("Device is already initialized : " + type.ToString());
            return;
        }

        GameObject obj = null;
        Device dev = new Device();
        switch (type)
        {
            case Devices.EMP:
                obj = Resources.Load<GameObject>("Devices/Emp");
                break;
            case Devices.CLOAK:
                obj = Resources.Load<GameObject>("Devices/Cloak");
                break;
            case Devices.RADAR:
                obj = Resources.Load<GameObject>("Devices/Radar");
                break;
            case Devices.DECOY:
                obj = Resources.Load<GameObject>("Devices/Decoy");
                break;
            case Devices.LASERS:
                obj = Resources.Load<GameObject>("Devices/Lasers");
                break;
            case Devices.MISSILES:
                obj = Resources.Load<GameObject>("Devices/Missiles");
                break;
            case Devices.HYPERDRIVE:
                obj = Resources.Load<GameObject>("Devices/HyperDrive");
                break;
        }

        if (obj != null)
        {
            GameObject go = Instantiate(obj, (transform.position + obj.transform.localPosition), obj.transform.rotation) as GameObject;
            go.transform.parent = transform;

            RetrieveData(type);

            dev.Object = go;
            dev.Status = DeviceStatus.ONLINE;
            MyDevices.Add(type, dev);
            Debug.Log(type.ToString() + " : " + dev.Status.ToString());
        }
    }

    public void ChangeDeviceStatus(Devices dev, DeviceStatus stat)
    {
        if (!MyDevices.ContainsKey(dev))
        {
            Debug.Log("Device is not initialized : " + dev.ToString());
            return;
        }

        Device m_dev = MyDevices[dev];

        if (stat == DeviceStatus.OFFLINE)
            m_dev.Object.SetActive(false);
        else
            m_dev.Object.SetActive(true);

        m_dev.Status = stat;
        MyDevices[dev] = m_dev;
    }

    public void ToggleDeviceStatus(Devices dev)
    {
        if (!MyDevices.ContainsKey(dev))
        {
            Debug.Log("Device is not initialized : " + dev.ToString());
            return;
        }

        Device m_dev = MyDevices[dev];
        if (m_dev.Status == DeviceStatus.OFFLINE)
        {
            m_dev.Status = DeviceStatus.ONLINE;
            m_dev.Object.SetActive(true);
        }
        else
        {
            m_dev.Status = DeviceStatus.OFFLINE;
            m_dev.Object.SetActive(false);
        }
        MyDevices[dev] = m_dev;
    }

    public DeviceStatus GetDeviceStatus(Devices dev)
    {
        if (MyDevices.ContainsKey(dev))
            return MyDevices[dev].Status;
        else
            return DeviceStatus.NOTAVAILABLE;
    }

    void RetrieveData(Devices type)
    {
        switch (type)
        {
            case Devices.EMP:
                Emp = GameObject.Find("Emp(Clone)").GetComponent<EMP>();
                break;
            case Devices.CLOAK:
                Cloak = GameObject.Find("Cloak(Clone)").GetComponent<Cloak>();
                break;
            case Devices.DECOY:
                Decoy = GameObject.Find("Decoy(Clone)").GetComponent<Decoy>();
                break;
            case Devices.HYPERDRIVE:
                HyperDrive = GameObject.Find("HyperDrive(Clone)").GetComponent<HyperDrive>();
                break;
            case Devices.MISSILES:
                Missiles = GameObject.Find("Missiles(Clone)").GetComponent<ShootObject>();
                break;
        }
    }
}