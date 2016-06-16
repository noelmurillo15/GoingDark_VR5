using UnityEngine;
using GoingDark.Core.Enums;

public class LaserSystem : ShipDevice
{
    #region Properties
    private GameObject gun1;
    private GameObject gun2;
    private GameObject[] Basicburst;
    private GameObject[] Chargeburst;
    private float size = 0.05f;
    public LaserType currentType;
    private float buffer;

    private LaserOverheat laser_overheat;

    private x360Controller controller;
    #endregion


    // Use this for initialization
    void Start()
    {
        controller = GamePadManager.Instance.GetController(0);
        buffer = 0f;
        Basicburst = new GameObject[2];
        Chargeburst = new GameObject[2];
        maxCooldown = .25f;
        gun1 = GameObject.Find("Gun1");
        gun2 = GameObject.Find("Gun2");
        Basicburst[0] = gun1.transform.GetChild(0).gameObject;
        Basicburst[1] = gun2.transform.GetChild(0).gameObject;
        Chargeburst[0] = gun1.transform.GetChild(1).gameObject;
        Chargeburst[1] = gun2.transform.GetChild(1).gameObject;

        laser_overheat = GameObject.Find("LaserOverHeat").GetComponent<LaserOverheat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (buffer > 0f)
            buffer -= Time.deltaTime;

        if (!laser_overheat.GetOverheat())
        {         
            if (Input.GetAxisRaw("RTrigger") > 0f)
                Activate();

            if (Activated)
                ShootGun();
        }

        if (controller.GetButtonDown("RightThumbstick"))
            WeaponSwap();

    }

    public void ShootGun()
    {
        switch (currentType)
        {
            case LaserType.Basic:
                Basicburst[0].SetActive(true);
                Basicburst[1].SetActive(true);
                break;
            case LaserType.Charged:
                Chargeburst[0].SetActive(true);
                Chargeburst[1].SetActive(true);
                break;
            case LaserType.Ball:
                break;
            case LaserType.Continuous:
                break;
        }
        laser_overheat.UpdateGauge(-10f);
        DeActivate();
    }

    public void WeaponSelect(LaserType type)
    {
        currentType = type;
    }

    public void WeaponSwap()
    {
        if (buffer <= 0f)
        {
            buffer = .5f;
            int curr = (int)(currentType + 1);
            if (System.Enum.GetValues(typeof(LaserType)).Length == curr)
                curr = 0;

            currentType = (LaserType)curr;
            Debug.Log("Current Laser : " + curr.ToString());
        }
    }
}