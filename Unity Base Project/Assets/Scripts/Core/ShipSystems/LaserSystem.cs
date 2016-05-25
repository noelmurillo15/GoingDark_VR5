using UnityEngine;

public class LaserSystem : ShipDevice
{
    #region Properties
    private GameObject gun1;
    private GameObject gun2;
    private GameObject Laser;
    #endregion


    // Use this for initialization
    void Start()
    {
        //Debug.Log("Initializing Lasers");
        maxCooldown = 1f;
        gun1 = GameObject.Find("Gun1");
        gun2 = GameObject.Find("Gun2");
        Laser = Resources.Load<GameObject>("ChargeBurst");
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
    }

    public void ShootGun()
    {
        GameObject go = Instantiate(Laser, gun1.transform.position, Quaternion.identity) as GameObject;
        go.transform.parent = transform;
        go = Instantiate(Laser, gun2.transform.position, Quaternion.identity) as GameObject;
        go.transform.parent = transform;
        Activated = false;
    }
}