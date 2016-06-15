using UnityEngine;
using GoingDark.Core.Enums;

public class LaserSystem : ShipDevice
{
    #region Properties
    private GameObject gun1;
    private GameObject gun2;
    private GameObject[] Basicburst;
    private GameObject[] Chargeburst;
    private GameObject EnergyBar;
    private float size = 0.05f;
    private float BarAmount;
    private bool overheat;
    private float OverheatTimer;
    public LaserType currentType;
    private float buffer;
    #endregion


    // Use this for initialization
    void Start()
    {
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
        overheat = false;
        BarAmount = 100.0f;
        EnergyBar = transform.GetChild(3).gameObject;
        UpdateEnergyGauge(0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (buffer > 0f)
            buffer -= Time.deltaTime;

        if (!overheat)
        {
            if(BarAmount < 100.0f)
                BarAmount += Time.fixedDeltaTime*5.0f;           

            if (Input.GetAxisRaw("RTrigger") > 0f)
                Activate();

            if (Activated)
                ShootGun();

            UpdateEnergyGauge(0.0f);
        }
        else
        {
            if(OverheatTimer>0.0f)
                OverheatTimer -= Time.fixedDeltaTime;
            else
            {
                BarAmount += Time.fixedDeltaTime*20.0f;
                UpdateEnergyGauge(0.0f);
                if(BarAmount>=100.0f)
                {
                    BarAmount = 100.0f;
                    overheat = false;
                }
            }
        }
    }

    public void ShootGun()
    {
        //burst1.SetActive(true);
        //burst2.SetActive(true);
        switch (currentType)
        {
            case LaserType.Basic:
                UpdateEnergyGauge(6f);
                Basicburst[0].SetActive(true);
                Basicburst[1].SetActive(true);
                break;
            case LaserType.Charged:
                UpdateEnergyGauge(14f);
                Chargeburst[0].SetActive(true);
                Chargeburst[1].SetActive(true);
                break;
            case LaserType.Ball:
                break;
            case LaserType.Continuous:
                break;
        }
        DeActivate();
    }

    public void UpdateEnergyGauge(float amount)
    {
        BarAmount -= amount;
        if(BarAmount<= 0.0f)
        {
            overheat = true;
            BarAmount = 0.0f;
            OverheatTimer = 5.0f;
        }
        Vector3 newScale;
        newScale = EnergyBar.transform.localScale;
        newScale.y = size * (BarAmount * 0.01f);
        EnergyBar.transform.localScale = newScale;
        Vector3 newPos = EnergyBar.transform.localPosition;
        float offset = ((BarAmount * 0.01f) * 0.00456f) - 0.00456f;
        newPos.x = offset;
        EnergyBar.transform.localPosition = newPos;
    }

    public void WeaponSelect(LaserType type)
    {
        currentType = type;
        //switch (currentType)
        //{
        //    case LaserType.Basic:
        //        break;
        //    case LaserType.Charged:
        //        break;
        //    case LaserType.Ball:
        //        break;
        //    case LaserType.Continuous:
        //        break;
        //}
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
            //switch (currentType)
            //{
            //    case LaserType.Basic:
            //        break;
            //    case LaserType.Charged:
            //        break;
            //    case LaserType.Ball:
            //        break;
            //    case LaserType.Continuous:
            //        break;
            //}
        }
    }
}