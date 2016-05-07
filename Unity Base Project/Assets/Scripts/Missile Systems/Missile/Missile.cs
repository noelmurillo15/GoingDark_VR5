using UnityEngine;


public class Missile : MonoBehaviour {
    //**        Attach to Player Missile        **//

    //  Missile Data
    public bool tracking;
    public int LookSpeed;
    public float velocity;
    public float destroyTimer;
    public GameObject Explosion;

    //  Target Data
    private Transform target;
    public Quaternion targetRotation;


    void Start() {
        tracking = false;
        LookSpeed = 2;
        velocity = 125.0f;
        destroyTimer = 7.5f;
    }

    void Update() {
        if (destroyTimer > 0.0f)
            destroyTimer -= Time.deltaTime;
        else
            Kill();

        if (tracking)
        {
            velocity = 150f;
            LookAt();
        }

        transform.position += transform.forward * velocity * Time.deltaTime;
    }

    private void Kill() {
        Instantiate(Explosion, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    private void LookAt()
    {
        targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * LookSpeed);
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
            Debug.Log("Player Destroyed " + col.transform.name);
            col.transform.SendMessage("Kill");
            Kill();
        }
    }
    #endregion
}