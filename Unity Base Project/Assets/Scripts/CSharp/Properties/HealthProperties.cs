using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HealthProperties
{
    #region Properties
    public float Health { get; private set; }
    private float MaxHealth;

    //  Enemy Data
    private Transform baseRef;

    //  Player Data
    public bool isPlayer;
    private CameraShake shake;
    private Image HealthBar;
    #endregion

    public HealthProperties(float _hp, Transform _ref)
    {
        MaxHealth = _hp;
        Health = MaxHealth;

        shake = null;
        baseRef = _ref;
        isPlayer = false;
    }
    public HealthProperties(float _hp, Transform _ref, bool _player)
    {
        MaxHealth = _hp;
        Health = MaxHealth;

        baseRef = _ref;
        isPlayer = _player;
        HealthBar = GameObject.Find("PlayerHealth").GetComponent<Image>();
        shake = GameObject.FindGameObjectWithTag("LeapMount").GetComponent<CameraShake>();
    }

    #region Modifiers
    public void Heal(float _val)
    {
        Health += _val;
        if (Health > MaxHealth)
            Health = MaxHealth;

        UpdateHPBar();
    }

    public void FullRestore()
    {
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
            AudioManager.instance.PlayHit();
            shake.PlayShake();
            UpdateHPBar();
        }      

        if (Health <= 0f)        
            baseRef.SendMessage("Kill");                
    }
    #endregion
}
