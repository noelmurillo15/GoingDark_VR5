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
    #endregion

    public HealthProperties(float _hp, Transform _ref)
    {
        MaxHealth = _hp;
        Health = MaxHealth;

        baseRef = _ref;
        isPlayer = false;
        isPlayerDead = false;
    }
    public HealthProperties(float _hp, Transform _ref, bool _player)
    {
        Health = _hp;
        MaxHealth = _hp;
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
        if (_dmg <= 0)
            return;

        Health -= _dmg;        
        if (isPlayer)
        {
            UpdateHPBar();
            AudioManager.instance.PlayHit();
            if (!isPlayerDead && Health <= 0f)
            {
                isPlayerDead = true;
                baseRef.SendMessage("Kill");
            }         
        }
        else
        {
            if(Health <= 0f)
                baseRef.SendMessage("Kill");
        }
    }
    #endregion
}
