using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
//    [SerializeField]
    public Image HealthCircle;
    [SerializeField]
    float CurrentHealth = 0.0f;
    [SerializeField]
    float Max_Health = 100.0f;
    [SerializeField]
    private PlayerStats stats;
    public CameraShake camShake;

    public int HitCount;
    private ScreenBreak screenBreak;


    // Use this for initialization
    void Start () {
        CurrentHealth = Max_Health;
        camShake = GameObject.FindGameObjectWithTag("LeapMount").GetComponent<CameraShake>();
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        screenBreak = GameObject.FindGameObjectWithTag("LeapMount").GetComponent<ScreenBreak>();
        // Debug.Log("HealthBar Script Start()");

    }

    /// Damage is based off 100% so taking 20 damage is 1/5 of full health/sheild
    /// try to stick with 20 health ticks, means 5 hits before death.
    public void DecreaseHealth(float DamageTaken)
    {
      //  Debug.Log("HealthBar Script DecreaseHealth()");

        CurrentHealth -= DamageTaken;
        float C_Health = CurrentHealth / Max_Health;
        C_Health *= .5f;
        SetHealth(C_Health);
        if (CurrentHealth <= 0.0f)
            stats.SendMessage("Kill");
    }

    void SetHealth(float NewHealth)
    {
        HealthCircle.fillAmount = NewHealth;
    }

    public void EnvironmentalDMG()
    {
        HitCount += 20;
        DecreaseHealth(HitCount);
    }

    public void Reset()
    {
        CurrentHealth = Max_Health;
        HitCount = 0;
        SetHealth(((CurrentHealth / Max_Health) * 0.5f));
    }
}
