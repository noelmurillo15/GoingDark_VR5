using UnityEngine;
using UnityEngine.UI;

public class LootCounter : MonoBehaviour {

    public int lootCounter;
    private GameObject messages;

    [SerializeField]
    private Text textCount;


	// Use this for initialization
	void Start () {
        lootCounter = 4;
        messages = GameObject.Find("Screen");
    }
	
	// Update is called once per frame
	void Update () {
        textCount.text = lootCounter.ToString();
	}    

    public void DecreaseLootCount() {
        messages.SendMessage("LootPickUp");
    }

    public int GetLootCount() {
        return lootCounter;
    }
}