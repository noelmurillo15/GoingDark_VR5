using UnityEngine;
using System.Collections;

public class WayPointsManager : MonoBehaviour
{


    [SerializeField]
    private GameObject[] WayPoints;

    private GameObject[] AllWayPointsInScene;

    private ArrayList ArrayListForSorting = new ArrayList();


    private GameObject SearchForThisWayPoint;

    private GameObject Player;

    // Use this for initialization
    void Start()
    {

        AllWayPointsInScene = GameObject.FindGameObjectsWithTag("WayPoint");
        // WayPoints = new GameObject[AllWayPointsInScene.Length];
        Player = GameObject.FindWithTag("Player");
        //WayPoints[0] = GameObject.Find("StationWayPoint");
        SearchForThisWayPoint = GameObject.Find("StationWayPoint");
        //SearchForThisWayPoint = WayPoints[0];
    }


    public void SetNextActive()
    {
        int i = 0;
        float Distance = 10000.0f;
        for (; i < ArrayListForSorting.Count; i++)
        {
            GameObject Temp = (GameObject)ArrayListForSorting[i];

            if (ArrayListForSorting[i] != null && Temp != null)
            {
                float TempDistance = Vector3.Distance(Temp.transform.position, Player.transform.position);

                if (TempDistance < Distance) // checked against EMP distance for middle circle to light up.
                {
                    Distance = TempDistance;
                    SearchForThisWayPoint = Temp;
                }
            }
            else
                ArrayListForSorting.RemoveAt(i);
        }
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



}
