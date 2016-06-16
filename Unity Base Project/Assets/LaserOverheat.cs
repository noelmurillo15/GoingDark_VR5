using UnityEngine;
using UnityEngine.UI;
using GoingDark.Core.Enums;

public class LaserOverheat : MonoBehaviour {
    
    private Image LaserGauge;

    public bool overheat;
    public float MaxAmount = 100.0f;
    public float CurrentAmount = 0.0f;


    void Start()
    {
        overheat = false;
        CurrentAmount = MaxAmount;
        LaserGauge = GetComponent<Image>();     
    }

    void Update()
    {
        if(!overheat)
            UpdateGauge(Time.deltaTime * 20f);
        else
        {
            if (CurrentAmount > 95f)
                Reset();
            UpdateGauge(Time.deltaTime * 15f);
        }
    }

    public bool GetOverheat()
    {
        return overheat;
    }

    void SetOverheat(bool _boolean)
    {
        overheat = _boolean;
    }
	
    public void UpdateGauge(float DamageTaken)
    {
        CurrentAmount += DamageTaken;
        if (CurrentAmount > 100f)
            CurrentAmount = 100f;
        else if(CurrentAmount < 0f)
        {
            Debug.Log("Lasers Overheated");
            SetOverheat(true);
        }

        float C_Shield = CurrentAmount / MaxAmount;
        C_Shield *= .5f;
        SetHealth(C_Shield);
    }

    void SetHealth(float NewShield)
    {
        if (NewShield >= .4f)
            LaserGauge.color = Color.cyan;
        else
            LaserGauge.color = Color.yellow;

        if (overheat)
            LaserGauge.color = Color.red;

        LaserGauge.fillAmount = NewShield;
    }

    public void Reset()
    {
        Debug.Log("Laser Overheat Reset");
        SetOverheat(false);
        CurrentAmount = MaxAmount;
        SetHealth(((CurrentAmount / MaxAmount) * 0.5f));
    }
}
