using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class CameraMovement : MonoBehaviour {
    // menu object for tactician
    public GameObject selected_arrow;
    //public GameObject tactician_menu;

    float sensitivity = 60.0f;
    bool tactician_trigger = false;
    bool collider_coroutine = false;

    // sonar button
    [SerializeField] private Button mSonar;
    // cloaking/uncloak button
    [SerializeField] private Button mCloak;

    Color color;
    Color normalColor;

    void Start () {
        normalColor = Color.white;
        //tactician_menu.SetActive(false);
        selected_arrow.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Player")
        {
            tactician_trigger = true;
            //tactician_menu.SetActive(true);
            selected_arrow.SetActive(true);
            Debug.Log("Collision with " + col.tag);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag != "Player")
        {
            tactician_trigger = false;
            selected_arrow.SetActive(false);
            //tactician_menu.SetActive(false);
            Debug.Log("Lost Collision with " + col.tag);
        }
    }

	// Update is called once per frame
	void Update () {
        // rotate camera 360 degrees
        transform.RotateAround(transform.position, Vector3.up, InputChecker.GetMouse(AXIS.X) * sensitivity);
        //if (tactician_trigger && Input.GetKeyDown(KeyCode.H))
        //    tactician_menu.SetActive(!tactician_menu.activeSelf);

        //if (tactician_menu.activeSelf)
        //{
        //    // use sonar
        //    if (Input.GetKeyDown(KeyCode.X))
        //    {
        //        // give visual que for using sonar
        //        collider_coroutine = true;
        //        StartCoroutine(changeColorForShortTime(mSonar));
        //    }
        //    else if (Input.GetKeyDown(KeyCode.C))
        //    {
        //        // give visual que for using sonar
        //        collider_coroutine = true;
        //        StartCoroutine(changeColorForShortTime(mCloak));
        //    }
        //}
    }

    public IEnumerator changeColorForShortTime(Button button)
    {
        button.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        //if (collider_coroutine)
           //tactician_menu.SetActive(false);
        button.GetComponent<Image>().color = normalColor;
        collider_coroutine = false;
    }
}
