using UnityEngine;

public class SpaceStation : MonoBehaviour {


    private MissionSystem m_missionSystem;
    private MissionLog m_missionLog;

	// Use this for initialization
	void Start () {
        m_missionLog = GameObject.Find("MissionLog").GetComponent<MissionLog>();
        m_missionSystem = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        // if player entered the space station, let them turn in missions
        if (col.transform.tag == "Player")
        {
            m_missionLog.SendMessage("AtStation");
        }
    }

    void OnTriggerExti()
    {
        m_missionLog.SendMessage("LeavingStation");
    }
}
