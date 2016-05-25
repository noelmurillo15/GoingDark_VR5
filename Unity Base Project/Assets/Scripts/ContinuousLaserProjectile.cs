using UnityEngine;
using System.Collections;

public class ContinuousLaserProjectile : MonoBehaviour
{
    public float lifetime = 8.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0.0f)
            Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy" /*|| col.gameObject.tag == "Asteroid"*/ || col.gameObject.tag == "TransportShip")
        {
            col.gameObject.SendMessage("Kill");
        }
    }
}
