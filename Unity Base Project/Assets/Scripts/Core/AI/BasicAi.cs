using UnityEngine;
using GoingDark.Core.Enums;

public class BasicAi : MonoBehaviour
{
    #region Properties
    private bool lockon;
    private float angle;
    private float missileCooldown;
    private EnemyBehavior behavior;
    #endregion


    void Start()
    {
        behavior = GetComponent<EnemyBehavior>();
        behavior.SetUniqueAi(this);
        missileCooldown = 0f;
        lockon = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (missileCooldown > 0.0f)
            missileCooldown -= Time.deltaTime;

        LockOn();

        if (behavior.Debuff != Impairments.Stunned)
            Fire();
    }

    private void LockOn()
    {
        if (behavior.Target != null)
        {
            Vector3 playerDir = (behavior.Target.position - behavior.MyTransform.position).normalized;
            angle = Vector3.Dot(playerDir, behavior.MyTransform.forward);

            if (angle > .985f)
                lockon = true;
            else
                lockon = false;
        }
    }

    private void Fire()
    {
        if (lockon && behavior.MissileCount > 0)
        {
            if (missileCooldown <= 0.0f)
            {
                missileCooldown = 5.0f;
                behavior.DecreaseMissileCount();
                GameObject miss = behavior.manager.GetEnemyMissile();
                miss.transform.position = behavior.MyTransform.position;
                miss.transform.rotation = behavior.MyTransform.rotation;
                miss.SetActive(true);
            }
        }
    }
}