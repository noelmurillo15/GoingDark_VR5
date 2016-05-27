using UnityEngine;

public class SpeedGauge : MonoBehaviour
{
    //**    Attach to Thruster  **//
    public float offset;
    public PlayerMovement stats;
    private GameObject speedGauge;


    // Use this for initialization
    void Start()
    {
        offset = 0.0044f;
        speedGauge = GameObject.Find("SpeedColor");
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeedGauge();
    }

    public void UpdateSpeedGauge()
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
}