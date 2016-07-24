using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    bool init = false;
    public float speed;
    private Transform MyTransform;
    private ChargeLaser MyParent;
    private ObjectPoolManager poolManager;

    void OnEnable()
    {
        if (!init)
        {
            init = true;
            speed = 1000f;
            MyTransform = transform;
            gameObject.SetActive(false);
            poolManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();
        }
        else
        {
            Invoke("Kill", 3f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MyTransform.Translate(0f, 0f, speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<EnemyStateManager>().LaserHit(this);
            Kill();
        }

        if (col.transform.CompareTag("Asteroid"))
        {
            col.transform.SendMessage("Kill");
            Kill();
        }
    }

    public void Kill()
    {
        if(IsInvoking("Kill"))
            CancelInvoke("Kill");

        GameObject go = poolManager.GetBaseLaserExplosion();
        go.transform.position = MyTransform.position;
        go.SetActive(true);

        gameObject.SetActive(false);
    }
}
