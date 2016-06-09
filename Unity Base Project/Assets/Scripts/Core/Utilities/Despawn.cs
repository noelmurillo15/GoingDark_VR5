using UnityEngine;

public class Despawn : MonoBehaviour
{
    public float Duration;

    void Start()
    {
        Invoke("Kill", Duration);
    }

    // Update is called once per frame
    void Kill()
    {
        Destroy(this.gameObject);
    }
}
