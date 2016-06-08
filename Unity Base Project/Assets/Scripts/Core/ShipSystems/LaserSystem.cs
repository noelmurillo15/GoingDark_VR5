using UnityEngine;

public class LaserSystem : ShipDevice
{
    #region Properties
    private GameObject gun1;
    private GameObject gun2;
    private GameObject burst1;
    private GameObject burst2;
    #endregion


    // Use this for initialization
    void Start()
    {
        maxCooldown = 1f;
        gun1 = GameObject.Find("Gun1");
        gun2 = GameObject.Find("Gun2");
        burst1 = gun1.transform.GetChild(0).gameObject;
        burst2 = gun2.transform.GetChild(0).gameObject;        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.L))
            Activate();

        if (Input.GetAxisRaw("RTrigger") > 0f)
            Activate();

        if (Activated)
            ShootGun();
    }

    public void ShootGun()
    {
        burst1.SetActive(true);
        burst2.SetActive(true);
        DeActivate();
    }
}