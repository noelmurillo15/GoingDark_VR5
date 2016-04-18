using UnityEngine;
using System.Collections;

public class ArmSettings : MonoBehaviour {
    
    public bool active;
    public GameObject m_settingBG;

    // Use this for initialization
    void Start () {
        active = false;
        m_settingBG = GameObject.Find("SettingsBG");
    }

    // Update is called once per frame
    void Update () {
        m_settingBG.SetActive(active);

        if (active)
            if (transform.localEulerAngles.z < 100.0f)
                CloseSettings();
	}

    void OnTriggerEnter(Collider col) {
        if (col.name == "bone3" && col.transform.parent.name == "rightIndex")        
            active = true;
    }

    public bool GetIsActive() {
        return active;
    }

    public void CloseSettings() {
        active = false;
    }
}
