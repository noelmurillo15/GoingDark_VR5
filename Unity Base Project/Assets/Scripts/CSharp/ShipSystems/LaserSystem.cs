using UnityEngine;
using GoingDark.Core.Enums;

public class LaserSystem : ShipSystem
{
    #region Properties
    [SerializeField]
    public LaserType Type;
    [SerializeField]
    public GameObject Gun1;
    [SerializeField]
    public GameObject Gun2;

    private x360Controller controller;
    private LaserOverheat laser_overheat;
    private ObjectPoolManager PoolManager;
    #endregion


    // Use this for initialization
    void Start()
    {
        maxCooldown = .25f;
        Type = LaserType.Basic;
        controller = GamePadManager.Instance.GetController(0);
        laser_overheat = GameObject.Find("LaserOverHeat").GetComponent<LaserOverheat>();
        PoolManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!laser_overheat.GetOverheat())
            if (Activated)
                ShootGun();
                       
        if (controller.GetButtonDown("RightThumbstick"))
            WeaponSwap();
    }

    public void ShootGun()
    {
        DeActivate();
        
        GameObject obj1 = null;
        GameObject obj2 = null;
        switch (Type)
        {
            case LaserType.Basic:
                obj1 = PoolManager.GetBaseLaser();
                obj1.transform.position = Gun1.transform.position;
                obj1.transform.rotation = Gun1.transform.rotation;
                obj1.SetActive(true);

                obj2 = PoolManager.GetBaseLaser();
                obj2.transform.position = Gun2.transform.position;
                obj2.transform.rotation = Gun2.transform.rotation;
                obj2.SetActive(true);

                //laser_overheat.UpdateGauge(-5f);
                break;
            case LaserType.Charged:
                obj1 = PoolManager.GetChargedLaser();
                obj1.transform.position = Gun1.transform.position;
                obj1.transform.rotation = Gun1.transform.rotation;
                obj1.SetActive(true);

                obj2 = PoolManager.GetChargedLaser();
                obj2.transform.position = Gun2.transform.position;
                obj2.transform.rotation = Gun2.transform.rotation;
                obj2.SetActive(true);

                //laser_overheat.UpdateGauge(-10f);
                break;
            case LaserType.Ball:
                obj1 = PoolManager.GetChargedBall();
                obj1.transform.position = Gun1.transform.position;
                obj1.transform.rotation = Gun1.transform.rotation;
                obj1.SetActive(true);

                obj2 = PoolManager.GetChargedBall();
                obj2.transform.position = Gun2.transform.position;
                obj2.transform.rotation = Gun2.transform.rotation;
                obj2.SetActive(true);

                //laser_overheat.UpdateGauge(-20f);
                break;
            case LaserType.Continuous:
                //laser_overheat.UpdateGauge(-25f);
                break;
        }
    }

    public void WeaponSwap()
    {
        int curr = (int)(Type + 1);
        if (System.Enum.GetValues(typeof(LaserType)).Length == curr)
            curr = 0;

        Type = (LaserType)curr;
        switch (Type)
        {
            case LaserType.Basic:
                maxCooldown = .25f;
                break;
            case LaserType.Charged:
                maxCooldown = .5f;
                break;
            case LaserType.Ball:
                maxCooldown = 1f;
                break;
            case LaserType.Continuous:
                maxCooldown = 30f;
                break;
        }
    }
}