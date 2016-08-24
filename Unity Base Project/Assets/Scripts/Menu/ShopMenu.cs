using UnityEngine;
using GoingDark.Core.Enums;
using System.Collections.Generic;

public class ShopMenu : MonoBehaviour
{

    #region Properties
    [SerializeField]
    private GameObject mainList;
    [SerializeField]
    private GameObject consumableList;

    [SerializeField]
    private GameObject itemInfo;

    [SerializeField]
    private AudioSource buttonPressed, back;

    private ItemInfo info;
    private List<Item> items;
    #endregion

    void Awake()
    {
        items = new List<Item>();
        info = itemInfo.GetComponent<ItemInfo>();
        Initialize();
    }

    void OnEnable()
    {
        mainList.SetActive(true);
        itemInfo.SetActive(false);
        consumableList.SetActive(false);
    }

    void Initialize()
    {
        AddItem("Basic Missile", 20, Items.BasicMissile);
        AddItem("Shieldbreak Missile", 60, Items.ShieldBreakMissile);
        AddItem("Chromatic Missile", 120, Items.ChromaticMissile);
        AddItem("EMP Missile", 50, Items.EMPMissile);
    }
    void AddItem(string _name, int _price, Items _type)
    {
        Item item;
        item.ItemName = _name; item.ItemPrice = _price;
        item.Type = _type;

        items.Add(item);
    }

    #region Sounds   
    public void PlayButtonSound()
    {
        buttonPressed.Play();
    }
    #endregion

    #region Panels
    public void OpenConsumableList()
    {
        mainList.SetActive(false);
        consumableList.SetActive(true);
        PlayButtonSound();
    }

    public void Back()
    {
        itemInfo.SetActive(false);
        consumableList.SetActive(false);
        mainList.SetActive(true);
        back.Play();
    }
    #endregion

    #region Consumables
    public void BasicMissile()
    {
        consumableList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(items[(int)Items.BasicMissile]);
        PlayButtonSound();
    }
    public void ShieldbreakMissile()
    {
        consumableList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(items[(int)Items.ShieldBreakMissile]);
        PlayButtonSound();
    }
    public void ChromaticMissile()
    {
        consumableList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(items[(int)Items.ChromaticMissile]);
        PlayButtonSound();
    }
    public void EMPMissile()
    {
        consumableList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(items[(int)Items.EMPMissile]);
        PlayButtonSound();
    }
    #endregion

}