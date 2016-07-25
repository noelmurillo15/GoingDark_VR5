using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GoingDark.Core.Enums;

public class ItemInfo : MonoBehaviour {

    Text headLine, NOI, priceText, currInfo, creditText;
    Button increment, decrement;
    int numItem, price, itemOwned, itemLevel, credit;
    ShopType sendFrom;
    float buttonBuffer;
    ShopMenu.Item item;
    GameObject consumableList;
    GameObject weaponList;
    GameObject deviceList;
    private AudioSource buy, buttonPressed, buyFail, back;

    // Use this for initialization
    void Start () {
        headLine = transform.FindChild("HeadLine").GetComponent<Text>();
        NOI = transform.FindChild("NOI").GetComponent<Text>();
        increment = transform.FindChild("Increment").GetComponent<Button>();
        decrement = transform.FindChild("Decrement").GetComponent<Button>();
        priceText = transform.FindChild("Price").GetComponent<Text>();
        currInfo = transform.FindChild("CurrInfo").GetComponent<Text>();
        creditText = transform.FindChild("Credit").GetComponent<Text>();
        numItem = 0;
        price = 0;
        UpdateItemNumberText();
        buttonBuffer = 0.0f;
        credit = 0;
        consumableList = GameObject.Find("Shop").transform.FindChild("ConsumableList").gameObject;
        weaponList = GameObject.Find("Shop").transform.FindChild("WeaponList").gameObject;
        deviceList = GameObject.Find("Shop").transform.FindChild("DeviceList").gameObject;
        itemOwned = 0;
        itemLevel = 1;
        buy = GameObject.Find("StoreSound").transform.FindChild("StoreBuy").GetComponent<AudioSource>();
        buttonPressed = GameObject.Find("StoreSound").transform.FindChild("StoreButton").GetComponent<AudioSource>();
        buyFail = GameObject.Find("StoreSound").transform.FindChild("StoreBuyFail").GetComponent<AudioSource>();
        back = GameObject.Find("StoreSound").transform.FindChild("StoreBack").GetComponent<AudioSource>();
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
        headLine.text = item.ItemName;
        UpdateCredit();
        GetItemInfo();
        UpdatePrice();
    }

    void GetItemInfo()
    {
        switch (item.Type)
        {
            case Items.BasicMissile:
                itemOwned = PlayerPrefs.GetInt("BasicMissileCount");
                break;
            case Items.ShieldBreakMissile:
                itemOwned = PlayerPrefs.GetInt("ShieldbreakMissileCount");
                break;
            case Items.ChromaticMissile:
                itemOwned = PlayerPrefs.GetInt("ChromaticMissileCount");
                break;
            case Items.EMPMissile:
                itemOwned = PlayerPrefs.GetInt("EMPMissileCount");
                break;
            case Items.BasicMissileUpgrade:
                itemLevel = PlayerPrefs.GetInt("BasicMissileLevel");
                break;
            case Items.ShieldBreakMissileUpgrade:
                itemLevel = PlayerPrefs.GetInt("ShieldbreakMissileLevel");
                break;
            case Items.ChromaticMissileUpgrade:
                itemLevel = PlayerPrefs.GetInt("ChromaticMissileLevel");
                break;
            case Items.EMPMissileUpgrade:
                itemLevel = PlayerPrefs.GetInt("EMPMissileLevel");
                break;
            case Items.LaserPowerUpgrade:
                itemLevel = PlayerPrefs.GetInt("LaserPowerLevel");
                break;
            case Items.LaserCooldownUpgrade:
                itemLevel = PlayerPrefs.GetInt("LaserCooldownLevel");
                break;
            case Items.HealthUpgrade:
                itemLevel = PlayerPrefs.GetInt("HealthLevel");
                break;
            case Items.ShieldUpgrade:
                itemLevel = PlayerPrefs.GetInt("ShieldLevel");
                break;
            case Items.HyperdriveUpgrade:
                itemLevel = PlayerPrefs.GetInt("HyperdriveLevel");
                break;
            case Items.CloakUpgrade:
                itemLevel = PlayerPrefs.GetInt("CloakLevel");
                break;
            case Items.EMPUpgrade:
                itemLevel = PlayerPrefs.GetInt("EMPLevel");
                break;
            default:
                break;
        }

        if (item.ItemType == ItemType.Consumable)
            currInfo.text = "Owned: " + itemOwned;
        else
            currInfo.text = "Level: " + itemLevel;
    }

    void UpdatePrice()
    {
      //  if (item.ItemType == ItemType.Consumable)
      //      price = item.ItemPrice * numItem;
       // else
        price = item.ItemPrice * itemLevel * numItem;
        
        priceText.text = "Cost: \n" + price.ToString() + " Credit";
    }

    void UpdateCredit()
    {
        credit = PlayerPrefs.GetInt("Credit");
        creditText.text = "Credit: " + credit;
    }

    void UpdateItemNumberText()
    {
        NOI.text = numItem.ToString();
        buttonBuffer = 0.1f;

        UpdatePrice();
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
        buttonPressed.Play();
    }

    void IncrementItem()
    {
        if (buttonBuffer<=0)
        {
            if (item.ItemType == ItemType.Consumable)
            {
                if (numItem < 99)
                    numItem++;
            }
            else
            {
                if (numItem < 1)
                    numItem++;
            }

            UpdateItemNumberText();
        }
        buttonPressed.Play();
    }

    void Buy()
    {
        if (buttonBuffer <= 0)
        {
            if (credit >= price && numItem > 0)
            {
                credit = credit - price;
                PlayerPrefs.SetInt("Credit", credit);
                UpdateCredit();

                LevelUp();

                GetItemInfo();

                buy.Play();
            }
            else
                buyFail.Play();
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
        itemLevel = 1;
        UpdateItemNumberText();
        gameObject.SetActive(false);
        back.Play();
    }

    void LevelUp()
    {
        switch (item.Type)
        {
            case Items.BasicMissile:
                PlayerPrefs.SetInt("BasicMissileCount", PlayerPrefs.GetInt("BasicMissileCount") + numItem);
                break;
            case Items.ShieldBreakMissile:
                PlayerPrefs.SetInt("ShieldbreakMissileCount", PlayerPrefs.GetInt("ShieldbreakMissileCount") + numItem);
                break;
            case Items.ChromaticMissile:
                PlayerPrefs.SetInt("ChromaticMissileCount", PlayerPrefs.GetInt("ChromaticMissileCount") + numItem);
                break;
            case Items.EMPMissile:
                PlayerPrefs.SetInt("EMPMissileCount", PlayerPrefs.GetInt("EMPMissileCount") + numItem);
                break;
            case Items.BasicMissileUpgrade:
                PlayerPrefs.SetInt("BasicMissileLevel", PlayerPrefs.GetInt("BasicMissileLevel") +1);
                break;
            case Items.ShieldBreakMissileUpgrade:
                PlayerPrefs.SetInt("ShieldbreakMissileLevel", PlayerPrefs.GetInt("ShieldbreakMissileLevel") +1);
                break;
            case Items.ChromaticMissileUpgrade:
                PlayerPrefs.SetInt("ChromaticMissileLevel", PlayerPrefs.GetInt("ChromaticMissileLevel") +1);
                break;
            case Items.EMPMissileUpgrade:
                PlayerPrefs.SetInt("EMPMissileLevel", PlayerPrefs.GetInt("EMPMissileLevel") +1);
                break;
            case Items.LaserPowerUpgrade:
                PlayerPrefs.SetInt("LaserPowerLevel", PlayerPrefs.GetInt("LaserPowerLevel") +1);
                break;
            case Items.LaserCooldownUpgrade:
                PlayerPrefs.SetInt("LaserCooldownLevel", PlayerPrefs.GetInt("LaserCooldownLevel") +1);
                break;
            case Items.HealthUpgrade:
                PlayerPrefs.SetInt("HealthLevel", PlayerPrefs.GetInt("HealthLevel") +1);
                break;
            case Items.ShieldUpgrade:
                PlayerPrefs.SetInt("ShieldLevel", PlayerPrefs.GetInt("ShieldLevel") +1);
                break;
            case Items.HyperdriveUpgrade:
                PlayerPrefs.SetInt("HyperdriveLevel", PlayerPrefs.GetInt("HyperdriveLevel") +1);
                break;
            case Items.CloakUpgrade:
                PlayerPrefs.SetInt("CloakLevel", PlayerPrefs.GetInt("CloakLevel") +1);
                break;
            case Items.EMPUpgrade:
                PlayerPrefs.SetInt("EMPLevel", PlayerPrefs.GetInt("EMPLevel") +1);
                break;
            default:
                break;
        }
        numItem = 0;
        UpdateItemNumberText();
        UpdatePrice();
    }
}
