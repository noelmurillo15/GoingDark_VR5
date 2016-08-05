using UnityEngine;

public class WayFinder_Arrow : MonoBehaviour
{

    [SerializeField]
    private GameObject WayPointManager;
    GameObject WayPoint;

    // Use this for initialization
    void Start()
    {
        WayPointManager = GameObject.FindGameObjectWithTag("WayPointManager");
        //Timing.RunCoroutine(NewWayPoint());
    }

    void LateUpdate()
    {
        WayPoint = WayPointManager.GetComponent<WayPointsManager>().GetWayPointToFollow();
        if (WayPoint != null)
            transform.LookAt(WayPoint.transform);
        else
            WayPointManager.GetComponent<WayPointsManager>().SetNextActive();
    }

    //#region Coroutine
    //private IEnumerator<float> NewWayPoint()
    //{
    //    while (true)
    //    {
    //        LookAtWayPoint();
    //        yield return Timing.WaitForSeconds(Random.Range(1f, 5f));
    //    }
    //}
    //private void LookAtWayPoint()
    //{
    //    Debug.Log("Look At Way Points Triggered");
    //    transform.LookAt(WayPointManager.GetComponent<WayPointsManager>().GetWayPointToFollow().transform);
    //}
    //#endregion



}
