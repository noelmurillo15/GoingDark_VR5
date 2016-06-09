using UnityEngine;
using UnityEngine.UI;
using GoingDark.Core.Enums;

public class DisplayCooldown : MonoBehaviour {
    private SystemManager system;
    private Text text;
    public SystemType type;
	// Use this for initialization
	void Start () {
        system = GameObject.Find("Devices").GetComponent<SystemManager>();
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (system.GetActive(type))
        {
            text.text = "ERROR";
        }
        else
        {
            if (system.GetSystemCooldown(type))
                text.text = ((int)system.GetSystemCooldownF(type)).ToString();
            else
                text.text = "0";
        }
	}
}
