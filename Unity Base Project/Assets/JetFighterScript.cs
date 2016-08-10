using UnityEngine;
using System.Collections;

public class JetFighterScript : MonoBehaviour {

    //  Enemy Data
    Transform MyTransform;
    private EnemyStateManager behavior;
    private bool lockedOn;
    private float fire  ;
    private ObjectPoolManager pool;
    
    // Use this for initialization
    void Start()
    {
        MyTransform = transform;
        fire = 0.2f;
        lockedOn = false;

        behavior = transform.parent.GetComponentInParent<EnemyStateManager>();
        pool = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();
        
        InvokeRepeating("DestroyPlayer", 0.1f, fire);
    }

    private void LockOn()
    {
        if (behavior.Target != null)
        {
            Vector3 playerDir = behavior.Target.position - MyTransform.position;
            Vector3 direction = Vector3.RotateTowards(MyTransform.forward, playerDir, Time.deltaTime * 5f, 0.0f);
            MyTransform.rotation = Quaternion.LookRotation(direction);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (behavior.Target != null)
        {
            if (lockedOn)
                LockOn();
        }
    }
    void DestroyPlayer()
    {
        if (behavior.Target != null)
            lockedOn = true;
        Shoot();
    }

    public void Shoot()
    {
        if (behavior.Target != null)
            if (lockedOn)
            {
                GameObject obj = pool.GetBaseEnemyLaser();
                if (obj != null)
                {
                    obj.transform.position = transform.position;
                    obj.transform.rotation = transform.rotation;
                    obj.SetActive(true);
                }
            }
    }

    public void SpawnExplosion(Vector3 pos)
    {
        GameObject obj = pool.GetBaseLaserExplosion();
        if (obj != null)
        {
            obj.transform.position = pos;
            obj.SetActive(true);
        }
    }
}
