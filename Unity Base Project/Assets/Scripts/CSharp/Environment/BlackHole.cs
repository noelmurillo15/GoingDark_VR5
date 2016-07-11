using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float yRot = -3.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, yRot, 0.0f);        
    }

    void OnBecameVisible()
    {
        enabled = true;
    }

    void OnBecameInvisible()
    {
        enabled = false;
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Enemy" || col.gameObject.tag == "Asteroid")
        {
            Vector3 pullDirection = new Vector3(transform.position.x, transform.position.y, transform.position.z) - col.transform.position;
            pullDirection.Normalize();
            //float distance = (transform.position - col.transform.position).magnitude;
            col.transform.position = col.transform.position + pullDirection * Time.deltaTime * 50.0f;
            if (col.gameObject.tag == "Player")
                AudioManager.instance.LowerVolume();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
            AudioManager.instance.RaiseVolume();
    }
}
