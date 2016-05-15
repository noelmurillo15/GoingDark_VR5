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
    private Transform MyTransform;

    //  Target Data
    private Transform target;
    public Quaternion targetRotation;

    // Messages
    private GameObject messages;

    void Start() {
        MyTransform = transform;
        tracking = false; 
        LookSpeed = 15;
        velocity = 180.0f;
        destroyTimer = 5.0f;

        messages = GameObject.Find("WarningMessages");
        messages.SendMessage("MissileIncoming");
    }

    void Update() {
        if (destroyTimer > 0.0f)
            destroyTimer -= Time.deltaTime;
        else Kill();

        if (tracking)
            LookAt();

        MyTransform.position += MyTransform.forward * velocity * Time.deltaTime;
    }

    private void Kill() {
        messages.SendMessage("MissileDestroyed");
        Instantiate(Explosion, MyTransform.position, MyTransform.rotation);
        Destroy(this.gameObject);
    }

    private void LookAt() {
        targetRotation = Quaternion.LookRotation(target.position - MyTransform.position);
        MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, targetRotation, Time.deltaTime * LookSpeed);
    }

    #region Collisions
    void OnTriggerEnter(Collider col) {
        if (!tracking) {
            if (col.transform.tag == "Player" || col.transform.tag == "Asteroid" || col.transform.tag == "Decoy") {              
                target = col.transform;
                tracking = true;
            }
        }
    }

    void OnCollisionEnter(Collision col) {
        if (col.transform.tag == "Player") {
            col.transform.SendMessage("Hit");            
            Kill();
        }
        else if (col.transform.tag == "Asteroid" || col.transform.tag == "Decoy") {
            col.gameObject.SendMessage("Kill");
            Kill();
        }
    }
    #endregion
}
