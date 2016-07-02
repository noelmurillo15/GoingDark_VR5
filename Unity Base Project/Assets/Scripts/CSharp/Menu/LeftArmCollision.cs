using UnityEngine;

public class LeftArmCollision : MonoBehaviour
{

    [SerializeField] GameObject settings;

    // Use this for initialization
    void Start()
    {
        settings = transform.GetChild(0).gameObject;
        settings.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (settings.activeSelf)
            if (transform.eulerAngles.z > 270.0f)
                settings.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "rightPalm" || col.name == "bone3")
        {
            settings.SetActive(true);
        }
    }
}