using UnityEngine;
using GoingDark.Core.Enums;

//  Parent class of all Ship Systems
public class ShipSystem : MonoBehaviour {

    #region Properties
    public SystemStatus Status { get; protected set; }
    public bool SystemReady { get; protected set; }
    public bool Activated { get; protected set; }

    protected float cooldown;
    protected float maxCooldown;
    #endregion


    public ShipSystem()
    {
        Status = SystemStatus.Online;
        SystemReady = true;
        Activated = false;
        cooldown = 0f;
    }    

    #region Public Methods
    public float GetCooldown()
    {
        return cooldown;
    }
    public float GetMaxCooldown()
    {
        return maxCooldown;
    }
    public bool GetSystemReady()
    {
        if (Status == SystemStatus.Online)
            return SystemReady;

        return false;
    }
    
    public void SetStatus(SystemStatus stat)
    {
        Status = stat;
    }

    public void Repair()
    {
        cooldown = 0;
        Activated = false;
        SystemReady = true;
        Status = SystemStatus.Online;
    }
    public void Activate()
    {
        Activated = true;
        SystemReady = false;
    }
    public void DeActivate()
    {
        Activated = false;
        SystemReady = false;
        cooldown = maxCooldown;
        Invoke("SystemAvailable", maxCooldown);       
    }

    public void SystemAvailable()
    {
        SystemReady = true;
    }
    #endregion
}