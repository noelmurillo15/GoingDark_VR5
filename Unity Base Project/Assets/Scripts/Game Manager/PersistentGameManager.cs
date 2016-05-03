using UnityEngine;
using System.Collections;

public class PersistentGameManager : MonoBehaviour {

    private static PersistentGameManager theGameManager = null;
	// Use this for initialization
	void Start () {
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
	}
	
	// Update is called once per frame
	void Update () {

	}
    #region Getters

    int GetPlayerResources() { return PlayerPrefs.GetInt("Resources", 0); }
    int GetPlayerCredits() { return PlayerPrefs.GetInt("Credits", 0); }
    string GetPlayerName() { return PlayerPrefs.GetString("PlayerName", "Captain Planet"); }
    int GetPlayerMissileCount() { return PlayerPrefs.GetInt("MissleCount", 10); }
    int GetPlayerHealth() { return PlayerPrefs.GetInt("PlayerHealth", 3); }
    int GetCurrentLevel() { return PlayerPrefs.GetInt("CurrentLevel", 0); }



    #endregion

    #region Setters
    void SetPlayerResources(int ResourceCount) { PlayerPrefs.SetInt("Resources", ResourceCount); }
    void SetPlayerCredits(int CreditCount) { PlayerPrefs.SetInt("Credits", CreditCount); }
    void SetPlayerName(string PlayerName) { PlayerPrefs.SetString("PlayerName", PlayerName); }
    void SetPlayerMissileCount(int MissileCount) { PlayerPrefs.SetInt("MissleCount", MissileCount); }
    void SetPlayerHealth(int Health) { PlayerPrefs.SetInt("PlayerHealth", Health); }
    void SetCurrentLevel(int Level) { PlayerPrefs.SetInt("CurrentLevel", Level); }


    #endregion

}
