using UnityEngine;

public class EnemyLaserProjectile : MonoBehaviour
{
    public float Speed;
    Transform MyTransform;
    Turret MyParent;

    // Use this for initialization
    void Initialize()
    {
        Speed = 1000f;
        MyTransform = transform;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MyTransform.Translate(0f, 0f, Speed * Time.deltaTime);
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

    private void Kill()
    {
        CancelInvoke();
        MyParent.SpawnExplosion(MyTransform.position);
        gameObject.SetActive(false);
    }
    private void SelfDestruct(Turret myturret)
    {
        MyParent = myturret;
        Invoke("Kill", 2f);
    }
}
