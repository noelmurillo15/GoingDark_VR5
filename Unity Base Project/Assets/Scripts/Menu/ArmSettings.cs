using UnityEngine;

public class ArmSettings : MonoBehaviour {
    
    public bool active;
    public GameObject m_settingBG;

    private PlayerShipData m_playerData;
    private ShootObject m_playerMissiles;

    private PlayerShipData stats;

    // Use this for initialization
    void Start () {
        active = false;
        m_settingBG = GameObject.Find("SettingsBG");
        m_playerMissiles = GameObject.FindGameObjectWithTag("Player").GetComponent<ShootObject>();
        m_playerData = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<PlayerShipData>();           
        stats = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<PlayerShipData>();
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

    public void FireMissile() {
        if (m_playerMissiles.GetFireCooldown() <= 0.0f)
        {
            m_playerMissiles.FireMissile();
            AudioManager.instance.PlayMissileLaunch();
        }
    }

    public void SetCloak() {
        if (stats.GetPlayerCloak().GetCloaked()) {
            stats.GetPlayerCloak().SetCloaked(false);
            CloseSettings();
        }
        else if (stats.GetPlayerCloak().GetCloakCooldown() <= 0.0f) {
            stats.GetPlayerCloak().SetCloaked(true);
            AudioManager.instance.PlayCloak();
            CloseSettings();
        }
    }

    public void InitializeHyperDrive() {
        if (stats.GetPlayerHyperDrive().GetHyperDriveCooldown() <= 0.0f) {
            stats.GetPlayerHyperDrive().HyperDriveInitialize();
            CloseSettings();
        }
    }

    public float HyperDriveCooldown() {
        return stats.GetPlayerHyperDrive().GetHyperDriveCooldown();
    }

    public float CloakCooldown() {
        return stats.GetPlayerCloak().GetCloakCooldown();
    }
}
