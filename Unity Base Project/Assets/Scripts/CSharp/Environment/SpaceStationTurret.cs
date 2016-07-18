using UnityEngine;

public class SpaceStationTurret : MonoBehaviour
{
    Transform enemy;
    Transform MyTransform;
    GameObject Laser;
    float randomShot;
    //private ObjectPooling pool;


    void Start()
    {
        randomShot = 0.3f;
        MyTransform = transform;
        enemy = null;
        //pool = new ObjectPooling();
        //pool.Initialize(Resources.Load<GameObject>("LaserBeam"), 35);
        InvokeRepeating("Shoot", 1f, randomShot);
    }

    void Update()
    {
        if (enemy != null)
            LockOn();
    }
    private void LockOn()
    {
        Vector3 playerDir = enemy.position - MyTransform.position;
        Vector3 direction = Vector3.RotateTowards(MyTransform.TransformDirection(Vector3.forward), playerDir, Time.deltaTime, 1.0f);
        MyTransform.rotation = Quaternion.LookRotation(direction);
    }

    public void Shoot()
    {
        if (enemy != null)
            if (Laser != null)
            {
                //GameObject obj = pool.GetPooledObject();
                //if (obj != null)
                //{
                //    obj.transform.position = transform.position;
                //    obj.transform.rotation = transform.rotation;
                //    obj.SetActive(true);
                //    obj.SendMessage("SelfDestruct");
                //}
            }
    }

    #region Collision
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
            enemy = col.transform;
    }


    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Enemy"))
            enemy = null;
    }
    #endregion
}