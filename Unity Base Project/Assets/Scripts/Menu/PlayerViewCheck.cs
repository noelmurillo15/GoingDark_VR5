using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerViewCheck : MonoBehaviour {

    private float fov;
    private Vector3 rayDirection;
    private GameObject[] stars;
    private GameObject curStar;
    private Canvas canvas;
    private Text[] texts;
    private Text acceptButton;
    public float delayTimer;
    public bool isSwitching;
    private TransitionHyperDrive hyperDrive;
    private MapConnection[] isUnlocked;
    
    // Use this for initialization
    void Start () {
        delayTimer = 2.0f;
        fov = transform.GetComponent<Camera>().fieldOfView/5;
        rayDirection = Vector3.zero;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        stars = GameObject.FindGameObjectsWithTag("Star");
        texts = canvas.GetComponentsInChildren<Text>();
        acceptButton = GameObject.Find("Accept").GetComponentInChildren<Text>();
        isSwitching = false;
        hyperDrive = GameObject.Find("TransitionHyperDrive").GetComponent<TransitionHyperDrive>();

        isUnlocked = new MapConnection[GameObject.FindGameObjectsWithTag("Star").Length];
        isUnlocked[0] = GameObject.Find("SupplyHud").transform.FindChild("OpenWorld").GetComponent<MapConnection>();
        isUnlocked[1] = GameObject.Find("SupplyHud").transform.FindChild("Level2").GetComponent<MapConnection>();
        isUnlocked[2] = GameObject.Find("SupplyHud").transform.FindChild("Level3").GetComponent<MapConnection>();
        isUnlocked[3] = GameObject.Find("SupplyHud").transform.FindChild("Level4").GetComponent<MapConnection>();
        isUnlocked[4] = GameObject.Find("SupplyHud").transform.FindChild("Level5").GetComponent<MapConnection>();
        isUnlocked[5] = GameObject.Find("SupplyHud").transform.FindChild("Level6").GetComponent<MapConnection>();
    }

    // Update is called once per frame
    void Update () {
        IsInSight();
        if (isSwitching)
        {
            transform.LookAt(curStar.transform.position);
            canvas.enabled =false;
            hyperDrive.HyperDriveInitialize();
            if (hyperDrive.IsOver())
            {
                Debug.Log("Loading : " + curStar.name.ToString());
                SceneManager.LoadScene(curStar.name.ToString());
            }
        }
    }

    public void IsInSight()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            rayDirection = stars[i].transform.position - transform.position;
            RaycastHit hit;

            if (Vector3.Angle(rayDirection, transform.forward) <= fov)
            {
                if (Physics.Raycast(transform.position, rayDirection, out hit))
                {
                    curStar = stars[i];
                    canvas.enabled = true;
                    MissionText(stars[i].name);
                    if (curStar.GetComponent<MapConnection>().isUnlocked == 1)
                    {
                        acceptButton.text = "Accept";
                        if (Input.GetKeyDown(KeyCode.X))
                            isSwitching = true;
                        return;
                    }
                    else
                    {
                        acceptButton.text = "Locked";
                        isSwitching = false;
                        return;
                    }
                    //return;
                }
            }
            
        }
                canvas.enabled = false;
                isSwitching = false;
    }

    private void MissionText(string missionName)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i].name == missionName || texts[i].name == "Text")
            {
                texts[i].enabled = true;
            }
            else
                texts[i].enabled = false;
        }
        //for (int j = 0; j < isUnlocked.Length; j++)
        //{
        //    if (missionName == isUnlocked[j].name)
        //    {
        //        acceptButton.SetActive(true);
        //    }
        //    else
        //    {
        //        acceptButton.SetActive(false);
        //    }
        //}
    }
}
