using UnityEngine;


public class LeapCommands : MonoBehaviour {
    //**    Attach Script to Leap Mount   **//

    private LeapData m_leapData;
    private PlayerMovement m_playerMove;


    // Use this for initialization
    void Start() {
        if (m_leapData == null)
            m_leapData = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<LeapData>();

        if (m_playerMove == null)
            m_playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update() {
        if (m_leapData != null) 
            CheckDriveCommand();

        if (m_playerMove.GetDriveMode()) {
            if (transform.localPosition.y < 2.4f)
                transform.Translate(0.0f, Time.deltaTime, 0.0f);
            else if (transform.localPosition.y > 2.4f)
                transform.localPosition.Set(transform.localPosition.x, 2.4f, transform.localPosition.z);
        }
        else {
            if (transform.localPosition.y > 0.36f)
                transform.Translate(0.0f, -Time.deltaTime, 0.0f);
            else if (transform.localPosition.y < 0.36f)
                transform.localPosition.Set(transform.localPosition.x, 0.36f, transform.localPosition.z);
        }
    }

    public void CheckDriveCommand() {
        if (m_leapData.GetManualDriveSign()) 
            if (!m_playerMove.GetDriveMode()) 
                m_playerMove.SetManualDrive(true);

        if (m_leapData.GetAutoPilotSign())
            if (m_playerMove.GetDriveMode())
                m_playerMove.SetManualDrive(false);
    }
}