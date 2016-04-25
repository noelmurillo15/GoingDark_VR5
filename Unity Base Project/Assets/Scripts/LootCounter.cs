using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LootCounter : MonoBehaviour {

    [SerializeField]
    PlayerData playerData;
    [SerializeField]
    Text textCount;

    public int lootCounter;
	// Use this for initialization
	void Start () {
        //lootCounter = 2;
       
	}
	
	// Update is called once per frame
	void Update () {
        textCount.text = lootCounter.ToString();
	}
}
