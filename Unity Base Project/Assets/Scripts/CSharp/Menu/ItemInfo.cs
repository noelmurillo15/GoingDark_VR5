using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GoingDark.Core.Enums;

public class ItemInfo : MonoBehaviour {

    Text headLine, NOI, priceText;
    Button increment, decrement;
    int numItem, price;
    ShopType sendFrom;
    float buttonBuffer;
    ShopMenu.Item item;
    GameObject consumableList;
    GameObject weaponList;
    GameObject deviceList;

    // Use this for initialization
    void Start () {
        headLine = transform.FindChild("HeadLine").GetComponent<Text>();
        NOI = transform.FindChild("NOI").GetComponent<Text>();
        increment = transform.FindChild("Increment").GetComponent<Button>();
        decrement = transform.FindChild("Decrement").GetComponent<Button>();
        priceText = transform.FindChild("Price").GetComponent<Text>();
        numItem = 0;
        price = 0;
        UpdateItemNumberText();
        buttonBuffer = 0.0f;
        consumableList = GameObject.Find("Shop").transform.FindChild("ConsumableList").gameObject;
        weaponList = GameObject.Find("Shop").transform.FindChild("WeaponList").gameObject;
        deviceList = GameObject.Find("Shop").transform.FindChild("DeviceList").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if(buttonBuffer>0)
            buttonBuffer -= Time.deltaTime;
	}

    public void GetItem(ShopType _sendFrom, ShopMenu.Item _item)
    {
        sendFrom = _sendFrom;
        item = _item;
        price = item.ItemPrice * item.ItemLevel;
        headLine.text = item.ItemName;
        priceText.text = "Cost: \n" + price.ToString();
    }

    void UpdateItemNumberText()
    {
        NOI.text = numItem.ToString();
        buttonBuffer = 0.1f;
    }

    void DecrementItem()
    {
        if (buttonBuffer <=0)
        {
            if (numItem > 0)
            {
                numItem--;
            }
            UpdateItemNumberText();
        }
        
    }

    void IncrementItem()
    {
        if (buttonBuffer<=0)
        {
            if (item.ItemType == ItemType.Consumable)
            {
                if (numItem < 100)
                    numItem++;
            }
            else
            {
                if (numItem < 1)
                    numItem++;
            }

            UpdateItemNumberText();
        }
    }

    void Return()
    {
        switch (sendFrom)
        {
            case ShopType.ConsumableList:
                consumableList.SetActive(true);
                break;
            case ShopType.WeaponList:
                weaponList.SetActive(true);
                break;
            case ShopType.DeviceList:
                deviceList.SetActive(true);
                break;
            default:
                break;
        }
        numItem = 0;
        UpdateItemNumberText();
        gameObject.SetActive(false);
    }
}
