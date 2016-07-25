using UnityEngine;

public class PersistentGameManager : MonoBehaviour {

    private static PersistentGameManager theGameManager = null;


    void Start() {
        if (theGameManager == null)
        {
            theGameManager = this;
            DontDestroyOnLoad(theGameManager);
        }
        else
        {
            DestroyImmediate(this);
            return;
        }

        Initialize();
    }

    void Initialize()
    {
        #region GeneralInfo
        SetPlayerName("Sheik");
        SetPlayerCredits(1000);
        SetCurrentLevel(1);
        SetLevel1Unlocked(1);
        SetLevel2Unlocked(1);
        SetLevel3Unlocked(1);
        SetLevel4Unlocked(1);
        #endregion

        #region ShopInfo
        SetBasicMissileCount(20);
        SetShieldbreakMissileCount(10);
        SetChromaticMissileCount(10);
        SetEMPMissileCount(10);

        SetBasicMissileLevel(1);
        SetShieldbreakMissileLevel(1);
        SetChromaticMissileLevel(1);
        SetEMPMissileLevel(1);
        SetLaserPowerLevel(1);
        SetLaserCooldownLevel(1);

        SetHealthLevel(1);
        SetShieldLevel(1);
        SetHyperdriveLevel(1);
        SetCloakLevel(1);
        SetEMPLevel(1);
        #endregion

    }

    #region Getters
    int GetPlayerCredits() { return PlayerPrefs.GetInt("Credit"); }
    string GetPlayerName() { return PlayerPrefs.GetString("PlayerName", "Zelda"); }
    int GetCurrentLevel() { return PlayerPrefs.GetInt("CurrentLevel"); }
    int GetLevel1Unlocked() { return PlayerPrefs.GetInt("Level1Unlocked"); }
    int GetLevel2Unlocked() { return PlayerPrefs.GetInt("Level2Unlocked"); }
    int GetLevel3Unlocked() { return PlayerPrefs.GetInt("Level3Unlocked"); }
    int GetLevel4Unlocked() { return PlayerPrefs.GetInt("Level4Unlocked"); }
    #endregion

    #region Setters
    void SetPlayerCredits(int CreditCount) { PlayerPrefs.SetInt("Credit", CreditCount); }
    void SetPlayerName(string PlayerName) { PlayerPrefs.SetString("PlayerName", PlayerName); }
    void SetCurrentLevel(int Level) { PlayerPrefs.SetInt("CurrentLevel", Level); }
    void SetLevel1Unlocked(int isUnlocked) { PlayerPrefs.SetInt("Level1Unlocked", isUnlocked); }
    void SetLevel2Unlocked(int isUnlocked) { PlayerPrefs.SetInt("Level2Unlocked", isUnlocked); }
    void SetLevel3Unlocked(int isUnlocked) { PlayerPrefs.SetInt("Level3Unlocked", isUnlocked); }
    void SetLevel4Unlocked(int isUnlocked) { PlayerPrefs.SetInt("Level4Unlocked", isUnlocked); }
    #endregion

    #region Shop
    #region Consumables
    int GetBasicMissileCount() { return PlayerPrefs.GetInt("BasicMissileCount"); }
    void SetBasicMissileCount(int num) { PlayerPrefs.SetInt("BasicMissileCount",num); }

    int GetEMPMissileCount() { return PlayerPrefs.GetInt("EMPMissileCount"); }
    void SetEMPMissileCount(int num) { PlayerPrefs.SetInt("EMPMissileCount", num); }

    int GetShieldbreakMissileCount() { return PlayerPrefs.GetInt("ShieldbreakMissileCount"); }
    void SetShieldbreakMissileCount(int num) { PlayerPrefs.SetInt("ShieldbreakMissileCount", num); }

    int GetChromaticMissileCount() { return PlayerPrefs.GetInt("ChromaticMissileCount"); }
    void SetChromaticMissileCount(int num) { PlayerPrefs.SetInt("ChromaticMissileCount", num); }
    #endregion

    #region WeaponUpgrades
    int GetBasicMissileLevel() { return PlayerPrefs.GetInt("BasicMissileLevel"); }
    void SetBasicMissileLevel(int level) { PlayerPrefs.SetInt("BasicMissileLevel", level); }

    int GetShieldbreakMissileLevel() { return PlayerPrefs.GetInt("ShieldbreakMissileLevel"); }
    void SetShieldbreakMissileLevel(int level) { PlayerPrefs.SetInt("ShieldbreakMissileLevel", level); }

    int GetChromaticMissileLevel() { return PlayerPrefs.GetInt("ChromaticMissileLevel"); }
    void SetChromaticMissileLevel(int level) { PlayerPrefs.SetInt("ChromaticMissileLevel", level); }

    int GetEMPMissileLevel() { return PlayerPrefs.GetInt("EMPMissileLevel"); }
    void SetEMPMissileLevel(int level) { PlayerPrefs.SetInt("EMPMissileLevel", level); }

    int GetLaserPowerLevel() { return PlayerPrefs.GetInt("LaserPowerLevel"); }
    void SetLaserPowerLevel(int level) { PlayerPrefs.SetInt("LaserPowerLevel", level); }

    int GetLaserCooldownLevel() { return PlayerPrefs.GetInt("LaserCooldownLevel"); }
    void SetLaserCooldownLevel(int level) { PlayerPrefs.SetInt("LaserCooldownLevel", level); }

    //int GetHasShieldbreakMissile() { return PlayerPrefs.GetInt("HasShieldbreakMissile"); }
    //void SetHasShieldbreakMissile(int has) { PlayerPrefs.SetInt("HasShieldbreakMissile", has); }

    //int GetHasChromaticMissile() { return PlayerPrefs.GetInt("HasChromaticMissile"); }
    //void SetHasChromaticMissile(int has) { PlayerPrefs.SetInt("HasChromaticMissile", has); }

    //int GetHasEMPMissile() { return PlayerPrefs.GetInt("HasEMPMissile"); }
    //void SetHasEMPMissile(int level) { PlayerPrefs.SetInt("HasEMPMissile", level); }
    #endregion

    #region DeviceUpdrages
    int GetHealthLevel() { return PlayerPrefs.GetInt("HealthLevel"); }
    void SetHealthLevel(int level) { PlayerPrefs.SetInt("HealthLevel", level); }

    int GetShieldLevel() { return PlayerPrefs.GetInt("ShieldLevel"); }
    void SetShieldLevel(int level) { PlayerPrefs.SetInt("ShieldLevel", level); }

    int GetHyperdriveLevel() { return PlayerPrefs.GetInt("HyperdriveLevel"); }
    void SetHyperdriveLevel(int level) { PlayerPrefs.SetInt("HyperdriveLevel", level); }

    int GetCloakLevel() { return PlayerPrefs.GetInt("CloakLevel"); }
    void SetCloakLevel(int level) { PlayerPrefs.SetInt("CloakLevel", level); }

    int GetEMPLevel() { return PlayerPrefs.GetInt("EMPLevel"); }
    void SetEMPLevel(int level) { PlayerPrefs.SetInt("EMPLevel", level); }
    #endregion

    #endregion
}
