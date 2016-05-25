using UnityEngine;
using System.Collections;

public class ContinuousLaserGrowth : MonoBehaviour
{
    private BoxCollider box;
    private VolumetricLines.VolumetricLineBehavior line;
    public float max = 500.0f;
    // Use this for initialization
    void Start()
    {
        box = GetComponent<BoxCollider>();
        line = GetComponent<VolumetricLines.VolumetricLineBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if(line.EndPos.z < max)
        {
            line.EndPos.Set(line.EndPos.x, line.EndPos.y, line.EndPos.z+ 1);
            box.center.Set(box.center.x, box.center.y, box.center.z + 0.5f);
            box.size.Set(box.size.x, box.size.y, box.size.z + 1);
        }
    }
}
