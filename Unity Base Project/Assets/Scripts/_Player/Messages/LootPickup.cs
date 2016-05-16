using UnityEngine;
using GD.Core.Enums;

public class LootPickup : MonoBehaviour {

    private SystemType Type { get; set; }
    private bool collected;
    //private GameObject messages;
    private GameObject mission;
    private SystemManager manager;
    private MessageScript messages;
    // Use this for initialization
    void Start () {
        Initialize();
        collected = false;
        mission = GameObject.Find("PersistentGameObject");
        messages = GameObject.Find("WarningMessages").GetComponent<MessageScript>();
        manager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (collected)
            Destroy(this.gameObject);        
    }

    void Initialize()
    {
        switch (transform.name)
        {
            case "ShipPart1":
                Type = SystemType.RADAR;
                break;
            case "ShipPart2":
                Type = SystemType.CLOAK;
                break;
            case "ShipPart3":
                Type = SystemType.MISSILES;
                break;
            case "ShipPart4":
                Type = SystemType.EMP;
                break;
            case "ShipPart5":
                Type = SystemType.DECOY;
                break;
            case "ShipPart6":
                Type = SystemType.LASERS;
                break;

            default:
                Type = SystemType.NONE;
                break;
        }
    }

    void OnTriggerEnter(Collider col) {
        if (col.transform.tag == "Player") {
            manager.SendMessage("InitializeDevice", Type);
            messages.SendMessage("SystemCollection", Type);
            mission.SendMessage("LootPickedUp");
            collected = true;
            AudioManager.instance.PlayCollect();                       
        }
    }
}