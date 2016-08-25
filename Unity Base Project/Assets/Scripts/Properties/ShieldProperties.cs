using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ShieldProperties
{
    #region Properties
    public GameObject ShieldGameobject;
    public bool Active { get; private set; }
    public float Health { get; private set; }
    public float MaxHealth { get; private set; }

    //  Player Data
    public bool isPlayer;
    private Image ShieldBar;
    #endregion

    public ShieldProperties(GameObject _shield, float shieldHP)
    {
        isPlayer = false;
        ShieldGameobject = _shield;
        Active = true;
        MaxHealth = shieldHP;
        Health = MaxHealth;
        ShieldBar = null;    
    }
    public ShieldProperties(GameObject _shield, float shieldHP, bool _player)
    {
        isPlayer = _player;

        if (_shield == null)
            Debug.LogError("Could not find player shield");        

        ShieldGameobject = _shield;
        Active = true;
        MaxHealth = shieldHP;
        Health = MaxHealth;
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
        if (Health > MaxHealth)
            Health = MaxHealth;

        UpdateShieldBar();
    }

    public void FullRestore()
    {
        Active = true;
        Health = MaxHealth;
        if(ShieldGameobject != null)
            ShieldGameobject.SetActive(true);
        UpdateShieldBar();
    }

    public void SetShieldActive(bool flip)
    {
        if (!flip)
            Health = 0f;                    
        else        
            Health = MaxHealth;

        Active = flip;
        ShieldGameobject.SetActive(flip);
    }

    public void UpdateShieldBar()
    {
        ShieldBar.fillAmount = (Health / MaxHealth) * .5f;
    }

    public void Damage(float _val)
    {
        if (Active)
        {
            Health -= _val;
            if (Health <= 0f)
            {
                Health = 0f;
                Active = false;

                if (ShieldGameobject != null)
                    ShieldGameobject.SetActive(false);
            }
            if (isPlayer)
            {
                UpdateShieldBar();
                AudioManager.instance.PlayShieldHit();
            }
        }
    }
    #endregion
}