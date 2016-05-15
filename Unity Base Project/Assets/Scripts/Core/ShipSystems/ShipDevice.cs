using UnityEngine;

public class ShipDevice : MonoBehaviour
{
    public bool Activated { get; protected set; }
    public float Cooldown { get; protected set; }

    protected float maxCooldown;


    public ShipDevice()
    {
        Cooldown = 0f;
        maxCooldown = 0f;
        Activated = false;
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
}
   