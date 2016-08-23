using UnityEngine;
using System.Xml.Linq;

public class SaveGame : MonoBehaviour
{

    private PersistentGameManager gameManager;
    // Use this for initialization
    void Start()
    {
        gameManager = PersistentGameManager.Instance;
    }

    public void Save(string saveSlot)
    {
        // load in the xml for the specific slot
        XDocument doc = XDocument.Load(Application.dataPath + "\\" + saveSlot + ".xml");
        // save out all the new information
        XElement element = doc.Element(saveSlot);

        string diff = gameManager.GetDifficulty();
        element.Element("difficulty").Value = diff;

        element.Element("playerName").Value = gameManager.GetPlayerName();
        element.Element("credits").Value = gameManager.GetPlayerCredits().ToString();
        element.Element("basicCount").Value = gameManager.GetBasicMissileCount().ToString();
        element.Element("empCount").Value = gameManager.GetEMPMissileCount().ToString();
        element.Element("shieldbreakerCount").Value = gameManager.GetShieldbreakMissileCount().ToString(); ;
        element.Element("chromaticCount").Value = gameManager.GetChromaticMissileCount().ToString();
        element.Element("levelUnlocked").Value = gameManager.GetLevelUnlocked().ToString();

        doc.Save(Application.dataPath + "\\" + saveSlot + ".xml");
    }
}