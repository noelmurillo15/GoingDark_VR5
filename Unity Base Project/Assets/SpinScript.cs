using UnityEngine;
using System.Collections;

public class SpinScript : MonoBehaviour
{
    public float x;
    public float y;
    public float z;
    // Use this for initialization
    void Start()
    {
        x = Random.Range(0.0f, 360.0f);
        y = Random.Range(0.0f, 360.0f);
        z = Random.Range(0.0f, 360.0f);
        gameObject.transform.localEulerAngles = new Vector3(x, y, z);
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.Rotate(new Vector3(x, y, z), 2.0f * Time.deltaTime);
    }
}
