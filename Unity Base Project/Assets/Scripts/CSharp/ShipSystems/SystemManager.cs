using UnityEngine;
using GoingDark.Core.Enums;
using System.Collections.Generic;

public class SystemManager : MonoBehaviour {

    #region Properties
    private MessageScript messages;
    private Dictionary<SystemType, ShipSystem> MainDevices;
    private Dictionary<SystemType, GameObject> SecondaryDevices;

    private x360Controller controller;
    private CloakSystem cloaking;
    private bool messageUp = false;
    #endregion


    void Awake()
    {
        messages = GameObject.Find("PlayerCanvas").GetComponent<MessageScript>();

        MainDevices = new Dictionary<SystemType, ShipSystem>();
        SecondaryDevices = new Dictionary<SystemType, GameObject>();

        InitializeDevice(SystemType.Emp);
        InitializeDevice(SystemType.Cloak);
        InitializeDevice(SystemType.Decoy);
        InitializeDevice(SystemType.Laser);
        InitializeDevice(SystemType.Shield);
        InitializeDevice(SystemType.Missile);
        InitializeDevice(SystemType.Hyperdrive);

        controller = GamePadManager.Instance.GetController(0);
    }

    void LateUpdate()
    {
        if (controller.GetButtonDown("X"))
            ActivateSystem(SystemType.Cloak);

        if (controller.GetButtonDown("A") && !messageUp)
            ActivateSystem(SystemType.Emp);

        if (controller.GetButtonDown("B"))
            ActivateSystem(SystemType.Decoy);

        if (controller.GetButtonDown("RightBumper"))
            ActivateSystem(SystemType.Missile);

        if (controller.GetButtonDown("LeftBumper"))
            ActivateSystem(SystemType.Hyperdrive);

        if (controller.GetRightTrigger() > 0f)
            ActivateSystem(SystemType.Laser);        
    }

    #region Public Methods
    public void ActivateSystem(SystemType key)
    {
        if (key == SystemType.Cloak && cloaking != null && cloaking.GetCloaked())
        {
            cloaking.UnCloakShip();
            return;
        }

        if (MainDevices.ContainsKey(key))   //  If System is installed
        {
            if (MainDevices[key].GetSystemReady())  //  If system is online and not on cooldown
            {
                if (cloaking.GetCloaked())  //  If we are currently cloaked
                    cloaking.UnCloakShip(); //  Fuck that

                MainDevices[key].Activate();    //  Activate System                
            }
        }
    }


    public MonoBehaviour GetSystemScript(SystemType key)
    {
        ShipSystem sdev = null;
        if (MainDevices.TryGetValue(key, out sdev))
            return sdev;

        Debug.LogError("GetSystemScript did not find " + key.ToString());
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
        Debug.Log("Player : Full system repair");
        List<SystemType> keylist = new List<SystemType>(MainDevices.Keys);
        for (int i = 0; i < keylist.Count; i++)
            MainDevices[keylist[i]].Repair();                         
    }

    public int GetSystemCooldown(SystemType key)
    {
        ShipSystem dev = null;
        if (MainDevices.TryGetValue(key, out dev))
        {
            if (dev.Status == SystemStatus.Online)
                return (int)dev.GetCooldown();
            else
            {
                return -10;
            }
        }
        else
        {
            return -1;
        }
    }
    #endregion

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
                MainDevices.Add(key, dev.GetComponent<DecoySystem>() as ShipSystem);
                break;

            case SystemType.Emp:
                MainDevices.Add(key, dev.GetComponent<EmpSystem>() as ShipSystem);
                break;

            case SystemType.Hyperdrive:
                MainDevices.Add(key, dev.GetComponent<HyperdriveSystem>() as ShipSystem);
                break;

            case SystemType.Missile:
                MainDevices.Add(key, dev.GetComponent<MissileSystem>() as ShipSystem);
                break;

            case SystemType.Cloak:
                cloaking = dev.GetComponent<CloakSystem>();
                MainDevices.Add(key, cloaking as ShipSystem);
                break;

            case SystemType.Laser:
                MainDevices.Add(key, dev.GetComponent<LaserSystem>() as ShipSystem);
                break;
            #endregion

            #region Secondary Devices
            case SystemType.Shield:
                SecondaryDevices.Add(key, dev);
                break;
                #endregion
        }
    }
    private void MessageUp(bool up)
    {
        messageUp = up;
    }
    #endregion
}