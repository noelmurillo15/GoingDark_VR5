using UnityEngine;

public class LaserProjectile : MonoBehaviour {

    public float speed;
    
    private Transform MyTransform;

    private ChargeLaser MyParent;

    //HitMarker
    GameObject HitMarker;

    // Use this for initialization
    void InitializeStats()
    {
        speed = 500f;
        MyTransform = transform;
        HitMarker = GameObject.Find("PlayerReticle");
    }

    // Update is called once per frame
    void FixedUpdate () 
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
        MyParent.SpawnExplosion(MyTransform.position);
        gameObject.SetActive(false);
    }
    private void SelfDestruct(ChargeLaser _parent)
    {
        MyParent = _parent;
        Invoke("Kill", 3f);
    }
}
