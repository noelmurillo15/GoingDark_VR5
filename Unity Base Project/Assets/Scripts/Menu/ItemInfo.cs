using UnityEngine;
using UnityEngine.UI;
using GoingDark.Core.Enums;

public class ItemInfo : MonoBehaviour
{
    [SerializeField]
    private Text headLine, NOI, priceText, currInfo, creditText;

    private Button increment, decrement;
    private int numItem, price, itemOwned, credit;
    private float buttonBuffer;
    private Item item;
    private PersistentGameManager gameManager;

    [SerializeField]
    GameObject consumableList;

    [SerializeField]
    private AudioSource buy, buttonPressed, buyFail, back;

    void Awake()
    {
        numItem = 0;
        price = 0;
        UpdateItemNumberText();
        buttonBuffer = 0.0f;
        credit = 0;
        gameManager = PersistentGameManager.Instance;
        itemOwned = 0;
    }

    void Update()
    {
        if (buttonBuffer > 0)
            buttonBuffer -= Time.deltaTime;
    }

    #region InfoUpdate
    public void GetItem(Item _item)
    {
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
                itemOwned = gameManager.GetBasicMissileCount();
                break;
            case Items.ShieldBreakMissile:
                itemOwned = gameManager.GetShieldbreakMissileCount();
                break;
            case Items.ChromaticMissile:
                itemOwned = gameManager.GetChromaticMissileCount();
                break;
            case Items.EMPMissile:
                itemOwned = gameManager.GetEMPMissileCount();
                break;
            default:
                break;
        }

        currInfo.text = "Owned: " + itemOwned;
    }

    void UpdatePrice()
    {
        price = item.ItemPrice * numItem;

        priceText.text = "Cost: \n" + price.ToString() + " Credit";

    }

    void UpdateCredit()
    {
        credit = gameManager.GetPlayerCredits();
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
        if (buttonBuffer <= 0)
        {
            if (numItem > 0)
                numItem--;
            UpdateItemNumberText();

            buttonPressed.Play();
        }
    }

    void IncrementItem()
    {
        if (buttonBuffer <= 0)
        {
            if (numItem + itemOwned < 99)
                numItem++;
            UpdateItemNumberText();

            buttonPressed.Play();
        }
    }

    void LevelUp()
    {
        switch (item.Type)
        {
            case Items.BasicMissile:
                gameManager.SetBasicMissileCount(gameManager.GetBasicMissileCount() + numItem);
                break;
            case Items.ShieldBreakMissile:
                gameManager.SetShieldbreakMissileCount(gameManager.GetShieldbreakMissileCount() + numItem);
                break;
            case Items.ChromaticMissile:
                gameManager.SetChromaticMissileCount(gameManager.GetChromaticMissileCount() + numItem);
                break;
            case Items.EMPMissile:
                gameManager.SetEMPMissileCount(gameManager.GetEMPMissileCount() + numItem);
                break;
            default:
                break;
        }
        numItem = 0;
        UpdateItemNumberText();
        UpdatePrice();
    }
    #endregion

    #region ButtonFunc
    public void Buy()
    {
        if (buttonBuffer <= 0)
        {
            if (credit >= price && numItem > 0)
            {
                credit = credit - price;
                gameManager.SetPlayerCredits(credit);
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
        consumableList.SetActive(true);

        numItem = 0;
        UpdateItemNumberText();
        back.Play();
        gameObject.SetActive(false);
    }
    #endregion
}
