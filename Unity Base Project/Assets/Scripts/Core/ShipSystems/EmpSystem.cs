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


        if (Cooldown == maxCooldown)
        {
            Debug.Log("Emp has been activated");
            ElectricMagneticPulse();
        }

        UpdateCooldown();
    }


    #region Modifiers
    public void ElectricMagneticPulse()
    {
        empTimer = 5f;
        shockwave.SetActive(true);
    }
    #endregion
}