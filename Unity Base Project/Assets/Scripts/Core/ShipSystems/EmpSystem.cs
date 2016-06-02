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
        maxCooldown = 10f;
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

        if (Input.GetKey(KeyCode.E))
            Activate();

        if (Input.GetButtonDown("A"))
            Activate();

        if (Activated)
            ElectricMagneticPulse();
    }


    #region Modifiers
    public void ElectricMagneticPulse()
    {
        empTimer = 4.5f;
        DeActivate();
        shockwave.SetActive(true);
    }
    #endregion
}