using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {
    //**    Attach to Player    **//

    //  Movement
    private Vector3 moveDir;
    private PlayerStats stats;
    private CharacterController m_controller;

    //  Auto-Movement
    private bool autoMove;
    private bool autoRotate;
    private bool resetRotation;
    private float orientationTimer;
    private Vector3 targetPosition;
    private GameObject autoPilotSign;
    private GameObject reorientSign;


    // Use this for initialization
    void Start() {
        autoMove = false;
        autoRotate = false;
        resetRotation = false;
        orientationTimer = 0.0f;    
        moveDir = Vector3.zero;

        autoPilotSign = GameObject.Find("AutoPilot");
        autoPilotSign.SetActive(autoRotate);
        reorientSign = GameObject.Find("Reorient");
        reorientSign.SetActive(resetRotation);

        stats = GetComponent<PlayerStats>();
        m_controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update(){
        if (!autoRotate && !autoMove && !resetRotation) {
            moveDir = Vector3.zero;

            if(stats.GetMoveSpeed() > 0.0f)
                ManualWalk();

            if(moveDir != Vector3.zero)
                m_controller.Move(moveDir);
        }
        else if(autoRotate || autoMove)
            Autopilot();
        else if (resetRotation)
            Reorient();        
    }

    #region Movement
    private void Autopilot() {
        float angle = 0.0f;
        if (autoRotate)
        {
            Vector3 playerDir = targetPosition - transform.position;
            Vector3 newEnemyDir = Vector3.RotateTowards(transform.forward, playerDir, (stats.GetRotateSpeed() * 0.1f) * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newEnemyDir);
            angle = Vector3.Angle(newEnemyDir, playerDir);
        }

        if (autoMove)
        {
            moveDir = Vector3.zero;
            moveDir = transform.TransformDirection(Vector3.forward);
            moveDir *= (stats.GetMaxSpeed() * Time.deltaTime * 5f);
            m_controller.Move(moveDir);
        }

        if (angle <= 2.5f && autoRotate)
        {
            autoRotate = false;
            autoMove = true;
        }
    }

    public void OutOfBounds(Vector3 targetPos)
    {
        autoRotate = true;
        autoPilotSign.SetActive(autoRotate);
        targetPosition = targetPos;
    }

    public void InBounds()
    {
        autoRotate = false;
        autoMove = false;
        autoPilotSign.SetActive(autoRotate);
    }

    public void ResetOrientation()
    {
        orientationTimer = 5.0f;
        resetRotation = true;
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

    public void SetPlayerRotation(Quaternion rot) {
        if(!resetRotation && !autoMove && !autoRotate)
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 0.5f);
    }

    private void ManualWalk() {
        moveDir = transform.TransformDirection(Vector3.forward);
        if (stats.GetMoveSpeed() >= stats.GetMaxSpeed())
            moveDir *= (stats.GetMaxSpeed() * Time.deltaTime) * 1.5f;
        else
            moveDir *= stats.GetMoveSpeed() * Time.deltaTime;        
    }     
    #endregion
}