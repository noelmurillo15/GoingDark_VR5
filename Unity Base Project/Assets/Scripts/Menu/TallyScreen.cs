using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TallyScreen : MonoBehaviour {

    public Text creditEarned, totalCredit;
    private int earnedCredit, prevCredit, currCredit;

	// Use this for initialization
	void Start () {
        prevCredit = PlayerPrefs.GetInt("Credits");
	}
	
	// Update is called once per frame
	void Update () {
	    currCredit = PlayerPrefs.GetInt("Credits");
        earnedCredit = currCredit - prevCredit;

        creditEarned.text = "Credit Earned: " + earnedCredit;
        totalCredit.text = "Total Credit: " + currCredit;
    }
}
