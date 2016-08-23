using UnityEngine;
using UnityEngine.UI;

public class LaserOverheat : MonoBehaviour
{

    private Image LaserGauge;
    public bool overheat;
    public float MaxAmount = 100.0f;
    public float CurrentAmount = 0.0f;
    [SerializeField]
    private GameObject smoke;

    void Start()
    {
        overheat = false;
        CurrentAmount = MaxAmount;
        LaserGauge = GameObject.Find("LaserOverHeat").GetComponent<Image>();
        smoke.SetActive(false);
    }

    void FixedUpdate()
    {
        if (!overheat)
            UpdateGauge(Time.fixedDeltaTime * 20f);
        else
        {
            if (CurrentAmount > 99f)
                Reset();

            UpdateGauge(Time.fixedDeltaTime * 10f);
        }
    }

    public bool GetOverheat()
    {
        return overheat;
    }

    void SetOverheat(bool _boolean)
    {
        overheat = _boolean;
        smoke.SetActive(_boolean);
    }

    public void UpdateGauge(float DamageTaken)
    {
        CurrentAmount += DamageTaken;
        if (CurrentAmount > 100f)
            CurrentAmount = 100f;
        else if (CurrentAmount < 0f)
        {
            SetOverheat(true);
        }

        float C_Shield = CurrentAmount / MaxAmount;

        if (!overheat)
            LaserGauge.color = Color.Lerp(Color.red, Color.cyan, C_Shield);
        else
            LaserGauge.color = Color.red;

        C_Shield *= .5f;
        SetHealth(C_Shield);
    }

    void SetHealth(float NewShield)
    {
        LaserGauge.fillAmount = NewShield;
    }

    public void Reset()
    {
        SetOverheat(false);
        CurrentAmount = MaxAmount;
        SetHealth(((CurrentAmount / MaxAmount) * 0.5f));
    }

}
