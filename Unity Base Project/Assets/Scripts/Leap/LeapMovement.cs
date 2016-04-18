using Leap;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeapMovement : MonoBehaviour {
    //**    Attach Script to Leap Control   **//

    private float HandOffset;
    private float axisControl;

    private LeapData m_leapData;
    private PlayerMovement m_playerMove;

    private GameObject visorHUD;
    private GameObject driveWarning;

    private GameObject RArrow;
    private GameObject LArrow;    


    // Use this for initialization
    void Start() {
        HandOffset = 0.0f;        
        axisControl = 0.18f;

        if (m_leapData == null)
            m_leapData = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<LeapData>();

        if (m_playerMove == null)
            m_playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        if (visorHUD == null)
            visorHUD = GameObject.Find("VisorHUD");

        if (driveWarning == null)
            driveWarning = GameObject.Find("DrivingWarning");

        if(RArrow == null)
            RArrow = GameObject.Find("rightArrow");

        if (LArrow == null)
            LArrow = GameObject.Find("leftArrow");        

        RArrow.SetActive(false);
        LArrow.SetActive(false);
        visorHUD.SetActive(false);
        driveWarning.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateManualMovement();
    }

    public void UpdateManualMovement() {
        if (m_playerMove.GetDriveMode()) {
            visorHUD.SetActive(true);

            if (m_leapData.GetNumHands() == 2) {                
                driveWarning.SetActive(false);

                HandOffset = Mathf.Abs(m_leapData.GetLPalmNormals().y - m_leapData.GetRPalmNormals().y);
                if (HandOffset > axisControl) {
                    if (m_leapData.GetLPalmNormals().y > m_leapData.GetRPalmNormals().y) {
                        RArrow.SetActive(true);
                        LArrow.SetActive(false);
                        m_playerMove.turnLeft();
                    }
                    else {
                        RArrow.SetActive(false);
                        LArrow.SetActive(true);
                        m_playerMove.turnRight();
                                              
                    }
                }
                else {
                    RArrow.SetActive(false);
                    LArrow.SetActive(false);
                    m_playerMove.SetTurnRate(0);
                }             

                if (m_leapData.GetLGripStrength() > 0.8f || m_leapData.GetRGripStrength() > 0.8f)
                    m_playerMove.IncreaseSpeed();
                else
                    m_playerMove.DescreaseSpeed();

            }
            else if (m_leapData.GetNumHands() == 1) {
                m_playerMove.StopAllMovement();
                driveWarning.SetActive(true);
            }
            else {
                m_playerMove.StopAllMovement();
                driveWarning.SetActive(true);
            }
        }
        else {
            RArrow.SetActive(false);
            LArrow.SetActive(false);
            visorHUD.SetActive(false);
            driveWarning.SetActive(false);
        }
    }
}