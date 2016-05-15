using UnityEngine;
using GD.Core.Enums;

//  Parent class of all Ship Systems
public class ShipDevice : MonoBehaviour {

    #region Properties
    public DeviceStatus Status { get; set; }
    public bool Activated { get; protected set; }
    public float Cooldown { get; protected set; }

    protected float maxCooldown;
    #endregion


    public ShipDevice()
    {
        Cooldown = 0f;
        maxCooldown = 0f;
        Activated = false;
        Status = DeviceStatus.OFFLINE;
    }

    public void UpdateCooldown()
    {
        if (Activated)
        {
            if(Cooldown > 0f)
                Cooldown -= Time.deltaTime;
            else
            {
                Activated = false;
                Cooldown = 0f;
            }                
        }
    }
    public void Activate()
    {
        if (Cooldown == 0f)
        {
            Activated = true;
            Cooldown = maxCooldown;
        }
    }

    public void SetStatus(DeviceStatus stat)
    {
        Status = stat;
    }
}
   