using UnityEngine;
using System.Collections;

public class ArmSettings : MonoBehaviour {
    
    public bool active;
    public bool camOn;
    public bool monitorOn;
    public GameObject m_settingBG;
    public GameObject m_missileCamBG;
    private GameObject m_Monitor;

    private Cloak playerCloak;
    private PlayerData m_playerData;
    private HyperDrive playerHyperdrive;

    // Use this for initialization
    void Start () {        
        camOn = false;
        active = false;
        monitorOn = true;
        m_settingBG = GameObject.Find("SettingsBG");
        m_missileCamBG = GameObject.Find("MissileKillCam");
        m_Monitor = GameObject.Find("Monitor");

        if (m_playerData == null)
            m_playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        
        playerCloak = m_playerData.GetPlayerCloak();
        playerHyperdrive = m_playerData.GetPlayerHyperDrive();
    }

    // Update is called once per frame
    void Update () {
        m_settingBG.SetActive(active);
        m_Monitor.SetActive(monitorOn);
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
    public void TurnCamOn() {
        camOn = true;
        m_missileCamBG.SetActive(camOn);
        Renderer rend = null;
        rend.material.shader = Shader.Find("Transparent/Diffuse");
        Color tmp = rend.material.color; tmp.a = 0.6f;
        rend.material.color = tmp;
    }

    public void SetCloak() {
        if (playerCloak.GetCloaked()) {
            playerCloak.SetCloaked(false);
            CloseSettings();
        }
        else if (playerCloak.GetCloakCooldown() <= 0.0f) {
            playerCloak.SetCloaked(true);
            CloseSettings();
        }
    }

    public void InitializeHyperDrive() {
        if (playerHyperdrive.GetHyperDriveCooldown() <= 0.0f) {
            playerHyperdrive.HyperDriveInitialize();
            CloseSettings();
        }
    }

    public float HyperDriveCooldown()
    {
        return playerHyperdrive.GetHyperDriveCooldown();
    }

    public float CloakCooldown()
    {
        return playerCloak.GetCloakCooldown();
    }

    public void MonitorPower()
    {
        monitorOn = !monitorOn;
        CloseSettings();
    }
}
