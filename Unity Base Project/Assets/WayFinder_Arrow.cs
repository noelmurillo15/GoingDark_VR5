using UnityEngine;
using System.Collections;

public class WayFinder_Arrow : MonoBehaviour {

    [SerializeField]
    private GameObject[] WayPoints;

    [SerializeField]
    private WayPointsManager WayPointManager;
    // Use this for initialization
    void Start () {
        WayPoints =WayPointManager.GetWaypoints();        
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
