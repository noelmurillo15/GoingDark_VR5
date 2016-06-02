using UnityEngine;

public class Turret : MonoBehaviour
{
    //  Enemy Data
    private EnemyBehavior behavior;

    private GameObject Laser;
    private bool lockedOn;
    private float angle;
    private float randomShot;

    Transform MyTransform;

    bool playerdetected = false;
    // Use this for initialization
    void Start()
    {
        randomShot = .5f;
        lockedOn = false;
        Laser = Resources.Load<GameObject>("EnemyLaser");
        behavior = transform.parent.parent.GetComponent<EnemyBehavior>();

        MyTransform = transform;

        InvokeRepeating("DestroyPlayer", 10f, randomShot);
    }

    private void LockOn()
    {
        Vector3 playerDir = behavior.Target.position - MyTransform.position;
        Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.deltaTime * 5f, 0.0f);
        MyTransform.rotation = Quaternion.LookRotation(direction);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (behavior.Target != null)
            LockOn();
    }
    void DestroyPlayer()
    {
        if (behavior.Target != null)
            Shoot();
    }

    public void Shoot()
    {
        if (Laser != null)
            Instantiate(Laser, new Vector3(MyTransform.position.x, MyTransform.position.y, MyTransform.position.z), MyTransform.rotation);
    }
}
