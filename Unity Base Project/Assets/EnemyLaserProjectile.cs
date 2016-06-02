using UnityEngine;

public class EnemyLaserProjectile : MonoBehaviour
{
    public float Speed = 1000.0f;
    public float lifetime = 1f;
    public GameObject explosion;
    Transform MyTransform;
    // Use this for initialization
    void Start()
    {
        MyTransform = transform;
        Invoke("Kill", lifetime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MyTransform.position += MyTransform.forward * Speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Player"))
        {
            col.gameObject.SendMessage("ShieldHit");
            col.gameObject.SendMessage("Hit");
        }

        if (col.transform.CompareTag("Asteroid"))
        {
            col.transform.SendMessage("Kill");
            Kill();
        }
    }

    void Kill()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
