using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatBoard : MonoBehaviour {

    PersistentGameManager gamemanager;

    // Use this for initialization
	void Start () {
        Text[] text = GetComponentsInChildren<Text>();
       
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i].name == "Name")
                text[i].text = PlayerPrefs.GetString("PlayerName");
            else if (text[i].name == "Credit")
                text[i].text = "Credit: " + PlayerPrefs.GetInt("Credits");
            else if (text[i].name == "Health")
                text[i].text = "Max HP: " + PlayerPrefs.GetInt("PlayerHealth");
            else if (text[i].name == "MissileCount")
                text[i].text = "Max Missile: " + PlayerPrefs.GetInt("MissleCount");
        }
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
