using UnityEngine;
using GoingDark.Core.Enums;

public class Missile : MonoBehaviour {

    #region Properties
    public MissileType Type;
    public MovementProperties moveData;

    private bool tracking;
    private bool deflected;
    private GameObject Explosion;
    private Transform MyTransform;

    //  Raycast
    private int range;
    private RaycastHit hit;

    //  Target
    private Transform target;
    private Vector3 direction;
    #endregion


    public void Initialize()
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
        direction = MyTransform.forward;

        FindExplosion();        
    }

    void FixedUpdate()
    {
        if (moveData.Speed < moveData.MaxSpeed)
            moveData.Speed += Time.deltaTime * moveData.Acceleration;

        if (tracking)
            LookAt();

        RaycastCheck();

        MyTransform.position += direction * moveData.Speed * Time.deltaTime;
    }

    private void LookAt()
    {
        if (target != null)
        {
            direction = MyTransform.forward;
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
        direction = -direction;
    }

    private void RaycastCheck()
    {
        if (!tracking && !deflected)
        {
            if (Physics.Raycast(MyTransform.position, MyTransform.forward, out hit, range))
            {
                if (hit.collider.CompareTag("Enemy") && hit.collider.GetType() == typeof(BoxCollider))
                {
                    target = hit.collider.transform;
                    tracking = true;
                }
            }
        }
    }

    void FindExplosion()
    {
        switch (Type)
        {
            case MissileType.Basic:
                Explosion = Resources.Load<GameObject>("Projectiles/Explosions/BasicExplosion");
                break;
            case MissileType.Emp:
                Explosion = Resources.Load<GameObject>("Projectiles/Explosions/EmpExplosion");
                break;
            case MissileType.ShieldBreak:
                Explosion = Resources.Load<GameObject>("Projectiles/Explosions/ShieldBreakExplosion");
                break;
            case MissileType.Chromatic:
                Explosion = Resources.Load<GameObject>("Projectiles/Explosions/ChromaticExplosion");
                break;
        }
    }

    public void Kill()
    {
        CancelInvoke();
        deflected = false;
        if (Explosion != null)
            Instantiate(Explosion, MyTransform.position, Quaternion.identity);
        else
            Debug.LogError("Missile Explosion == null");

        gameObject.SetActive(false);
    }
    private void SelfDestruct()
    {
        direction = MyTransform.forward;

        moveData.Speed = 50f;

        target = null;
        tracking = false;

        Invoke("Kill", 4f);
    }   
}