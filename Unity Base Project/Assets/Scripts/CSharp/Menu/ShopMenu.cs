using UnityEngine;
using System.Collections;

public class ShopMenu : MonoBehaviour {

    private GameObject consumableList;
    private GameObject weaponList;
    private GameObject deviceList;
    private GameObject mainList;
    private GameObject itemInfo;

    // Use this for initialization
    void Start () {
        mainList = GameObject.Find("MainList");
        consumableList = GameObject.Find("ConsumableList");
        weaponList = GameObject.Find("WeaponList");
        deviceList = GameObject.Find("DeviceList");
        itemInfo = GameObject.Find("ItemInfo");

        consumableList.SetActive(false);
        weaponList.SetActive(false);
        deviceList.SetActive(false);
        itemInfo.SetActive(false);
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

    void Back()
    {
        consumableList.SetActive(false);
        weaponList.SetActive(false);
        deviceList.SetActive(false);
        mainList.SetActive(true);
    }
}
