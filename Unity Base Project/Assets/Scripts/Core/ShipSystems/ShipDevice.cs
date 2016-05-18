using UnityEngine;
using GD.Core.Enums;

//  Parent class of all Ship Systems
public class ShipDevice : MonoBehaviour {

    #region Properties
    public SystemStatus Status { get; protected set; }
    public bool Activated { get; protected set; }
    public float Cooldown;

    // When activated Cooldown Gets set to this
    protected float maxCooldown;    
    #endregion


    public ShipDevice()
    {
        Cooldown = 0f;
        maxCooldown = 0f;
        Activated = false;
        Status = SystemStatus.OFFLINE;
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

    public void SetStatus(SystemStatus stat)
    {
        Status = stat;
    }
}
   