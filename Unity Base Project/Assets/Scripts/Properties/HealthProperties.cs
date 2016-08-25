using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HealthProperties
{
    #region Properties
    public float Health;
    public float MaxHealth;

    //  Enemy Data
    private Transform baseRef;

    //  Player Data
    public bool isPlayer;
    public bool isPlayerDead;
    private Image HealthBar;
    private float DmgModifier;

    #endregion

    public HealthProperties(float _hp, Transform _ref)
    {
        MaxHealth = _hp;
        Health = MaxHealth;
        DmgModifier = MaxHealth * 0.33f;

        baseRef = _ref;
        isPlayer = false;
    }
    public HealthProperties(float _hp, Transform _ref, bool _player)
    {
        MaxHealth = _hp;
        Health = MaxHealth;
        isPlayerDead = false;

        baseRef = _ref;
        isPlayer = _player;
        HealthBar = GameObject.Find("PlayerHealth").GetComponent<Image>();
    }

    #region Modifiers
    public void FullRestore()
    {
        isPlayerDead = false;
        Health = MaxHealth;
        UpdateHPBar();
    }  
    
    public void UpdateHPBar()
    {
        HealthBar.fillAmount = (Health / MaxHealth) * .5f;
    }  

    public void Damage(float _dmg)
    {
        Health -= _dmg;
        if (isPlayer)
        {
            UpdateHPBar();
            AudioManager.instance.PlayHit();
            if (Health <= 0f && !isPlayerDead)
            {
                isPlayerDead = true;
                baseRef.SendMessage("Kill");
            }         
        }
    }
    #endregion
}
