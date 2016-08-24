using UnityEngine;
using UnityEngine.UI;

public class SpeedGauge : MonoBehaviour
{

    private Text number;
    private float percent;
    private PlayerMovement stats;


    // Use this for initialization
    void Start()
    {
        percent = 0f;
        number = GetComponent<Text>();
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeedGauge();
    }

    public void UpdateSpeedGauge()
    {
        if(stats.GetMoveData().MaxSpeed > 0f)
            percent = stats.GetMoveData().Speed / stats.GetMoveData().MaxSpeed;

        int num = (int)(percent * 100f);
        number.text = num.ToString();
        number.color = Color.Lerp(Color.red, Color.green, percent);
    }
}