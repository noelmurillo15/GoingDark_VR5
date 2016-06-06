using UnityEngine;

public class PersistentGameManager : MonoBehaviour {

    private static PersistentGameManager theGameManager = null;
    // Use this for initialization
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

        SetPlayerResources(100);
        SetPlayerCredits(100);
        SetPlayerName("Captain");
        SetPlayerMissileCount(10);
        SetPlayerHealth(2);
        SetCurrentLevel(1);
        SetLevel1Unlocked(1);
        SetLevel2Unlocked(1);
        SetLevel3Unlocked(0);
        SetLevel4Unlocked(0);
        SetLevel5Unlocked(0);
        SetLevel6Unlocked(0);
    }

    // Update is called once per frame
    void Update() {

    }
    #region Getters

    int GetPlayerResources() { return PlayerPrefs.GetInt("Resources", 0); }
    int GetPlayerCredits() { return PlayerPrefs.GetInt("Credits", 100); }
    string GetPlayerName() { return PlayerPrefs.GetString("PlayerName", "Captain Planet"); }
    int GetPlayerMissileCount() { return PlayerPrefs.GetInt("MissleCount", 10); }
    int GetPlayerHealth() { return PlayerPrefs.GetInt("PlayerHealth", 3); }
    int GetCurrentLevel() { return PlayerPrefs.GetInt("CurrentLevel", 0); }
    int GetLevel1Unlocked() { return PlayerPrefs.GetInt("Level1Unlocked",0); }
    int GetLevel2Unlocked() { return PlayerPrefs.GetInt("Level2Unlocked",0); }
    int GetLevel3Unlocked() { return PlayerPrefs.GetInt("Level3Unlocked",0); }
    int GetLevel4Unlocked() { return PlayerPrefs.GetInt("Level4Unlocked",0); }
    int GetLevel5Unlocked() { return PlayerPrefs.GetInt("Level5Unlocked",0); }
    int GetLevel6Unlocked() { return PlayerPrefs.GetInt("Level6Unlocked",0); }


    #endregion

    #region Setters
    void SetPlayerResources(int ResourceCount) { PlayerPrefs.SetInt("Resources", ResourceCount); }
    void SetPlayerCredits(int CreditCount) { PlayerPrefs.SetInt("Credits", CreditCount); }
    void SetPlayerName(string PlayerName) { PlayerPrefs.SetString("PlayerName", PlayerName); }
    void SetPlayerMissileCount(int MissileCount) { PlayerPrefs.SetInt("MissleCount", MissileCount); }
    void SetPlayerHealth(int Health) { PlayerPrefs.SetInt("PlayerHealth", Health); }
    void SetCurrentLevel(int Level) { PlayerPrefs.SetInt("CurrentLevel", Level); }
    void SetLevel1Unlocked(int isUnlocked) { PlayerPrefs.SetInt("Level1Unlocked", isUnlocked); }
    void SetLevel2Unlocked(int isUnlocked) { PlayerPrefs.SetInt("Level2Unlocked", isUnlocked); }
    void SetLevel3Unlocked(int isUnlocked) { PlayerPrefs.SetInt("Level3Unlocked", isUnlocked); }
    void SetLevel4Unlocked(int isUnlocked) { PlayerPrefs.SetInt("Level4Unlocked", isUnlocked); }
    void SetLevel5Unlocked(int isUnlocked) { PlayerPrefs.SetInt("Level5Unlocked", isUnlocked); }
    void SetLevel6Unlocked(int isUnlocked) { PlayerPrefs.SetInt("Level6Unlocked", isUnlocked); }

    #endregion
}
