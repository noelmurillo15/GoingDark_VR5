using UnityEngine;
using System.Collections;

public class WayPointsManager : MonoBehaviour
{


    [SerializeField]
    private GameObject[] AllWayPointsInScene;

    private ArrayList ArrayListForSorting = new ArrayList();


    private GameObject SearchForThisWayPoint;
    private GameObject StationsWayPoint;

    private GameObject Player;
    private bool SendBackToStation;

    // Use this for initialization
    void Start()
    {
        SendBackToStation = true;

        // WayPoints = new GameObject[AllWayPointsInScene.Length];
        Player = GameObject.FindWithTag("Player");
        //WayPoints[0] = GameObject.Find("StationWayPoint");

        StationsWayPoint = GameObject.Find("StationWayPoint");
        SearchForThisWayPoint = StationsWayPoint;
        //SearchForThisWayPoint = WayPoints[0];
        Invoke("FindWayPoints", 2);
        InvokeRepeating("SetNextActive", 5, 5);
    }


    public void SetNextActive()
    {
        int i = 0;
        float Distance = float.MaxValue;
        SendBackToStation = true;
        for (; i < ArrayListForSorting.Count; i++)
        {
            GameObject Temp = (GameObject)ArrayListForSorting[i];

            if (ArrayListForSorting[i] != null && Temp != null)
            {
                float TempDistance = Vector3.Distance(Temp.transform.position, Player.transform.position);

                if (TempDistance < Distance) 
                {
                    Distance = TempDistance;
                    SearchForThisWayPoint = Temp;
                    SendBackToStation = false;
                }
            }
            else
            {
                ArrayListForSorting.RemoveAt(i);
            }
        }
        ArrayListForSorting.TrimToSize();
        if (SendBackToStation)
            SearchForThisWayPoint = StationsWayPoint;
    }

    public void SetWayPoints()
    {
        for (int i = 0; i < AllWayPointsInScene.Length; i++)
        {
            ArrayListForSorting.Add(AllWayPointsInScene[i]);
        }
    }

    public GameObject GetWayPointToFollow()
    {
        return SearchForThisWayPoint;
    }

    public void FindWayPoints()
    {
        AllWayPointsInScene = GameObject.FindGameObjectsWithTag("WayPoint");
    }



}
