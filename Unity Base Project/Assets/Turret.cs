using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
    private float cooldown;
    private GameObject Laser;
    private GameObject gun;
    // Use this for initialization
    void Start()
    {
        cooldown = 0.0f;
        Laser = Resources.Load<GameObject>("LaserBeam");
        gun = GameObject.Find("machinegun");
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0.0f)
            cooldown -= Time.deltaTime;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            Debug.Log("KillPlayer.exe");
            transform.LookAt(col.transform.position);
            Shoot();
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("KillPlayer.exe");
            transform.LookAt(col.transform.position);
            Shoot();
        }
    }

    public void Shoot()
    {
        if (cooldown <= 0.0f)
        {
            cooldown = 0.5f;
            if (Laser != null)
            {
                Instantiate(Laser, transform.position, transform.rotation);
            }
        }
    }
}
