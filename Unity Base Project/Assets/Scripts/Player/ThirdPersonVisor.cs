using Leap;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThirdPersonVisor : MonoBehaviour {
    //**    Attach Script to Leap Control   **//

    private GameObject visorHUD;
    private GameObject speedBar;
    private GameObject speedBarColor;
    private JoyStickMovement m_playerMove;

    public float speedBarX;


    // Use this for initialization
    void Start() {

        if (visorHUD == null)
            visorHUD = GameObject.Find("VisorHUD");

        if (speedBar == null)
            speedBar = GameObject.Find("SpeedBar");

        if (speedBarColor == null)
            speedBarColor = GameObject.Find("SpeedBarColor");

        m_playerMove = this.GetComponent<JoyStickMovement>();

        visorHUD.SetActive(true);
        speedBar.SetActive(true);
        speedBarColor.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        speedBarColor.SetActive(true);
        speedBarX = (m_playerMove.GetSpeed() / m_playerMove.GetMaxSpeed()) * 0.25f;

        Vector3 newScale;
        newScale.x = speedBarColor.transform.localScale.x;
        newScale.y = speedBarX;
        newScale.z = speedBarColor.transform.localScale.z;
        speedBarColor.transform.localScale = newScale;

        Vector3 newPos;
        newPos.x = speedBarColor.transform.localPosition.x;
        newPos.y = -1.1f + (m_playerMove.GetSpeed() / m_playerMove.GetMaxSpeed() * 1.2f);
        newPos.z = speedBarColor.transform.localPosition.z;
        speedBarColor.transform.localPosition = newPos;
    }
}
