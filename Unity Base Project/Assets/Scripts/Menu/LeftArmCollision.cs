using UnityEngine;

public class LeftArmCollision : MonoBehaviour
{

    private ArmSettings settings;

    // Use this for initialization
    void Start()
    {
        settings = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<ArmSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        if (settings.Active)
            if (transform.localEulerAngles.z < 100.0f)
                settings.CloseSettings();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3" || col.name == "rightPalm")
        {
            settings.Active = true;
        }
    }
}