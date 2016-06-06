using UnityEngine;
using GD.Core.Enums;

[RequireComponent(typeof(EnemyBehavior))]
public class EnemyAttack : MonoBehaviour
{
    #region Properties
    //  Missile Data
    private float angle;
    private bool lockedOn;
    private float missileCooldown;
    public GameObject missilePrefab;

    //  Enemy Data
    private EnemyBehavior behavior;
    #endregion


    // Use this for initialization
    void Start()
    {
        behavior = GetComponent<EnemyBehavior>();
        behavior.SetUniqueAi(this);
        missileCooldown = 0f;
        lockedOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (missileCooldown > 0.0f)
            missileCooldown -= Time.deltaTime;

        LockOn();

        if (lockedOn && behavior.Debuff != Impairments.STUNNED)
            Fire();
    }

    private void LockOn()
    {
        if (behavior.Target != null)
        {
            Vector3 playerDir = (behavior.Target.position - behavior.MyTransform.position).normalized;
            angle = Vector3.Dot(playerDir, behavior.MyTransform.forward);

            if (angle > .985f)
                lockedOn = true;
            else
                lockedOn = false;
        }
    }

    private void Fire()
    {
        if(missilePrefab == null)
        {
            missilePrefab = Resources.Load<GameObject>("Missiles/EnemyMissile");
        }

        if (behavior.MissileCount > 0)
        {
            if (missileCooldown <= 0.0f)
            {
                missileCooldown = 5.0f;
                behavior.DecreaseMissileCount();
                Instantiate(missilePrefab, new Vector3(behavior.MyTransform.position.x, behavior.MyTransform.position.y, behavior.MyTransform.position.z), behavior.MyTransform.rotation);
            }
        }
    }
}