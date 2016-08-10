using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadSlots : MonoBehaviour {

    [SerializeField]
    private GameObject[] Slots;

    private LoadGame LoadGame;
    // Use this for initialization
    void OnEnable () {
        LoadGame = gameObject.GetComponent<LoadGame>();
        CheckSlots();
	}
	
    void CheckSlots()
    {
        string name = string.Empty;
        for (int i = 0; i < Slots.Length; i++)
        {
            name = LoadGame.IsSlotUsed(Slots[i].name);
            if (name != "Name")
                Slots[i].GetComponentInChildren<Text>().text = name;
            else
                Slots[i].SetActive(false);
        }
            
    }
}
