using UnityEngine;
using System.Collections;

public class HyperDrive : MonoBehaviour {

    public bool hyperDrive;
    public float hyperDriveTimer;
    public float hyperDriveStartTimer;

    //public Vector3 particleOriginalPos;
    //private GameObject hyperDriveParticles;

    private JoyStickMovement m_playerMove;


    // Use this for initialization
    void Start () {
        hyperDrive = false;

        //if (shipLights.Length == 0)
        //shipLights = GameObject.FindGameObjectsWithTag("ShipLights");

        //if (hyperDriveParticles == null)
        //    hyperDriveParticles = GameObject.Find("HyperDriveParticles");
        //
        //particleOriginalPos = hyperDriveParticles.transform.localPosition;
        //hyperDriveParticles.SetActive(false);

        if (m_playerMove == null)
            m_playerMove = this.GetComponent<JoyStickMovement>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.H))
            HyperDriveInitialize();

        if (hyperDrive)
            HyperDriveMotherFucker();
    }

    public void HyperDriveMotherFucker()
    {
        if (hyperDriveStartTimer > 0.0f)
        {
            hyperDriveStartTimer -= Time.deltaTime;
            //hyperDriveParticles.transform.Translate(Vector3.forward * 10.0f * Time.deltaTime);
            hyperDriveTimer = 0.5f;
            m_playerMove.SetMoveSpeed(0.0f);
        }
        else
        {
            if (hyperDriveTimer > 0.0f)
            {
                hyperDriveTimer -= Time.deltaTime;
                transform.Translate(Vector3.forward * 800.0f * Time.deltaTime);
            }
            else
            {
                //hyperDriveParticles.transform.localPosition = particleOriginalPos;
                //hyperDriveParticles.SetActive(false);
                hyperDrive = false;
            }
        }
    }

    public void HyperDriveInitialize()
    {
        Debug.Log("Hyper Drive Initializing");
        m_playerMove.SetMoveSpeed(0.0f);
        //hyperDriveParticles.SetActive(true);
        hyperDrive = true;
        hyperDriveStartTimer = 5.0f;
    }
}
