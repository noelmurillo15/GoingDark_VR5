using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionMessages : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
    void Completed(string missionName)
    {
        this.GetComponentInChildren<Text>().text = "Mission '" + missionName + "' is completed";
        StartCoroutine(Messages());
    }

    void Failed(string missionName)
    {
        this.GetComponentInChildren<Text>().text = "Mission '" + missionName + "' failed";
        StartCoroutine(Messages());
    }

    IEnumerator Messages()
    {
        yield return new WaitForSeconds(5.0f);
        this.gameObject.SetActive(false);
    }
}
