using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerViewCheck : MonoBehaviour {

    private float fov;
    private Vector3 rayDirection;
    private GameObject[] stars;
    private GameObject curStar;
    private Canvas canvas;
    private Text[] texts;
    public bool isSwitching;
    
    // Use this for initialization
    void Start () {
        fov = transform.GetComponent<Camera>().fieldOfView/5;
        rayDirection = Vector3.zero;
        canvas = gameObject.GetComponentInChildren<Canvas>();
        stars = GameObject.FindGameObjectsWithTag("Star");
        texts = canvas.GetComponentsInChildren<Text>();
        isSwitching = false;

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
            SceneManager.LoadScene("level1");
            //Application.LoadLevel();
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
                    //Debug.Log(stars[i].name);
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
