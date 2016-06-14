using UnityEngine;
using GoingDark.Core.Enums;

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
        messages = GameObject.Find("PlayerCanvas").GetComponent<MessageScript>();
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
                Type = SystemType.Cloak;
                break;
            case "ShipPart2":
                Type = SystemType.Missile;
                break;
            case "ShipPart3":
                Type = SystemType.Emp;
                break;
            case "ShipPart4":
                Type = SystemType.Decoy;
                break;
            case "ShipPart5":
                Type = SystemType.Laser;
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