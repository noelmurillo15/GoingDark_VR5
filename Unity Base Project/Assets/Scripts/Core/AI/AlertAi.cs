using UnityEngine;
using System.Collections;
using GoingDark.Core.Enums;

public class AlertAi : MonoBehaviour
{

    #region Properties
    //  Raycast Data
    private int range;
    private bool blocked;
    private RaycastHit hit;

    //  Movement    
    private Vector3 targetLocation;

    //  Enemy Data 
    private Transform MyTransform;
    private Rigidbody MyRigidbody;
    private EnemyBehavior behavior;
    #endregion


    void Awake()
    {
        Debug.Log("Enemy Alert Start Called");
        // Patrol Data
        targetLocation = Vector3.zero;
        blocked = false;
        range = 100;

        // Enemy Data
        MyRigidbody = GetComponent<Rigidbody>();
        behavior = GetComponent<EnemyBehavior>();
        MyTransform = transform;
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
            if(Vector3.Distance(MyTransform.position, targetLocation) < 20f)
            {
                Debug.Log("Enemy has not founds player, going back on patrol");
                behavior.losingsightTimer = 0f;
            }

            behavior.IncreaseSpeed();
            Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.fixedDeltaTime / behavior.GetMoveData().RotateSpeed, 0.0f);
            MyTransform.rotation = Quaternion.LookRotation(direction);
            MyRigidbody.MovePosition(MyTransform.position + MyTransform.forward * Time.deltaTime * behavior.GetMoveData().Speed);
        }
    }
}
