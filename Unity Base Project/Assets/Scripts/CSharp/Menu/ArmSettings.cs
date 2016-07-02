using UnityEngine;

public class ArmSettings : MonoBehaviour {
    
    public bool Active { get; set; }
    public GameObject settings;

    // Use this for initialization
    void Start () {
        Active = false; 
    }

    // Update is called once per frame
    void Update () {
        if(settings.activeSelf != Active)
            settings.SetActive(Active);
	}

    public void CloseSettings() {
        Active = false;
    }
}
