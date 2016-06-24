using System;
using UnityEngine;

[Serializable]
public class ShieldProperties
{
    public GameObject Shield;
    public bool ShieldActive;
    public float ShieldHealth;

    public void Initialize(GameObject _shield)
    {
        Shield = _shield;
        ShieldActive = true;
        ShieldHealth = 100;
    }

    public bool GetShieldActive()
    {
        return ShieldActive;
    }

    public void TakeDamage()
    {
        if (ShieldActive)
        {
            AudioManager.instance.PlayShieldHit();
            ShieldHealth -= 10.0f;

            if (ShieldHealth <= 0.0f)
            {
                ShieldActive = false;
                Shield.SetActive(false);
            }
        }
        else
        {
            if (!ShieldActive && ShieldHealth > 0)
            {
                ShieldActive = true;
                Shield.SetActive(true);
            }
        }
    }

    public void ShieldRecharge(float _val)
    {
        ShieldActive = true;
        if (ShieldHealth < 100)
        {
            ShieldHealth += _val;

            if (ShieldHealth > 100)
                ShieldHealth = 100;
        }
    }

}