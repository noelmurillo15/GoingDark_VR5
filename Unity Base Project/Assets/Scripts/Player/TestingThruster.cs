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

            if (m_palm.GetisLHandClosed() && !isStatic && m_palm.GetIsLeftHandIn())
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

        if (isStatic)
        {
            m_palm.SetTHAttached(true);
            Vector3 velocity;
            velocity = m_palm.GetLPalmVelocity();
            velocity.z = velocity.y;
            velocity.x = 0.0f;
            velocity.y = 0.0f;
            if((transform.localPosition.z + (velocity.z * Time.deltaTime * 0.001f)) > -0.0044f &&
                (transform.localPosition.z + (velocity.z * Time.deltaTime * 0.001f)) < 0.0044f) 
                transform.localPosition += (velocity * Time.deltaTime * 0.001f);
        }

        if (transform.localPosition.z < 0.0f)
            m_playerMove.DecreaseSpeed();
        else if (transform.localPosition.z > 0.0f)
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