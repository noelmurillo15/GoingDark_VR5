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
        //lr.SetPosition(1, nextPlanet.transform.position);
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

    }

    void CheckLevelUnlocked()
    {
        switch (transform.name)
        {
            case "Easy":
                isUnlocked = PlayerPrefs.GetInt("Level1Unlocked");
                break;
            case "Medium":
                isUnlocked = PlayerPrefs.GetInt("Level2Unlocked");
                break;
            case "Hard":
                isUnlocked = PlayerPrefs.GetInt("Level3Unlocked");
                break;
            case "Nightmare":
                isUnlocked = PlayerPrefs.GetInt("Level4Unlocked");
                break;
        }
    }
}
