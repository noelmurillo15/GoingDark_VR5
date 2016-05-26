using UnityEngine;

public class LaserProjectile : MonoBehaviour {

    public float speed = 1.0f;
    public float lifetime = 2.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.Translate(0f, 0f, speed * Time.deltaTime);
        lifetime -= Time.deltaTime;
        if (lifetime < 0.0f)
            Destroy(gameObject);
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Enemy"))
        {
            col.gameObject.SendMessage("ShieldHit"); 
            col.gameObject.SendMessage("Hit");
            Destroy(gameObject);
        }

        if (col.transform.CompareTag("Asteroid"))
        {
            col.transform.SendMessage("Kill");
            Destroy(gameObject);
        }
    }
}
