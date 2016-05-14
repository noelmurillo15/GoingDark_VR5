using UnityEngine;

public class Thrusters : MonoBehaviour {
    //**    Attach to Thruster  **//
    public bool inRange;
    public float offset;
    public HandBehavior m_palm;
    public PlayerStats stats;
    private GameObject speedGauge;


    // Use this for initialization
    void Start()
    {
        inRange = false;
        offset = 0.0044f;
        speedGauge = GameObject.Find("SpeedColor");
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update() {
        if (m_palm.GetIsLeftHandIn())
        {
            if (inRange && m_palm.GetisLHandClosed())
            {
                Vector3 velocity;
                velocity = m_palm.GetLPalmVelocity();
                velocity.x = velocity.y;
                velocity.z = 0.0f;
                velocity.y = 0.0f;

                if (transform.localEulerAngles.x > 30f && transform.localEulerAngles.x < 45)
                {
                    Vector3 euler = transform.localEulerAngles;
                    euler.x = 30f;
                    transform.localEulerAngles = euler;
                }
                else if (transform.localEulerAngles.x < 330f && transform.localEulerAngles.x > 315f)
                {
                    Vector3 euler = transform.localEulerAngles;
                    euler.x = 330f;
                    transform.localEulerAngles = euler;
                }

                transform.localEulerAngles += (velocity * Time.deltaTime);
            }
        }
        else
            inRange = false;

        if (transform.localEulerAngles.x > 328f && transform.localEulerAngles.x < 355f)
        {
            stats.DecreaseSpeed();
            UpdateSpeedGauge();
        }
        else if (transform.localEulerAngles.x > 5f && transform.localEulerAngles.x < 32)
        {
            stats.IncreaseSpeed();
            UpdateSpeedGauge();
        }        
    }

    private void UpdateSpeedGauge()
    {
        float percent = stats.GetMoveData().Speed / stats.GetMoveData().MaxSpeed;

        Vector3 newScale;
        newScale.x = speedGauge.transform.localScale.x;
        newScale.y = percent * 0.001f;
        newScale.z = speedGauge.transform.localScale.z;
        speedGauge.transform.localScale = newScale;

        Vector3 newPos = speedGauge.transform.localPosition;
        float offset = (percent * 0.00456f) - 0.00456f;
        newPos.z = offset;
        speedGauge.transform.localPosition = newPos;
    }

    #region Collision Detection
    void OnTriggerEnter(Collider col)
    {
        if (col.name == "leftPalm")
            inRange = true;
    }
    void OnTriggerExit(Collider col)
    {
        if (col.name == "leftPalm")
            inRange = false;
    }
    #endregion
}