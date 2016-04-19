using UnityEngine;

public class LeapMovement : MonoBehaviour
{
    //**    Attach Script to DriveBox   **//

    public Renderer rend;

    public bool goingLeft;
    public bool goingRight;
    public bool goingUp;
    public bool goingDown;

    public LeapData m_leapData;
    public PlayerMovement m_playerMove;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        goingLeft = false;
        goingRight = false;
        goingDown = false;
        goingUp = false;
        rend.material.shader = Shader.Find("Transparent/Diffuse");

        m_leapData = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<LeapData>();
        m_playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();        
    }

    // Update is called once per frame
    void Update() {
        if (goingLeft && goingRight)
        {
            m_playerMove.turnRateZero();
            ChangeColor(Color.green);
        }
        else if (!goingLeft && !goingRight)
        {
            ChangeColor(Color.red);
            m_playerMove.turnRateZero();

            float offset = m_leapData.GetLPalmPosition().normalized.z;
            if (offset < 0.0f)
                m_playerMove.goUp();
            else if (offset > 0.0f)
                m_playerMove.goDown();
        }

        if (m_leapData.GetNumLFingersHeld() == 5 && m_leapData.GetNumRFingersHeld() == 5)
            m_playerMove.DescreaseSpeed();
        else
            m_playerMove.IncreaseSpeed();

        TurnLeft();
        TurnRight();
    }

    #region Collision Detection
    public void OnTriggerEnter(Collider col)
    {
        goingUp = false;
        goingDown = false;
        if (col.name == "palm")
        {
            if (col.transform.parent.name == "LeftHand")
                goingLeft = true;

            if (col.transform.parent.name == "RightHand")
                goingRight = true;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.name == "palm")
        {
            if (col.transform.parent.name == "LeftHand")
                goingLeft = false;

            if (col.transform.parent.name == "RightHand")
                goingRight = false;
        }
    }
    #endregion

    #region Public Methods
    public void TurnRight()
    {
        if (goingLeft && !goingRight)
        {
            m_playerMove.turnRight();
            ChangeColor(Color.blue);
        }       
    }

    public void TurnLeft()
    {
        if (!goingLeft && goingRight)
        {
            m_playerMove.turnLeft();
            ChangeColor(Color.magenta);
        }
    }

    public void ChangeColor(Color col)
    {
        Color tmp = col;
        tmp.a = 0.25f;
        rend.material.color = tmp;  
    }
    #endregion
}