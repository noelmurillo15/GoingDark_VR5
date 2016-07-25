using UnityEngine;
using System.Collections;

public class WayPointsManager : MonoBehaviour {


    [SerializeField]
    private GameObject[] WayPoints;

    // Use this for initialization
    void Start () {
        WayPoints = new GameObject[transform.childCount]; 

        for (int i = 0; i < transform.childCount; i++)
        {
            WayPoints[i] = transform.GetChild(i).gameObject;
            WayPoints[i].SetActive(false);
        }
        WayPoints[0].SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

   public GameObject[] GetWaypoints() { return WayPoints; }

    void SetNextActive()
    {
        int i = 0;
        for (; i < WayPoints.Length; i++)
        {
            if (WayPoints[i].activeInHierarchy)
            {
                WayPoints[i].SetActive(false);
                if (++i < transform.childCount)
                    WayPoints[i].SetActive(true);
                break;
            }
        }
    }
}
