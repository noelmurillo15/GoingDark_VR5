using UnityEngine;

public class SpinScript : MonoBehaviour
{
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public float speed = 0f;
    Vector3 spin;
    // Use this for initialization
    void Start()
    {
        spin = new Vector3(x, y, z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.Rotate(spin, speed * Time.fixedDeltaTime);
    }

    void OnBecameVisible()
    {
        enabled = true;
    }

    void OnBecameInvisible()
    {
        enabled = false;
    }
}
