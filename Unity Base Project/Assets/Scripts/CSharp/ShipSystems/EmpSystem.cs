using UnityEngine;

public class EmpSystem : ShipSystem
{
    #region Properties
    private GameObject shockwave;
    #endregion


    // Use this for initialization
    void Start()
    {
        maxCooldown = 30f;
        shockwave = transform.GetChild(0).gameObject;
        shockwave.SetActive(Activated);
    }

    // Update is called once per frame
    void Update()
    {         
        if (Activated)
            ElectricMagneticPulse();

        if (cooldown > 0f)
            cooldown -= Time.deltaTime;
    }

    #region Modifiers
    public void ElectricMagneticPulse()
    {
        DeActivate();
        shockwave.SetActive(true);
        Invoke("ResetEmp", 1.5f);
    }

    void ResetEmp()
    {
        shockwave.SetActive(false);
    }
    #endregion
}