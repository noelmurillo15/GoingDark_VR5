using UnityEngine;

public class LeapDriveHUD : MonoBehaviour {
    //**    Attach Script to Leap Control   **//

    private LeapData m_leapData;
    private PlayerMovement m_playerMove;

    private GameObject visorHUD;
    private GameObject driveWarning;

    private GameObject RArrow;
    private GameObject LArrow;

    private GameObject drivingBox;  


    // Use this for initialization
    void Start() {

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

        if (drivingBox == null)
            drivingBox = GameObject.Find("DrivingBox");     

        RArrow.SetActive(false);
        LArrow.SetActive(false);
        visorHUD.SetActive(false);
        drivingBox.SetActive(false);
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
            drivingBox.SetActive(true);

            if (m_leapData.GetNumHands() == 2) {                
                driveWarning.SetActive(false);
               
            }
            else if (m_leapData.GetNumHands() == 1) {
                
            }
            else {
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

    public void OnTriggerStay(Collider col)
    {
        Debug.Log("LeapMoveBox Collision with : " + col.name);
    }
}