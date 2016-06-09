using UnityEngine;
using GD.Core.Enums;

public class Missile : ShipDevice
{

    #region Properties
    private bool tracking;
    public MissileType Type;
    public GameObject Explosion;
    private Transform MyTransform;
    public MovementProperties moveData;

    //  Raycast
    private int range;
    private RaycastHit hit;

    //  Target Data
    private bool deflected;
    private Vector3 dir;
    private Transform target;

    //HitMarker
    Hitmarker hitMarker;
    #endregion


    public void InitializeStats()
    {
        range = 1600;
        target = null;
        tracking = false;
        deflected = false;

        moveData.Boost = 1f;
        moveData.MaxSpeed = 500f;
        moveData.RotateSpeed = 25f;
        moveData.Acceleration = 200f;
        moveData.Speed = 100f;

        MyTransform = transform;
        dir = MyTransform.forward;
        hitMarker = GameObject.Find("PlayerReticle").GetComponent<Hitmarker>();
    }

    void FixedUpdate()
    {
        if (moveData.Speed < moveData.MaxSpeed)
            moveData.Speed += Time.deltaTime * moveData.Acceleration;

        if (tracking)
            LookAt();

        RaycastCheck();

        MyTransform.position += dir * moveData.Speed * Time.deltaTime;
    }

    private void LookAt()
    {
        if (target != null)
        {
            dir = MyTransform.forward;
            Quaternion targetRotation = Quaternion.LookRotation(target.position - MyTransform.position);
            MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, targetRotation, Time.deltaTime * moveData.RotateSpeed);
        }
    }

    public void Deflect()
    {
        Debug.Log("Missile was deflected by enemy shield");
        CancelInvoke();
        Invoke("Kill", 2f);
        deflected = true;
        tracking = false;
        target = null;
        dir = -dir;
    }

    private void RaycastCheck()
    {
        if (!tracking && !deflected)
        {
            if (Physics.Raycast(MyTransform.position, MyTransform.forward, out hit, range))
            {
                if (hit.collider.CompareTag("Enemy") && hit.collider.GetType() == typeof(BoxCollider))
                {
                    Debug.Log("Missile tracking "+ hit.collider.tag + " : " + hit.distance);
                    target = hit.collider.transform;
                    tracking = true;
                }
            }
        }
    }

    private void Kill()
    {
        CancelInvoke();
        if (Explosion != null)
            Instantiate(Explosion, MyTransform.position, MyTransform.rotation);

        deflected = false;
        gameObject.SetActive(false);
    }
    private void SelfDestruct()
    {
        dir = MyTransform.forward;

        moveData.Speed = 50f;

        target = null;
        tracking = false;

        Invoke("Kill", 4f);
    }

    #region Collisions
    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Enemy"))
        {
            switch (Type)
            {
                case MissileType.EMP:
                    col.transform.SendMessage("EMPHit");
                    break;
                case MissileType.BASIC:
                    col.transform.SendMessage("Hit", this);
                    break;
                case MissileType.CHROMATIC:
                    col.transform.SendMessage("Hit", this);
                    break;
                case MissileType.SHIELDBREAKER:
                    col.transform.SendMessage("ShieldHit");
                    break;
            }
            hitMarker.HitMarkerShow(Time.time);
        }
        else if (col.transform.CompareTag("Asteroid"))
        {
            hitMarker.HitMarkerShow(Time.time);
            col.transform.SendMessage("Kill");
            Kill();
        }
        else if (col.transform.CompareTag("Turret"))
        {
            hitMarker.HitMarkerShow(Time.time);
            col.transform.SendMessage("Kill");
            Kill();
        }
    }
    #endregion
}