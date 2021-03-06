﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Xml.Linq;
using GoingDark.Core.Enums;

public class MissionLoader : MonoBehaviour
{

    public List<Mission> LoadMissions(string fileName)
    {
        // load in the file into an element
        XElement mRoot = XElement.Load(Application.dataPath + "\\" + fileName + ".xml");

        // get the list of elements in the file
        IEnumerable<XElement> missonList = mRoot.Elements();
        List<Mission> tempList = new List<Mission>();
        // get each attribute separately
        foreach (XElement mission in missonList)
        {
            Mission tempMission = new Mission();
            // get name of mission
            XAttribute attribute = mission.Attribute("name");
            tempMission.missionName = attribute.Value;
            // get mission info
            attribute = mission.Attribute("info");
            tempMission.missionInfo = attribute.Value;
            // get number of credits
            attribute = mission.Attribute("credits");
            tempMission.credits = int.Parse(attribute.Value);
            // get number of objectives
            attribute = mission.Attribute("objectives");
            tempMission.objectives = int.Parse(attribute.Value);
            // get mission type (Scavenge, Combat, Stealth)
            attribute = mission.Attribute("type");
            // convert string into mission type
            tempMission.type = ConvertType(attribute.Value);
            // get enemy type for mission
            attribute = mission.Attribute("enemy");
            tempMission.enemy = ConvertEnemy(attribute.Value);
            // get timer for mission
            attribute = mission.Attribute("time");
            tempMission.missionTimer = float.Parse(attribute.Value);
            // get value for optional/non optional missions
            attribute = mission.Attribute("optional");
            tempMission.isOptional = bool.Parse(attribute.Value);
            // get string for blueprint 
            attribute = mission.Attribute("blueprint");
            tempMission.blueprint = attribute.Value;

            tempList.Add(tempMission);
        }

        return tempList;
    }

    public Mission LoadMission(string fileName)
    {
        // load in the file into an element
        XElement mRoot = XElement.Load(@"Assets\Resources\XML\" + fileName + ".xml");

        // get the list of elements in the file
        IEnumerable<XElement> missonList = mRoot.Elements();
        Mission returnMission = new Mission();
        // get each attribute separately

        foreach (XElement mission in missonList)
        {
            Mission tempMission = new Mission();
            // get name of mission
            XAttribute attribute = mission.Attribute("name");
            tempMission.missionName = attribute.Value;
            // get mission info
            attribute = mission.Attribute("info");
            tempMission.missionInfo = attribute.Value;
            // get number of credits
            attribute = mission.Attribute("credits");
            tempMission.credits = int.Parse(attribute.Value);
            // get number of objectives
            attribute = mission.Attribute("objectives");
            tempMission.objectives = int.Parse(attribute.Value);
            // get mission type (Scavenge, Combat, Stealth)
            attribute = mission.Attribute("type");
            // convert string into mission type
            tempMission.type = ConvertType(attribute.Value);
            // get enemy type for mission
            attribute = mission.Attribute("enemy");
            tempMission.enemy = ConvertEnemy(attribute.Value);
            // get timer for mission
            attribute = mission.Attribute("time");
            tempMission.missionTimer = float.Parse(attribute.Value);
            // get value for optional/non optional missions
            attribute = mission.Attribute("optional");
            tempMission.isOptional = bool.Parse(attribute.Value);

            returnMission = tempMission;
        }

        return returnMission;
    }

    /// <summary>
    /// Converts string loaded from XML to valid MissionType
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    MissionType ConvertType(string type)
    {
        MissionType ret = MissionType.Scavenge;

        if (type == "Combat")
            ret = MissionType.Combat;
        else if (type == "Stealth")
            ret = MissionType.Stealth;
        else if (type == "Elimination")
            ret = MissionType.Elimination;
        else if (type == "ControlPoint")
            ret = MissionType.ControlPoint;
        else if (type == "RockBreak")
            ret = MissionType.RockBreak;

        return ret;
    }

    /// <summary>
    /// Converts enemy string from XML Doc to valid EnemyType
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    EnemyTypes ConvertEnemy(string enemy)
    {
        EnemyTypes ret = EnemyTypes.None;

        if (enemy == "BasicEnemy")
            ret = EnemyTypes.Basic;
        else if (enemy == "Droid")
            ret = EnemyTypes.Droid;
        else if (enemy == "Transport")
            ret = EnemyTypes.Transport;
        else if (enemy == "Trident")
            ret = EnemyTypes.Trident;
        else if (enemy == "FinalBoss")
            ret = EnemyTypes.FinalBoss;
        else if (enemy == "Tank")
            ret = EnemyTypes.Tank;
        else if (enemy == "JetFighter")
            ret = EnemyTypes.JetFighter;
        else if (enemy == "SquadLead")
            ret = EnemyTypes.SquadLead;
        else if (enemy == "Any")
            ret = EnemyTypes.Any;

        return ret;
    }

}
