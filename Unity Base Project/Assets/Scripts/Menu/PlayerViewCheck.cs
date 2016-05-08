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
    public float delayTimer;
    public bool isSwitching;
    private TransitionHyperDrive hyperDrive;
    
    // Use this for initialization
    void Start () {
        delayTimer = 2.0f;
        fov = transform.GetComponent<Camera>().fieldOfView/5;
        rayDirection = Vector3.zero;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        stars = GameObject.FindGameObjectsWithTag("Star");
        texts = canvas.GetComponentsInChildren<Text>();
        isSwitching = false;
        hyperDrive = GameObject.Find("TransitionHyperDrive").GetComponent<TransitionHyperDrive>();
    }

    // Update is called once per frame
    void Update () {
        IsInSight();
        if (isSwitching)
        {
            transform.LookAt(curStar.transform.position);
            //Vector3.Lerp(transform.position, curStar.transform.position, Time.deltaTime * 10);
            //transform.Rotate()
            canvas.enabled = false;
            hyperDrive.HyperDriveInitialize();
            if (hyperDrive.IsOver())
            {
                SceneManager.LoadScene(curStar.name.ToString());
            }
            //if (delayTimer <= 0.0f)
            //    SceneManager.LoadScene("level1");
            //else
            //    delayTimer -= Time.deltaTime;
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
                    return;
                }
            }
            else
                canvas.enabled = false;
        }
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
    }
    
}
