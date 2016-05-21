using UnityEngine;

public class LaserSystem : ShipDevice
{
    private float reload;
    private GameObject Laser;
    private GameObject gun1;
    private GameObject gun2;
    // Use this for initialization
    void Start()
    {
        //Debug.Log("Initializing Lasers");
        reload = 0f;
        maxCooldown = 10f;
        Laser = Resources.Load<GameObject>("LaserBeam");
        gun1 = GameObject.Find("Gun1");
        gun2 = GameObject.Find("Gun2");
    }

    // Update is called once per frame
    void Update()
    {
        if (reload > 0f)
            reload -= Time.deltaTime;

        if(Input.GetKey(KeyCode.L) && Cooldown <= 0F)
            Activate();

        if (Cooldown > 0f)
            ShootGun();
    }

    public void ShootGun()
    {
        if (Laser == null)
            return;

        if (reload <= 0f)
        {
            GameObject go = Instantiate(Laser, gun1.transform.position, gun1.transform.rotation) as GameObject;
            go.transform.parent = transform.parent;        
            go = Instantiate(Laser, gun2.transform.position, gun2.transform.rotation) as GameObject;
            go.transform.parent = transform.parent;
            reload = 2f;
        }
    }
}