using System;
using UnityEngine;

[Serializable]
public class ShieldProperties
{
    public GameObject Shield;
    public bool ShieldActive;
    public float ShieldHealth;

    public ShieldProperties()
    {

    }

    public void Initialize(GameObject _shield, float shieldHP)
    {
        Shield = _shield;
        ShieldActive = true;
        ShieldHealth = shieldHP;
    }

    public bool GetShieldActive()
    {
        return ShieldActive;
    }

    public void TakeDamage(float _val)
    {
        ShieldHealth -= _val;
        if (ShieldHealth <= 0.0f)
        {
            ShieldHealth = 0f;            
            ShieldActive = false;
            Shield.SetActive(false);
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