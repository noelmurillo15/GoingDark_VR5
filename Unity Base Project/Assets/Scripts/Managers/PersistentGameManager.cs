using System;
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
        SetChromaticMissileCount(10);
        SetEMPMissileCount(10);
        #endregion

        #region Settings
        SetOptionInvert(true);
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
    #endregion

    #region Settings
    public void SetOptionInvert(bool boolean) { PlayerPrefs.SetInt("InvertControl", Convert.ToInt32(boolean)); }
    public int GetOptionInvert() { return (PlayerPrefs.GetInt("InvertControl") == 1) ? 1 : -1; }
    #endregion
}