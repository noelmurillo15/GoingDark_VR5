using UnityEngine;

public class SpinScript : MonoBehaviour
{
    #region Properties
    [SerializeField]
    private float x = 0f;
    [SerializeField]
    private float y = 0f;
    [SerializeField]
    private float z = 0f;
    [SerializeField]
    private float speed = 0f;

    Vector3 spin;
    Transform MyTransform;
    #endregion


    // Use this for initialization
    void Start()
    {
        MyTransform = transform;
        spin = new Vector3(x, y, z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MyTransform.Rotate(spin, speed * Time.fixedDeltaTime);
    }
}
