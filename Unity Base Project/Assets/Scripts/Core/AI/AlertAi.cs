using UnityEngine;
using GoingDark.Core.Enums;

public class AlertAi : MonoBehaviour
{

    #region Properties  
    public float Distance;
    public Vector3 targetLocation;

    private Rigidbody MyRigidbody;
    private Transform MyTransform;
    private EnemyBehavior behavior;
    #endregion


    void Awake()
    {
        targetLocation = Vector3.zero;
        MyRigidbody = GetComponent<Rigidbody>();
        behavior = GetComponent<EnemyBehavior>();
        MyTransform = transform;
        Distance = 0f;
    }

    void OnEnable()
    {
        targetLocation = Vector3.zero;
        if(behavior != null)
            targetLocation = behavior.LastKnownPos;
    }

    void FixedUpdate()
    {
        if (targetLocation != Vector3.zero)
        {
            Vector3 playerDir = targetLocation - MyTransform.position;
            Distance = Vector3.Distance(MyTransform.position, targetLocation);
            if (Distance < 25f)
            {
                Debug.Log("Enemy has not found player, going back on patrol");
                Distance = 0;
                behavior.losingsightTimer = 0f;
                return;
            }

            if (behavior.Debuff != Impairments.Stunned)
                behavior.IncreaseSpeed();
            else
                behavior.DecreaseSpeed();

            Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.fixedDeltaTime / behavior.GetMoveData().RotateSpeed, 0.0f);
            MyTransform.rotation = Quaternion.LookRotation(direction);
            MyRigidbody.MovePosition(MyTransform.position + MyTransform.forward * Time.deltaTime * behavior.GetMoveData().Speed);
        }
    }
}
