using UnityEngine;
using UnityEngine.UI;
using GoingDark.Core.Enums;

public class ItemInfo : MonoBehaviour
{
    [SerializeField]    //  This was breaking but i fixed it
    private Text headLine, NOI, priceText, currInfo, creditText;

    private Button increment, decrement;   
    private int numItem, price, itemOwned, itemLevel, credit, hasItem;
    private ShopType sendFrom;
    private float buttonBuffer;
    private Item item;

    [SerializeField]
    GameObject consumableList;
    [SerializeField]
    GameObject weaponList;
    [SerializeField]
    GameObject deviceList;

    [SerializeField]    //  I started refactoring for you because i was bored
    private AudioSource buy, buttonPressed, buyFail, back;

    void Start()
    {
        numItem = 0;
        price = 0;
        UpdateItemNumberText();
        buttonBuffer = 0.0f;
        credit = 0;

        itemOwned = 0;
        itemLevel = 1;
        hasItem = 1;

        //  Never do this again..
        //buy = GameObject.Find("StoreSound").transform.FindChild("StoreBuy").GetComponent<AudioSource>();
        //buttonPressed = GameObject.Find("StoreSound").transform.FindChild("StoreButton").GetComponent<AudioSource>();
        //buyFail = GameObject.Find("StoreSound").transform.FindChild("StoreBuyFail").GetComponent<AudioSource>();
        //back = GameObject.Find("StoreSound").transform.FindChild("StoreBack").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (buttonBuffer > 0)
            buttonBuffer -= Time.deltaTime;
    }

    public void GetItem(ShopType _sendFrom, Item _item)
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
                hasItem = PlayerPrefs.GetInt("HasShieldbreakMissile");
                break;
            case Items.ChromaticMissile:
                itemOwned = PlayerPrefs.GetInt("ChromaticMissileCount");
                hasItem = PlayerPrefs.GetInt("HasChromaticMissile");
                break;
            case Items.EMPMissile:
                itemOwned = PlayerPrefs.GetInt("EMPMissileCount");
                hasItem = PlayerPrefs.GetInt("HasEMPMissile");
                break;
            case Items.LaserPowerUpgrade:
                itemLevel = PlayerPrefs.GetInt("LaserPowerLevel");
                break;
            case Items.Laser2PowerUpgrade:
                itemLevel = PlayerPrefs.GetInt("Laser2PowerLevel");
                hasItem = PlayerPrefs.GetInt("HasLaser2");
                break;
            case Items.HealthUpgrade:
                itemLevel = PlayerPrefs.GetInt("HealthLevel");
                break;
            case Items.ShieldUpgrade:
                itemLevel = PlayerPrefs.GetInt("ShieldLevel");
                break;
            default:
                break;
        }

        if (item.ItemType == ItemType.Consumable)
        {
            if (hasItem == 1)
                currInfo.text = "Owned: " + itemOwned;
            else
                currInfo.text = "Out of stock";
        }
        else
        {
            if (hasItem == 1)
            {
                if (itemLevel < 5)
                    currInfo.text = "Level: " + itemLevel;
                else
                {
                    currInfo.text = "Max Level";
                    UpdatePrice();
                    //  currInfo.color = Color.yellow;
                }
            }
            else
                currInfo.text = "Out of stock";
        }
    }

    void UpdatePrice()
    {
        price = item.ItemPrice * itemLevel * numItem;

        priceText.text = "Cost: \n" + price.ToString() + " Credit";

        if (itemLevel == 5 || hasItem == 0)
        {
            priceText.text = "Cost:\nN/A";
        }
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

    public void DecrementItem()
    {
        if (hasItem == 1 && itemLevel != 5)
        {
            if (buttonBuffer <= 0)
            {
                if (numItem > 0)
                {
                    numItem--;
                }
                UpdateItemNumberText();
            }
            buttonPressed.Play();
        }
        else
            buyFail.Play();
    }

    public void IncrementItem()
    {
        if (hasItem == 1 && itemLevel != 5)
        {
            if (buttonBuffer <= 0)
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
        else
            buyFail.Play();
    }

    public void Buy()
    {
        if (buttonBuffer <= 0)
        {
            if (credit >= price && numItem > 0 && itemLevel < 5 && hasItem == 1)
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

    public void Return()
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
        hasItem = 1;
        UpdateItemNumberText();
        back.Play();
        gameObject.SetActive(false);
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
            case Items.LaserPowerUpgrade:
                PlayerPrefs.SetInt("LaserPowerLevel", PlayerPrefs.GetInt("LaserPowerLevel") + 1);
                break;
            case Items.Laser2PowerUpgrade:
                PlayerPrefs.SetInt("Laser2PowerLevel", PlayerPrefs.GetInt("Laser2PowerLevel") + 1);
                break;
            case Items.HealthUpgrade:
                PlayerPrefs.SetInt("HealthLevel", PlayerPrefs.GetInt("HealthLevel") + 1);
                break;
            case Items.ShieldUpgrade:
                PlayerPrefs.SetInt("ShieldLevel", PlayerPrefs.GetInt("ShieldLevel") + 1);
                break;
            default:
                break;
        }
        numItem = 0;
        UpdateItemNumberText();
        UpdatePrice();
    }
}
