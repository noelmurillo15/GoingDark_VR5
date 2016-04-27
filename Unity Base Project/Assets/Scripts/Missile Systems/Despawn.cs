using UnityEngine;
using System.Collections;

public class Despawn : MonoBehaviour
{
    public float Duration;
    // Use this for initialization
    void Start()
    {
        Duration = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Duration > 0.0f)
            Duration -= Time.deltaTime;
        else
            Destroy(this.gameObject);
    }
}
