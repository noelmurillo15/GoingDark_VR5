using UnityEngine;
using System.Collections;
using System.Xml.Linq;

public class SaveGame : MonoBehaviour {

    private PlayerStats playerStats;
	// Use this for initialization
	void Start () {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
	}
	
	public void Save(string saveSlot)
    {
        // load in the xml for the specific slot
        XDocument doc = XDocument.Load(Application.dataPath + "\\" + saveSlot + ".xml");
        // save out all the new information
        XElement element = doc.Element(saveSlot);
        if (element == null)
            Debug.Log("Element is null ");
        element.Element("playerName").Value = PlayerPrefs.GetString("PlayerName");
        element.Element("playerName").Value = PlayerPrefs.GetString("PlayerName");
        element.Element("credits").Value = playerStats.SaveData.Credits.ToString();
        element.Element("basicCount").Value = PlayerPrefs.GetInt("BasicMissileCount").ToString();
        element.Element("empCount").Value = PlayerPrefs.GetInt("EMPMissileCount").ToString();
        element.Element("shieldbreakerCount").Value = PlayerPrefs.GetInt("ShieldbreakMissileCount").ToString();
        element.Element("chromaticCount").Value = PlayerPrefs.GetInt("ChromaticMissileCount").ToString();
        element.Element("levelUnlocked").Value = LevelUnlocked().ToString();

        doc.Save(Application.dataPath + "\\" + saveSlot + ".xml");
    }

    private int LevelUnlocked()
    {
        int[] levels = new int[4];
        int unlocked = 0;

        levels[0] = PlayerPrefs.GetInt("Level1Unlocked");
        levels[1] = PlayerPrefs.GetInt("Level2Unlocked");
        levels[2] = PlayerPrefs.GetInt("Level3Unlocked");
        levels[3] = PlayerPrefs.GetInt("Level4Unlocked");

        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i] == 1) // unlocked
                unlocked = i;
            else
                break;
        }

        return unlocked;
    }

}
