using UnityEngine;
using System.Collections;

public class BlackHole : MonoBehaviour
{
    private float yRot = -1.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, yRot, 0.0f);        
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Enemy" || col.gameObject.tag == "Asteroid" || col.gameObject.tag == "TransportShip")
        {
            Debug.Log("Pulling " + col.tag);
            Vector3 pullDirection = new Vector3(transform.position.x, transform.position.y + 70, transform.position.z) - col.transform.position;
            pullDirection.Normalize();
            col.transform.position = col.transform.position + pullDirection * Time.deltaTime * 50.0f;
        }
    }
}