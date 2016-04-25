using UnityEngine;
using System.Collections;

public class EnemyMissile : MonoBehaviour {
    //**        Attach to Enemy Missile Prefab      **//

    //  Missile Data
    public int LookSpeed;
    public bool tracking;
    public float velocity;
    public float destroyTimer;
    public bool destroyMissile;
    public GameObject Explosion;

    //  Target Data
    private Vector3 targetLocation;
    public Quaternion targetRotation;


    void Start() {
        tracking = false; 
        destroyMissile = false;
        velocity = 80.0f;
        destroyTimer = 5.0f;
        LookSpeed = 20;
    }

    void Update() {

        if (destroyTimer > 0.0f)
            destroyTimer -= Time.deltaTime;
        else
            Destroy(this.gameObject);


        if (!destroyMissile) {
            transform.position += transform.forward * velocity * Time.deltaTime;
            if (tracking)
            {
                targetRotation = Quaternion.LookRotation(targetLocation - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * LookSpeed);
            }
        }

        if (destroyMissile && Explosion != null) {
            Instantiate(Explosion, transform.position, transform.rotation);
            Explosion = null;
            this.GetComponent<MeshRenderer>().enabled = false;
        }        
    }

    void OnCollisionEnter(Collision col) {       
        if (col.gameObject.tag == "PlayerShip") {
           Debug.Log("Missile Hit Player");
           destroyMissile = true;
           col.gameObject.SendMessage("Hit");
        }

        if (col.gameObject.tag == "Asteroid")
        {
            Debug.Log("Missile Hit Asteroid");
            destroyMissile = true;
            col.gameObject.SendMessage("DestroyAsteroid");
        }
    }

    void OnTriggerEnter(Collider col) {
        if (!tracking) {
            if (col.tag == "PlayerShip" || col.tag == "Asteroid") {
                tracking = true;
                targetLocation = col.transform.position;
            }
        }
    }

    void OnTriggerExit(Collider col) {
        if(col.tag == "PlayerShip" || col.tag == "Asteroid")
            tracking = false;     
    }
}
