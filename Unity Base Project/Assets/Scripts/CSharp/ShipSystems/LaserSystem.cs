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


    private bool flip;
    private Vector2 rumble;
    private x360Controller controller;
    private LaserOverheat laser_overheat;
    private ObjectPoolManager PoolManager;
    #endregion


    // Use this for initialization
    void Start()
    {
        flip = false;
        maxCooldown = .25f;
        Type = LaserType.Basic;
        rumble = new Vector2(.2f, .2f);
        laser_overheat = GetComponent<LaserOverheat>();
        controller = GamePadManager.Instance.GetController(0);
        PoolManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!laser_overheat.GetOverheat())
            if (Activated)
                ShootGun();
    }

    public void ShootGun()
    {
        DeActivate();
        
        GameObject laser = null;
        controller.AddRumble(maxCooldown - .05f, rumble);
        switch (Type)
        {
            case LaserType.Basic:
                laser_overheat.UpdateGauge(-10f);
                laser = PoolManager.GetBaseLaser();                
                break;

            case LaserType.Charged:
                laser_overheat.UpdateGauge(-20f);
                laser = PoolManager.GetChargedLaser();
                break;
        }

        if (flip)
        {
            laser.transform.position = Gun1.transform.position;
            laser.transform.rotation = Gun1.transform.rotation;
        }
        else
        {
            laser.transform.position = Gun2.transform.position;
            laser.transform.rotation = Gun2.transform.rotation;
        }
        flip = !flip;
        laser.SetActive(true);
    }

    public void WeaponSwap()
    {
        int curr = (int)(Type + 1);
        if (curr == (int)LaserType.NumberOfType)
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
        }
    }
}