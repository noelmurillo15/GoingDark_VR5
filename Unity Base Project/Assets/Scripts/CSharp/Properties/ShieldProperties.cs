﻿using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ShieldProperties
{
    #region Properties
    public GameObject Shield;
    public bool Active { get; private set; }
    public float Health { get; private set; }

    //  Player Data
    public bool isPlayer;
    private Image ShieldBar;
    #endregion

    public ShieldProperties(GameObject _shield, float shieldHP)
    {
        isPlayer = false;
        Shield = _shield;
        Active = true;
        Health = shieldHP;
        ShieldBar = null;    
    }
    public ShieldProperties(GameObject _shield, float shieldHP, bool _player)
    {
        isPlayer = _player;
        Shield = _shield;
        Active = true;
        Health = shieldHP;
        ShieldBar = GameObject.Find("PlayerShield").GetComponent<Image>();
    }

    #region Accessors
    public bool GetShieldActive()
    {
        return Active;
    }
    #endregion

    #region Modifiers
    public void Heal(float _val)
    {
        Health += _val;
        if (Health > 100f)
            Health = 100f;

        UpdateShieldBar();
    }

    public void FullRestore()
    {
        Health = 100f;
        Active = true;
        Shield.SetActive(true);
        UpdateShieldBar();
    }

    public void UpdateShieldBar()
    {
        ShieldBar.fillAmount = (Health / 100f) * .5f;
    }

    public void Damage(float _val)
    {
        Health -= _val;
        if (Health <= 0f)
        {
            Health = 0f;            
            Active = false;
            Shield.SetActive(false);
        }
        if (isPlayer)
        {
            AudioManager.instance.PlayShieldHit();
            UpdateShieldBar();
        }
    }
    #endregion
}