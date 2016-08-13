using UnityEngine;
using System.Xml.Linq;

public class SaveGame : MonoBehaviour
{

    private PlayerStats playerStats;
    private PersistentGameManager gameManager;
    // Use this for initialization
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        gameManager = PersistentGameManager.Instance;
    }

    public void Save(string saveSlot)
    {
        // load in the xml for the specific slot
        XDocument doc = XDocument.Load(Application.dataPath + "\\" + saveSlot + ".xml");
        // save out all the new information
        XElement element = doc.Element(saveSlot);

        element.Element("playerName").Value = PlayerPrefs.GetString("PlayerName");
        element.Element("playerName").Value = PlayerPrefs.GetString("PlayerName");
        element.Element("credits").Value = playerStats.GetCredits().ToString();
        element.Element("basicCount").Value = PlayerPrefs.GetInt("BasicMissileCount").ToString();
        element.Element("empCount").Value = PlayerPrefs.GetInt("EMPMissileCount").ToString();
        element.Element("shieldbreakerCount").Value = PlayerPrefs.GetInt("ShieldbreakMissileCount").ToString();
        element.Element("chromaticCount").Value = PlayerPrefs.GetInt("ChromaticMissileCount").ToString();
        element.Element("levelUnlocked").Value = gameManager.GetLevelUnlocked().ToString();

        doc.Save(Application.dataPath + "\\" + saveSlot + ".xml");
    }
}