using UnityEngine;
using System.Collections;

public class GunObject : MonoBehaviour
{
    private float cooldown;
    private GameObject Laser;
    private GameObject gun1;
    private GameObject gun2;
    // Use this for initialization
    void Start()
    {
        cooldown = 0.0f;
        Laser = Resources.Load<GameObject>("LaserBeam");
        gun1 = GameObject.Find("Gun1");
        gun2 = GameObject.Find("Gun2");
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0.0f)
            cooldown -= Time.deltaTime;

        if (Input.GetKey(KeyCode.H) || Input.GetKeyDown(KeyCode.H))
            ShootGun();
    }

    public void ShootGun()
    {
        if(cooldown <= 0.0f)
        {
            cooldown = 0.25f;
            if(Laser != null)
            {
                Instantiate(Laser, gun1.transform.position, gun1.transform.rotation);
                Instantiate(Laser, gun2.transform.position, gun2.transform.rotation);
            }
        }
    }
}
