//  NO MORE COPYRIGHT.. YAY
using UnityEngine;
using GoingDark.Core.Enums;
using System.Collections.Generic;

// HI JOE
public class ShopMenu : MonoBehaviour {

    #region Properties
    [SerializeField]    //  USE THIS
    private GameObject mainList;
    [SerializeField]    //  AND THIS
    private GameObject consumableList;
    [SerializeField]    //   THIS ONE TOO
    private GameObject weaponList;
    [SerializeField]    //  DONT EVEN NEED GAMEOBJECT.FIND
    private GameObject deviceList;
    [SerializeField]    //  COPY AND PASTE THIS EVERYWHERE
    private GameObject itemInfo;

    [SerializeField]    //  LIKE HERE TOO
    private AudioSource buttonPressed,back;

    private ItemInfo info;
    private List<Item> items;
    #endregion

    // I AM NOT REFACTORING ITEMINFO.CS THAT ONE IS ON YOU
    void Awake () {
        // NOOOOO GAMEOBJECT.FINDS() ISNT THIS SO MUCH BETTER
        items = new List<Item>();
        info = itemInfo.GetComponent<ItemInfo>();
        Initialize();
    }    

    void OnEnable()
    {
        mainList.SetActive(true);
        itemInfo.SetActive(false);
        weaponList.SetActive(false);
        deviceList.SetActive(false);
        consumableList.SetActive(false);
    }

    // I DONT EVEN WANT TO LOOK AT ITEMINFO ANYMORE
    void Initialize()
    {
        AddItem(1, "Basic Missile", 20, Items.BasicMissile, ItemType.Consumable);
        AddItem(1, "Shieldbreak Missile", 60, Items.ShieldBreakMissile, ItemType.Consumable);
        AddItem(1, "Chromatic Missile", 120, Items.ChromaticMissile, ItemType.Consumable);
        AddItem(1, "EMP Missile", 50, Items.EMPMissile, ItemType.Consumable);
    }    
    //  IT HURTS MY EYES
    void AddItem(int _level, string _name, int _price, Items _type, ItemType _itemType)
    {
        Item item;
        item.ItemLevel = _level; item.ItemName = _name; item.ItemPrice = _price;
        item.Type = _type; item.ItemType = _itemType;
        
        items.Add(item);
    }

    //  #REGIONS MAKE EVERYTHING SOOO MUCH BETTER
    #region Sounds   
    public void PlayButtonSound()
    {
        buttonPressed.Play();
    }
    #endregion
    // DONT YOU THINK
    #region Panels
    //  THESE NEED TO BE PUBLIC
    public void OpenConsumableList()
    {
        mainList.SetActive(false);
        consumableList.SetActive(true);
        PlayButtonSound();
    }
    //  STOP SENDING MESSAGES
    public void OpenWeaponList()
    {
        mainList.SetActive(false);
        weaponList.SetActive(true);
        PlayButtonSound();
    }
    //  WHY U DO THIS TO ME
    public void OpenDeviceList()
    {
        mainList.SetActive(false);
        deviceList.SetActive(true);
        PlayButtonSound();
    }


    public void Back()
    {
        itemInfo.SetActive(false);
        consumableList.SetActive(false);
        weaponList.SetActive(false);
        deviceList.SetActive(false);
        mainList.SetActive(true);
        back.Play();
    }
    #endregion
    // NOW I KNOW WHAT I AM LOOKING AT
    #region Consumables
    public void BasicMissile()
    {
        consumableList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.ConsumableList, items[(int)Items.BasicMissile]);
        PlayButtonSound();
    }
    public void ShieldbreakMissile()
    {
        consumableList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.ConsumableList, items[(int)Items.ShieldBreakMissile]);
        PlayButtonSound();
    }
    public void ChromaticMissile()
    {
        consumableList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.ConsumableList, items[(int)Items.ChromaticMissile]);
        PlayButtonSound();
    }
    public void EMPMissile()
    {
        consumableList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.ConsumableList, items[(int)Items.EMPMissile]);
        PlayButtonSound();
    }
    #endregion
    // AHHHH ORGANIZATION
    #region Upgrades
    public void LaserPowerUP()
    {
        weaponList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.WeaponList, items[(int)Items.LaserPowerUpgrade]);
        PlayButtonSound();
    }
    public void Laser2PowerUP()
    {
        weaponList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.WeaponList, items[(int)Items.Laser2PowerUpgrade]);
        PlayButtonSound();
    }
    public void HealthUP()
    {
        deviceList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.DeviceList, items[(int)Items.HealthUpgrade]);
        PlayButtonSound();
    }
    public void ShiledUP()
    {
        deviceList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.DeviceList, items[(int)Items.ShieldUpgrade]);
        PlayButtonSound();
    }
    #endregion    
}