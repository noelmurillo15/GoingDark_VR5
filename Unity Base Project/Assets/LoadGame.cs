using UnityEngine;
using System.Collections;
using System.Xml.Linq;

public class LoadGame : MonoBehaviour
{

    private PersistentGameManager gameManager;
    // Use this for initialization
    void Start()
    {
        // sigh
        gameManager = PersistentGameManager.Instance;
    }

    public void Load(string saveSlot)
    {
        // load in the xml for the specific slot
        XDocument doc = XDocument.Load(Application.dataPath + "\\" + saveSlot + ".xml");
        XElement element = doc.Element(saveSlot);

        gameManager.SetPlayerCredits(int.Parse(element.Element("credits").Value));
        gameManager.SetPlayerName(element.Element("playerName").Value);
        gameManager.SetBasicMissileCount(int.Parse(element.Element("basicCount").Value));
        gameManager.SetChromaticMissileCount(int.Parse(element.Element("chromaticCount").Value));
        gameManager.SetEMPMissileCount(int.Parse(element.Element("empCount").Value));
        gameManager.SetShieldbreakMissileCount(int.Parse(element.Element("shieldbreakerCount").Value));
        gameManager.SetLevelUnlocked(int.Parse(element.Element("levelUnlocked").Value));
        gameManager.SetDifficulty(element.Element("difficulty").Value);
    }

    public string IsSlotUsed(string slot)
    {
        // load in the xml for the specific slot
        XDocument doc = XDocument.Load(Application.dataPath + "\\" + slot + ".xml");
        XElement element = doc.Element(slot);
        return element.Element("playerName").Value;
    }


}
