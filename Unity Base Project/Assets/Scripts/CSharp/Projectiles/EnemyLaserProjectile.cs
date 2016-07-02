using UnityEngine;

public class EnemyLaserProjectile : MonoBehaviour
{
    bool init;
    float Speed;
    Turret MyParent;
    Transform MyTransform;


    void OnEnable()
    {
        if (!init)
        {
            init = true;
            Speed = 1000f;
            MyTransform = transform;
            gameObject.SetActive(false);
        }
        else
        {
            Invoke("Kill", 3f);
        }
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
        CancelInvoke("Kill");
        MyParent.SpawnExplosion(MyTransform.position);
        gameObject.SetActive(false);
    }
    private void SelfDestruct(Turret myturret)
    {
        MyParent = myturret;
        Invoke("Kill", 2f);
    }
}