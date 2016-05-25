using UnityEngine;

public class LaserSystem : ShipDevice
{
    #region Properties
    private GameObject gun1;
    private GameObject gun2;
    private GameObject Laser;
    private GameObject Environment;

    private Transform Gun1Tranform;
    private Transform Gun2Tranform;
    private Transform leapcam;
    #endregion


    // Use this for initialization
    void Start()
    {
        //Debug.Log("Initializing Lasers");
        maxCooldown = 1f;
        gun1 = GameObject.Find("Gun1");
        gun2 = GameObject.Find("Gun2");
        Gun1Tranform = gun1.transform;
        Gun2Tranform = gun2.transform;
        leapcam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        Environment = GameObject.Find("Environment");
        Laser = Resources.Load<GameObject>("LaserBeam");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.L) && Cooldown <= 0F)
            Activate();

        if (Input.GetAxisRaw("RTrigger") > 0f && Cooldown <= 0F)
            Activate();

        if (Activated)
            ShootGun();

        Gun1Tranform.rotation = leapcam.rotation;
        Gun2Tranform.rotation = leapcam.rotation;
    }

    public void ShootGun()
    {
        GameObject go = Instantiate(Laser, gun1.transform.position, gun1.transform.rotation) as GameObject;
        go.transform.parent = Environment.transform;
        go = Instantiate(Laser, gun2.transform.position, gun2.transform.rotation) as GameObject;
        go.transform.parent = Environment.transform;
        Activated = false;
    }
}