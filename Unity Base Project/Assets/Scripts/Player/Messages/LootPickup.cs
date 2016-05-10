using UnityEngine;

public class LootPickup : MonoBehaviour {

    private bool collected;

    private GameObject messages;
    // Use this for initialization
    void Start () {
        collected = false;
        messages = GameObject.Find("Screen");
    }
	
	// Update is called once per frame
	void Update () {
	    if (collected)
            Destroy(this.gameObject);        
    }

    void OnTriggerEnter(Collider col) {
        if (col.transform.tag == "Player") {
            //LootCounter lootcounter = GameObject.Find("LootCounter").GetComponent<LootCounter>();
            //lootcounter.DecreaseLootCount();
            messages.SendMessage("LootPickUp");

            collected = true;
            AudioManager.instance.PlayCollect();                       
        }
    }
}