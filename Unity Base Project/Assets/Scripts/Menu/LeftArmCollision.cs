using UnityEngine;

public class LeftArmCollision : MonoBehaviour
{

    [SerializeField] GameObject settings;

    // Use this for initialization
    void Start()
    {
        settings = GameObject.Find("SettingsBG");
    }

    // Update is called once per frame
    void Update()
    {
        if (settings.activeSelf)
            if (transform.localEulerAngles.z < 100.0f)
                settings.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3" || col.name == "rightPalm")
        {
            settings.SetActive(true);
        }
    }
}