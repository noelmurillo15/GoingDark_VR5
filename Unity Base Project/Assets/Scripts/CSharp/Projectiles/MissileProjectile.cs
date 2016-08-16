using UnityEngine;
using GoingDark.Core.Enums;

public class MissileProjectile : MonoBehaviour
{

    #region Properties
    [SerializeField]
    public MissileType Type;
    [SerializeField]
    public float speed;
    [SerializeField]
    private float rotateSpeed;

    private float baseDmg;

    private bool init = false;
    private bool tracking;
    private bool deflected;

    private Transform MyTransform;
    private Rigidbody MyRigidbody;
    private Quaternion targetRotation;

    private PlayerStats stats;
    private Transform target;
    private ObjectPoolManager poolManager;
    #endregion


    public void OnEnable()
    {
        if (!init)
        {
            init = true;
            target = null;
            tracking = false;
            deflected = false;

            switch (Type)
            {
                case MissileType.Basic:
                    baseDmg = 5;
                    break;
                case MissileType.Emp:
                    baseDmg = 3;
                    break;
                case MissileType.ShieldBreak:
                    baseDmg = 2;
                    break;
                case MissileType.Chromatic:
                    baseDmg = 20;
                    break;
            }

            MyTransform = transform;
            MyRigidbody = GetComponent<Rigidbody>();
            stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
            poolManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();
            gameObject.SetActive(false);
        }
        else
            Invoke("Kill", 3f);
        
    }

    void FixedUpdate()
    {
        if (tracking)
            LookAt();

        if (!deflected)
            MyRigidbody.MovePosition(MyTransform.position + MyTransform.forward * Time.fixedDeltaTime * speed);
        else
            MyRigidbody.MovePosition(MyTransform.position + -MyTransform.forward * Time.fixedDeltaTime * speed);
    }

    public float GetBaseDmg()
    {
        return baseDmg;
    }

    #region Tracking
    void LookAt()
    {
        if (target != null)
        {
            targetRotation = Quaternion.LookRotation(target.position - MyTransform.position);
            MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
        }
    }

    void LockedOn(Transform _target)
    {
        if (_target != null)
        {
            target = _target;
            tracking = true;
        }
    }

    public void Deflect()
    {
        CancelInvoke();
        target = null;
        tracking = false;
        deflected = true;
        float timer = Random.Range(.5f, 2f);
        Invoke("Kill", timer);
    }
    #endregion

    #region Recycle Death
    void SetInactive()
    {
        target = null;
        tracking = false;
        deflected = false;
        gameObject.SetActive(false);
    }
    public void Kill()
    {
        if (IsInvoking("Kill"))
            CancelInvoke("Kill");

        GameObject go = null;
        switch (Type)
        {
            case MissileType.Basic:
                go = poolManager.GetBaseMissExplode();
                break;
            case MissileType.Emp:
                go = poolManager.GetEmpMissExplode();
                break;
            case MissileType.ShieldBreak:
                go = poolManager.GetSBMissExplode();
                break;
            case MissileType.Chromatic:
                go = poolManager.GetChromeMissExplode();
                break;
        }
        if (go != null)
        {
            go.transform.position = MyTransform.position;
            go.SetActive(true);
        }
        else
            Debug.Log("Player Missile does not have explosion : " + transform.name);

        SetInactive();
    }
    #endregion

    #region Collision
    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Enemy") || col.transform.CompareTag("Orb"))
        {
            col.transform.SendMessage("MissileHit", this);
        }
        else if (col.transform.CompareTag("Asteroid"))
        {
            stats.SendMessage("UpdateCredits", 5);
            col.transform.SendMessage("Kill");
            Kill();
        }
    }
    #endregion
}