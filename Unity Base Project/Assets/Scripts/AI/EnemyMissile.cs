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
    private Vector3 targetLocation;
    public Quaternion targetRotation;

    // Messages
    private GameObject messages;

    void Start() {
        tracking = false; 
        LookSpeed = 20;
        velocity = 80.0f;
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
            if (col.transform.tag == "PlayerShip" || col.transform.tag == "Asteroid") {
                Debug.Log("Enemy Missile Tracking " + col.transform.tag);
                this.GetComponent<MeshRenderer>().enabled = false;
                targetLocation = col.transform.position;
                tracking = true;
            }
        }
    }

    void OnCollisionEnter(Collision col) {
        if (col.transform.tag == "PlayerShip") {
            col.gameObject.SendMessage("Hit");
            messages.SendMessage("MissileDestroyed");
            Kill();
        }
        if (col.transform.tag == "Asteroid") {
            messages.SendMessage("MissileDestroyed");
            col.gameObject.SendMessage("DestroyAsteroid");
            Kill();
        }
    }
    #endregion
}
