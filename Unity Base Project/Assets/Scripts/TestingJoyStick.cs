using UnityEngine;
using System.Collections;

public class TestingJoyStick : MonoBehaviour {
    //**        Attach To Joystick Prefab        **//

    public bool isStatic;
    public bool palmAttached;

    public Transform m_handPos;
    public TestingHandBehavior m_palm;
    public JoyStickMovement m_playerMove;

    public Quaternion originalRotation;


	// Use this for initialization
	void Start () {
        isStatic = false;
        m_handPos = null;
        palmAttached = false;
        originalRotation = transform.localRotation;                  
        m_playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<JoyStickMovement>();
    }
	
	// Update is called once per frame
	void Update () {

        if(m_palm != null) {
            if (m_palm.GetisHandClosed() && !isStatic)                            
                isStatic = true;                            
            else if (!m_palm.GetisHandClosed() && isStatic) {
                isStatic = false;
                m_playerMove.turnRateZero();
                m_palm.SetJSAttached(false);
                transform.localRotation = originalRotation;
            }
        }

        if (isStatic) {
            m_palm.SetJSAttached(true);
            Vector3 velocity;
            velocity = m_palm.GetPalmVelocity();
            velocity.z = velocity.x;
            velocity.x = velocity.y;
            velocity.y = 0.0f;            
            transform.Rotate((velocity * 1.5f) * Time.deltaTime);            
        }

        if (transform.localEulerAngles.x > 1.0f && transform.localEulerAngles.x < 90.0f)
            m_playerMove.goUp();        
        else if (transform.localEulerAngles.x > 90.0f && transform.localEulerAngles.x < 360.0f)      
            m_playerMove.goDown();        

        if (transform.localEulerAngles.z > 1.0f && transform.localEulerAngles.z < 90.0f)
            m_playerMove.TurnLeft();        
        else if (transform.localEulerAngles.z > 90.0f && transform.localEulerAngles.z < 360.0f)
            m_playerMove.TurnRight();               
    }

    void OnTriggerEnter(Collider col) {
        if (col.name == "palm")
            m_handPos = col.transform;        
    }
    void OnTriggerStay(Collider col) {
        if (col.name == "palm")
            m_handPos = col.transform;        
    }
}
