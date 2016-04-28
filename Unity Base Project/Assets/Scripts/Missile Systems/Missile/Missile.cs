using UnityEngine;


public class Missile : MonoBehaviour {
    //**        Attach to Player Missile        **//

    public bool trackTarget;
    public bool destroyMissile;

    public float aliveTimer;
    public float missileSpeed;

    public Vector3 targetPos;
    public Quaternion targetRotation;
    public GameObject Explosion;


    void Start() {
        aliveTimer = 10.0f;
        missileSpeed = 120.0f;        

        trackTarget = false;
        destroyMissile = false;
    }

    void Update() {
        if (aliveTimer > 0.0f)
            aliveTimer -= Time.deltaTime;
        else
            destroyMissile = true;

        if (!destroyMissile) {
            transform.Translate(0, 0, missileSpeed * Time.deltaTime);
            if (trackTarget) {
                targetRotation = Quaternion.LookRotation(targetPos - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * missileSpeed);
            }
        }
        else {
            if (Explosion != null) {
                Instantiate(Explosion, transform.position, transform.rotation);
                Explosion = null;
                this.GetComponent<MeshRenderer>().enabled = false;
            }
            Destroy(this.gameObject, 0);
        }
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Asteroid") {
            trackTarget = true;
            targetPos = col.transform.position;
        }
    }

    void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag == "Enemy" || col.gameObject.tag == "TransportShip") {
            destroyMissile = true;
            Debug.Log("Missile Hit Enemy Ship");
            col.gameObject.SendMessage("Kill");
        }

        else if(col.gameObject.tag == "Asteroid") {
            destroyMissile = true;
            Debug.Log("Missile Hit Asteroid");
            col.gameObject.SendMessage("DestroyAsteroid");
        }
    }
}