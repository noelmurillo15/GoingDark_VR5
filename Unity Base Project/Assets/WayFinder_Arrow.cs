using UnityEngine;
using System.Collections;

public class WayFinder_Arrow : MonoBehaviour {

    [SerializeField]
    private GameObject[] WayPoints;

    private GameObject WayPointManager;
    // Use this for initialization
    void Start () {
        WayPointManager = GameObject.FindGameObjectWithTag("WayPointManager");
        WayPoints = WayPointManager.GetComponent<WayPointsManager>().GetWaypoints();
    }

    // Update is called once per frame
    void Update () {

        int i = 0;
        for (; i < WayPoints.Length; i++)
        {
            if (WayPoints[i] && WayPoints[i].activeInHierarchy)
            {
                transform.LookAt(WayPoints[i].transform);
                break;
            }
        }
       
    }
}
