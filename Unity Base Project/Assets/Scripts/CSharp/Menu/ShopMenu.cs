using UnityEngine;
using System.Collections;
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

        Initialize();

    }

    void Initialize()
    {
        AddItem(1, "BasicMissile", 20, Items.BasicMissile, ItemType.Consumable);
        AddItem(1, "ShieldbreakMissile", 50, Items.ShieldBreakMissile, ItemType.Consumable);
        AddItem(1, "ChromaticMissile", 50, Items.ChromaticMissile, ItemType.Consumable);
        AddItem(1, "EMPMissile", 50, Items.EMPMissile, ItemType.Consumable);
        AddItem(1, "BasicMissile UP", 50, Items.BasicMissileUpgrade, ItemType.Upgrade);
        AddItem(1, "ShieldbreakMissile UP", 100, Items.ShieldBreakMissileUpgrade, ItemType.Upgrade);
        AddItem(1, "ChromaticMissile UP", 100, Items.ChromaticMissileUpgrade, ItemType.Upgrade);
        AddItem(1, "EMPMissile UP", 100, Items.EMPMissileUpgrade, ItemType.Upgrade);
        AddItem(1, "LaserPower UP", 50, Items.LaserPowerUpgrade, ItemType.Upgrade);
        AddItem(1, "LaserCooldown UP", 50, Items.LaserCooldownUpgrade, ItemType.Upgrade);
        AddItem(1, "Health UP", 50, Items.HealthUpgrade, ItemType.Upgrade);
        AddItem(1, "Shiled UP", 50, Items.ShieldUpgrade, ItemType.Upgrade);
        AddItem(1, "Hyperdrive UP", 100, Items.HyperdriveUpgrade, ItemType.Upgrade);
        AddItem(1, "Cloak UP", 100, Items.CloakUpgrade, ItemType.Upgrade);
        AddItem(1, "EMP UP", 100, Items.EMPUpgrade, ItemType.Upgrade);
    }

    void AddItem(int _level, string _name, int _price, Items _type, ItemType _itemType)
    {
        Item item;
        item.ItemLevel = _level; item.ItemName = _name; item.ItemPrice = _price;
        item.Type = _type; item.ItemType = _itemType;
        
        items.Add(item);
    }

    // Update is called once per frame
    void Update () {

    }

    void OpenConsumableList()
    {
        mainList.SetActive(false);
        consumableList.SetActive(true);
    }

    void OpenWeaponList()
    {
        mainList.SetActive(false);
        weaponList.SetActive(true);
    }

    void OpenDeviceList()
    {
        mainList.SetActive(false);
        deviceList.SetActive(true);
    }

    void BasicMissile()
    {
        consumableList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.ConsumableList, items[(int)Items.BasicMissile]);
    }

    void ShieldbreakMissile()
    {
        consumableList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.ConsumableList, items[(int)Items.ShieldBreakMissile]);
    }

    void ChromaticMissile()
    {
        consumableList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.ConsumableList, items[(int)Items.ChromaticMissile]);
    }

    void EMPMissile()
    {
        consumableList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.ConsumableList, items[(int)Items.EMPMissile]);
    }
    void BasicMissileUP()
    {
        weaponList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.WeaponList, items[(int)Items.BasicMissileUpgrade]);
    }
    void ShieldbreakMissileUP()
    {
        weaponList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.WeaponList, items[(int)Items.ShieldBreakMissileUpgrade]);
    }
    void ChromaticMissileUP()
    {
        weaponList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.WeaponList, items[(int)Items.ChromaticMissileUpgrade]);
    }
    void EMPMissileUP()
    {
        weaponList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.WeaponList, items[(int)Items.EMPMissileUpgrade]);
    }
    void LaserPowerUP()
    {
        weaponList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.WeaponList, items[(int)Items.LaserPowerUpgrade]);
    }
    void LaserCooldownUP()
    {
        weaponList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.WeaponList, items[(int)Items.LaserCooldownUpgrade]);
    }
    void HealthUP()
    {
        deviceList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.DeviceList, items[(int)Items.HealthUpgrade]);
    }
    void ShiledUP()
    {
        deviceList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.DeviceList, items[(int)Items.ShieldUpgrade]);
    }
    void HyperdriveUP()
    {
        deviceList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.DeviceList, items[(int)Items.HyperdriveUpgrade]);
    }
    void CloakUP()
    {
        deviceList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.DeviceList, items[(int)Items.CloakUpgrade]);
    }
    void EMPUP()
    {
        deviceList.SetActive(false);
        itemInfo.SetActive(true);
        info.GetItem(ShopType.DeviceList, items[(int)Items.EMPUpgrade]);
    }

    void Back()
    {
        itemInfo.SetActive(false);
        consumableList.SetActive(false);
        weaponList.SetActive(false);
        deviceList.SetActive(false);
        mainList.SetActive(true);
    }
}
