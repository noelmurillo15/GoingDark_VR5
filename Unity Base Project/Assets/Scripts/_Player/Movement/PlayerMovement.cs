using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {
    //**    Attach to Player    **//

    #region Properties
    private Vector3 moveDir;
    private PlayerStats stats;
    private HeadMovement headMove;
    private Transform MyTransform;
    private CharacterController m_controller;

    //  Auto-Movement
    private bool autoPilot;
    private bool resetRotation;
    private float orientationTimer;
    private Vector3 autoPilotDestination;
    #endregion


    // Use this for initialization
    void Start() {
        moveDir = Vector3.zero;
        MyTransform = transform;
        stats = GetComponent<PlayerStats>();
        m_controller = GetComponent<CharacterController>();
        headMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<HeadMovement>();

        autoPilot = false;
        resetRotation = false;
        orientationTimer = 0.0f;
        autoPilotDestination = Vector3.zero;
        OutOfBounds(autoPilotDestination);
    }

    // Update is called once per frame
    void Update(){
        if (!autoPilot && !resetRotation) {            
            Flight();                
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
    }
    public void InBounds()
    {
        autoPilot = false;
        headMove.enabled = true;
        autoPilotDestination = Vector3.zero;        
    }
    public void ResetOrientation()
    {
        orientationTimer = 5.0f;
        resetRotation = true;
    }

    private void Autopilot() {
        //  Look At
        Vector3 playerDir = autoPilotDestination - MyTransform.position;
        Vector3 destination = Vector3.RotateTowards(MyTransform.forward, playerDir, (stats.GetMoveData().RotateSpeed * 0.05f) * Time.deltaTime, 0.0f);
        MyTransform.rotation = Quaternion.LookRotation(destination);

        //  Move Towards
        moveDir = Vector3.zero;
        moveDir = MyTransform.TransformDirection(Vector3.forward);
        moveDir *= (stats.GetMoveData().MaxSpeed * Time.deltaTime * 5f);
        m_controller.Move(moveDir);        
    }        
    public void Reorient()
    {
        if (MyTransform.rotation != Quaternion.identity && orientationTimer > 0.0f) {
            orientationTimer -= Time.deltaTime;
            MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.identity, Time.deltaTime * 0.5f);
        }
        else
            resetRotation = false;
    }

    private void Flight() {
        if (stats.GetMoveData().Speed <= 0f)
            return;

        moveDir = Vector3.zero;
        moveDir = MyTransform.TransformDirection(Vector3.forward);
        moveDir *= stats.GetMoveData().Speed * Time.deltaTime;
        m_controller.Move(moveDir);
    }     
    #endregion
}