using UnityEngine;
using GoingDark.Core.Enums;
using System.Collections.Generic;

public class ShopMenu : MonoBehaviour {

    private GameObject consumableList;
    private GameObject weaponList;
    private GameObject deviceList;
    private GameObject mainList;
    private GameObject itemInfo;
    private ItemInfo info;
    public List<Item> items;
    private AudioSource buttonPressed,back;
    
    public struct Item
    {
        public string ItemName;
        public int ItemPrice;
        public int ItemLevel;
        public ItemType ItemType;
        public Items Type;
    }

    // Use this for initialization
    void Start () {
        mainList = GameObject.Find("MainList");
        consumableList = GameObject.Find("ConsumableList");
        weaponList = GameObject.Find("WeaponList");
        deviceList = GameObject.Find("DeviceList");
        itemInfo = GameObject.Find("ItemInfo");
        info = itemInfo.GetComponent<ItemInfo>();
        items = new List<Item>();
        consumableList.SetActive(false);
        weaponList.SetActive(false);
        deviceList.SetActive(false);
        itemInfo.SetActive(false);

        buttonPressed = GameObject.Find("StoreSound").transform.FindChild("StoreButton").GetComponent<AudioSource>();
        back = GameObject.Find("StoreSound").transform.FindChild("StoreBack").GetComponent<AudioSource>();

        Initialize();

    }

    public void PlayButtonSound()
    {
        buttonPressed.Play();
    }

    void Initialize()
    {
        AddItem(1, "Basic Missile", 20, Items.BasicMissile, ItemType.Consumable);
        AddItem(1, "Shieldbreak Missile", 50, Items.ShieldBreakMissile, ItemType.Consumable);
        AddItem(1, "Chromatic Missile", 50, Items.ChromaticMissile, ItemType.Consumable);
        AddItem(1, "EMP Missile", 50, Items.EMPMissile, ItemType.Consumable);
        //AddItem(1, "BasicMissile UP", 50, Items.BasicMissileUpgrade, ItemType.Upgrade);
        //AddItem(1, "ShieldbreakMissile UP", 100, Items.ShieldBreakMissileUpgrade, ItemType.Upgrade);
        //AddItem(1, "ChromaticMissile UP", 100, Items.ChromaticMissileUpgrade, ItemType.Upgrade);
        //AddItem(1, "EMPMissile UP", 100, Items.EMPMissileUpgrade, ItemType.Upgrade);
        AddItem(1, "Laser UP", 50, Items.LaserPowerUpgrade, ItemType.Upgrade);
        AddItem(1, "HyperLaser UP", 100, Items.Laser2PowerUpgrade, ItemType.Upgrade);
        //AddItem(1, "LaserCooldown UP", 50, Items.LaserCooldownUpgrade, ItemType.Upgrade);
        AddItem(1, "Health UP", 50, Items.HealthUpgrade, ItemType.Upgrade);
        AddItem(1, "Shiled UP", 50, Items.ShieldUpgrade, ItemType.Upgrade);
        //AddItem(1, "Hyperdrive UP", 100, Items.HyperdriveUpgrade, ItemType.Upgrade);
        //AddItem(1, "Cloak UP", 100, Items.CloakUpgrade, ItemType.Upgrade);
        //AddItem(1, "EMP UP", 100, Items.EMPUpgrade, ItemType.Upgrade);
    }

    void AddItem(int _level, string _name, int _price, Items _type, ItemType _itemType)
    {
        Item item;
        item.ItemLevel = _level; item.ItemName = _name; item.ItemPrice = _price;
        item.Type = _type; item.ItemType = _itemType;
        
        items.Add(item);
    }

    void OpenConsumableList()
    {
        mainList.SetActive(false);
        consumableList.SetActive(true);
        PlayButtonSound();
    }

    void OpenWeaponList()
    {
        mainList.SetActive(false);
        weaponList.SetActive(true);
        PlayButtonSound();
    }

    void OpenDeviceList()
    {
        mainList.SetActive(false);
        deviceList.SetActive(true);
        PlayButtonSound();
    }

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
    //void BasicMissileUP()
    //{
    //    weaponList.SetActive(false);
    //    itemInfo.SetActive(true);
    //    info.GetItem(ShopType.WeaponList, items[(int)Items.BasicMissileUpgrade]);
    //    PlayButtonSound();
    //}
    //void ShieldbreakMissileUP()
    //{
    //    weaponList.SetActive(false);
    //    itemInfo.SetActive(true);
    //    info.GetItem(ShopType.WeaponList, items[(int)Items.ShieldBreakMissileUpgrade]);
    //    PlayButtonSound();
    //}
    //void ChromaticMissileUP()
    //{
    //    weaponList.SetActive(false);
    //    itemInfo.SetActive(true);
    //    info.GetItem(ShopType.WeaponList, items[(int)Items.ChromaticMissileUpgrade]);
    //    PlayButtonSound();
    //}
    //void EMPMissileUP()
    //{
    //    weaponList.SetActive(false);
    //    itemInfo.SetActive(true);
    //    info.GetItem(ShopType.WeaponList, items[(int)Items.EMPMissileUpgrade]);
    //    PlayButtonSound();
    //}
    void LaserPowerUP()
    {
        weaponList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.WeaponList, items[(int)Items.LaserPowerUpgrade]);
        PlayButtonSound();
    }

    void Laser2PowerUP()
    {
        weaponList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.WeaponList, items[(int)Items.Laser2PowerUpgrade]);
        PlayButtonSound();
    }

    void HealthUP()
    {
        deviceList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.DeviceList, items[(int)Items.HealthUpgrade]);
        PlayButtonSound();
    }
    void ShiledUP()
    {
        deviceList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.DeviceList, items[(int)Items.ShieldUpgrade]);
        PlayButtonSound();
    }
    //void HyperdriveUP()
    //{
    //    deviceList.SetActive(false);
    //    itemInfo.SetActive(true);
    //    info.GetItem(ShopType.DeviceList, items[(int)Items.HyperdriveUpgrade]);
    //    PlayButtonSound();
    //}
    //void CloakUP()
    //{
    //    deviceList.SetActive(false);
    //    itemInfo.SetActive(true);
    //    info.GetItem(ShopType.DeviceList, items[(int)Items.CloakUpgrade]);
    //    PlayButtonSound();
    //}
    //void EMPUP()
    //{
    //    deviceList.SetActive(false);
    //    itemInfo.SetActive(true);
    //    info.GetItem(ShopType.DeviceList, items[(int)Items.EMPUpgrade]);
    //    PlayButtonSound();
    //}

    public void Back()
    {
        itemInfo.SetActive(false);
        consumableList.SetActive(false);
        weaponList.SetActive(false);
        deviceList.SetActive(false);
        mainList.SetActive(true);
        back.Play();
    }
}
