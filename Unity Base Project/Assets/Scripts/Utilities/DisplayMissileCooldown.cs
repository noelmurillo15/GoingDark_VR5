using UnityEngine;
using UnityEngine.UI;
using MovementEffects;
using System.Collections.Generic;

public class DisplayMissileCooldown : MonoBehaviour
{

    private Image MissileGauge;
    private MissileSystem system;

    void Start()
    {
        MissileGauge = GetComponent<Image>();
        Invoke("FindMissileSystem", 1.2f);
    }

    void FindMissileSystem()
    {
        system = GameObject.FindGameObjectWithTag("Systems").GetComponentInChildren<MissileSystem>();
        if (system == null)
            Debug.LogError("Cannot find missile system");

        Timing.RunCoroutine(UpdateCooldowns());
    }

    private IEnumerator<float> UpdateCooldowns()
    {
        while (true)
        {
            UpdateGauge();
            yield return Timing.WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    public void UpdateGauge()
    {
        float percent = system.GetCooldown() / system.GetMaxCooldown();
        percent *= .5f;
        SetHealth(percent);
    }

    void SetHealth(float percentage)
    {
        MissileGauge.fillAmount = percentage;
        MissileGauge.color = Color.Lerp(Color.white, Color.red, percentage * 2f);
    }
}
