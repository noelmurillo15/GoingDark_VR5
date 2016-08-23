using UnityEngine;
using System.Collections;

public class WayPointStation : MonoBehaviour
{

    private GameObject WayPointManager;

    // Use this for initialization
    void Start()
    {
        WayPointManager = GameObject.FindGameObjectWithTag("WayPointManager");
    }


    void OnTriggerEnter(Collider ColliderObject)
    {
        if (ColliderObject.CompareTag("Player"))
        {
            WayPointManager.SendMessage("SetWayPoints");
            WayPointManager.SendMessage("SetNextActive");
        }
    }

}