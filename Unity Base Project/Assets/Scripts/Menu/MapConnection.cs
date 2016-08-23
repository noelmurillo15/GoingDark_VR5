using UnityEngine;

public class MapConnection : MonoBehaviour
{
    public int isUnlocked;


    void Start()
    {
        CheckLevelUnlocked();
        if (isUnlocked == 0)
        {
            GetComponent<MeshRenderer>().material.color = new Color(.5f,.5f,.5f);
            Transform temp = transform.GetChild(0);
            for (int i = 0; i < temp.GetComponentsInChildren<MeshRenderer>().Length; i++)
                temp.GetComponentsInChildren<MeshRenderer>()[i].enabled = false;            
        }        
    }

    void CheckLevelUnlocked()
    {
        switch (transform.name)
        {
            case "Level1":
                isUnlocked = PlayerPrefs.GetInt("Level1Unlocked");
                break;
            case "Level2":
                isUnlocked = PlayerPrefs.GetInt("Level2Unlocked");
                break;
            case "Level3":
                isUnlocked = PlayerPrefs.GetInt("Level3Unlocked");
                break;
            case "Level4":
                isUnlocked = PlayerPrefs.GetInt("Level4Unlocked");
                break;
        }
    }
}
