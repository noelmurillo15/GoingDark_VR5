using UnityEngine;
using UnityEngine.UI;

public class SpeedGauge : MonoBehaviour
{
    private Image Gauge;
    private float amount;
    private float percent;
    private Text number;
    private PlayerMovement stats;


    // Use this for initialization
    void Start()
    {
        amount = 0f;
        percent = 0f;
        Gauge = GetComponent<Image>();
        number = transform.GetChild(0).GetComponent<Text>();
        stats = GameObject.Find("PlayerTutorial").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeedGauge();
    }

    public void UpdateSpeedGauge()
    {
        percent = stats.GetMoveData().Speed / stats.GetMoveData().MaxSpeed;

        int num = (int)(percent * 100f);
        number.text = num.ToString();

        SetSpeed(percent * .5f);      
        number.color = Color.Lerp(Color.red, Color.green, percent);
        Gauge.color = Color.Lerp(Color.red, Color.green, percent);
    }

    void SetSpeed(float newSpeed)
    {
        if (newSpeed > .5f)
            Gauge.fillAmount = .5f;
        else
            Gauge.fillAmount = newSpeed;
    }
}