using UnityEngine;
using System.Collections;

public class TestingJoyStick : MonoBehaviour {

    public Transform m_handPos;
    public Transform m_joyStickPos;
    public TestingHandBehavior m_palm;

    public Quaternion originalRotation;

    public float absX, absZ;
    public bool xAxis, zAxis;

    public GameObject upArrow, downArrow;
    public GameObject leftArrow, rightArrow;

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

        xAxis = false;
        zAxis = false;

        upArrow = GameObject.Find("up");
        downArrow = GameObject.Find("down");
        leftArrow = GameObject.Find("left");
        rightArrow = GameObject.Find("right");

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
                xAxis = false;
                zAxis = false;
                isStatic = false;
                m_palm.SetJSAttached(false);
                transform.localRotation = originalRotation;
            }
        }

        if (isStatic)
        {
            m_palm.SetJSAttached(true);
            velocity = m_palm.GetPalmVelocity();
            velocity.z = velocity.x;
            absZ = Mathf.Abs(velocity.z);
            velocity.x = velocity.y;
            absX = Mathf.Abs(velocity.x);
            velocity.y = 0.0f;

            if(absX > absZ)
            {
                xAxis = true;
                zAxis = false;
                velocity.z = 0.0f;
            }
            else if(absZ > absX)
            {
                xAxis = false;
                zAxis = true;
                velocity.x = 0.0f;
            }            

            Debug.Log("Rotating the Joystick with palm velocity");
            transform.Rotate((velocity * 1.5f) * Time.deltaTime);
        }

        if (xAxis)
        {
            if (velocity.x > 0.0f && velocity.x < 90.0f)
                m_playerMove.goUp();
            else
                m_playerMove.goDown();
        }
        else if (zAxis)
        {
            if (velocity.z > 0.0f && velocity.z < 90.0f)
                m_playerMove.turnLeft();
            else
                m_playerMove.turnRight();
        }
        else
            m_playerMove.StopAllMovement();

        upArrow.SetActive(xAxis); downArrow.SetActive(xAxis);
        leftArrow.SetActive(zAxis); rightArrow.SetActive(zAxis);
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
    //void OnTriggerEnter(Collision col)
    //{
    //    m_handPos = null;
    //}
}
