using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GoingDark.Core.Enums;

public class MissionMessages : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
    void Completed(string missionName)
    {
        GetComponentInChildren<Text>().text = "Mission '" + missionName + "' is completed";
        StartCoroutine(Messages());
    }

    void Failed(string missionName)
    {
        GetComponentInChildren<Text>().text = "Mission '" + missionName + "' failed";
        StartCoroutine(Messages());
    }

    public void NewMission(string name, string info)
    {
        GetComponentInChildren<Text>().text = "New Mission aquired : " + name + ". " + info;
        StartCoroutine(Messages());
    }

    void TurnInLastMission()
    {
        GetComponentInChildren<Text>().text = "You have completed all your missions. Return to the station for further instructions.";
        StartCoroutine(Messages());
    }

    IEnumerator Messages()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }
}
