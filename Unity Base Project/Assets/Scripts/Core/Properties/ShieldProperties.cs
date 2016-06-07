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
    }       
}