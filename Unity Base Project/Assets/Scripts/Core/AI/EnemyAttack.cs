using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    //  Missile Data
    private float angle;
    private bool lockedOn;
    private float missileCooldown;
    public GameObject missilePrefab;

    private EnemyBehavior behavior;


    // Use this for initialization
    void Start()
    {
        Debug.Log("EnemyAI Initializing...");
        behavior = GetComponent<EnemyBehavior>();
        missileCooldown = 0f;
        lockedOn = false;
        Debug.Log("EnemyAI Initialized");
    }

    // Update is called once per frame
    void Update()
    {
        if (missileCooldown > 0.0f)
            missileCooldown -= Time.deltaTime;

        LockOn();

        if (lockedOn)
            Fire();
    }

    private void LockOn()
    {
        Vector3 playerDir = (behavior.Target.position - behavior.MyTransform.position).normalized;
        angle = Vector3.Dot(playerDir, behavior.MyTransform.forward);

        if (angle > .985f)
            lockedOn = true;
        else
            lockedOn = false;
    }

    private void Fire()
    {
        if (behavior.MissileCount > 0)
        {
            if (missileCooldown <= 0.0f)
            {
                missileCooldown = 5.0f;
                behavior.DecreaseMissileCount();
                Instantiate(missilePrefab, new Vector3(behavior.MyTransform.position.x, behavior.MyTransform.position.y, behavior.MyTransform.position.z), behavior.MyTransform.rotation);
            }
        }
    }
}