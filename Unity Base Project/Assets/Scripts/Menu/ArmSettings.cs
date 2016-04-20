using UnityEngine;
using System.Collections;

public class ArmSettings : MonoBehaviour {
    
    public bool active;
    public bool camOn;
    public GameObject m_settingBG;
    public GameObject m_missileCamBG;

    public Renderer rend;

    // Use this for initialization
    void Start () {        
        active = false;
        m_settingBG = GameObject.Find("SettingsBG");
        m_missileCamBG = GameObject.Find("MissileKillCam");        
    }

    // Update is called once per frame
    void Update () {
        m_settingBG.SetActive(active);
        m_missileCamBG.SetActive(camOn);

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
        camOn = false;
    }
    public void TurnCamOn()
    {
        camOn = true;
        m_missileCamBG.SetActive(camOn);
        rend.material.shader = Shader.Find("Transparent/Diffuse");
        Color tmp = rend.material.color;
        tmp.a = 0.6f;
        rend.material.color = tmp;
    }
}
