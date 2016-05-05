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
        velocity = 180.0f;
        destroyTimer = 10.0f;
    }

    void Update() {
        if (destroyTimer > 0.0f)
            destroyTimer -= Time.deltaTime;
        else
            Kill();

        if (tracking)
            LookAt();

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
        if (!tracking && col.GetType() == typeof(SphereCollider) && destroyTimer < 5f) {
            if (col.transform.tag == "Enemy" || col.transform.tag == "TransportShip") {
                Debug.Log("Player Missile Tracking " + col.transform.tag);
                target = col.transform;
                tracking = true;
            }
        }
        else if (col.GetType() == typeof(CharacterController)) {
            if (col.transform.tag == "Enemy" || col.transform.tag == "TransportShip") {
                col.gameObject.SendMessage("Kill");
                Kill();
            }
        }
    }

    void OnCollisionEnter(Collision col) {
        if(col.transform.tag == "Asteroid") {
            col.gameObject.SendMessage("Kill");
            Kill();
        }
    }
    #endregion
}