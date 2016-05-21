using UnityEngine;

public class EmpSystem : ShipDevice
{
    #region Properties
    private float empTimer;
    private GameObject shockwave;
    #endregion


    // Use this for initialization
    void Start()
    {
        //Debug.Log("Initializing Emp");
        empTimer = 0f;
        maxCooldown = 30f;
        shockwave = transform.GetChild(0).gameObject;
        shockwave.SetActive(Activated);
    }

    // Update is called once per frame
    void Update()
    {
        if (empTimer > 0f)
            empTimer -= Time.deltaTime;
        else
            shockwave.SetActive(false);

        if (Input.GetKey(KeyCode.E) && Cooldown <= 0F)
            Activate();

        if (Activated)
            ElectricMagneticPulse();               
    }


    #region Modifiers
    public void ElectricMagneticPulse()
    {
        empTimer = 5f;
        Activated = false;
        shockwave.SetActive(true);
        Debug.Log("Emp Activated");
    }
    #endregion
}