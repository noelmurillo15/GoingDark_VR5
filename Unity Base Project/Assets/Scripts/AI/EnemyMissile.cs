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

    //  Target Data
    private Transform target;
    public Quaternion targetRotation;

    // Messages
    private GameObject messages;

    void Start() {
        tracking = false; 
        LookSpeed = 15;
        velocity = 180.0f;
        destroyTimer = 5.0f;

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
        targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * LookSpeed);
    }

    #region Collisions
    void OnTriggerEnter(Collider col) {
        if (!tracking) {
            if (col.transform.tag == "PlayerShip" || col.transform.tag == "Asteroid" || col.transform.tag == "Decoy") {              
                target = col.transform;
                tracking = true;
            }
        }
    }

    void OnCollisionEnter(Collision col) {
        if (col.transform.tag == "PlayerShip") {
            col.transform.parent.SendMessage("Hit");            
            Kill();
        }
        else if (col.transform.tag == "Asteroid" || col.transform.tag == "Decoy") {
            col.gameObject.SendMessage("Kill");
            Kill();
        }
    }
    #endregion
}
