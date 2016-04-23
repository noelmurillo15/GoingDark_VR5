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

    void Awake()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void UpdateSpeedGauge() {
        float percentage = m_playerMove.GetSpeed() / m_playerMove.GetMaxSpeed();

        Vector3 newScale;
        newScale.x = speedBarColor1.transform.localScale.x;
        newScale.y = percentage * 0.001f;
        newScale.z = speedBarColor1.transform.localScale.z;

        speedBarColor1.transform.localScale = newScale;
        speedBarColor2.transform.localScale = newScale;

        Vector3 newPos = speedBarColor1.transform.localPosition;
        float offset = (percentage * 0.00456f) - 0.00456f;
        newPos.z = offset;
        speedBarColor1.transform.localPosition = newPos;

        newPos = speedBarColor2.transform.localPosition;
        newPos.z = offset;
        speedBarColor2.transform.localPosition = newPos;
    }
}
