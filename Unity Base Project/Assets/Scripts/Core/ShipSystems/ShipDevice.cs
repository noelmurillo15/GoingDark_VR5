using UnityEngine;
using GD.Core.Enums;

//  Parent class of all Ship Systems
public class ShipDevice : MonoBehaviour {

    #region Properties
    public SystemStatus Status { get; protected set; }
    public bool Activated { get; protected set; }
    public bool Cooldown { get; protected set; }

    // How Long Does the Cooldown Last
    protected float maxCooldown;
    #endregion


    public ShipDevice()
    {
        maxCooldown = 0f;
        Cooldown = false;
        Activated = false;
        Status = SystemStatus.OFFLINE;
    }

    public void SetStatus(SystemStatus stat)
    {
        Status = stat;
    }

    public void Activate()
    {
        if (Status == SystemStatus.ONLINE && Cooldown == false)
            Activated = true;        
    }

    protected void DeActivate()
    {
        Cooldown = true;
        Activated = false;
        Invoke("ResetCooldown", maxCooldown);
    }

    public float GetCooldown()
    {
        return maxCooldown;
    }

    void ResetCooldown()
    {
        Cooldown = false;
    }    
}
   