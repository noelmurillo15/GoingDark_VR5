using UnityEngine;
using GD.Core.Enums;

public class Missile : MonoBehaviour {

    #region Properties
    public MissileType Type = MissileType.NONE;
    public MovementProperties moveData;
    private bool tracking;

    public GameObject Explosion;
    private Transform MyTransform;

    //  Target Data
    private Transform target;
    #endregion


    void Start() {
        MyTransform = transform;
        target = null;
        tracking = false;

        moveData.Boost = 1f;
        moveData.MaxSpeed = 500f;
        moveData.RotateSpeed = 10f;
        moveData.Acceleration = 100f;
        moveData.Speed = 100f;

        Invoke("Kill", 5f);
    }

    void FixedUpdate() {
        if (moveData.Speed < moveData.MaxSpeed)
            moveData.Speed += Time.deltaTime * moveData.Acceleration;

        if (tracking)        
            LookAt();        

        MyTransform.position += MyTransform.forward * moveData.Speed * Time.deltaTime;
    }

    private void Kill() {
        Instantiate(Explosion, MyTransform.position, MyTransform.rotation);
        Destroy(gameObject);
    }

    private void LookAt()
    {
        if (target != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - MyTransform.position);
            MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, targetRotation, Time.deltaTime * moveData.RotateSpeed);
        }
    }

    #region Collisions
    void OnTriggerEnter(Collider col) {
        if (!tracking && col.GetType() == typeof(CharacterController)) {
            if (col.CompareTag("Enemy")) {
                Debug.Log("Player Missile Tracking " + col.transform.tag);
                target = col.transform;
                tracking = true;
            }
        }        
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.transform.CompareTag("Enemy"))
        {
            switch (Type)
            {
                case MissileType.EMP:
                    col.transform.SendMessage("EMPHit");
                    break;
                case MissileType.BASIC:
                    col.transform.SendMessage("Hit");
                    break;
                case MissileType.CHROMATIC:
                    col.transform.SendMessage("Hit");
                    break;
                case MissileType.SHIELDBREAKER:
                    col.transform.SendMessage("ShieldHit");
                    break;
            }            
            Kill();
        }
        else if (col.transform.CompareTag("Asteroid"))
        {
            col.transform.SendMessage("Kill");
            Kill();
        }
    }
    #endregion
}