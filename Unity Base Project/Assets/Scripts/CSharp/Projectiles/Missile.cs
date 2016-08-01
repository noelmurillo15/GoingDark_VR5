using UnityEngine;
using GoingDark.Core.Enums;

public class Missile : MonoBehaviour {

    #region Properties
    public MissileType Type;

    [SerializeField]
    private GameObject Explosion;
    [SerializeField]
    private float explodeTimer;

    public MovementProperties moveData;

    private bool init = false;
    private bool killing;
    private bool tracking;
    private bool deflected;

    private Transform MyTransform;
    private Rigidbody MyRigidbody;
    private BoxCollider collide;
    private Quaternion targetRotation;

    private Transform target;
    #endregion


    public void OnEnable()
    {
        Explosion.SetActive(false);        

        if (!init)
        {
            init = true;
            target = null;
            killing = false;
            tracking = false;
            deflected = false;

            moveData.Boost = 1f;
            moveData.MaxSpeed = 500f;
            moveData.RotateSpeed = 20f;
            moveData.Acceleration = 200f;
            moveData.Speed = 150f;

            MyTransform = transform;
            MyRigidbody = GetComponent<Rigidbody>();

            collide = GetComponent<BoxCollider>();
            gameObject.SetActive(false);
        }
        else
        {
            moveData.Speed = 150f;
            Invoke("Kill", 5f);
        }    
    }

    void FixedUpdate()
    {
        if (moveData.Speed < moveData.MaxSpeed && !killing)
            moveData.Speed += Time.fixedDeltaTime * moveData.Acceleration;

        if (tracking)
            LookAt();

        if(!deflected)
            MyRigidbody.MovePosition(MyTransform.position + MyTransform.forward * Time.fixedDeltaTime * moveData.Speed);
        else
            MyRigidbody.MovePosition(MyTransform.position + -MyTransform.forward * Time.fixedDeltaTime * moveData.Speed);
    }

    #region Methods
    void LookAt()
    {
        if (target != null)
        {                       
            targetRotation = Quaternion.LookRotation(target.position - MyTransform.position);
            MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, targetRotation, Time.fixedDeltaTime * moveData.RotateSpeed);
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
        killing = false;
        tracking = false;
        deflected = false;
        collide.enabled = true;
        gameObject.SetActive(false);
    } 
    public void Kill()
    {
        CancelInvoke();
        killing = true;
        moveData.Speed = 0f;
        Explosion.SetActive(true);
        collide.enabled = false;
        Invoke("SetInactive", explodeTimer);
    }
    #endregion
}