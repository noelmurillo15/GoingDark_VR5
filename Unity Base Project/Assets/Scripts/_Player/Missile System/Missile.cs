using UnityEngine;

public class Missile : MonoBehaviour {
    //**        Attach to Player Missile        **//

    //  Missile Data
    public bool tracking;
    public int LookSpeed;
    public float acceleration;

    public float velocity;
    public float maxVelocity;

    public float destroyTimer;
    public GameObject Explosion;
    private Transform MyTransform;

    //  Target Data
    private Transform target;
    public Quaternion targetRotation;


    void Start() {
        target = null;
        tracking = false;

        velocity = 0f;
        LookSpeed = 5;
        maxVelocity = 200f;
        acceleration = 50f;
        destroyTimer = 10f;

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
            velocity = 180f;
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
            if (col.transform.tag == "Enemy" || col.transform.tag == "TransportShip") {
                Debug.Log("Player Missile Tracking " + col.transform.tag);
                target = col.transform;
                tracking = true;
            }
        }        
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.transform.CompareTag("Enemy") || col.transform.CompareTag("TransportShip") || col.transform.CompareTag("Asteroid"))
        {
            col.transform.SendMessage("Kill");
            Kill();
        }
    }
    #endregion
}