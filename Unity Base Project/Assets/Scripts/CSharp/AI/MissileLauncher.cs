using UnityEngine;
using GoingDark.Core.Enums;

public class MissileLauncher : MonoBehaviour
{
    #region Properties
    private float angle;
    private float Cooldown;
    private float MaxCooldown;
    private Transform MyTransform;
    private EnemyStateManager behavior;
    #endregion


    void Start()
    {
        behavior = transform.parent.parent.GetComponent<EnemyStateManager>();
        MyTransform = transform;
        MaxCooldown = 5f;
        Cooldown = 0f;
        angle = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cooldown > 0.0f)
            Cooldown -= Time.deltaTime;
        else if (behavior.State == EnemyStates.Attack)
        {
            if (behavior.Debuff != Impairments.Stunned)
            {
                LockOn();
                Fire();
            }
        }
    }

    private void LockOn()
    {
        if (behavior.Target != null)
            transform.LookAt(behavior.Target);
    }

    private void Fire()
    {
        Cooldown = MaxCooldown;
        GameObject miss = behavior.GetPoolManager().GetEnemyMissile();
        if (miss != null)
        {
            miss.transform.position = MyTransform.position;
            miss.transform.rotation = MyTransform.rotation;
            miss.SetActive(true);
        }
        else
            Debug.LogError("Enemy Missile was not found");
    }
}