using UnityEngine;

public class Despawn : MonoBehaviour
{
    public float Duration;

    void Initialize()
    {
        Invoke("Kill", Duration);
    }

    // Update is called once per frame
    void Kill()
    {
        Destroy(this.gameObject);
    }
}
