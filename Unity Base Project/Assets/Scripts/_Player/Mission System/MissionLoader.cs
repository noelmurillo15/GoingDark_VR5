using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Xml.Linq;


public class MissionLoader : MonoBehaviour {


    public MissionSystem.Mission[] m_missions;

	// Use this for initialization
	void Start () {
        m_missions = new MissionSystem.Mission[4];
	}


    public MissionSystem.Mission[] LoadMissions()
    {
        Debug.Log("Loading Missions");
        int count = 0;
        string levelName = SceneManager.GetActiveScene().name;

        // load in the file into an element
        XElement mRoot = XElement.Load(@"Assets\Resources\" + levelName + ".xml");
        
        // get the list of elements in the file
        IEnumerable<XElement> missonList = mRoot.Elements();

        // get each attribute separately
        foreach (XElement mission in missonList)
        {
            // get name of mission
            XAttribute attribute = mission.Attribute("name");
            m_missions[count].missionName = attribute.Value;
            // get mission info
            attribute = mission.Attribute("info");
            m_missions[count].missionInfo = attribute.Value;
            // get number of credits
            attribute = mission.Attribute("objectives");
            m_missions[count].objectives = int.Parse(attribute.Value);
            // get mission type (Scavenge, Combat, Stealth)
            attribute = mission.Attribute("type");
            // convert string into mission type
            m_missions[count].type = ConvertType(attribute.Value);
            // get enemy type for mission
            attribute = mission.Attribute("enemy");
            m_missions[count].enemy = ConvertEnemy(attribute.Value);
            // get timer for mission
            attribute = mission.Attribute("time");
            m_missions[count].missionTimer = float.Parse(attribute.Value);
            // get value for optional/non optional missions
            attribute = mission.Attribute("optional");
            m_missions[count].isOptional = bool.Parse(attribute.Value);
            // increase index count used for missions
            count++;
        }

        return m_missions;

    }

    /// <summary>
    /// Converts string loaded from XML to valid MissionSystem.MissionType
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    MissionSystem.MissionType ConvertType(string type)
    {
        MissionSystem.MissionType ret = MissionSystem.MissionType.SCAVENGE;

        if (type == "Combat")
            ret = MissionSystem.MissionType.COMBAT;
        else if (type == "Stealth")
            ret = MissionSystem.MissionType.STEALTH;

        return ret;
    }

    /// <summary>
    /// Converts enemy string from XML Doc to valid MissionSystem.EnemyType
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    MissionSystem.EnemyType ConvertEnemy(string enemy)
    {
        MissionSystem.EnemyType ret = MissionSystem.EnemyType.NONE;

        if (enemy == "BasicEnemy")
            ret = MissionSystem.EnemyType.BASIC_ENEMY;
        else if (enemy == "Kamikaze")
            ret = MissionSystem.EnemyType.KAMIKAZE;
        else if (enemy == "Transport")
            ret = MissionSystem.EnemyType.TRANSPORT;
        else if (enemy == "Any")
            ret = MissionSystem.EnemyType.ANY;

        return ret;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
