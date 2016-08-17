using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyMissileProjectile : MonoBehaviour
{
    #region Properties
    [SerializeField]
    public EnemyMissileType Type;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotateSpeed;

    private bool init = false;
    private bool tracking;
    private float baseDmg;
    private Transform target;
    private Quaternion targetRotation;

    private Transform MyTransform;
    private Rigidbody MyRigidbody;
    private MessageScript messages;
    private ObjectPoolManager poolManager;
    #endregion


    public void OnEnable()
    {
        if (!init)
        {
            init = true;
            target = null;
            tracking = false;
            targetRotation = Quaternion.identity;

            switch (Type)
            {
                case EnemyMissileType.Basic:
                    baseDmg = 10f;
                    break;
            }

            MyTransform = transform;
            MyRigidbody = GetComponent<Rigidbody>();
            messages = GameObject.Find("PlayerCanvas").GetComponent<MessageScript>();
            poolManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();

            gameObject.SetActive(false);
        }
        else
        {
            tracking = false;
            messages.MissileIncoming();
            Invoke("Kill", 3f);
        }
    }

    void FixedUpdate()
    {
        if (tracking)
            LookAt();

        MyRigidbody.MovePosition(MyTransform.position + MyTransform.forward * Time.fixedDeltaTime * speed);
    }

    #region Accessors
    public float GetBaseDamage()
    {
        return baseDmg;
    }
    #endregion

    #region Tracking
    void LookAt()
    {
        if (target != null)
        {
            targetRotation = Quaternion.LookRotation(target.position - MyTransform.position);
            MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
        }
    }
    #endregion

    #region Recycle Death
    void SetInactive()
    {
        target = null;
        tracking = false;
        gameObject.SetActive(false);
    }
    public void Kill()
    {
        if (IsInvoking("Kill"))
            CancelInvoke("Kill");

        GameObject go = poolManager.GetMissileExplosion(MissileType.Basic);

        if (go != null)
        {
            go.transform.position = MyTransform.position;
            go.SetActive(true);
        }

        SetInactive();
    }
    #endregion

    #region Collision
    void OnTriggerEnter(Collider col)
    {
        if (!tracking)
        {
            if (col.transform.tag == "Player")
            {
                tracking = true;
                target = col.transform;
            }

            if (col.transform.tag == "Asteroid" || col.transform.tag == "Decoy")
            {
                tracking = true;
                target = col.transform;
            }
        }
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Player")
        {
            if (IsInvoking("Kill"))
                CancelInvoke("Kill");

            col.transform.SendMessage("MissileDmg", this);
        }
        else if (col.transform.tag == "Asteroid" || col.transform.tag == "Decoy")
        {
            col.gameObject.SendMessage("Kill");
            Kill();
        }
    }
    #endregion
}