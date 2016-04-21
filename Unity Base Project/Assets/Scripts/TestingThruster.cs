using UnityEngine;
using System.Collections;

public class TestingThruster : MonoBehaviour {
    //**        Attach to Thruster Prefab       **//

    public bool isStatic;
    public bool palmAttached;

    public Transform m_handPos;
    public TestingHandBehavior m_palm;
    public JoyStickMovement m_playerMove;

    public Quaternion originalRotation;


    // Use this for initialization
    void Start()
    {
        isStatic = false;
        m_handPos = null;
        palmAttached = false;
        originalRotation = transform.localRotation;
        m_playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<JoyStickMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        if (m_palm != null)
        {
            if (m_palm.GetisLHandClosed() && !isStatic)
            {
                isStatic = true;
                transform.localRotation = originalRotation;
            }
            else if (!m_palm.GetisLHandClosed() && isStatic)
            {
                isStatic = false;
                m_playerMove.turnRateZero();
                m_palm.SetTHAttached(false);                
            }
        }

        if (isStatic)
        {
            m_palm.SetTHAttached(true);
            Vector3 velocity;
            velocity = m_palm.GetLPalmVelocity();
            velocity.z = 0.0f;
            velocity.x = 0.0f;
            transform.Rotate((velocity * 1.5f) * Time.deltaTime);
        }

        if (transform.localEulerAngles.x > 1.0f && transform.localEulerAngles.x < 90.0f)
            m_playerMove.DecreaseSpeed();
        else if (transform.localEulerAngles.x > 90.0f && transform.localEulerAngles.x < 360.0f)
            m_playerMove.IncreaseSpeed();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "leftPalm")
            m_handPos = col.transform;
    }
    void OnTriggerStay(Collider col)
    {
        if (col.name == "leftPalm")
            m_handPos = col.transform;
    }
}