using UnityEngine;

public class PersistentGameManager : MonoBehaviour
{

    private static PersistentGameManager theGameManager = null;

    private string saveSlot;

    public static PersistentGameManager Instance
    {
        get
        {
            // instance does not exist
            if (theGameManager == null)
            {
                Debug.Log("No instance of PersistentGameManager currently exists");
                return null;
            }

            return theGameManager;
        }
    }

    void Start()
    {
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
        SetLevelUnlocked(1);
        #endregion

        #region ShopInfo
        SetBasicMissileCount(20);
        SetShieldbreakMissileCount(10);
        SetHasShieldbreakMissile(0);
        SetChromaticMissileCount(10);
        SetHasChromaticMissile(0);
        SetEMPMissileCount(10);
        SetHasEMPMissile(0);

        SetLaserPowerLevel(1);
        SetLaser2PowerLevel(1);
        SetHasLaser2(0);

        SetHealthLevel(1);
        SetShieldLevel(1);
        #endregion

    }

    #region Getters
    public string GetSaveSlot() { return saveSlot; }
    public int GetPlayerCredits() { return PlayerPrefs.GetInt("Credits"); }
    public string GetPlayerName() { return PlayerPrefs.GetString("PlayerName", "Zelda"); }
    public int GetCurrentLevel() { return PlayerPrefs.GetInt("CurrentLevel"); }
    public int GetLevelUnlocked() { return PlayerPrefs.GetInt("LevelUnlocked"); }
    public string GetDifficulty() { return PlayerPrefs.GetString("Difficulty"); }
    #endregion

    #region Setters
    public void SetSaveSlot(string slot) { saveSlot = slot; }
    public void SetPlayerCredits(int CreditCount) { PlayerPrefs.SetInt("Credits", CreditCount); }
    public void SetPlayerName(string PlayerName) { PlayerPrefs.SetString("PlayerName", PlayerName); }
    public void SetCurrentLevel(int Level) { PlayerPrefs.SetInt("CurrentLevel", Level); }
    public void SetLevelUnlocked(int num) { PlayerPrefs.SetInt("LevelUnlocked", num); }
    public void SetDifficulty(string diff) { PlayerPrefs.SetString("Difficulty", diff); }
    #endregion

    #region Consumables
    public int GetBasicMissileCount() { return PlayerPrefs.GetInt("BasicMissileCount"); }
    public void SetBasicMissileCount(int num) { PlayerPrefs.SetInt("BasicMissileCount", num); }

    public int GetEMPMissileCount() { return PlayerPrefs.GetInt("EMPMissileCount"); }
    public void SetEMPMissileCount(int num) { PlayerPrefs.SetInt("EMPMissileCount", num); }

    public int GetShieldbreakMissileCount() { return PlayerPrefs.GetInt("ShieldbreakMissileCount"); }
    public void SetShieldbreakMissileCount(int num) { PlayerPrefs.SetInt("ShieldbreakMissileCount", num); }

    public int GetChromaticMissileCount() { return PlayerPrefs.GetInt("ChromaticMissileCount"); }
    public void SetChromaticMissileCount(int num) { PlayerPrefs.SetInt("ChromaticMissileCount", num); }

    public int GetHasShieldbreakMissile() { return PlayerPrefs.GetInt("HasShieldbreakMissile"); }
    public void SetHasShieldbreakMissile(int has) { PlayerPrefs.SetInt("HasShieldbreakMissile", has); }

    public int GetHasChromaticMissile() { return PlayerPrefs.GetInt("HasChromaticMissile"); }
    public void SetHasChromaticMissile(int has) { PlayerPrefs.SetInt("HasChromaticMissile", has); }

    public int GetHasEMPMissile() { return PlayerPrefs.GetInt("HasEMPMissile"); }
    public void SetHasEMPMissile(int has) { PlayerPrefs.SetInt("HasEMPMissile", has); }
    #endregion

    #region Weapon
    public int GetLaserPowerLevel() { return PlayerPrefs.GetInt("LaserPowerLevel"); }
    public void SetLaserPowerLevel(int level) { PlayerPrefs.SetInt("LaserPowerLevel", level); }

    public int GetLaser2PowerLevel() { return PlayerPrefs.GetInt("Laser2PowerLevel"); }
    public void SetLaser2PowerLevel(int level) { PlayerPrefs.SetInt("Laser2PowerLevel", level); }

    public int GetHasLaser2() { return PlayerPrefs.GetInt("HasLaser2"); }
    public void SetHasLaser2(int has) { PlayerPrefs.SetInt("HasLaser2", has); }
    #endregion

    #region Device
    public int GetHealthLevel() { return PlayerPrefs.GetInt("HealthLevel"); }
    public void SetHealthLevel(int level) { PlayerPrefs.SetInt("HealthLevel", level); }

    public int GetShieldLevel() { return PlayerPrefs.GetInt("ShieldLevel"); }
    public void SetShieldLevel(int level) { PlayerPrefs.SetInt("ShieldLevel", level); }
    #endregion
}
