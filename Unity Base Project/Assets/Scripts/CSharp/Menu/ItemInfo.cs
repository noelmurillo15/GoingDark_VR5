using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GoingDark.Core.Enums;

public class ItemInfo : MonoBehaviour {

    public struct ItemList
    {
        public string itemName;
        public int ItemPrice;
        public int ItemLevel;
        //const int BasicMissile = 20, ShieldbreakMissile = 50, ChromaticMissile = 50, EMPMissile = 50;
        //const int BasicMissileUP = 100, ShieldbreakMissileUP = 200, ChromaticMissileUP = 200, EMPMissileUP = 200;
        //const int HealthUP = 100;
    }

    Text headLine, NOI;
    Button increment, decrement;
    int numItem;
    string itemName, sendFrom;
    ItemType type;
	// Use this for initialization
	void Start () {
        headLine = transform.FindChild("HeadLine").GetComponent<Text>();
        NOI = transform.FindChild("NOI").GetComponent<Text>();
        increment = transform.FindChild("Increment").GetComponent<Button>();
        decrement = transform.FindChild("Decrement").GetComponent<Button>();
        numItem = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void GetItem(string _sendFrom ,string _itemName, ItemType _type)
    {
        sendFrom = _sendFrom;
        itemName = _itemName;
        type = _type;

        headLine.text = itemName;

    }

    void UpdateItemNumberText()
    {
        NOI.text = numItem.ToString();
    }
}
