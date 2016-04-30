using UnityEngine;
using System.Collections;

public class ArmSettings : MonoBehaviour {
    
    public bool active;
    public GameObject m_settingBG;

    private Cloak playerCloak;
    private PlayerShipData m_playerData;
    private HyperDrive playerHyperdrive;
    private ShootObject m_playerMissiles;

    // Use this for initialization
    void Start () {
        Debug.Log("Arm Start");
        active = false;

        m_settingBG = GameObject.Find("SettingsBG");
        m_playerMissiles = GameObject.FindGameObjectWithTag("Player").GetComponent<ShootObject>();
        m_playerData = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<PlayerShipData>();       
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
        if(m_playerMissiles.GetFireCooldown() <= 0.0f)
            m_playerMissiles.FireMissile();
    }

    public void SetCloak() {
        if(playerCloak == null)
            playerCloak = m_playerData.GetPlayerCloak();

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
        if(playerHyperdrive == null)
            playerHyperdrive = m_playerData.GetPlayerHyperDrive();

        if (playerHyperdrive.GetHyperDriveCooldown() <= 0.0f) {
            playerHyperdrive.HyperDriveInitialize();
            CloseSettings();
        }
    }

    public float HyperDriveCooldown() {
        if (playerHyperdrive == null)
            playerHyperdrive = m_playerData.GetPlayerHyperDrive();

        return playerHyperdrive.GetHyperDriveCooldown();
    }

    public float CloakCooldown() {
        if (playerCloak == null)
            playerCloak = m_playerData.GetPlayerCloak();

        return playerCloak.GetCloakCooldown();
    }
}
