using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadSlots : MonoBehaviour {

    [SerializeField]
    private GameObject[] mLoadSlots;
    [SerializeField]
    private GameObject[] mSaveSlots;

    private LoadGame LoadGame;
    // Use this for initialization
    void OnEnable () {
        LoadGame = gameObject.GetComponent<LoadGame>();
        CheckSlots();
	}
	
    void CheckSlots()
    {
        string name = string.Empty;
        for (int i = 0; i < mLoadSlots.Length; i++)
        {
            name = LoadGame.IsSlotUsed(mLoadSlots[i].name);
            if (name != "Name")
                mLoadSlots[i].GetComponentInChildren<Text>().text = name;
            else
                mLoadSlots[i].SetActive(false);

            
        }

        for (int i = 0; i < mSaveSlots.Length; i++)
        {
            mSaveSlots[i].GetComponentInChildren<Text>().text = LoadGame.IsSlotUsed(mLoadSlots[i].name);
        }            
    }
}
