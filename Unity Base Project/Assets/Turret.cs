using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
    private float cooldown;

    private Transform target;
    private GameObject Laser;
    // Use this for initialization
    void Start()
    {
        target = null;
        cooldown = 0.0f;
        Laser = Resources.Load<GameObject>("LaserBeam");
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0.0f)
            cooldown -= Time.deltaTime;

        if(target != null)
        {
            transform.LookAt(target.position);
            Shoot();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            Debug.Log("Initializing KillPlayer.exe");
            target = col.transform;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("AsteroidTurret Lost Target");
            target = null;
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
