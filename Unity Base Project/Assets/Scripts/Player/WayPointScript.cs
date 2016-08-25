using UnityEngine;
using System.Collections;

public class WayPointScript : MonoBehaviour
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
            WayPointManager.SendMessage("SetNextActive");
            Destroy(gameObject);
        }
    }

}
