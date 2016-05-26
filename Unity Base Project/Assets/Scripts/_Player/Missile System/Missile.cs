using UnityEngine;
using GD.Core.Enums;

public class Missile : MonoBehaviour {
    //**        Attach to Player Missile        **//

    //  Missile Data
    public MissileType Type = MissileType.NONE;
    public bool tracking;
    public int LookSpeed;
    public float acceleration;

    public float velocity;
    public float maxVelocity;

    public float destroyTimer;
    public GameObject Explosion;
    private Transform MyTransform;

    //  Target Data
    private PlayerStats stats;
    private Transform target;
    public Quaternion targetRotation;


    void Start() {
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        target = null;
        tracking = false;

        velocity = 0f;
        LookSpeed = 5;
        maxVelocity = 200f;
        acceleration = 50f;
        destroyTimer = 10f;

        velocity = stats.GetMoveData().Speed + 25f;
        maxVelocity = velocity + 100;

        MyTransform = transform;
    }

    void Update() {

        if (destroyTimer > 0.0f)
            destroyTimer -= Time.deltaTime;
        else
            Kill();

        if (velocity < maxVelocity)
            velocity += Time.deltaTime * acceleration;

        if (tracking)
        {            
            LookAt();
        }

        MyTransform.position += MyTransform.forward * velocity * Time.deltaTime;
    }

    private void Kill() {
        Instantiate(Explosion, MyTransform.position, MyTransform.rotation);
        Destroy(this.gameObject);
    }

    private void LookAt()
    {
        if (target != null)
        {
            targetRotation = Quaternion.LookRotation(target.position - MyTransform.position);
            MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, targetRotation, Time.deltaTime * LookSpeed);
        }
    }

    #region Collisions
    void OnTriggerEnter(Collider col) {
        if (!tracking && col.GetType() == typeof(CharacterController)) {
            if (col.CompareTag("Enemy")) {
                Debug.Log("Player Missile Tracking " + col.transform.tag);
                target = col.transform;
                tracking = true;
            }
        }        
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.transform.CompareTag("Enemy"))
        {
            switch (Type)
            {
                case MissileType.EMP:
                    col.transform.SendMessage("EMPHit");
                    break;
                case MissileType.BASIC:
                    col.transform.SendMessage("Hit");
                    break;
                case MissileType.CHROMATIC:
                    col.transform.SendMessage("Hit");
                    break;
                case MissileType.SHIELDBREAKER:
                    col.transform.SendMessage("ShieldHit");
                    break;
            }            
            Kill();
        }
        else if (col.transform.CompareTag("Asteroid"))
        {
            col.transform.SendMessage("Kill");
            Kill();
        }
    }
    #endregion
}