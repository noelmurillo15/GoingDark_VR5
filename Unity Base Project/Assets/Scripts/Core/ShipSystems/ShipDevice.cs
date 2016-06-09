using UnityEngine;
using GoingDark.Core.Enums;

//  Parent class of all Ship Systems
public class ShipDevice : MonoBehaviour {

    #region Properties
    public SystemStatus Status { get; protected set; }
    public bool Activated { get; protected set; }
    public bool Cooldown { get; protected set; }

    protected float maxCooldown;
    protected float cooldown;
    #endregion


    public ShipDevice()
    {
        maxCooldown = 0f;
        cooldown = 0f;
        Cooldown = false;
        Activated = false;
        Status = SystemStatus.OFFLINE;
    }

    void FixedUpdate()
    {
        if (Cooldown)
        {
            if (cooldown > 0f)
                cooldown -= Time.fixedDeltaTime;
            else
                ResetCooldown();
        }
    }

    public void SetStatus(SystemStatus stat)
    {
        Status = stat;
    }

    public void Repair()
    {
        Cooldown = false;
        Activated = false;
        cooldown = maxCooldown;
        Status = SystemStatus.ONLINE;
    }

    public float GetCooldown()
    {
        return cooldown;
    }
    void ResetCooldown()
    {
        cooldown = 0f;
        Cooldown = false;
    }
   
    public void Activate()
    {
        if (Status == SystemStatus.ONLINE && Cooldown == false)
        {
            cooldown = maxCooldown;
            Activated = true;
        }
    }
    public void DeActivate()
    {
        Cooldown = true;
        Activated = false;        
    }
}