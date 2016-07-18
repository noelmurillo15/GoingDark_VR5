using UnityEngine;
using GoingDark.Core.Enums;

public class MissionLog : MonoBehaviour
{
    // message shit, fuck it
    public GameObject m_pMissionMessage;

    private MissionSystem m_missionSystem;

    // Use this for initialization
    void Start()
    {
        m_missionSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionSystem>();
    }


   
    public void Failed(Mission mission)
    {
        m_pMissionMessage.SetActive(true);
        m_pMissionMessage.SendMessage("Failed", mission.missionName);
    }

    public void Completed(Mission mission)
    {
        m_pMissionMessage.SetActive(true);
        m_pMissionMessage.SendMessage("Completed", mission.missionName);
    }

    public void NewMission(Mission mission)
    {
        m_pMissionMessage.SetActive(true);
        m_pMissionMessage.GetComponent<MissionMessages>().NewMission(mission.missionName, mission.missionInfo);
    }

    public void TurnInLastMission()
    {
        m_pMissionMessage.SetActive(true);
        m_pMissionMessage.SendMessage("TurnInLastMission");
    }



}
