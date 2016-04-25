using UnityEngine;
using System.Collections;

public class SpinScript : MonoBehaviour
{
    public float x = 0.0f;
    public float y = 0.0f;
    public float z = 0.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(x, y, z), 2.0f * Time.deltaTime);
    }
}
