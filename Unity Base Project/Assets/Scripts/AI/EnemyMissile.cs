using UnityEngine;
using System.Collections;

public class EnemyMissile : MonoBehaviour {
    //**        Attach to Enemy Missile Prefab      **//

    //  Missile Data
    public bool tracking;
    public int LookSpeed;
    public float velocity;
    public float destroyTimer;
    public GameObject Explosion;

    private string triggerName;

    //  Target Data
    private Vector3 targetLocation;
    public Quaternion targetRotation;

    // Messages
    private GameObject messages;

    void Start() {
        tracking = false; 
        LookSpeed = 10;
        velocity = 80.0f;
        destroyTimer = 10.0f;

        messages = GameObject.Find("Screen");
        messages.SendMessage("MissileIncoming");
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
        messages.SendMessage("MissileDestroyed");
        Instantiate(Explosion, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    private void LookAt() {
        targetRotation = Quaternion.LookRotation(targetLocation - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * LookSpeed);
    }

    #region Collisions
    void OnTriggerEnter(Collider col) {
        if (!tracking) {
            if (col.transform.tag == "PlayerShip" || col.transform.tag == "Asteroid" || col.transform.tag == "Decoy") {              
                tracking = true;
                triggerName = col.transform.tag;
                targetLocation = col.transform.position;
            }
        }
    }

    void OnTriggerStay(Collider col) {
        if (col.transform.tag == triggerName) {
            targetLocation = col.transform.position;
        }
    }

    void OnCollisionEnter(Collision col) {
        if (col.transform.tag == "PlayerShip") {
            col.gameObject.SendMessage("Hit");            
            Kill();
        }
        else if (col.transform.tag == "Asteroid" || col.transform.tag == "Decoy") {
            col.gameObject.SendMessage("Kill");
            Kill();
        }
    }
    #endregion
}
