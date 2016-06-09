using UnityEngine;
using System.Collections;

public class MapConnection : MonoBehaviour
{
    public GameObject nextPlanet;
    public int isUnlocked;
    GameObject myLine;
    LineRenderer lr;
    Color color;
    // Use this for initialization
    void Start()
    {
        CheckLevelUnlocked();
        Vector3 start = transform.position;
       
        myLine = new GameObject("Line");
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Additive"));
        lr.SetColors(color, color);
        lr.SetWidth(1f, 1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, nextPlanet.transform.position);
        //GameObject.Destroy(myLine);
        if (isUnlocked == 0)
        {
            GetComponent<MeshRenderer>().material.color = new Color(.5f,.5f,.5f);
            Transform temp = transform.FindChild("Supernova");
            for (int i = 0; i < temp.GetComponentsInChildren<MeshRenderer>().Length; i++)
            {
                temp.GetComponentsInChildren<MeshRenderer>()[i].enabled = false;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isUnlocked == 0 || nextPlanet.GetComponent<MapConnection>().isUnlocked == 0)
            color = Color.black;
        else
            color = Color.white;

        lr.SetColors(color, color);
    }

    void CheckLevelUnlocked()
    {
        switch (transform.name)
        {
            case "Tutorial":
                isUnlocked = PlayerPrefs.GetInt("Level1Unlocked");
                break;
            case "Tutorial2":
                isUnlocked = PlayerPrefs.GetInt("Level2Unlocked");
                break;
            case "Tutorial3":
                isUnlocked = PlayerPrefs.GetInt("Level3Unlocked");
                break;
            case "OpenWorld":
                isUnlocked = PlayerPrefs.GetInt("Level4Unlocked");
                break;
            case "Level5":
                isUnlocked = PlayerPrefs.GetInt("Level5Unlocked");
                break;
            case "Level6":
                isUnlocked = PlayerPrefs.GetInt("Level6Unlocked");
                break;
            default:
                break;
        }
    }
}
