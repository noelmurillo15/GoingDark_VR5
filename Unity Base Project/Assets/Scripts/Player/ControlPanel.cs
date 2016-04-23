using UnityEngine;
using System.Collections;

public class ControlPanel : MonoBehaviour {

    private GameObject speedBarColor1;
    private GameObject speedBarColor2;
    private JoyStickMovement m_playerMove;


    // Use this for initialization
    void Start () {

        if (speedBarColor1 == null)
            speedBarColor1 = GameObject.Find("SpeedColor1");

        if (speedBarColor2 == null)
            speedBarColor2 = GameObject.Find("SpeedColor2");

        m_playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<JoyStickMovement>();
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 newScale;
        newScale.x = speedBarColor1.transform.localScale.x;
        newScale.y = (m_playerMove.GetSpeed() / m_playerMove.GetMaxSpeed()) * 0.001f;
        newScale.z = speedBarColor1.transform.localScale.z;

        if (newScale.y < 0.00095 && newScale.y > 0.0f)
        {
            speedBarColor1.transform.localScale = newScale;
            speedBarColor2.transform.localScale = newScale;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.name + " is triggering " + transform.name);
    }
    void OnTriggerExit(Collider col)
    {
        Debug.Log(col.name + " is not triggering " + transform.name);
    }
}
