using UnityEngine;
using GD.Core.Enums;
using UnityEngine.SceneManagement;

public class SystemsManager : MonoBehaviour
{

    private float padding;
    private ShipSystems Systems;

    private EmpSystem emp;
    private CloakSystem cloak;
    private DecoySystem decoy;
    private MissileSystem missiles;
    private HyperdriveSystem hypedrive;


    // Use this for initialization
    void Start()
    {
        Systems = GameObject.FindGameObjectWithTag("Systems").GetComponent<ShipSystems>();
    }

    // Update is called once per frame
    void Update()
    {
        if (padding > 0f)
            padding -= Time.deltaTime;
    }

    public void ActivateSystem(SystemType type)
    {
        if (padding <= 0f)
        {
            switch (type)
            {
                case SystemType.EMP:
                    ActivateEmp();
                    break;
                case SystemType.CLOAK:
                    ActivateCloak();
                    break;
                case SystemType.DECOY:
                    ActivateDecoy();
                    break;
                case SystemType.MISSILES:
                    LaunchMissile();
                    break;
                case SystemType.HYPERDRIVE:
                    ActivateHyperDrive();
                    break;


                default:
                    break;
            }
            padding = .2f;
        }
    }


    private void LaunchMissile()
    {
        if (Systems.GetDeviceStatus(SystemType.MISSILES) != SystemStatus.ONLINE)
        {
            Debug.Log("Missile System Offline");
            return;
        }
        if (missiles == null)
        {
            Debug.Log("Initializing Missile System");
            missiles = Systems.AccessSystemData(SystemType.MISSILES) as MissileSystem;
        }
        Debug.Log("Firing Missile");
        missiles.Activate();
    }

    private void ActivateEmp()
    {
        if (Systems.GetDeviceStatus(SystemType.EMP) != SystemStatus.ONLINE)
        {
            Debug.Log("Emp System Offline");
            return;
        }
        if (emp == null)
        {
            Debug.Log("Initializing Emp System");
            emp = Systems.AccessSystemData(SystemType.EMP) as EmpSystem;
        }
        Debug.Log("Activating Emp");
        emp.Activate(true);
    }

    private void ActivateDecoy()
    {
        if (Systems.GetDeviceStatus(SystemType.DECOY) != SystemStatus.ONLINE)
        {
            Debug.Log("Decoy System Offline");
            return;
        }
        if (decoy == null)
        {
            Debug.Log("Initializing Decoy System");
            decoy = Systems.AccessSystemData(SystemType.DECOY) as DecoySystem;
        }
        Debug.Log("Activating Decoy");
        decoy.Activate();
    }
    private void ActivateCloak()
    {
        if (Systems.GetDeviceStatus(SystemType.CLOAK) != SystemStatus.ONLINE)
        {
            Debug.Log("Cloak System Offline");
            return;
        }
        if (cloak == null)
        {
            Debug.Log("Initializing Cloak System");
            cloak = Systems.AccessSystemData(SystemType.CLOAK) as CloakSystem;
        }
        Debug.Log("Activating Cloak");
        cloak.Activate(true);
    }

    private void ActivateHyperDrive()
    {
        if (Systems.GetDeviceStatus(SystemType.HYPERDRIVE) != SystemStatus.ONLINE)
        {
            Debug.Log("Hyperdrive System Offline");
            return;
        }
        if (hypedrive == null)
        {
            Debug.Log("Initializing Hyperdrive System");
            hypedrive = Systems.AccessSystemData(SystemType.HYPERDRIVE) as HyperdriveSystem;
        }
        Debug.Log("Activating Hyperdrive");
        hypedrive.Activate();
    }
}