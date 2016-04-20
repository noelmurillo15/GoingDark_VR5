using UnityEngine;
using System.Collections;

public class TestingJoyStick : MonoBehaviour {

    public Transform m_handPos;
    public Transform m_joyStickPos;
    public TestingHandBehavior m_palm;

    public Quaternion originalRotation;

    public JoyStickMovement m_playerMove;

    public Vector3 velocity;

    public bool isStatic;

    public float zOffset;

    public bool palmAttached;

	// Use this for initialization
	void Start () {
        palmAttached = false;
        isStatic = false;
        m_handPos = null;
        m_joyStickPos = transform;

        m_playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<JoyStickMovement>();

        originalRotation = transform.localRotation;                  
    }
	
	// Update is called once per frame
	void Update () {

        if(m_palm != null)
        {
            if (m_palm.GetisHandClosed() && !isStatic)                            
                isStatic = true;                            
            else if (!m_palm.GetisHandClosed() && isStatic)
            {
                velocity = Vector3.zero;
                isStatic = false;
                m_playerMove.StopAllMovement();
                m_palm.SetJSAttached(false);
                transform.localRotation = originalRotation;
            }
        }

        if (isStatic) {
            m_palm.SetJSAttached(true);
            velocity = m_palm.GetPalmVelocity();
            velocity.z = velocity.x;
            velocity.x = velocity.y;
            velocity.y = 0.0f;            
            transform.Rotate((velocity * 1.5f) * Time.deltaTime);            
        }

        if (transform.localEulerAngles.x > 1.0f && transform.localEulerAngles.x < 90.0f)
        {
            m_playerMove.goUp();
        }
        else if (transform.localEulerAngles.x > 90.0f && transform.localEulerAngles.x < 360.0f)
        {
            m_playerMove.goDown();
        }

        if (transform.localEulerAngles.z > 1.0f && transform.localEulerAngles.z < 90.0f)
        {
            m_playerMove.turnLeft();
        }
        else if (transform.localEulerAngles.z > 90.0f && transform.localEulerAngles.z < 360.0f)
        {
            m_playerMove.turnRight();
        }        
    }

    void OnTriggerEnter(Collider col)
    {
        m_handPos = col.transform;        
    }
    void OnTriggerStay(Collider col)
    {
        if (col.name == "palm")
        {
            Debug.Log("Palm Attached");
            m_handPos = col.transform;
        }
    }
}
