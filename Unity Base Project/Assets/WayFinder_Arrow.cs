using UnityEngine;

public class WayFinder_Arrow : MonoBehaviour {

    [SerializeField]
    private GameObject WayPointManager;
    // Use this for initialization
    void Start () {
        WayPointManager = GameObject.FindGameObjectWithTag("WayPointManager");
    }

    // Update is called once per frame
    void Update () {

        int i = 0;
        for (; i < WayPointManager.transform.childCount; i++)
        {
            GameObject Temp = WayPointManager.transform.GetChild(i).gameObject;
            if (Temp && Temp.activeInHierarchy)
            {
                transform.LookAt(Temp.transform);
                break;
            }
        }
       
    }
}
