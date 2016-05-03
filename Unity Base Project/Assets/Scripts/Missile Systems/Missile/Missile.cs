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
    private Vector3 targetLocation;
    public Quaternion targetRotation;


    void Start() {
        tracking = false;
        LookSpeed = 20;
        velocity = 80.0f;
        destroyTimer = 5.0f;
    }

    void Update() {
        if (destroyTimer > 0.0f)
            destroyTimer -= Time.deltaTime;
        else Kill();

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
        targetRotation = Quaternion.LookRotation(targetLocation - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * LookSpeed);
    }

    #region Collisions
    void OnTriggerEnter(Collider col) {
        if (!tracking) {
            if (col.transform.tag == "Enemy" || col.transform.tag == "Asteroid") {
                Debug.Log("Player Missile Tracking " + col.transform.tag);
                targetLocation = col.transform.position;
                tracking = true;
            }
        }
    }

    void OnCollisionEnter(Collision col) {
        if (col.transform.tag == "Asteroid" || col.transform.name == "Enemy") {
            col.gameObject.SendMessage("Kill");
            Kill();
        }
    }
    #endregion
}