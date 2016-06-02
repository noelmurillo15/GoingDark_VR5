using UnityEngine;

public class Turret : MonoBehaviour
{
    //  Enemy Data
    private EnemyBehavior behavior;

    private GameObject Laser;
    private bool lockedOn;
    private float angle;
    private float randomShot;

    bool playerdetected = false;
    // Use this for initialization
    void Start()
    {
        randomShot = 1;
        lockedOn = false;
        Laser = Resources.Load<GameObject>("EnemyLaser");
        behavior = transform.parent.parent.GetComponent<EnemyBehavior>();

        InvokeRepeating("DestroyPlayer", 10f, randomShot);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (behavior.Target != null)
        {
            transform.LookAt(behavior.Target);
        }
    }
    void DestroyPlayer()
    {
        if (behavior.Target != null)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (Laser != null)
            Instantiate(Laser, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
    }
}
