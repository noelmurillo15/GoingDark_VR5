using UnityEngine;
using GD.Core.Enums;

public class Missile : MonoBehaviour
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
    private Vector3 dir;
    private Transform target;

    //HitMarker
    Hitmarker hitMarker;
    #endregion


    void Start()
    {
        range = 1600;
        target = null;
        tracking = false;
        MyTransform = transform;
        hitMarker = GameObject.Find("PlaceHolderCircle").GetComponent<Hitmarker>();

        moveData.MaxSpeed = 500f;
        moveData.RotateSpeed = 10f;
        moveData.Acceleration = 250f;
        moveData.Speed = 50f;

        dir = MyTransform.forward;
        
        Invoke("Kill", 4f);
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
        dir = -dir;
    }

    private void RaycastCheck()
    {
        if (!tracking)
        {
            if (Physics.Raycast(MyTransform.position, MyTransform.forward, out hit, range, LayerMask.GetMask("Enemies")))
            {
                if (hit.collider.CompareTag("Enemy") && hit.collider.GetType() == typeof(BoxCollider))
                {
                    Debug.Log("Missile tracking enemy : " + hit.distance);
                    target = hit.collider.transform;
                    tracking = true;
                }
            }
        }
    }

    private void Kill()
    {
        if (Explosion != null)
            Instantiate(Explosion, MyTransform.position, MyTransform.rotation);

        Destroy(gameObject);
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