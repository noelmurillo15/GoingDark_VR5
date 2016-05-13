using UnityEngine;
using System.Collections;

public class LaserProjectile : MonoBehaviour {

    public float speed = 1.0f;
    public float lifetime = 2.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
        lifetime -= Time.deltaTime;
        if (lifetime < 0.0f)
            Destroy(this.gameObject);
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Asteroid" || col.gameObject.tag == "TransportShip")
        {
            col.gameObject.SendMessage("Kill");
            Destroy(this.gameObject);
        }
    }
}
