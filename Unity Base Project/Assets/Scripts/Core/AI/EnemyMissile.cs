using UnityEngine;

public class EnemyMissile : MonoBehaviour {

    #region Properties
    public MovementProperties moveData;
    private bool tracking;
    private bool init = false;

    private Transform MyTransform;
    public GameObject Explosion;

    //  Target Data
    private Transform target;

    // Messages
    private GameObject messages;
    #endregion

    void OnEnable()
    {
        if (!init)
        {
            init = true;
            tracking = false;
            MyTransform = transform;

            moveData.Boost = 1f;
            moveData.MaxSpeed = 750f;
            moveData.RotateSpeed = 10f;
            moveData.Acceleration = 100f;
            moveData.Speed = 150f;

            messages = GameObject.Find("PlayerCanvas");
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Enemy Missile was used");
            tracking = false;
            moveData.Speed = 150f;

            messages.SendMessage("MissileIncoming");
            Invoke("Kill", 3f);
        }
    }

    void FixedUpdate() {
        if (moveData.Speed < moveData.MaxSpeed)
            moveData.Speed += Time.deltaTime * moveData.Acceleration;

        if (tracking)
            LookAt();

        MyTransform.position += MyTransform.forward * moveData.Speed * Time.deltaTime;
    }

    private void Kill() {
        Debug.Log("Enemy Missile Blewup");
        CancelInvoke("Kill");
        Instantiate(Explosion, MyTransform.position, MyTransform.rotation);
        gameObject.SetActive(false);
    }

    private void LookAt() {
        if (target != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - MyTransform.position);
            MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, targetRotation, Time.deltaTime * moveData.RotateSpeed);
        }
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
