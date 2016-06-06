using UnityEngine;
using GD.Core.Enums;

public class Missile : MonoBehaviour
{

    #region Properties
    public MissileType Type;
    public MovementProperties moveData;
    private bool tracking;

    public GameObject Explosion;
    private Transform MyTransform;

    //  Target Data
    private Transform target;
    //HitMarker
    GameObject HitMarker;// = GameObject.Find("PlaceHolderCircle");
    #endregion


    void Start()
    {
        MyTransform = transform;
        target = null;
        tracking = false;

        moveData.Boost = 1f;
        moveData.MaxSpeed = 500f;
        moveData.RotateSpeed = 10f;
        moveData.Acceleration = 250f;
        moveData.Speed = 50f;

        Invoke("Kill", 4f);

        HitMarker = GameObject.Find("PlaceHolderCircle");
    }

    void FixedUpdate()
    {
        if (moveData.Speed < moveData.MaxSpeed)
            moveData.Speed += Time.deltaTime * moveData.Acceleration;

        if (tracking)
            LookAt();

        MyTransform.position += MyTransform.forward * moveData.Speed * Time.deltaTime;
    }

    public void SetMissileType(MissileType _type, GameObject _particle)
    {
        Type = _type;

        GameObject go = Instantiate(_particle);
        go.transform.parent = transform;
    }

    private void Kill()
    {
        if (Explosion != null)
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
    void OnTriggerEnter(Collider col)
    {
        if (!tracking && col.GetType() == typeof(SphereCollider))
        {
            if (col.CompareTag("Turret"))
            {
                Debug.Log("Player Missile Tracking " + col.transform.tag);
                target = col.transform;
                tracking = true;
            }
            return;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Enemy"))
        {
            HitMarker.GetComponent<Hitmarker>().HitMarkerShow(Time.time);
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
            HitMarker.GetComponent<Hitmarker>().HitMarkerShow(Time.time);
            col.transform.SendMessage("Kill");
            Kill();
        }
        else if (col.transform.CompareTag("Turret"))
        {
            HitMarker.GetComponent<Hitmarker>().HitMarkerShow(Time.time);
            col.transform.SendMessage("Kill");
            Kill();
        }
    }
    #endregion
}