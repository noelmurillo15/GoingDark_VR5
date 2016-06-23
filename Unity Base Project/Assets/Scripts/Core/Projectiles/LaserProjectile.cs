using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    bool init = false;
    public float speed;
    private Transform MyTransform;
    private ChargeLaser MyParent;

    //HitMarker
    GameObject HitMarker;



    void OnEnable()
    {
        if (!init)
        {
            init = true;
            speed = 1000f;
            MyTransform = transform;
            HitMarker = GameObject.Find("PlayerReticle");
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
        MyTransform.Translate(0f, 0f, speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Enemy"))
        {
            HitMarker.GetComponent<Hitmarker>().HitMarkerShow(Time.time);
            col.gameObject.SendMessage("ShieldHit");
            Kill();
        }

        if (col.transform.CompareTag("Asteroid"))
        {
            HitMarker.GetComponent<Hitmarker>().HitMarkerShow(Time.time);
            col.transform.SendMessage("Kill");
            Kill();
        }
        else if (col.transform.CompareTag("Turret"))
        {
            HitMarker.GetComponent<Hitmarker>().HitMarkerShow(Time.time);
            col.transform.SendMessage("Kill");
            Kill();
        }
    }

    private void Kill()
    {
        CancelInvoke();
        if (MyParent != null)
            MyParent.SpawnExplosion(MyTransform.position);

        gameObject.SetActive(false);
    }
    private void SelfDestruct()
    {
        Invoke("Kill", 3f);
    }
    private void SelfDestruct(ChargeLaser _parent)
    {
        MyParent = _parent;
        Invoke("Kill", 3f);
    }
}
