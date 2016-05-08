using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {
    //**    Attach to Player    **//

    //  Player
    private Vector3 moveDir;
    private PlayerStats stats;
    private HeadMovement headMove;
    private CharacterController m_controller;

    //  Auto-Movement
    private bool autoPilot;
    private bool resetRotation;
    private float orientationTimer;
    private Vector3 autoPilotDestination;

    private GameObject reorientSign;
    private GameObject autoPilotSign;


    // Use this for initialization
    void Start() {
        autoPilot = false;
        resetRotation = false;
        orientationTimer = 0.0f;    
        moveDir = Vector3.zero;
        autoPilotDestination = Vector3.zero;

        autoPilotSign = GameObject.Find("AutoPilot");
        autoPilotSign.SetActive(autoPilot);
        reorientSign = GameObject.Find("Reorient");
        reorientSign.SetActive(resetRotation);

        stats = GetComponent<PlayerStats>();
        m_controller = GetComponent<CharacterController>();
        headMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<HeadMovement>();

        OutOfBounds(Vector3.zero);
    }

    // Update is called once per frame
    void Update(){
        if (!autoPilot && !resetRotation) {
            moveDir = Vector3.zero;

            if(stats.GetMoveSpeed() > 0f)
                ManualWalk();

            if(moveDir != Vector3.zero)
                m_controller.Move(moveDir);
        }
        else if(autoPilot)
            Autopilot();
        else if (resetRotation)
            Reorient();        
    }

    #region Movement
    public void OutOfBounds(Vector3 targetPos)
    {
        autoPilot = true;
        headMove.enabled = false;
        autoPilotDestination = targetPos;
        autoPilotSign.SetActive(autoPilot);
    }
    public void InBounds()
    {
        autoPilot = false;
        headMove.enabled = true;
        autoPilotDestination = Vector3.zero;
        autoPilotSign.SetActive(autoPilot);
    }
    public void ResetOrientation()
    {
        orientationTimer = 5.0f;
        resetRotation = true;
    }

    private void Autopilot() {
        //  Look At
        Vector3 playerDir = autoPilotDestination - transform.position;
        Vector3 destination = Vector3.RotateTowards(transform.forward, playerDir, (stats.GetRotateSpeed() * 0.05f) * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(destination);

        //  Move Towards
        moveDir = Vector3.zero;
        moveDir = transform.TransformDirection(Vector3.forward);
        moveDir *= (stats.GetMaxSpeed() * Time.deltaTime * 5f);
        m_controller.Move(moveDir);        
    }        
    public void Reorient()
    {
        if (transform.rotation != Quaternion.identity && orientationTimer > 0.0f) {
            orientationTimer -= Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * 0.5f);
        }
        else
            resetRotation = false;

        reorientSign.SetActive(resetRotation);
    }

    private void ManualWalk() {
        moveDir = transform.TransformDirection(Vector3.forward);
        moveDir *= stats.GetMoveSpeed() * Time.deltaTime;   
    }     
    #endregion
}